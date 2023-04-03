using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : BaseInteractableObject
{
    private LadderClimbingPlayerMovement ladderPlayer;
    private void Awake()
    {
        //TODO:No encontre una buena forma de volver a suscribir esto si lo desuscribo
        OnPlayerLeaveTrigger += OnPlayerTriggerExit;

    }

    private void OnPlayerTriggerExit()
    {
        if (ladderPlayer == null)
            return;

        ladderPlayer.FinishLadderClimbing();
    }

    protected override void OnPlayerInteraction(GameObject go)
    {
        if (ladderPlayer==null)
        {
            ladderPlayer = go.GetComponent<LadderClimbingPlayerMovement>();
        }
       
        ladderPlayer.InteractWithLadder();
    }
}

