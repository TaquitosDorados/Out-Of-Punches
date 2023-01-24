using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image saludUI;

    public GameObject tapUI;
    public Image barraUI;
    public Image staminaUI;
    public void DisplayHealth(float valor)
    {
        valor /= 100.0f;

        if (valor <= 0f)
        {
            valor = 0f;
            barraUI.fillAmount = 0.0f;
        }

        saludUI.fillAmount = valor;
    }

    public void llenarBarra(float points, float maxPoints)
    {
        barraUI.fillAmount = points / maxPoints;
    }

    public void DisplayStamina(float valor)
    {
        valor /= 10.0f;

        if (valor <= 0f)
        {
            valor = 0f;
        }
        staminaUI.fillAmount = valor;
    }
}
