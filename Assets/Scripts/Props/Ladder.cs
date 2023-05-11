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
    private LadderClimbingPlayerMovement ladderClimbingPlayerMovement;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        
        Gizmos.DrawLine(lowPosition.position, lowPosition.position + Vector3.up * lowPositionTollerance);
        Gizmos.DrawLine(highPosition.position, highPosition.position + Vector3.down * highPositionTollerance);
    }

    protected override void OnPlayerExitTrigger()
    {
        if (ladderClimbingPlayerMovement == null) { return; }
        playerOnLadder = false;
        ladderClimbingPlayerMovement.FinishLadderClimbing();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="go">Game object del que se saca el componente</param>
    protected override void OnPlayerInteraction(GameObject go)
    {
      
        if (ladderClimbingPlayerMovement==null)
        {
            ladderClimbingPlayerMovement = go.GetComponent<LadderClimbingPlayerMovement>();
        }

        if (playerOnLadder)
        {
            var newPos = CheckNearestPosition();
           
            if (newPos != Vector2.zero)
            {
                player.transform.position = newPos;
                ladderClimbingPlayerMovement.InteractWithLadder();
                playerOnLadder=false;
            }
        }
        else
        {
            ladderClimbingPlayerMovement.InteractWithLadder();
            player.transform.position = new Vector2(transform.position.x,player.transform.position.y); // para que se snapee el player a la escalera
            playerOnLadder = true;
        }
       
    }
    public Vector2 CheckNearestPosition()
    {
        Vector2 virtualPlayerPosition= new Vector2(transform.position.x, player.transform.position.y);
        if (Vector2.Distance(virtualPlayerPosition + Vector2.up*1.5f, highPosition.position) < highPositionTollerance)
        {
            return highPosition.position;
        }
        if (Vector2.Distance(virtualPlayerPosition, lowPosition.position) < lowPositionTollerance)
        {
            return lowPosition.position;
        }
        return default;
    }
}

