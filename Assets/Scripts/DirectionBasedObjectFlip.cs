using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionBasedObjectFlip : MonoBehaviour
{
    [SerializeField] NPCScript npcScript;

    [Space(10)]

    [SerializeField] bool inverse;
    [SerializeField] bool dynamicObject;
    Vector2 offsetPosition;
    public void Init()
    {
        offsetPosition = transform.localPosition;

        if(npcScript != null)
        {
            npcScript.UpdateEvent.AddListener(delegate { UpdatePosition(); });
        }

        else
        {
            PlayerController.instance.UpdateEvent.AddListener(delegate { UpdatePosition(); });
        }
         
    }

    void UpdatePosition()
    {
        if (dynamicObject)
        {
            bool facingRight = PlayerController.instance.IsFacingRight;
            Vector3 newPos = Vector3.zero;
            if (facingRight && !inverse || !facingRight && inverse)
            {
                newPos = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
            }
            else if (!facingRight && !inverse || facingRight && inverse)
            {
                newPos = new Vector3(-transform.localPosition.x, transform.localPosition.y, 0);
            }
            offsetPosition = newPos;
        }
    }

    public void FlipObject(bool facingRight)
    {
        Vector3 newPos = Vector3.zero;

        if (facingRight && !inverse || !facingRight && inverse)
        {
            newPos = new Vector3(offsetPosition.x, offsetPosition.y, 0);
        }

        else if (!facingRight && !inverse || facingRight && inverse)
        {
            newPos = new Vector3(-offsetPosition.x, offsetPosition.y, 0);
        }


        transform.localPosition = newPos;
    }
}
