using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ChangeScene : MonoBehaviour
{
    public string NextScene;
    public string GameOverScene;
    public string GameWinScene;

   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Slime" ){
        GameWin();
        print("Changing to the game win scene.");
        }
    }
    public void NextSceneSwitch () {
        SceneManager.LoadScene(NextScene);
    }

    public void GameOver () {
        SceneManager.LoadScene(GameOverScene);
    }

    public void GameWin (){
        SceneManager.LoadScene(GameWinScene);
    }
}
