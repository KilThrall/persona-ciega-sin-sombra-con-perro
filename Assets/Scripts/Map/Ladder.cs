using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    LadderClimbingPlayerMovement ladderPlayer;
    private void Awake()
    {
        ladderPlayer = FindObjectOfType<LadderClimbingPlayerMovement>();
    }

    public void OnInteraction()
    {
        ladderPlayer.BeginLadderClimbing();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Blind"))
        {
            ladderPlayer.FinishLadderClimbing();
        }
    }
}

