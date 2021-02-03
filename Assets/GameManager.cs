using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameObject WinScreenObj;
    public GameObject GamePanel;
    public GameObject LoseText;
    public GameObject WinText;

    public destinationReached HomeObj;
    private StencilSphereBehavior heartbeatscriptholder;

    public GameObject PlayerObj;
    public GameObject enemyObject;

    public float WaitTillHowlIsDone =1f;
    public AudioSource BGmusic;
    public AudioSource ClickSouncSource;
    public AudioSource GameMusicSource;
    public AudioClip  GameOverMusic;
    public AudioClip  GameComepleteMusic;
    public AudioClip ClickSoundClip;

    private bool CanPlayGameOverMusic;
    private bool CanPlayGameCompleteMusic;
    
    public TextMeshProUGUI TimetakenText;

    public float TimeToComplete =0;
    private void Start()
    {
        TimeToComplete = 0;
        CanPlayGameCompleteMusic = true;
        CanPlayGameOverMusic = true;
        ClickSouncSource.clip = ClickSoundClip;
        heartbeatscriptholder = FindObjectOfType<StencilSphereBehavior>();
        if(heartbeatscriptholder==null)
        {
            Debug.Log("didnt get the script");
        }
        Time.timeScale = 1;
    }
    private void Update()
    {
        
        if(HomeObj.Reached)
        {
    
         
            heartbeatscriptholder.heartbeatsource.Stop();
            PlayerObj.GetComponent<PlayerMoveNav>().ReachedHome = true;
            Time.timeScale = 0;
            GameFinished();
        }
        else
        {
            TimeToComplete += Time.deltaTime;
        }
        if (PlayerObj.GetComponent<PlayerMoveNav>().wolfattacked == true) 
        {
            Invoke("GameOver", WaitTillHowlIsDone);
        }
    }
    public void GameFinished()
    {
       
        TimetakenText.text = TimeToComplete.ToString();
       
        GamePanel.SetActive(false);
        WinScreenObj.SetActive(true);
        WinText.SetActive(true);
        LoseText.SetActive(false);
        if(CanPlayGameCompleteMusic)
        {
         GameCompleteMusicFunc();
        }
        //WinMusic
        //GameShouldStop

    }


    public void PauseMenu()
    {
        ClickSouncSource.Play();
        PlayerObj.GetComponent<PlayerMoveNav>().isPaused = true;
        Time.timeScale = 0;
        BGmusic.Pause();
        //PauseScreenObj.SetActive(true);
    }

    public void Restart()
    {
        ClickSouncSource.Play();
        Scene GameScene =  SceneManager.GetActiveScene();
        SceneManager.LoadScene(GameScene.name);
    }
    public void MainMenu()
    {
        ClickSouncSource.Play();
        SceneManager.LoadScene(0);
    }

    public void Return()
    {
        BGmusic.Play();
        ClickSouncSource.Play();
        PlayerObj.GetComponent<PlayerMoveNav>().isPaused = false;
        Time.timeScale = 1;
    }

    public void Quit()
    {
        ClickSouncSource.Play();
        Application.Quit();
    }

    public void GameOver()
    {
        
        GamePanel.SetActive(false);
        WinScreenObj.SetActive(true);
        WinText.SetActive(false);
        LoseText.SetActive(true);
        if (CanPlayGameOverMusic)
        {
          GameOverMusicFunc();
        }
        //Menu popup

    }

    public void ClickSound()
    {
        ClickSouncSource.Play();
    }

    void GameOverMusicFunc()
    {
        GameMusicSource.clip = GameOverMusic;
        CanPlayGameOverMusic = false;
      
        //yield return new WaitForSeconds(2.5f);
        GameMusicSource.Play();
        //return null;
      
    }

    void GameCompleteMusicFunc()
    {
        GameMusicSource.clip = GameComepleteMusic;
        CanPlayGameCompleteMusic = false;

        //yield return new WaitForSeconds(2.5f);
        GameMusicSource.Play();
        //return null;
    }
}
