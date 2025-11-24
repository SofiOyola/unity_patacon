using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public GameObject axe;
    public Rigidbody axeRb;
    public GameObject axeTempHolder;
    public Camera playerCamera;

    //Variables para la hacha
    public float axeFlightSpeed = 10f;
    public float axeThrowPower = 10f;
    public float axeRotationSpeed = 10f;

    public NewAxeCollisions axeCollisions;

    public enum AxeState {Static, Thrown, Travelling, Returning}
    public AxeState axeState;

    private float startTime;
    private float journeyLenght;
    private Vector3 startPos;
    private Vector3 endPos;
    
    void Start()
    {
        axeRb = axe.GetComponent<Rigidbody>();
        axeRb.isKinematic = true;
        axeRb.useGravity = false;
        axe.transform.position = axeTempHolder.transform.position;
        axe.transform.rotation = axeTempHolder.transform.rotation;
        axe.transform.parent = this.transform;
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            axeState = AxeState.Thrown;
        }

        if (Input.GetMouseButtonDown(1))
        {
            startPos = axe.transform.position;
            endPos = axeTempHolder.transform.position;
            startTime = Time.time;
            journeyLenght = Vector3.Distance(startPos, endPos);
            axeState = AxeState.Returning;
        }

        if(axeState == AxeState.Thrown)
        {
            ThrownAxeWithPhysics();
        }

        //Rotación del hacha
        if (axeState == AxeState.Travelling || axeState == AxeState.Returning)
        {
            axe.transform.Rotate(6.0f * axeRotationSpeed * Time.deltaTime, 0, 0);

        }

        if (axeState == AxeState.Returning)
        {
            RecallAxe();
        }
    }

    void ThrownAxeWithPhysics()
    {
        axe.transform.parent = null;

        axeState = AxeState.Travelling;
        axeRb.isKinematic = false;
        axeRb.useGravity = true;

        // Dirección de la cámara
        Vector3 throwDirection = playerCamera.transform.forward;

        // Aplicar fuerza en la dirección del mouse/cámara
        axeRb.AddForce(throwDirection * axeThrowPower, ForceMode.Impulse);
    }


    void RecallAxe()
    {
        float distCovered = (Time.time - startTime) * axeFlightSpeed;
        float fracJourney = distCovered / journeyLenght;
        axe.transform.position = Vector3.Lerp(startPos, endPos, fracJourney);

        if (fracJourney >= 1.0f)
        {
            RecalledAxe();
        }
    }

    void RecalledAxe()
    {
        axeState = AxeState.Static;
        axeCollisions.RemoveConstraints();
        axe.transform.position = axeTempHolder.transform.position;
        axe.transform.rotation = axeTempHolder.transform.rotation;
        axeRb.isKinematic = true;
        axeRb.useGravity = false;
        axe.transform.parent = this.transform;
    }

    public void CollisionOccured()
    {
        axeState = AxeState.Static;
    }

}
