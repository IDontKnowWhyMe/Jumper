using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum gameState{
        START,
        GAME_OVER
    }
    public static int score;
    public static int bestScore;
    public gameState gs = gameState.START;
    public GameObject player;
    // Start is called before the first frame update
    [SerializeField]
    private GameObject scoreTxt;
    [SerializeField]
    private GameObject gameOver;
    [SerializeField]
    private AudioSource howerSound;
    [SerializeField]
    private Animator Transition;
    private bool afterload = false;
    public AudioSource transition;



    void Start()
    {
        transition = GetComponent<AudioSource>();
        player = GameObject.Find("player");
    }

    // Update is called once per frame

    void Update()
    {
        if(scoreTxt != null){
            scoreTxt.GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString(); 
        }
        
    }

    public void GameOver(){
        gs = gameState.GAME_OVER;
        gameOver.SetActive(true);
        player.GetComponent<PlayerMovment>().enabled = false;
        if (score > bestScore){
            bestScore = score;
        }
    }

    public void GameReset(){
        StartCoroutine(LevelReset());
    }

    public void SetScore(int newScore){
        score = newScore;
    }

    public int GetScore(){
        return score;
    }

    public void PlayButton(){
        StartCoroutine(LoadLevel());
    }

    public void ButtonHower(){
        howerSound.Play();
    }

    IEnumerator LoadLevel(){
        Transition.SetTrigger("Start");
        transition.Play();
        yield return new WaitForSeconds(1.9f);
        SceneManager.LoadScene(1);
        Debug.Log("load");
    }

    IEnumerator LevelReset(){
        Transition.SetTrigger("Start");
        transition.Play();
        yield return new WaitForSeconds(1.9f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gs = gameState.START;
        score = 0;

    }

}
