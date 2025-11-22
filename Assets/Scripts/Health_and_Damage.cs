using UnityEngine;

public class Health_and_Damage : MonoBehaviour
{
    public int vida = 100;
    public bool invencible = false;
    public float tiempo_invencible = 1f;

    public void RestarVida(int cantidad){

        if (!invencible && vida > 0)
        {
            vida -= cantidad;
        }

    }
/*
    IEnumerator Invulnerabilidad()
    {
        invencible = true;
        yield return new WaitForSeconds();
    }*/
}
