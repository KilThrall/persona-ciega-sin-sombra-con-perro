using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : BaseInteractableObject
{
    #region Serialized Variables
    [Space(10)]
    [SerializeField]
    private Transform highPosition;
    [SerializeField]
    private float highPositionTollerance;
    [Space(10)]
    [SerializeField]
    private Transform lowPosition;
    [SerializeField]
    private float lowPositionTollerance;
    #endregion

    private bool playerOnLadder=false;

    private LadderClimbingPlayerMovement ladderPlayer;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        
        Gizmos.DrawLine(lowPosition.position, lowPosition.position + Vector3.up * lowPositionTollerance);
        Gizmos.DrawLine(highPosition.position, highPosition.position + Vector3.down * highPositionTollerance);
    }

    protected override void OnPlayerExitTrigger()
    {
        if (ladderPlayer == null) { return; }
        playerOnLadder = false;
        ladderPlayer.FinishLadderClimbing();
    }

    private void Update()
    {
      
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

        if (playerOnLadder)
        {
            var newPos = CheckNearestPosition();
           
            if (newPos != default(Vector2))
            {
                player.transform.position = newPos;
                ladderPlayer.InteractWithLadder();
                playerOnLadder=false;
            }
        }
        else
        {
            ladderPlayer.InteractWithLadder();
            playerOnLadder = true;
        }
       
    }

    public Vector2 CheckNearestPosition()
    {
        Vector2 virtualPlayerPosition= new Vector2(transform.position.x, player.transform.position.y);
        if (Vector2.Distance(virtualPlayerPosition + Vector2.up*1.5f, highPosition.position) < highPositionTollerance)
        {
            print("hp: " + highPosition.position);
            return highPosition.position;
        }
        if (Vector2.Distance(virtualPlayerPosition, lowPosition.position) < lowPositionTollerance)
        {
            print("lp: " + lowPosition.position);
            return lowPosition.position;
        }
        return default;
    }
}

