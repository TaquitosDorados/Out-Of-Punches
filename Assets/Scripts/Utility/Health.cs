using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public float healt = 100f; //Cantidad de vida del personaje
    public bool isPlayer;
    public bool isTyson;

    public HealthUI saludUI;

    void Awake()
    {
        if (isPlayer)
        {
            saludUI = GetComponent<HealthUI>();
        }
    }

    public void ApplyDamage(float _damage)
    {
        healt -= _damage;
        if (isPlayer)
        {
            saludUI.DisplayHealth(healt);
            if (healt <= 0)
            {
                healt= 0;
                GetComponent<MauricioController>().KOd();
                saludUI.tapUI.SetActive(true);
            }
        }
        else
        {
            if (healt <= 0)
            {
                healt= 0;
                if (isTyson)
                {
                    GetComponent<TysonBehaviour>().KOd();
                }
                else
                {
                    GetComponent<JoeBehaviour>().KOd();
                }
            }
        }
    }
}