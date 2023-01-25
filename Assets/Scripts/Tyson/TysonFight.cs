using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TysonFight : MonoBehaviour
{
    public MauricioController player;
    public TysonBehaviour Tyson;
    private RefereeScript referee;

    [SerializeField] private CameraShader cameraShader;

    public Text[] txtRound;
    public Text txtTime;
    public GameObject RoundUI;
    public GameObject Periodico;

    public AudioSource playerAudio;
    public AudioSource TysonAudio;

    public AudioClip jabSound;
    public AudioClip superPunchSound;
    public AudioClip TysonJabSound;
    public AudioClip TysonUpperSound;
    public AudioClip TysonHookSound;
    public AudioClip starSound;

    public float MacPunchDmg;
    public float MacSuperDmg;
    public float TysonJabDmg;
    public float TysonUpperDmg;
    public float TysonHookDmg;

    public bool fightEnded;

    private int roundTimer = 60;
    private float timer;
    private bool timerOn;
    private int MacKOthisRound;
    private int TysonKOthisRound;
    private float dmgOnMac = 0;
    private float dmgOnTyson = 0;
    [SerializeField]
    private int Round = 1;

    private void Start()
    {
        referee = FindObjectOfType<RefereeScript>();
        StartCoroutine(StartFight());
        txtRound[0].text = "" + Round;
        txtRound[1].text = "Round " + Round;
        Periodico.SetActive(true);
    }

    private void Update()
    {
        if (fightEnded)
        {
            player.enabled = false;
            Tyson.enabled = false;
        }

        if (player.punching)
        {
            AttackEnemy(MacPunchDmg);
            player.punching = false;
        }

        if (player.KOpunch)
        {
            SuperPunch(MacSuperDmg);
            player.KOpunch = false;
        }

        if (Tyson.jabbing)
        {
            JabPlayer(TysonJabDmg);
            Tyson.jabbing = false;
        }

        if (Tyson.uppercutting)
        {
            UppercutPlayer(TysonUpperDmg);
            Tyson.uppercutting = false;
        }

        if (Tyson.hooking)
        {
            HookPlayer(TysonHookDmg);
            Tyson.hooking = false;
        }

        if (timerOn)
        {
            if (Time.time - timer >= 1)
            {
                roundTimer--;
                txtTime.text = "" + roundTimer;
                timer = Time.time;
                if (roundTimer <= 0)
                {
                    timerOn = false;
                    EndRound();
                }
            }
        }


    }

    IEnumerator StartFight()
    {
        yield return new WaitForSeconds(5);
        RoundUI.SetActive(true);
        Periodico.SetActive(false);
        yield return new WaitForSeconds(2);
        RoundUI.SetActive(false);
        Tyson.StartFight();
        player.StartFight();
        referee.StartFight();
        yield return new WaitForSeconds(4.13f);
        timerOn = true;
        timer = Time.time;
    }

    public void AttackEnemy(float _damage)
    {
        if (Tyson.hittable)
        {
            if (Tyson.canBlock)
            {
                Tyson.Block();
                player.Exhaust();
            }
            else
            {
                playerAudio.clip = jabSound;
                playerAudio.Play();
                if (Tyson.givesStar)
                {
                    player.SuperPunchLoaded = true;
                    playerAudio.clip = starSound;
                    playerAudio.Play();
                }

                Tyson.Hit(_damage);
                dmgOnTyson += _damage;
                CheckForKO();
            }
        }
    }

    public void SuperPunch(float _damage)
    {
        if (Tyson.hittable)
        {
            playerAudio.clip = superPunchSound;
            playerAudio.Play();
            dmgOnTyson += _damage;
            Tyson.Hit(_damage);
            CheckForKO();
        }
    }

    public void JabPlayer(float _damage)
    {
        if (!player.KO)
        {
            if (!player.dodging)
            {
                if (!player.blocking)
                {
                    cameraShader.GetHit();
                    TysonAudio.clip = TysonJabSound;
                    TysonAudio.Play();
                    player.Hit(_damage);
                    dmgOnMac += _damage;
                    CheckForKO();
                }
            }
            else
            {
                player.Recover();
            }
        }
    }

    public void UppercutPlayer(float _damage)
    {
        if (!player.KO)
        {
            if (!player.dodging)
            {
                cameraShader.GetHit();
                TysonAudio.clip = TysonUpperSound;
                TysonAudio.Play();
                dmgOnMac += _damage;
                player.Hit(_damage);
                CheckForKO();
            }
            else
            {
                player.Recover();
            }
        }
    }

    public void HookPlayer(float _damage)
    {
        if (!player.KO)
        {
            if (!player.dodging)
            {
                cameraShader.GetHit();
                TysonAudio.clip = TysonHookSound;
                TysonAudio.Play();
                dmgOnMac += _damage;
                player.Hit(_damage);
                CheckForKO();
            }
            else
            {
                player.Recover();
            }
        }
    }

    void CheckForKO()
    {
        if (player.KO)
        {
            Tyson.Victory();
            MacKOthisRound++;
            timerOn = false;
            if (MacKOthisRound == 3)
            {
                player.fightEnded = true;
                fightEnded = true;
                referee.walkinKO();
                PlayerPrefs.SetInt("Result", 0);
                StartCoroutine(GoToResultScene());
            }
            else
            {
                StartCoroutine(StartCountForMac());
            }
        }
        else if (Tyson.KO)
        {
            TysonKOthisRound++;
            timerOn = false;

            if (TysonKOthisRound == 3)
            {
                fightEnded = true;
                player.Victory();
                referee.walkinKO();
                PlayerPrefs.SetInt("Result", 1);
                StartCoroutine(GoToResultScene());
            }
            else
            {
                StartCoroutine(StartCountForTyson());
            }
        }
        else
        {
            timerOn = true;
        }


    }

    IEnumerator StartCountForMac()
    {
        yield return new WaitForSeconds(0.5f);
        referee.startCount();
        cameraShader.GetHit();
        yield return new WaitForSeconds(1.5f);
        for (int i = 1; i < 11; i++)
        {
            if (!player.KO)
            {
                StopAllCoroutines();
                referee.stopCount();
                Tyson.StopTaunt();
            }
            cameraShader.GetHit();

            Debug.Log(i);
            if (i != 10)
                yield return new WaitForSeconds(1);
        }
        fightEnded = true;
        referee.KO();
        player.fightEnded = true;
        PlayerPrefs.SetInt("Result", 0);
        StartCoroutine(GoToResultScene());
    }

    IEnumerator StartCountForTyson()
    {
        yield return new WaitForSeconds(0.5f);
        referee.startCount();
        yield return new WaitForSeconds(1.5f);
        for (int i = 1; i < 11; i++)
        {
            if (!Tyson.KO)
            {
                StopAllCoroutines();
                referee.stopCount();
            }
            Debug.Log(i);
            if (i != 10)
                yield return new WaitForSeconds(1);
        }
        fightEnded = true;
        player.Victory();
        referee.KO();
        PlayerPrefs.SetInt("Result", 1);
        StartCoroutine(GoToResultScene());
    }

    IEnumerator GoToResultScene()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("TysonResultsScene");
    }

    void EndRound()
    {
        MacKOthisRound = 0;
        TysonKOthisRound = 0;
        Round++;
        roundTimer = 60;
        txtRound[0].text = "" + Round;
        txtRound[1].text = "Round " + Round;
        player.GoToCorner();
        Tyson.GoToCorner();
        RoundUI.SetActive(true);

        if (Round == 4)
        {
            if(dmgOnMac > dmgOnTyson)
            {
                PlayerPrefs.SetInt("Result", 3); //Por decision perdiste
            } else
            {
                PlayerPrefs.SetInt("Result", 4);//Por decision ganaste
            }
            SceneManager.LoadScene("TysonResultsScene");
        }
        else
        {
            StartCoroutine(StartFight());
        }
    }
}
