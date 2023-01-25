using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoeBehaviour : MonoBehaviour
{
    public bool canBlock;
    public bool hittable;
    public bool jabbing;
    public bool uppercutting;
    public bool givesStar;

    private Animator joeAnim;
    private Health health;
    private MauricioController player;
    private bool attackSelected;
    private bool stunned;
    public bool KO;
    private bool inAction;
    private JoeFight manager;

    void Start()
    {
        manager = FindObjectOfType<JoeFight>();
        joeAnim = GetComponent<Animator>();
        health = GetComponent<Health>();
        player = FindObjectOfType<MauricioController>();
    }

    private void Update()
    {
        if (joeAnim.GetCurrentAnimatorStateInfo(0).IsName("IdleLow"))
        {
            canBlock = false;
            hittable = true;
            inAction = false;
            attackSelected = false;
            givesStar = false;
        }

        if (joeAnim.GetCurrentAnimatorStateInfo(0).IsName("IdleHigh"))
        {
            canBlock = true;
            hittable = true;
            inAction = false;
            attackSelected = false;
            givesStar = false;
        }

        if (joeAnim.GetCurrentAnimatorStateInfo(0).IsName("Jab")&&!inAction)
        {
            StartCoroutine(JabCouroutine());
        }

        if (joeAnim.GetCurrentAnimatorStateInfo(0).IsName("Uppercut") && !inAction)
        {
            StartCoroutine(UppercutCouroutine());
        }

    }

    public void StartFight()
    {
        joeAnim.SetTrigger("StartFight");
        Invoke("ChangeStances", 10);
        Invoke("Fight", 4);
    }

    void Fight()
    {
        int selection = Random.Range(0, 100);

        if (attackSelected|| !player.Fighting)
        {
            return;
        }

        if (selection > 66)
        {
            attackSelected = true;
            Jab();
        } 
        else  if (selection < 33)
        {
            attackSelected = true;
            Uppercut();
        } else
        {
            Invoke("Fight", 2);
        }
    }

    void ChangeStances()
    {
        if (Random.Range(0f, 100f) < 25f)
        {
            joeAnim.SetTrigger("ChangeStance");
        }

        Invoke("ChangeStances", 1);
    }

    void Jab()
    {
        joeAnim.SetTrigger("Jab");
    }



    IEnumerator JabCouroutine()
    {
        inAction = true;
        yield return new WaitForSeconds(0.01f);
        canBlock = true;
        hittable = true;
        yield return new WaitForSeconds(0.86f);
        givesStar = true;
            canBlock = false;
            hittable = true;
            Debug.Log("Jab");
            jabbing = true;
            yield return new WaitForSeconds(0.5f);
        givesStar = false;
        attackSelected = false;
            Invoke("Fight", 4);

        
    }

    void Uppercut()
    {
        joeAnim.SetTrigger("Uppercut");
    }

    IEnumerator UppercutCouroutine()
    {
        inAction = true;
        yield return new WaitForSeconds(0.01f);
        canBlock = false;
        hittable = false;
        yield return new WaitForSeconds(0.96f);

        
            hittable = true;
            canBlock = false;
            Debug.Log("Uppercut");
            uppercutting = true;
            yield return new WaitForSeconds(0.5f);
            attackSelected = false;
            Invoke("Fight", 4);
        
    }

    public void Block() {
        joeAnim.SetTrigger("Block");
        StopAllCoroutines();
        inAction = true;
        canBlock = true;
        hittable = true;
        Invoke("Fight", 2);
    }

    public void Hit(float _damage)
    {
        givesStar = false;
        joeAnim.SetTrigger("Hit");
        inAction = true;
        health.ApplyDamage(_damage);
        if (!stunned && !KO)
        {
            StopAllCoroutines();
            if (Random.Range(0, 2) < 1) //50% de quedar stuneado
            {
                stunned = true;
                StartCoroutine(TimeStunned());
            } else
            {
                joeAnim.SetBool("Stuned", false);
                Invoke("Fight", 2);
            }
        }
    }

    IEnumerator TimeStunned()
    {
        yield return new WaitForSeconds(0.01f);
        hittable = true;
        canBlock = false;
        joeAnim.SetBool("Stuned", true);
        yield return new WaitForSeconds(2f);
        joeAnim.SetBool("Stuned", false);        
        stunned = false;
        Invoke("Fight", 2);
    }

    public void KOd()
    {
        hittable = false;
        canBlock = false;
        KO = true;
        player.Fighting = false;
        joeAnim.SetBool("KO", true);
        if(!manager.fightEnded)
            StartCoroutine(TryToGetUp());
    }

    IEnumerator TryToGetUp()
    {
        
        yield return new WaitForSeconds(2);
        if (manager.fightEnded)
            StopCoroutine(TryToGetUp());
        for (int i = 1; i < 10; i++)
        {
            yield return new WaitForSeconds(1);
            if (Random.Range(0, 4) < 1)
            {
                health.healt += 60;
                joeAnim.SetBool("KO", false);
                KO = false;
                player.Fighting = true;
                i = 10;
            } else
            {
                Debug.Log("Fallo");
            }
        }
    }

    public void GoToCorner()
    {
        StopAllCoroutines();
        joeAnim.Play("FirstPosition");

    }

    public void Victory()
    {
        StartCoroutine(VictoryCoroutine());
    }

    IEnumerator VictoryCoroutine()
    {
        yield return new WaitForSeconds(2);
        joeAnim.Play("Victory");
    }
}
