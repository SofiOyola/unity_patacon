using UnityEngine;

public class GiveLife : MonoBehaviour
{
    public int cantidadL = 10; 

    private void OnTriggerEnter(Collider other) 
    { 
        if (other.tag == "Player") 
        { 
            other.GetComponent<Health_and_Damage>().SumarVida(cantidadL); 
        } 
    } 

    private void OnTriggerStay(Collider other) 
    { 
        if (other.tag == "Player") 
        { 
            other.GetComponent<Health_and_Damage>().SumarVida(cantidadL); 
        } 
    }
}
