using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public LevelManager.ParInfo par;

    public Animator guiAnim;

    [HideInInspector]
    public int currentCannonsUsed;
    [HideInInspector]
    public int currentWindmillsUsed;
    [HideInInspector]
    public int currentPuttersUsed;
    
    public bool finishedPutt = false;
    public bool planningMode = true;
    public bool ballInHole = false;
    public bool ballInCannon = false;

    public const float BALL_DEATH_MAGNITUDE = 0.005f; //Velocity magnitude that counts as dead.
    public const float BALL_DEATH_TIME = 0.5f; //How long the ball must be stopped before it's considered dead.
    public float deathTimer = 0.0f;

    public float BALL_WIN_TIME = 2.0f;
    public float winTimer = 0.0f;

    public bool ballCanDie = false;

    public int windmillsTouchingBall;
    public bool ballInWind = false;

    private GameObject ball;
    private Collider2D ballCollider;
    private Rigidbody2D ballRB;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }

    private void Start()
    {
        ball = GameObject.Find("Ball");
        ballCollider = ball.GetComponent<Collider2D>();
        ballRB = ball.GetComponent<Rigidbody2D>();

        guiAnim = GameObject.Find("GUI").GetComponent<Animator>();

        par = LevelManager.instance.GetLevelInfo(SceneManager.GetActiveScene().name);

        if(!AudioManager.instance.musicSource.isPlaying)
        {
            AudioManager.instance.PlayMusic("Background");
        }
        else
        {
            AudioManager.instance.musicSource.volume = 1.0f;
        }
    }

    private void Update()
    {
        //Update current tower counts;
        currentCannonsUsed = 0;
        currentWindmillsUsed = 0;
        currentPuttersUsed = 0;
        foreach(GameObject tower in GameObject.FindGameObjectsWithTag("Tower"))
        {
            if(tower.GetComponentInChildren<PutterPusher>() != null)
            {
                currentPuttersUsed++;
            }
            else if (tower.GetComponentInChildren<WindmillPusher>() != null)
            {
                currentWindmillsUsed++;
            }
            else if (tower.GetComponentInChildren<CannonShooter>() != null)
            {
                currentCannonsUsed++;
            }
        }

        if(windmillsTouchingBall > 0)
        {
            ballInWind = true;
        }
        else
        {
            ballInWind = false;
        }

        if(planningMode || ballInHole || !finishedPutt || ballInCannon)
        {
            ballCanDie = false;
        }
        else
        {
            ballCanDie = true;
        }

        if(planningMode)
        {
            ballCollider.enabled = false;
        }

        if(ballCanDie)
        {
            ballCollider.enabled = true;

            if(ballRB.velocity.magnitude <= BALL_DEATH_MAGNITUDE && !ballInHole && finishedPutt)
            {
                if(deathTimer < BALL_DEATH_TIME)
                {
                    deathTimer += Time.deltaTime;
                }
                else
                {
                    guiAnim.SetTrigger("GameOver");
                }
            }
            else
            {
                deathTimer = 0.0f;
            }
        }

        if (ballInHole)
        {
            if (winTimer <= BALL_WIN_TIME)
            {
                winTimer += Time.deltaTime;
            }
            else
            {
                guiAnim.SetTrigger("GameOver");
            }
        }
        else
        {
            winTimer = 0.0f;
        }
        
        if (ballInWind && !AudioManager.instance.sustainedEffectSource.isPlaying)
        {
            AudioManager.instance.PlaySustainedEffect("LoopWind", 0.5f);
        }
        if ((!ballInWind && AudioManager.instance.sustainedEffectSource.isPlaying)
            || winTimer >= BALL_WIN_TIME)
        {
                AudioManager.instance.StopSustainedEffect();
        }
    }

    public void ResetLevel()
    {
        StartCoroutine(ResetLevel_Coroutine(0.25f));
    }

    private IEnumerator ResetLevel_Coroutine(float seconds)
    {
        guiAnim.SetTrigger("Retry");
        float timer = 0.0f;

        while(timer < seconds)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}