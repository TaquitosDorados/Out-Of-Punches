using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MauricioMovement : MonoBehaviour
{
    private Animator macAnimator;

    private void Start()
    {
        macAnimator = GetComponent<Animator>();
    }
    public void DodgeLeft()
    {
        macAnimator.SetTrigger("DodgeLeft");
    }
    public void DodgeRight()
    {
        macAnimator.SetTrigger("DodgeRight");
    }
    public void Punch()
    {
        macAnimator.SetTrigger("Punching");
    }
    public void StarPunch()
    {
        macAnimator.SetTrigger("Super");
    }
    public void Duck()
    {
        macAnimator.SetTrigger("Ducking");
    }
}
