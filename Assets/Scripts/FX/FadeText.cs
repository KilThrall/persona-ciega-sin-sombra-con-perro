using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FadeText : MonoBehaviour, IFader
{
    [SerializeField]
    private TextMeshProUGUI textToFade;
    [SerializeField]
    [Range(0.1f, 10)]
    private float fadeSpeed = 1;

    private float fadeTarget;
    private bool mustFade = false;

    void FixedUpdate()
    {
        if (mustFade && textToFade.color.a != fadeTarget)
        {
            int multiplier = 1;
            if (fadeTarget < textToFade.color.a)
            {
                multiplier = -1;
            }
            Color spriteColor = textToFade.color;
            spriteColor.a += Time.fixedDeltaTime * fadeSpeed * multiplier;

            if ((fadeTarget <= spriteColor.a && multiplier == 1)
                || fadeTarget >= spriteColor.a && multiplier == -1)
            {
                spriteColor.a = fadeTarget;
                mustFade = false;
            }

            textToFade.color = spriteColor;
        }
    }
    public void FadeIn()
    {
        mustFade = true;
        fadeTarget = 1;
    }
    public void FadeOut()
    {
        mustFade = true;
        fadeTarget = 0;
    }
}
