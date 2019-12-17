using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{
    GameObject playerObject;
    bool inScene;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        inScene = true;
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (SceneManager.GetActiveScene().name == "LevelUp")
            {
                SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Single);
                playerObject.GetComponent<PlayerController>().setActive();
            }
            if (SceneManager.GetActiveScene().name == "Level1")
            {
                SceneManager.LoadScene("Level1", LoadSceneMode.Single);
                //playerObject.GetComponent<PlayerController>().setInactive();
            }
            if (SceneManager.GetActiveScene().name == "EmptyScene")
            {
                SceneManager.LoadScene("Level1", LoadSceneMode.Single);
            }
        }

    }

    private void OnLevelWasLoaded(int level)
    {
        playerObject.transform.position = Vector2.zero;
    }

}
