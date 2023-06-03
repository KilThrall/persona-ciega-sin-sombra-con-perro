using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crank : GenericInteractionTrigger
{
    [SerializeField]
    private string animatorTrigger = "Trigger de la animacion";
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private Transform targetPlatform;
    [SerializeField]
    private float platformSpeed;
    [SerializeField]
    private Vector2 endPosition;

    private Vector2 startingPosition;
    private Vector2 currentDirection;
    private bool goingToEndPos;
    private bool isBeingUsed;

    private Animator anim;
    private BasePlayerMovement playerMovement;

    private void Start()
    {
        startingPosition = targetPlatform.position;
        currentDirection = (endPosition - startingPosition).normalized;
    }

    protected override void OnPlayerInteraction(GameObject go)
    {
        base.OnPlayerInteraction(go);
        isBeingUsed = !isBeingUsed;
        if (go.TryGetComponent<Animator>(out var animator))
        {
            anim = animator;
            anim.SetBool(animatorTrigger,isBeingUsed);
        }
        if (go.TryGetComponent<BasePlayerMovement>(out var mov))
        {
            playerMovement = mov;
        }
    }


    private void Update()
    {
        if (!isBeingUsed)
        {
            return;
        }
        if (playerMovement != null)
        {
            playerMovement.StopPlayer();
        }

        targetPlatform.Translate(currentDirection * platformSpeed * Time.deltaTime);
        if (goingToEndPos)
        {
            transform.eulerAngles += new Vector3(0, 0, rotationSpeed * Time.deltaTime);
            if (Vector2.Distance(targetPlatform.position, endPosition) < 0.5f)
            {
                currentDirection = (startingPosition - endPosition).normalized;
                goingToEndPos = false;
            }
        }
        else
        {
            transform.eulerAngles -= new Vector3(0, 0, rotationSpeed * Time.deltaTime);
            if (Vector2.Distance(targetPlatform.position, startingPosition) < 0.5f)
            {
                currentDirection = (endPosition - startingPosition).normalized;
                goingToEndPos = true;
            }
        }
    }
}
