using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarController : MonoBehaviour
{

    PlayerController playerController;
    RectTransform rectTransform;
    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        rectTransform.localScale = new Vector3((float)playerController.GetCurrentHP() / (float)playerController.GetMaxHP()
            ,1,1);
    }
}
