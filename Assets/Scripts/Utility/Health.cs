using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour{
    
public float healt = 100f; //Cantidad de vida del personaje
public bool isPlayer;
    
private HealthUI salud;
    void Awake()
    {
        if (isPlayer)
        {
            salud = GetComponent<HealthUI>();
        }
    }
    
    public void ApplyDamage(float _damage)
    {
        healt -= _damage;
        if (isPlayer)
        {
            salud.DisplayHealth(healt);
            if (healt <= 0)
            {
                GetComponent<MauricioController>().KOd();
            }
        }
        else
        { 
            if (healt <= 0)
            {
                GetComponent<JoeBehaviour>().KOd();
            }   
        }
    }
}
