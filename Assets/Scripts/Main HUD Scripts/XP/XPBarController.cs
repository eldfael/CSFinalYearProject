using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPBarController : MonoBehaviour
{
    RectTransform rectTransform;
    PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rectTransform = GetComponent<RectTransform>();
        
    }

    private void FixedUpdate()
    {
        rectTransform.localScale = new Vector3(
            ((float)playerController.GetTotalXP() - (float)playerController.GetLevelThreshhold(playerController.GetLevel()))/
            ((float)playerController.GetLevelThreshhold(playerController.GetLevel()+1) - (float)playerController.GetLevelThreshhold(playerController.GetLevel()))
            ,1,1);
    }
}
