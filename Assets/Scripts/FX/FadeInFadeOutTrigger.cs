using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInFadeOutTrigger : GenericOnTriggerEnter
{
    [SerializeField]
    private GameObject objectToFade;
  
    private Fader componentToFade;

    private void Awake()
    {
        componentToFade=objectToFade.GetComponent<Fader>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CheckTags(collision.tag))
        {
            componentToFade.FadeIn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CheckTags(collision.tag))
        {
            componentToFade.FadeOut();
        }
    }
}
