using UnityEngine;
using System.Collections;
using TMPro;

public class Health_and_Damage : MonoBehaviour
{
    public int vida = 100;
    public TextMeshProUGUI vidaCounter;
    public bool invencible = false;
    public float tiempo_invencible = 1f;
    public float tiempo_frenado = 0.2f;
    public Animator animator; 

    private void Start() 
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
        UpdatedVidaCounterUI();
    }

    public void UpdatedVidaCounterUI()
    {
        vidaCounter.text = $"{vida}";
    }

    public void SumarVida(int cantidadL){

        if (!invencible && vida < 100)
        {
            vida += cantidadL;
            animator.SetTrigger("DamageTrigger");
            StartCoroutine(Invulnerabilidad());
            StartCoroutine(FrenarVelocidad());
            UpdatedVidaCounterUI();
        }

    }

    public void RestarVida(int cantidad){

        if (!invencible && vida > 0)
        {
            vida -= cantidad;
            animator.SetTrigger("DamageTrigger");
            StartCoroutine(Invulnerabilidad());
            StartCoroutine(FrenarVelocidad());
            if (vida == 0)
            {
                GameOver();
            }
            UpdatedVidaCounterUI();
        }

    }

    void GameOver()
    {
        Debug.Log("GAME OVER!!");
        Time.timeScale = 0;
    }

    IEnumerator Invulnerabilidad()
    {
        invencible = true;
        yield return new WaitForSeconds(tiempo_invencible);
        invencible = false;
    }

    IEnumerator FrenarVelocidad()
    {
        var velocidadActual = GetComponent<New_CharacterController>().currentSpeed;
        GetComponent<New_CharacterController>().currentSpeed = 0;
        yield return new WaitForSeconds(tiempo_frenado);
        GetComponent<New_CharacterController>().currentSpeed = velocidadActual;
    }
}
