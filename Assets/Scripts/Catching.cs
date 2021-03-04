using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catching : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.CompareTag("Player"))
        {
            obj.GetComponent<CarAgent>().Catched();
        }
    }
}
