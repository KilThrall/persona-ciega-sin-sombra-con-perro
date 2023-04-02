using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayerSkill : MonoBehaviour
{
    #region Serialized variables

    #endregion

    protected const float DEAD_CONTROLLER_VALUE = 0.2f;

    protected PlayerInput input;
    protected Rigidbody2D rb;
    protected Animator anim;

    private Vector2 aimDir;
    private float facingDir;

    #region MonoBehaviour callbacks
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
        input.OnVerticalAimInput += OnVerticalAim;
        input.OnMovementInput += OnHorizontalAim;
        input.OnSkillUsed += OnSkillUsed;
    }
    private void OnDestroy()
    {
        input.OnVerticalAimInput -= OnVerticalAim;
        input.OnMovementInput -= OnHorizontalAim;
        input.OnSkillUsed -= OnSkillUsed;
    }
    #endregion

    protected void OnVerticalAim(float dir)
    {
        if (Mathf.Abs(dir) < DEAD_CONTROLLER_VALUE)
        {
            aimDir.y = 0;
            return;
        }
        dir = dir / Mathf.Abs(dir);
        aimDir.y = dir;
    }

    protected void OnHorizontalAim(float dir)
    {
        if (Mathf.Abs(dir) < DEAD_CONTROLLER_VALUE)
        {
            aimDir.x = 0;
            return;
        }
        dir = dir / Mathf.Abs(dir);
        facingDir = dir;
        aimDir.x = dir;
    }

    //Lo comenté y le saque el parametro porque no era necesario en el DogBark, pero tampoco estaba seguro de borrarlo
    #region OnSkillUsed(Vector2)
    //protected void OnSkillUsed()
    //{
    //    if (aimDir.y == 0)
    //    {
    //        aimDir.x = facingDir;
    //    }
    //    OnSkillUsed(aimDir);
    //}


    //protected abstract void OnSkillUsed(Vector2 aimDir);
    #endregion

    protected abstract void OnSkillUsed();
}
