using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EchoSpriteLight : MonoBehaviour
{
    public float LightDuration;

    [SerializeField]
    private float lightFallOffDuration = 5;
    [SerializeField]
    private Vector2 targetScale;


    private void FixedUpdate()
    {
        transform.localScale += (Vector3)((Time.deltaTime / lightFallOffDuration) * targetScale) + Vector3.forward;


        LightDuration -= Time.deltaTime;

        if (LightDuration <= 0)
        {
            Destroy(gameObject);
        }
    }
}
