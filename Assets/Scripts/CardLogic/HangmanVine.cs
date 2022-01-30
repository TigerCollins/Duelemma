using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangmanVine : MonoBehaviour
{
    PlayerAbilities playerAbilities;

    [HideInInspector] public Rigidbody objectRB;
    [HideInInspector] public NPCScript objectNPC;

    [SerializeField] float durationOfVineDecent;
    [SerializeField] float durationOfVineAcent;
    float timeElapsedDecent;
    float timeElapsedAcent;
    Vector3 positionOfVine;
    [HideInInspector] public GameObject obj;
    [SerializeField] LineRenderer lineRenderer;
    [HideInInspector] public Vector3 originalPosition;
    [SerializeField] float objectHoldHeightOffCeiling;
    Vector3 objHoldPosition;
    [HideInInspector]public bool objHasRB = false;
    bool releaseObj = false;
    [SerializeField] Renderer renderer;
    [SerializeField] Material material;
    [SerializeField] float fadeSpeed;
    Color matColour;


    // Start is called before the first frame update
    void Start()
    {
        playerAbilities = PlayerAbilities.instance;
        lineRenderer.SetPosition(0, transform.position);
        objHoldPosition = new Vector3(transform.position.x, transform.position.y - objectHoldHeightOffCeiling, transform.position.z);
        StartCoroutine(ReleaseObject());

    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(1, positionOfVine);
        Lerp();

    }


    void Lerp()
    {
        if (timeElapsedDecent < durationOfVineDecent)
        {
            positionOfVine = Vector3.Lerp(transform.position, obj.transform.position, timeElapsedDecent / durationOfVineDecent);
            timeElapsedDecent += Time.deltaTime;
        }
        else if(!releaseObj && timeElapsedAcent < durationOfVineAcent)
        {
            positionOfVine = obj.transform.position;
            obj.transform.position = Vector3.Lerp(originalPosition,objHoldPosition, timeElapsedAcent / durationOfVineAcent);
            timeElapsedAcent += Time.deltaTime;

        }
    }

    IEnumerator ReleaseObject()
    {
        yield return new WaitForSecondsRealtime(durationOfVineAcent + durationOfVineDecent + playerAbilities.suspendDuration);

        releaseObj = true;
        if (objHasRB)
        {
            objectRB.isKinematic = false;
        }
        else
        {
            objectNPC.characterGrabbed = false;

        }
        FadeRope();

    }

    private void FadeRope()
    {
        renderer.material = material;
        matColour = renderer.material.color;
        StartCoroutine(AlphaFade());
    }

    IEnumerator AlphaFade()
    {
        // Alpha start value.
        float alpha = 1.0f;

        // Loop until aplha is below zero (completely invisalbe)
        while (alpha > 0.0f)
        {
            // Reduce alpha by fadeSpeed amount.
            alpha -= fadeSpeed * Time.deltaTime;

            // Create a new color using original color RGB values combined
            // with new alpha value. We have to do this because we can't 
            // change the alpha value of the original color directly.
            renderer.material.color = new Color(matColour.r, matColour.g, matColour.b, alpha);

            yield return null;
        }
    }



}
