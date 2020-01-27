using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ENDNumberController : MonoBehaviour
{
    Text text;
    PlayerController playerController;

    private void Start()
    {
        text = GetComponent<Text>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        text.text = playerController.GetEND().ToString();
    }
}
