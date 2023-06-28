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
    public event Action onGearActivation;

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

    /// <summary>
    /// Se debe llamar al finalizar la animacion de transicion
    /// </summary>
    /// <returns>Devuelve si termino la animación de transicion</returns>
    public bool HasFinished()
    {
        return isTransitioning;
    }

    public void FinishedTransition()
    {
        isTransitioning = false;

        if (isActivated)
        {
            onGearActivation?.Invoke();
            soundEmmiter.enabled = true;
        }
        else
        {
            soundEmmiter.enabled = false;
        }
    }

    public void Sync()
    {
        animator.SetTrigger(RESETACTIVATION_ANIMATION_PARAMETER);
        soundEmmiter.ResetTimer();
    }
}
