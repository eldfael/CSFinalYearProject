using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STABarController : MonoBehaviour
{
    GameObject playerObject;
    RectTransform rectTransform;
    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        rectTransform = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        rectTransform.localScale = new Vector3(playerObject.GetComponent<PlayerController>().getSTAPercentage()
            , 1, 1);
    }
}
