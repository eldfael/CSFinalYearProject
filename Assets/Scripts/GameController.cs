using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{
    GameObject playerObject;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        playerObject = GameObject.FindGameObjectWithTag("Player");
        SceneManager.sceneLoaded += OnSceneLoaded;
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
            SceneManager.LoadSceneAsync("Level1");
        }
    }

}
