using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterAnimatorTrigger : GenericOnTriggerEnter
{
    [SerializeField]
    private string animatorTrigger="Trigger de la animacion";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CheckTags(collision.tag))
        {
            if (collision.TryGetComponent<Animator>(out var animator))
            {
                animator.SetTrigger(animatorTrigger);
                genericOntriggerEvent.Invoke();
            }
        }
    }
}
