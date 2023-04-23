using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Light2D currentLight;
    [SerializeField]
    private Light2D dogLight;
    [SerializeField]
    private Light2D blindLight;
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

    private AudioListener listener;

    #region Monobehaviour callbacks
    private void Awake()
    {
        input = new InputMaster();

        input.Enable();

        input.Camera.SwitchCharacter.performed += ctx => OnSwitch();

        listener = GetComponent<AudioListener>();

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

        if (currentLight.intensity != fadeTarget)
        {
            int multiplier = 1;
            if (fadeTarget < currentLight.intensity)
            {
                multiplier = -1;
            }
            currentLight.intensity += Time.fixedDeltaTime * timeForFade * multiplier;
            if((fadeTarget <= currentLight.intensity && multiplier == 1)
                || fadeTarget >= currentLight.intensity && multiplier == -1)
            {
                currentLight.intensity = fadeTarget;
            }
        }
    }

    #endregion

    private void OnSwitch()
    {
        isFollowingBlind = !isFollowingBlind;
        if (isFollowingBlind)
        {
            FadeLight(blindLightIntensity, blindLightFadeTime, blindLight);
        }
        else
        {
            FadeLight(dogLightIntensity, dogLightFadeTime, dogLight);
        }
        blindCharacter.enabled = isFollowingBlind;
        dogCharacter.enabled = !isFollowingBlind;
        listener.enabled = isFollowingBlind;
    }

    private void FadeLight(float intensity, float time, Light2D newLight)
    {
        currentLight.intensity = 0;
        currentLight = newLight;
        if (time <= 0)
        {
            currentLight.intensity = intensity;
        }
        timeForFade = time;
        fadeTarget = intensity;
    }

    public void TurnDogLight()
    {
        FadeLight(dogLightIntensity, dogLightFadeTime, dogLight);
    }
}
