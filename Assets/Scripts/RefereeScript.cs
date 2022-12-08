using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefereeScript : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StartFight()
    {
        StartCoroutine(StartFightCoroutine());
    }

    IEnumerator StartFightCoroutine()
    {
        animator.SetTrigger("Start Fight");
        yield return new WaitForSeconds(4);
        animator.Play("Fight");
    }

    public void startCount()
    {
        animator.Play("Count");
    }

    public void stopCount()
    {
        StartCoroutine(stopCountCoroutine());
    }

    IEnumerator stopCountCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        animator.Play("Idle");
        yield return new WaitForSeconds(3.5f);
        animator.Play("Fight");
    }

    public void KO()
    {
        animator.Play("KO");
        GetComponent<AudioSource>().Play();
    }

    public void walkinKO()
    {
        animator.Play("walkinKO");
        GetComponent<AudioSource>().Play();
    }
}
