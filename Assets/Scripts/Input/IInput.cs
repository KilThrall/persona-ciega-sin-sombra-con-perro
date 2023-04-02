using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IInput
{
    public event Action<float> OnMovementInput;
    public event Action<float> OnVerticalAimInput;
    public event Action OnSkillInput;
    public event Action OnInteract;
    public event Action<bool> OnJump;
    public event Action OnSkillUsed;
    public event Action OnDisabled;
}
