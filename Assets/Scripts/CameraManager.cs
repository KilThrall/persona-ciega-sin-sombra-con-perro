using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Light2D globalLight;
    [SerializeField]
    private Vector2 minPosition, maxPosition;

    [SerializeField]
    private PlayerInput blindCharacter;
    [SerializeField]
    private PlayerInput dogCharacter;

    [SerializeField]
    private float blindLightIntensity, blindLightFadeTime;
    [SerializeField]
    private float dogLightIntensity, dogLightFadeTime;

    private bool isFollowingBlind;

    private InputMaster input;

    private float fadeTarget, timeForFade;

    #region Monobehaviour callbacks
    private void Awake()
    {
        input = new InputMaster();

        input.Enable();

        input.Camera.SwitchCharacter.performed += ctx => OnSwitch();

        /*  input.Player.ItemGrab.performed += ctx => OnItemGrabbed();

          input.Player.Skill1.started += ctx => OnSkillUsed(0, true);*/
    }
    private void Start()
    {
        OnSwitch();
    }
    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void FixedUpdate()
    {
        var target = isFollowingBlind ? blindCharacter : dogCharacter;
        var pos = target.transform.position;

        pos.x = Mathf.Clamp(pos.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(pos.y, minPosition.y, maxPosition.y);
        pos.z = -10;

        transform.position = pos;

        if (globalLight.intensity != fadeTarget)
        {
            int multiplier = 1;
            if (fadeTarget < globalLight.intensity)
            {
                multiplier = -1;
            }
            globalLight.intensity += Time.fixedDeltaTime * timeForFade * multiplier;
            if((fadeTarget <= globalLight.intensity && multiplier == 1)
                || fadeTarget >= globalLight.intensity && multiplier == -1)
            {
                globalLight.intensity = fadeTarget;
            }
        }
    }

    #endregion
    private void OnSwitch()
    {
        isFollowingBlind = !isFollowingBlind;
        if (isFollowingBlind)
        {
            FadeLight(blindLightIntensity, blindLightFadeTime);
        }
        else
        {
            FadeLight(dogLightIntensity, dogLightFadeTime);
        }
        blindCharacter.enabled = isFollowingBlind;
        dogCharacter.enabled = !isFollowingBlind;
    }

    private void FadeLight(float intensity, float time)
    {
        if (time <= 0)
        {
            globalLight.intensity = intensity;
        }
        timeForFade = time;
        fadeTarget = intensity;
    }
}
