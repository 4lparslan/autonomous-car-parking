using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CarAgent : Agent
{
    public float h;
    public float v;
    public float acceleration;
    public float steering;
    public Rigidbody2D RigidBodycar;
    private CarController carcontroller;
    public Transform spawnpoint;
    public Transform target;
    float[] carCoordinates = new float[] {  -7.34f ,  -6.1f ,  -4.86f ,  -3.52f ,
         -2.18f ,  -0.94f ,  0.38f ,  1.68f ,  2.96f ,  4.3f };

    public int random;
    public GameObject carPrefab;
    public GameObject[] killEmAll;
    public Transform fishT;
    public Transform catchT;
    public Transform parkT;
    public Vector3 temp;
    public int say1;
    public int say2;

    public override void Initialize()
    {
        RigidBodycar = GetComponent<Rigidbody2D>();
    }
    public override void OnEpisodeBegin()
    {
        say1 = 0;
        say2 = 0;
        if (killEmAll.Length > 0)
        {
            killSpawned();
        }
        random=Random.Range(0, 10);
        temp = new Vector3((1.27556f) * random, 0, 0);
        fishT.transform.position += temp;
        catchT.transform.position += temp;
        parkT.transform.position += temp;
        Spawner();
        this.transform.position = spawnpoint.transform.position;
        this.transform.rotation = spawnpoint.rotation;
    
    }

    public void killSpawned()
    {       
        for (int y=0; y<10 ;y++)
        {
            if (y != random)
            {
                Destroy(killEmAll[y]);
            }
        }
        fishT.transform.position -= temp;
        catchT.transform.position -= temp;
        parkT.transform.position -= temp;
    }

    public void Spawner()
    {
        killEmAll = new GameObject[10];
        

        for (int i = 0; i < 10; i++)
        {
            if (i != random)
            {
                
                killEmAll[i] = (GameObject)Instantiate(carPrefab, new Vector2(carCoordinates[i], 11.12f), Quaternion.identity) as GameObject;
            }
        }
        //yield return new WaitForEndOfFrame();
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 dirToTarget = (target.transform.position - this.transform.position).normalized;
        sensor.AddObservation(transform.position.normalized);
        sensor.AddObservation(this.transform.InverseTransformPoint(target.transform.position));
        sensor.AddObservation(this.transform.InverseTransformVector(RigidBodycar.velocity.normalized));
        sensor.AddObservation(this.transform.InverseTransformDirection(dirToTarget));
        sensor.AddObservation(transform.forward);
        sensor.AddObservation(transform.right);
        float velocityAlignment = Vector3.Dot(target.transform.position.normalized, RigidBodycar.velocity);
        AddReward(0.001f * velocityAlignment);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        h = actionBuffers.ContinuousActions[0];
        v = actionBuffers.ContinuousActions[1];
    }

    public void OnCollisionEnter2D(Collision2D obj)
    {
        if (obj.gameObject.CompareTag("carobstacle") || obj.gameObject.CompareTag("obstacle"))
        {
            AddReward(-0.02f);
          //  Debug.Log("KAZA !");
          //  killSpawned();
            //yield WaitForSeconds (0.25f);
            EndEpisode();
        }
    }
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = -Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

    public IEnumerator Parked()
    {
        AddReward(0.4f);  
        Debug.Log("PARK ETTİ !");
      //  killSpawned();
        yield return new WaitForEndOfFrame();
        EndEpisode();
    }

    public void Fished()
    {
        if (say1==0)
        {
            AddReward(0.01f);
        }
        say1++;
     //   Debug.Log("OLTALANDI !");
    }

    public void Catched()
    {
        if (say2 == 0)
        {
            AddReward(0.01f);
        }
        say2++;
        //  Debug.Log("YAKALANDI !");
    }

    public void FixedUpdate()
    {
        Vector2 speed = transform.up * (v * acceleration)*5;
        RigidBodycar.AddForce(speed);

        float direction = Vector2.Dot(RigidBodycar.velocity, RigidBodycar.GetRelativeVector(Vector2.up));
        if (direction >= 0.0f)
        {
            RigidBodycar.rotation += h * steering * (RigidBodycar.velocity.magnitude / 5.0f);
            //rb.AddTorque((h * steering) * (rb.velocity.magnitude / 10.0f));
        }
        else
        {
            RigidBodycar.rotation -= h * steering * (RigidBodycar.velocity.magnitude / 5.0f);
            //rb.AddTorque((-h * steering) * (rb.velocity.magnitude / 10.0f));
        }

        Vector2 forward = new Vector2(0.0f, 0.5f);
        float steeringRightAngle;

        if (RigidBodycar.angularVelocity > 0)
        {
            steeringRightAngle = -90;
        }
        else
        {
            steeringRightAngle = 90;
        }

        Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward;
        Debug.DrawLine((Vector3)RigidBodycar.position, (Vector3)RigidBodycar.GetRelativePoint(rightAngleFromForward), Color.green);

        float driftForce = Vector2.Dot(RigidBodycar.velocity, RigidBodycar.GetRelativeVector(rightAngleFromForward.normalized));

        Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);


        Debug.DrawLine((Vector3)RigidBodycar.position, (Vector3)RigidBodycar.GetRelativePoint(relativeForce), Color.red);

        RigidBodycar.AddForce(RigidBodycar.GetRelativeVector(relativeForce));
    }
}
