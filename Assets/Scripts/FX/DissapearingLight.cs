using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DissapearingLight : MonoBehaviour
{
    public float LightDuration;

    private Light2D light2D;

    [SerializeField]
    private float lightFallOffDuration = 5;
    [SerializeField]
    private float minimumInnerSize, minimumOuterSize;

    private float startingInnerCircle, startingOuterCircle;


    private void Awake()
    {
        light2D = GetComponent<Light2D>();
        startingInnerCircle = light2D.pointLightInnerRadius;
        startingOuterCircle = light2D.pointLightOuterRadius;
    }

    private void FixedUpdate()
    {
        light2D.pointLightInnerRadius = light2D.pointLightInnerRadius - Time.deltaTime / lightFallOffDuration * startingInnerCircle;
        light2D.pointLightOuterRadius = light2D.pointLightOuterRadius - Time.deltaTime / lightFallOffDuration * startingOuterCircle;

        light2D.pointLightInnerRadius = Mathf.Clamp(light2D.pointLightInnerRadius, minimumInnerSize, startingInnerCircle);
        light2D.pointLightOuterRadius = Mathf.Clamp(light2D.pointLightOuterRadius, minimumOuterSize, startingOuterCircle);

        LightDuration -= Time.deltaTime;

        if (LightDuration <= 0)
        {
            Destroy(gameObject);
        }
    }
}
