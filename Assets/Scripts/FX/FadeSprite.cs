using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFader
{
    void FadeIn();
    void FadeOut();
}

public class FadeSprite : MonoBehaviour, IFader
{
    [SerializeField]
    private SpriteRenderer spriteToFade;
    [SerializeField][Range(0.1f,10)]
    private float fadeSpeed=1;

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
            spriteColor.a += Time.fixedDeltaTime * fadeSpeed * multiplier;

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
