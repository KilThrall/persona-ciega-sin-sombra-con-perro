using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    public const string ON_CHARACTER_SWITCH_KEY = "OnCharacterSwitch";

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
    
    //Cuando el player se bajaba de la escalera o cuando terminaba de trepar daba un pantallazo porque el player desaparecia del centro de la pantalla
    //y la camara se tepeaba al frame siguiente
    private void LateUpdate()
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
            if ((fadeTarget <= currentLight.intensity && multiplier == 1)
                || fadeTarget >= currentLight.intensity && multiplier == -1)
            {
                currentLight.intensity = fadeTarget;
            }
        }
    }

    #endregion

    /// <summary>
    /// Cambio de peronaje, luz y foco de la camara
    /// </summary>
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
        ActionsManager.InvokeAction(ON_CHARACTER_SWITCH_KEY, isFollowingBlind);
        blindCharacter.enabled = isFollowingBlind;
        dogCharacter.enabled = !isFollowingBlind;
 
    }

    /// <summary>
    /// Fade-in de la intensidad de la luz
    /// </summary>
    /// <param name="intensity">Cantidad hasta la que se aumentará la intensidad de la luz</param>
    /// <param name="time">Tiempo que demora el fade in en terminar</param>
    /// <param name="newLight">Luz que sera afectada</param>
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
