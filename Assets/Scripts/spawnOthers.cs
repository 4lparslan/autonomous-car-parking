using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnOthers : MonoBehaviour
{
    float[] carCoordinates = new float[] {  -7.34f ,  -6.1f ,  -4.86f ,  -3.52f ,
         -2.18f ,  -0.94f ,  0.38f ,  1.68f ,  2.96f ,  4.3f };
    
    public int random;
    public GameObject[] objeler;
    

    public void Start()
    {
        random = Random.Range(0, 10);
        for(int i=0; i<10; i++){
            if(i!=random)
                Instantiate(objeler[0], new Vector2(carCoordinates[i], 11.12f), Quaternion.identity);
        }
    }
    
}
