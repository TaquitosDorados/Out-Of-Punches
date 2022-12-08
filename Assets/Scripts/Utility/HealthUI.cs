using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image saludUI;

    public void DisplayHealth(float valor)
    {
        valor /= 100.0f;

        if (valor < 0f)
        {
            valor = 0f;
        }

        saludUI.fillAmount = valor;
    }
}
