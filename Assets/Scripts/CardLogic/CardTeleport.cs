using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTeleport : MonoBehaviour
{
    PlayerController playerController;

    private void Start()
    {
        playerController = PlayerController.instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        playerController.characterController.enabled = false;
        playerController.gameObject.transform.position = transform.position;
        playerController.characterController.enabled = true;
        Destroy(this.gameObject);
    }
}
