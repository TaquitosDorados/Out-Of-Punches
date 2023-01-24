using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MauricioController : MonoBehaviour
{

    public bool SuperPunchLoaded;
    public bool blocking;
    public bool dodging;
    public bool KO;
    public bool punching;
    public bool Fighting;
    public bool KOpunch;
    public bool fightEnded;

    public GameObject NoStar;

    private bool inAction;
    private Animator macAnimator;
    private Health health;
    private int tapsNeeded = 20;
    [SerializeField]
    private int currentTaps;
    [SerializeField] private int stamina;
    private bool exhausted;

    private void Start()
    {
        macAnimator = GetComponent<Animator>();
        health = GetComponent<Health>();
        stamina = 10;
    }
    private void LateUpdate()
    {
        if (SuperPunchLoaded)
        {
            NoStar.SetActive(false);
        } else
        {
            NoStar.SetActive(true);
        }

        if(macAnimator.GetCurrentAnimatorStateInfo(0).IsName("LeftDodge") || macAnimator.GetCurrentAnimatorStateInfo(0).IsName("RightDodge"))
        {
            dodging = true;
        }else if(macAnimator.GetCurrentAnimatorStateInfo(0).IsName("Block"))
        {
            blocking = true;
        }
        else if (macAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            dodging = false;
            blocking = false;
            inAction = false;
        }

        if (macAnimator.GetCurrentAnimatorStateInfo(0).IsName("Punched"))
        {
            inAction = true;
        }

        if (stamina <= 0)
        {
            exhausted = true;
            macAnimator.SetBool("Exhaustion", true);
        }else
        {
            exhausted = false;
            macAnimator.SetBool("Exhaustion", false);
        }
    }

    public void StartFight()
    {
        StartCoroutine(StartFightCouroutine());
    }

    IEnumerator StartFightCouroutine()
    {
        macAnimator.SetTrigger("StartFight");
        yield return new WaitForSeconds(4.8f);
        Fighting = true;
    }

    public void DodgeLeft()
    {
        if (!Fighting || inAction)
            return;
        StopAllCoroutines();
        StartCoroutine(DodgeLeftCoroutine());
    }

    IEnumerator DodgeLeftCoroutine()
    {
        inAction = true;
        macAnimator.SetTrigger("DodgeLeft");
        //yield return new WaitForSeconds(0.01f);
        dodging = true;
        yield return new WaitForSeconds(0.44f);
        inAction = false;
    }
    public void DodgeRight()
    {
        if (!Fighting || inAction)
            return;
        StopAllCoroutines();
        StartCoroutine(DodgeRightCoroutine());
    }
    IEnumerator DodgeRightCoroutine()
    {
        inAction = true;
        macAnimator.SetTrigger("DodgeRight");
        //yield return new WaitForSeconds(0.01f);
        dodging = true;

        yield return new WaitForSeconds(0.44f);
        inAction = false;
    }
    public void Punch()
    {
        if (!Fighting || exhausted || punching || inAction)
            return;
        StopAllCoroutines();
        StartCoroutine(PunchCouroutine());
    }

    IEnumerator PunchCouroutine()
    {
        macAnimator.SetTrigger("Punching");
        //yield return new WaitForSeconds(0.01f);
        inAction = true;
        yield return new WaitForSeconds(0.14f);
            punching = true;
            yield return new WaitForSeconds(0.33f);
;
        inAction = false;
    }
    public void StarPunch()
    {
        if (!Fighting || inAction || !SuperPunchLoaded || exhausted)
            return;
        StopAllCoroutines();
        StartCoroutine(StarPunchCoroutine());
    }
    IEnumerator StarPunchCoroutine()
    {
        SuperPunchLoaded = false ;
        macAnimator.SetTrigger("Super");
        inAction = true;
        yield return new WaitForSeconds(0.5f);
        KOpunch = true;
        yield return new WaitForSeconds(0.8f);
        inAction = false;
    }
    public void Duck()
    {
        if (!Fighting || inAction)
            return;
        StopAllCoroutines();
        StartCoroutine(DuckCoroutine());
    }

    IEnumerator DuckCoroutine()
    {
        macAnimator.SetTrigger("Ducking");
        //yield return new WaitForSeconds(0.01f);
        inAction = true;
        dodging = true;
        yield return new WaitForSeconds(0.44f);
        inAction = false;
    }
    public void startBlocking()
    {
        if (!Fighting || inAction)
            return;
        macAnimator.SetBool("Blocking", true);
        blocking = true;
    }
    public void endBlocking()
    {
        if (!Fighting)
            return;
        macAnimator.SetBool("Blocking", false);
        blocking = false;
    }

    public void Hit(float _damage)
    {
        inAction = true;
        macAnimator.SetTrigger("Punched");
        StopAllCoroutines();
        health.ApplyDamage(_damage);
    }

    public void KOd()
    {
        KO = true;
        Fighting = false;
        macAnimator.SetBool("KO'd", true);
    }

    public void KoTapping()
    {
        if (fightEnded) return;

        currentTaps++;
        health.saludUI.llenarBarra(currentTaps, tapsNeeded);
        if (currentTaps >= tapsNeeded)
        {
            health.saludUI.tapUI.SetActive(false);
            stamina = 10;
            KO = false;
            macAnimator.SetBool("KO'd", false);
            StartFight();
            tapsNeeded += 20;
            currentTaps = 0;
            health.ApplyDamage(-80f);
        }
    }

    public void GoToCorner()
    {
        Fighting = false;
        StopAllCoroutines();
        macAnimator.Play("FirstPosition");
    }

    public void Exhaust()
    {
        stamina--;
        health.saludUI.DisplayStamina(stamina);
    }

    public void Recover()
    {
        if (stamina <= 0)
        {
            stamina = 10;
            health.saludUI.DisplayStamina(stamina);
        }
    }

    public void Victory()
    {
        Fighting = false;
        StartCoroutine(VictoryCoroutine());
    }

    IEnumerator VictoryCoroutine()
    {
        yield return new WaitForSeconds(2);
        macAnimator.Play("Victory");
    }
}
