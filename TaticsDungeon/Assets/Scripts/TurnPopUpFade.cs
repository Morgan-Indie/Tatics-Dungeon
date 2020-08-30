using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnPopUpFade : MonoBehaviour
{
    [Header("Not Required")]
    public RectTransform rectTransform;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        text = GetComponentInChildren<Text>();
        LeanTween.alpha(rectTransform, 0, 0);
    }

    public void FadeOut()
    {
        LeanTween.alpha(rectTransform, 0, 2f);
    }

    public void Activate()
    {
        if (text.text == "Player Turn")
            text.text = "Enemy Turn";
        else
            text.text = "Player Turn";
        LeanTween.alpha(rectTransform, 1, 0f);
        FadeOut();
    }
}
