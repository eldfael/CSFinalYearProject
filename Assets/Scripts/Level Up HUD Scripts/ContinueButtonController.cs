using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonController : MonoBehaviour
{
    Button button;
    PlayerController playerController;
    GameController gameController;

    private void Start()
    {
        button = GetComponent<Button>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        
        button.interactable = false;

        button.onClick.AddListener(gameController.SwapScene);
    }

    private void Update()
    {
        
        if (playerController.GetStatPoints() == 0)
        {
            button.interactable = true;
        }
        
    }


    


}
