using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TysonBehaviour : MonoBehaviour
{
    public bool canBlock;
    public bool hittable;
    public bool jabbing;
    public bool hooking;
    public bool uppercutting;
    public bool givesStar;
    public bool KO;
    public AudioClip wink;

    private Animator tysonAnim;
    private Health health;
    private MauricioController player;
    private TysonFight manager;
    private AudioSource tysonAudio;
    private bool stunned;
    private bool stunnable;
    private bool inAction;
    private bool attackSelected;

    private float timer;
    [SerializeField] private float selectionTick = 2.0f;

    private void Awake()
    {
        tysonAnim = GetComponent<Animator>();
        health = GetComponent<Health>();
        player = FindObjectOfType<MauricioController>();
        manager = FindObjectOfType<TysonFight>();
        tysonAudio = GetComponent<AudioSource>();
    }
    private void Start()
    {
        timer = Time.time;
        tysonAnim.Play("FirstPosition");
    }

    private void Update()
    {
        if (tysonAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            stunnable = false;
            canBlock = true;
            hittable = true;
            inAction = false;
            attackSelected = false;
            Attack();
        }
    }

    public void StartFight()
    {
        tysonAnim.Play("StartFight");
    }

    public void GoToCorner()
    {
        StopAllCoroutines();
        tysonAnim.Play("FirstPosition");
    }

    void Attack()
    {
        if (attackSelected || inAction || !player.Fighting) 
            return;

        if (Time.time - timer < selectionTick)
            return;

        timer = Time.time;

        int selector = Random.Range(0, 100);
        Debug.Log(selector);

        if (selector > 85)
        {
            //Uppercut
            StartCoroutine(UppercutCoroutine());
        } else if(selector > 65)
        {
            //Hook
            StartCoroutine(HookCoroutine());
        } else if (selector > 30)
        {
            //Jab
            StartCoroutine(JabCoroutine());
        } else if (selector > 20)
        {
            StartCoroutine(TauntCoroutine());
        }
    }

    IEnumerator UppercutCoroutine()
    {
        tysonAnim.Play("Uppercut");
        attackSelected = true;
        canBlock= false;
        hittable = false;
        yield return new WaitForSeconds(2f / 3f);
        uppercutting= true;
        stunnable = true;
        hittable = true;
        attackSelected = false;
    }

    IEnumerator HookCoroutine()
    {
        tysonAnim.Play("Hook");
        attackSelected = true;
        canBlock = false;
        hittable = true;
        yield return new WaitForSeconds(1f / 3f);
        tysonAudio.clip = wink;
        tysonAudio.Play();
        givesStar = true;
        yield return new WaitForSeconds(1f/3f);
        givesStar = false;
        hittable = false;
        yield return new WaitForSeconds(1f / 3f);
        hooking = true;
        hittable = true;
        stunnable= true;
    }

    IEnumerator JabCoroutine()
    {
        tysonAudio.clip = wink;
        tysonAudio.Play();
        tysonAnim.Play("Jab");
        attackSelected = true;
        canBlock = true;
        hittable = true;
        yield return new WaitForSeconds(0.466f);
        canBlock = false;
        jabbing = true;
    }

    IEnumerator TauntCoroutine()
    {
        tysonAnim.Play("Taunt");
        attackSelected = true;
        canBlock = true;
        hittable = true;
        yield return new WaitForSeconds(1f / 3f);
        canBlock = false;
        givesStar = true;
        yield return new WaitForSeconds(1f / 3f);
        givesStar = false;
    }

    public void Hit(float _damage)
    {
        StopAllCoroutines();
        if (stunnable)
        {
            stunnable = false;
            tysonAnim.SetBool("Stunned", true);
            Invoke("OutOfStun", 2.0f);
        }

        givesStar = false;
        tysonAnim.Play("Hit");
        inAction = true;
        health.ApplyDamage(_damage);
    }

    void OutOfStun()
    {
        tysonAnim.SetBool("Stunned", false);
    }

    public void Block()
    {
        tysonAudio.Stop();
        tysonAnim.Play("Block");
        StopAllCoroutines();
        inAction = true;
        canBlock = true;
        hittable = true;
    }

    public void Victory()
    {
        tysonAnim.Play("Victory");
    }

    public void StopTaunt()
    {
        tysonAnim.Play("Idle");
    }

    public void KOd()
    {
        hittable = false;
        canBlock = false;
        KO = true;
        player.Fighting = false;
        tysonAnim.SetBool("KO", true);
        if (!manager.fightEnded)
            StartCoroutine(TryToGetUp());
    }

    IEnumerator TryToGetUp()
    {

        yield return new WaitForSeconds(2);

        yield return new WaitForSeconds(Random.Range(1,9));

                health.healt += 90;
                tysonAnim.SetBool("KO", false);
                KO = false;
        yield return new WaitForSeconds(4.8f);
                player.Fighting = true;

        
    }
}
