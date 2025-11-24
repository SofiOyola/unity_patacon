using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public GameObject axe;
    public Rigidbody axeRb;
    public GameObject axeTempHolder;
    public Camera playerCamera;
    public GameObject lastEnemyHit;

    // Variables del hacha
    public float axeFlightSpeed = 10f;
    public float axeThrowPower = 10f;
    public float axeRotationSpeed = 10f;

    public NewAxeCollisions axeCollisions;

    public enum AxeState { Static, Thrown, Travelling, Returning }
    public AxeState axeState = AxeState.Static;

    private float startTime;
    private float journeyLenght;
    private Vector3 startPos;
    private Vector3 endPos;

    void Start()
    {
        axeRb = axe.GetComponent<Rigidbody>();
        ResetAxeToHolder();
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && axeState == AxeState.Static)
        {
            axeState = AxeState.Thrown;
        }

        if (Input.GetMouseButtonDown(1) && axeState != AxeState.Returning)
        {
            PrepareReturn();
        }

        if (axeState == AxeState.Thrown)
            ThrownAxeWithPhysics();

        if (axeState == AxeState.Travelling || axeState == AxeState.Returning)
            AxeRotation();

        if (axeState == AxeState.Returning)
            RecallAxe();
    }

    void ThrownAxeWithPhysics()
    {
        axe.transform.SetParent(null);

        axeState = AxeState.Travelling;
        axeRb.isKinematic = false;
        axeRb.useGravity = true;

        Vector3 throwDirection = playerCamera.transform.forward;

        axeRb.AddForce(throwDirection * axeThrowPower, ForceMode.Impulse);
    }

    void AxeRotation()
    {
        if (lastEnemyHit != null) return;

        axe.transform.Rotate(6.0f * axeRotationSpeed * Time.deltaTime, 0, 0);
    }


    void PrepareReturn()
    {
        startPos = axe.transform.position;
        endPos = axeTempHolder.transform.position;

        startTime = Time.time;
        journeyLenght = Vector3.Distance(startPos, endPos);

        axeState = AxeState.Returning;

        if (lastEnemyHit != null)
        {
            Destroy(lastEnemyHit);
            lastEnemyHit = null;
        }

        axeRb.isKinematic = true;
        axeRb.useGravity = false;
        axeCollisions.RemoveConstraints();
    }


    void RecallAxe()
    {
        float distCovered = (Time.time - startTime) * axeFlightSpeed;
        float fracJourney = distCovered / journeyLenght;

        axe.transform.position = Vector3.Lerp(startPos, endPos, fracJourney);

        if (fracJourney >= 1.0f)
            RecalledAxe();
    }

    void RecalledAxe()
    {
        ResetAxeToHolder();
        axeState = AxeState.Static;
    }

    void ResetAxeToHolder()
    {
        axe.transform.SetParent(this.transform);
        axe.transform.position = axeTempHolder.transform.position;
        axe.transform.rotation = axeTempHolder.transform.rotation;

        axeRb.isKinematic = true;
        axeRb.useGravity = false;

        axeCollisions.RemoveConstraints();
    }

    public void CollisionOccured()
    {
        axeState = AxeState.Static;
    }

}
