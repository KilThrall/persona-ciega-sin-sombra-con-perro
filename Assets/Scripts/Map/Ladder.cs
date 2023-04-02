using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : BaseInteractableObject
{
    private LadderClimbingPlayerMovement ladderPlayer;
    private void Awake()
    {
    
    }


    public void OnInteraction()
    {
        ladderPlayer.InteractWithLadder();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (ladderPlayer == null)
            return;
        if (collision.CompareTag("Blind"))
        {
            ladderPlayer.FinishLadderClimbing();
        }
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

