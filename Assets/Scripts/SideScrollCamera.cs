using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrollCamera : MonoBehaviour
{
    public bool useCamera;
    [SerializeField] GameObject cameraObject;
    [SerializeField] Transform cameraTarget;
    [SerializeField] PlayerController playerController;

    [Header("Camera Movement Settings")]
    [Tooltip("This clamps the movable range from the original player")]
    [SerializeField] bool useCameraBounds;
    [SerializeField] Vector2 cameraXBounds;
    Vector3 originalPosition;

    [Space(10)]
    [Tooltip("The lower the number, the shorter the delay. 0 for instant follow.")]
    [SerializeField]float cameraDelay = .2f;
    [SerializeField] Vector3 cameraOffset = new Vector3(0,2.5f,-9f);
    Vector3 velocity = Vector3.zero;

    [Space(10)]
    [SerializeField] bool moveDirectionChangesXOffset;
    [SerializeField] float moveDirectionXOffset;


    private void Start()
    {
        originalPosition = cameraTarget.transform.position;
    }

    void FixedUpdate()
    {
        CameraLogic();
    }

    void CameraLogic()
    {
        float effectiveXOffset = cameraOffset.x;

        if(useCamera)
        {
            if (moveDirectionChangesXOffset)
            {
                if (playerController.IsFacingRight)
                {
                    effectiveXOffset = moveDirectionXOffset;
                }

                else
                {
                    effectiveXOffset = -moveDirectionXOffset;
                }
            }

            Vector3 desiredPosition = new Vector3(cameraTarget.position.x + effectiveXOffset, cameraTarget.position.y + cameraOffset.y, cameraTarget.position.z + cameraOffset.z);
            if (useCameraBounds)
            {
                desiredPosition.x = Mathf.Clamp(desiredPosition.x, cameraXBounds.x, cameraXBounds.y);
            }

            cameraObject.transform.position = Vector3.SmoothDamp(cameraObject.transform.position, desiredPosition, ref velocity, cameraDelay);


        }


    }
}
