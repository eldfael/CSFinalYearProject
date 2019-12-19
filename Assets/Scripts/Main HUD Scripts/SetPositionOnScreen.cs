using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionOnScreen : MonoBehaviour
{
    RectTransform rectTransform;
    RectTransform parentRectTransform;



    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = GetComponentInParent<RectTransform>();

        rectTransform.SetPositionAndRotation(new Vector2((parentRectTransform.rect.width/2)*-0.8f, (parentRectTransform.rect.height/2)*-0.8f),Quaternion.identity);
        Debug.Log(parentRectTransform.rect.width);
        Debug.Log(parentRectTransform.rect.height);
    }
}
