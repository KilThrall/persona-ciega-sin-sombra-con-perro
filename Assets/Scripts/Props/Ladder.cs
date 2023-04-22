using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : BaseInteractableObject
{
    private LadderClimbingPlayerMovement ladderPlayer;
    protected override void OnPlayerExitTrigger()
    {
        if (ladderPlayer == null) { return; }

        ladderPlayer.FinishLadderClimbing();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="go">Game object del que se saca el componente</param>
    protected override void OnPlayerInteraction(GameObject go)
    {
        if (ladderPlayer==null)
        {
            ladderPlayer = go.GetComponent<LadderClimbingPlayerMovement>();
        }
        ladderPlayer.InteractWithLadder();
    }
}

