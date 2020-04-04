using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{
    GameObject playerObject;
    int levelPointer;
    public int difficulty;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        playerObject = GameObject.FindGameObjectWithTag("Player");
        SceneManager.sceneLoaded += OnSceneLoaded;
        levelPointer = 0;
        difficulty = 0;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {

            playerObject.GetComponent<PlayerController>().HandleXPGain(5);
        }

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    { 
        playerObject.transform.position = Vector2.zero;
        if (scene.name == "LevelUp")
        {
            playerObject.GetComponent<PlayerController>().SetInactive();
        }
        else
        {
            playerObject.GetComponent<PlayerController>().SetActive();
        }
    }

    public void SwapScene()
    {
        if (playerObject.GetComponent<PlayerController>().GetStatPoints() >= 1)
        {
            SceneManager.LoadSceneAsync("LevelUp");
            
        }
        else
        {
            levelPointer++;
            difficulty++;
            SceneManager.LoadSceneAsync("Level");
        }
    }

    public int GetLevelPointer()
    {
        return levelPointer;
    }

    public int GetDifficulty()
    {
        return difficulty;
    }
}
