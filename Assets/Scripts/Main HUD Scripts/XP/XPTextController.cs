using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPTextController : MonoBehaviour
{
    RectTransform rectTransform;
    PlayerController playerController;
    Text text;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        text = GetComponent<Text>();
    }

    private void FixedUpdate()
    {
        text.text = playerController.GetLevel().ToString();
    }
}
