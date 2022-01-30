using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspendCard : MonoBehaviour
{

    [SerializeField] GameObject vineToPullObject;
    [SerializeField] LayerMask raycastMask;
    private void Awake()
    {
        
    }

    private void Start()
    {
        Destroy(this.gameObject, 0.8f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Rigidbody rb))
        {
            LiftObjectRB(collision.gameObject, rb);
        }
        else if (collision.gameObject.TryGetComponent(out NPCScript npcScript))
        {
            LiftObjectNPC(collision.gameObject, npcScript);
        }
        
        //Destroy(this.gameObject);
    }

    private void LiftObjectRB(GameObject objToLift, Rigidbody rb)
    {
        rb.isKinematic = true;
        RaycastHit hit;
        Debug.DrawRay(objToLift.transform.position, Vector3.up*15f, Color.green);
        if (Physics.Raycast(objToLift.transform.position, Vector3.up, out hit, 15f,raycastMask))
        {
            GameObject vine = Instantiate(vineToPullObject,hit.point,Quaternion.Euler(0,0,0));
            if(vine.TryGetComponent(out HangmanVine hangmanVine))
            {
                hangmanVine.objectRB = rb;
                hangmanVine.obj = objToLift;
                hangmanVine.originalPosition = objToLift.transform.position;
                hangmanVine.objHasRB = true;
            }

        }
        else
        {
            Vector3 SpawnPoint = new Vector3(transform.position.x, transform.position.y + 15, transform.position.z);
            GameObject vine = Instantiate(vineToPullObject, SpawnPoint, Quaternion.Euler(0, 0, 0));
            if (vine.TryGetComponent(out HangmanVine hangmanVine))
            {
                hangmanVine.objectRB = rb;
                hangmanVine.obj = objToLift;
                hangmanVine.originalPosition = objToLift.transform.position;
                hangmanVine.objHasRB = true;
            }
        }

    }

    private void LiftObjectNPC(GameObject objToLift, NPCScript npcScript)
    {
        npcScript.characterGrabbed = true;
        RaycastHit hit;
        Debug.DrawRay(objToLift.transform.position, Vector3.up * 15f, Color.green);
        if (Physics.Raycast(objToLift.transform.position, Vector3.up, out hit, 15f, raycastMask))
        {
            GameObject vine = Instantiate(vineToPullObject, hit.point, Quaternion.Euler(0, 0, 0));
            if (vine.TryGetComponent(out HangmanVine hangmanVine))
            {
                hangmanVine.objectNPC = npcScript;
                hangmanVine.obj = objToLift;
                hangmanVine.originalPosition = objToLift.transform.position;
                hangmanVine.objHasRB = false;
            }

        }
        else
        {
            Vector3 SpawnPoint = new Vector3(transform.position.x, transform.position.y + 15, transform.position.z);
            GameObject vine = Instantiate(vineToPullObject, SpawnPoint, Quaternion.Euler(0, 0, 0));
            if (vine.TryGetComponent(out HangmanVine hangmanVine))
            {
                hangmanVine.objectNPC = npcScript;
                hangmanVine.obj = objToLift;
                hangmanVine.originalPosition = objToLift.transform.position;
                hangmanVine.objHasRB = false;
            }
        }

    }
}
