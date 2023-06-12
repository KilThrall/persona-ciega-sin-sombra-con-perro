using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteToFade;
    [SerializeField][Tooltip("Tiempo que se quiere que tarde el fade, no pongan 0 culiados")]
    private float fadeTime=1;

    private float fadeTarget;
    private bool mustFade=false;

    void FixedUpdate()
    {
        if (mustFade && spriteToFade.color.a != fadeTarget)
        {
            int multiplier = 1;
            if (fadeTarget < spriteToFade.color.a)
            {
                multiplier = -1;
            }
            Color spriteColor = spriteToFade.color;
            spriteColor.a += Time.fixedDeltaTime * fadeTime * multiplier;

            if ((fadeTarget <= spriteColor.a && multiplier == 1)
                || fadeTarget >= spriteColor.a && multiplier == -1)
            {
                spriteColor.a = fadeTarget;
                mustFade = false;
            }

            spriteToFade.color = spriteColor;
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
