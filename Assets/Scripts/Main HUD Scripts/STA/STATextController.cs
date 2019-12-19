using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STATextController : MonoBehaviour
{
    GameObject playerObject;
    Text text;

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        text = GetComponent<Text>();
        text.text = playerObject.GetComponent<PlayerController>().GetCurrentSTA() + " / " + playerObject.GetComponent<PlayerController>().GetMaxSTA();

    }

    private void FixedUpdate()
    {
        text.text = playerObject.GetComponent<PlayerController>().GetCurrentSTA() + " / " + playerObject.GetComponent<PlayerController>().GetMaxSTA();
    }
}
