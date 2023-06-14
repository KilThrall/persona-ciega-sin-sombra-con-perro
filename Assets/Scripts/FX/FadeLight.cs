using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class FadeLight : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField][Tooltip("Luz que se mostrara para senalar interacciones no opcionales, como de puzzles")] //Github no me leia las enies no es que no se escribir
    private Color interactionColor;
    [SerializeField][Tooltip("Luz que se mostrara para senalar interacciones opcionales, como de lore")]
    private Color optionalInteractionColor;
    [SerializeField]
    private Light2D ligthToFade;
    [SerializeField]
    [Range(0.1f, 10)]
    private float fadeSpeed = 1;
    [SerializeField]
    private float lightMaxIntensity = 1;
    #endregion
    private float fadeTarget;
    private bool mustFade = false;


    void FixedUpdate()
    {
        if (mustFade && ligthToFade.intensity != fadeTarget)
        {
            int multiplier = 1;
            if (fadeTarget < ligthToFade.intensity)
            {
                multiplier = -1;
            }

            ligthToFade.intensity += Time.fixedDeltaTime * fadeSpeed * multiplier;

            if ((fadeTarget <= ligthToFade.intensity && multiplier == 1)
                || fadeTarget >= ligthToFade.intensity && multiplier == -1)
            {
                ligthToFade.intensity = fadeTarget;
                mustFade = false;
            }
        }

    }

    public void SetInteractionColor()
    {
        ligthToFade.color = interactionColor;
    }

    public void SetOptionalInteractionColor()
    {
        ligthToFade.color = optionalInteractionColor;
    }

    public void FadeIn()
    {
        mustFade = true;
        fadeTarget = lightMaxIntensity;
    }

    public void FadeOut()
    {
        mustFade = true;
        fadeTarget = 0; 
    } 
}
