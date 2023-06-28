using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public interface IMustBeWaited
{
    bool HasFinished();
}  

public class Gear : MonoBehaviour, IMustBeWaited
{
    public bool IsActivated => isActivated;
    public event Action OnGearActivation;

    private const string ACTIVATE_ANIMATION_PARAMETER ="Activate";
    private const string DEACTIVATE_ANIMATION_PARAMETER ="Deactivate";
    private const string RESETACTIVATION_ANIMATION_PARAMETER = "ResetActivation"; //PARA COORDINAR LAS ANIMACIONES CUANDO SE ACTIVA UN NUEVO ENGRANAJE

    private Animator animator;
    private GenericSoundEmmiter soundEmmiter;

    private bool isActivated=false;
    private bool isTransitioning=false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        soundEmmiter = GetComponent<GenericSoundEmmiter>();
    }

    public void ChangeState()
    {
        isActivated = !isActivated;

        if (isActivated)
        {
            animator.SetTrigger(ACTIVATE_ANIMATION_PARAMETER);
        }

        else
        {
            animator.SetTrigger(DEACTIVATE_ANIMATION_PARAMETER);
        }

        isTransitioning = true;
    }


    public bool HasFinished()
    {
        return isTransitioning;
    }
    /// <summary>
    /// Se debe llamar al finalizar la animacion de transicion
    /// </summary>
    /// <returns>Devuelve si termino la animación de transicion</returns>
    public void FinishedTransition()
    {
        isTransitioning = false;

        if (isActivated)
        {
            OnGearActivation?.Invoke();
            soundEmmiter.enabled = true;
        }
        else
        {
            soundEmmiter.enabled = false;
        }
    }
    /// <summary>
    /// Resetea las animaciones y sonidos de los gears para que se sincornicen cuando se activan nuevos
    /// </summary>
    public void Sync()
    {
        animator.SetTrigger(RESETACTIVATION_ANIMATION_PARAMETER);
        soundEmmiter.ResetTimer();
    }
}
