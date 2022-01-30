using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrow : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.CompareTag("Destructable"))
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
        
    }
}
