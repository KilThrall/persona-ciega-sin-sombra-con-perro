using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IInput
{
    public event Action<float> OnMovementInput;
    public event Action<float> OnVerticalAimInput;
    public event Action OnSkillInput;
    public event Action OnInteract;
    public event Action<bool> OnJump;
    public event Action OnSkillUsed;
    public event Action OnDisabled;

    private InputMaster input;

    #region Monobehaviour Callbacks
    private void Awake()
    {
        input = new InputMaster();

        input.Enable();

        input.Player.Movement.performed += ctx => OnMovement(ctx.ReadValue<float>());
        input.Player.VerticalAim.performed += ctx => OnVerticalAim(ctx.ReadValue<float>());

        input.Player.Jump.started += ctx => OnJumped(true);
        input.Player.Jump.canceled += ctx => OnJumped(false);

        input.Player.Skill.performed += ctx => OnSkill();

        /*  input.Player.ItemGrab.performed += ctx => OnItemGrabbed();

          input.Player.Skill1.started += ctx => OnSkillUsed(0, true);*/
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
        OnDisabled?.Invoke();
    }

    #endregion

    private void OnMovement(float dir)
    {
        OnMovementInput?.Invoke(dir);
    }

    private void OnVerticalAim(float dir)
    {
        OnVerticalAimInput?.Invoke(dir);
    }

    private void OnJumped(bool isPressed)
    {
        OnJump?.Invoke(isPressed);
    }

    private void OnSkill()
    {
        OnSkillUsed?.Invoke();
    }
}
