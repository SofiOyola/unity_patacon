using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public bool isPickable = true;

    //Cuando el objeto entre al trigger de mi jugador
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerInteractionZone")
        {
            other.GetComponentInParent<PickUpObjects>().ObjectToPickUp = this.gameObject;
        }
    }

    private void onTriggerExit(Collider other)
    {
        if (other.tag == "PlayerInteractionZone")
        {
            other.GetComponentInParent<PickUpObjects>().ObjectToPickUp = null; 
        }
    }
}
