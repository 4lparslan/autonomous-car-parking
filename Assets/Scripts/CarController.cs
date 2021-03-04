using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float h;
    public float v;
    public float acceleration;
    public float steering;
    public Rigidbody2D RigidBodycar;
    // Start is called before the first frame update
    void Start()
    {
        RigidBodycar = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector2 speed = transform.up * (v * acceleration)*20;
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
