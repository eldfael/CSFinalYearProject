using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AGIButtonController : MonoBehaviour
{
    Button button;
    PlayerController playerController;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickEvent);
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (playerController.GetStatPoints() == 0)
        {
            button.interactable = false;
        }
    }
    private void OnClickEvent()
    {
        if (playerController.GetStatPoints() > 0)
        {
            playerController.SetAGI(playerController.GetAGI()+1);
            playerController.SetStatPoints(playerController.GetStatPoints()-1);
            Debug.Log("Agility Up");
        }
    }
}
