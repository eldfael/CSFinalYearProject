using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPTextController : MonoBehaviour
{
    GameObject playerObject;
    Text text;

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        text = GetComponent<Text>();
        text.text = playerObject.GetComponent<PlayerController>().GetCurrentHP() + " / " + playerObject.GetComponent<PlayerController>().GetMaxHP();

    }

    private void FixedUpdate()
    {
        text.text = playerObject.GetComponent<PlayerController>().GetCurrentHP() + " / " + playerObject.GetComponent<PlayerController>().GetMaxHP();
    }
}
