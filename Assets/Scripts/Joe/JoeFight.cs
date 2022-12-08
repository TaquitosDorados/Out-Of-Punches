using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JoeFight : MonoBehaviour
{
    public MauricioController player;
    public JoeBehaviour Joe;
    private RefereeScript referee;

    public Text[] txtRound;
    public Text txtTime;
    public GameObject RoundUI;
    public GameObject Periodico;

    public AudioSource playerAudio;
    public AudioSource joeAudio;

    public AudioClip jabSound;
    public AudioClip superPunchSound;
    public AudioClip joeJabSound;
    public AudioClip joeUpperSound;

    public float MacPunchDmg;
    public float MacSuperDmg;
    public float JoeJabDmg;
    public float JoeUpperDmg;

    public bool fightEnded;

    private int roundTimer = 60;
    private float timer;
    private bool timerOn;
    private int MacKOthisRound;
    private int JoeKOthisRound;
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
            Joe.enabled = false;
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

        if (Joe.jabbing)
        {
            JabPlayer(JoeJabDmg);
            Joe.jabbing = false;
        }

        if (Joe.uppercutting)
        {
            UppercutPlayer(JoeUpperDmg);
            Joe.uppercutting = false;
        }

        if (timerOn)
        {
            if(Time.time-timer >= 1)
            {
                roundTimer--;
                txtTime.text = ""+roundTimer;
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
        Joe.StartFight();
        player.StartFight();
        referee.StartFight();
        yield return new WaitForSeconds(4.13f);
        timerOn = true;
        timer = Time.time;
    }

    public void AttackEnemy(float _damage)
    {
        if (Joe.hittable)
        {
            if (Joe.canBlock)
            {
                Joe.Block();
                //Quitar Stamina
            } else
            {
                if (Joe.givesStar)
                    player.SuperPunchLoaded = true;
                playerAudio.clip = jabSound;
                playerAudio.Play();
                Joe.Hit(_damage);
                CheckForKO();
            }
        }
    }

    public void SuperPunch(float _damage)
    {
        if (Joe.hittable)
        {
            playerAudio.clip = superPunchSound;
            playerAudio.Play();
            Joe.Hit(_damage);
            CheckForKO();
        }
    }

    public void JabPlayer(float _damage)
    {
        if (!player.dodging)
        {
            if (!player.blocking)
            {
                joeAudio.clip = joeJabSound;
                joeAudio.Play();
                player.Hit(_damage);
                CheckForKO();
            }
        }
    }

    public void UppercutPlayer(float _damage)
    {
        if (!player.dodging)
        {
            joeAudio.clip = joeUpperSound;
            joeAudio.Play();
            player.Hit(_damage);
            CheckForKO();
        }
    }

    void CheckForKO()
    {
        if (player.KO)
        {
            MacKOthisRound++;
            timerOn = false;
            if (MacKOthisRound == 3)
            {
                fightEnded = true;
                Joe.Victory();
                referee.walkinKO();
                PlayerPrefs.SetInt("Result", 0);
                StartCoroutine(GoToResultScene());
            } else
            {
                StartCoroutine(StartCountForMac());
            }
        } else if (Joe.KO)
        {
            JoeKOthisRound++;
            timerOn = false;

            if (JoeKOthisRound == 3)
            {
                fightEnded = true;
                player.Victory();
                referee.walkinKO();
                PlayerPrefs.SetInt("Result", 1);
                StartCoroutine(GoToResultScene());
            }
            else
            {
                StartCoroutine(StartCountForJoe());
            }
        } else
        {
            timerOn = true;
        }


    }

    IEnumerator StartCountForMac()
    {
        yield return new WaitForSeconds(0.5f);
        referee.startCount();
        yield return new WaitForSeconds(1.5f);
        for(int i = 1; i < 11; i++)
        {
            if (!player.KO)
            {
                StopAllCoroutines();
                referee.stopCount();
            }

            Debug.Log(i);
            if (i != 10) 
                yield return new WaitForSeconds(1);
        }
        fightEnded = true;
        Joe.Victory();
        referee.KO();
        PlayerPrefs.SetInt("Result", 0);
        StartCoroutine(GoToResultScene());
    }

    IEnumerator StartCountForJoe()
    {
        yield return new WaitForSeconds(0.5f);
        referee.startCount();
        yield return new WaitForSeconds(1.5f);
        for (int i = 1; i < 11; i++)
        {
            if (!Joe.KO)
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
        SceneManager.LoadScene("JoeResultsScene");
    }

    void EndRound()
    {
        MacKOthisRound = 0;
        JoeKOthisRound = 0;
        Round++;
        roundTimer = 60;
        txtRound[0].text = "" + Round;
        txtRound[1].text = "Round " + Round;
        player.GoToCorner();
        Joe.GoToCorner();
        RoundUI.SetActive(true);

        if(Round == 4)
        {
            SceneManager.LoadScene("JoeResultsScene");
        }else
        {
            StartCoroutine(StartFight());
        }
    }
}
