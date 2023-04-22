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
    [SerializeField]
    private float scalingSpeed = 1;//EL CANDELABRO TIENE UNA LUZ MUY GRANDE Y SE TARDA MUCHO EN ESCALAR A ESO
    private void FixedUpdate()
    {
        transform.localScale += (Vector3)((Time.deltaTime*scalingSpeed / lightFallOffDuration) * targetScale) + Vector3.forward;


        LightDuration -= Time.deltaTime;

        if (LightDuration <= 0)
        {
            Destroy(gameObject);
        }
    }
}
