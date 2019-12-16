using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarController : MonoBehaviour
{
    GameObject playerObject;
    RectTransform rectTransform;
    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rectTransform.localScale = new Vector3(playerObject.GetComponent<PlayerController>().getHPPercentage()
            ,1,1);
    }
}
