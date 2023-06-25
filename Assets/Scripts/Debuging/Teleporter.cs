using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToMove;
    [SerializeField]
    private Transform finalPos;

    private void Awake()
    {
        objectToMove= objectToMove==null? gameObject : objectToMove;
    }

    public void Teleport()
    {
        objectToMove.transform.position = finalPos.position;
    }

    public void ChangeFinalPos(Transform newFinalPos)
    {
        finalPos = newFinalPos;
    }
}
