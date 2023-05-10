using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPositionInGame : MonoBehaviour
{
    public GameObject objectToMove;
    public Transform finalPos;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Teleport();
        }
    }
    public void Teleport()
    {
        objectToMove.transform.position = finalPos.position;
    }
}
