using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPositionInGame : MonoBehaviour
{
    public GameObject objectToMove;
    public Transform finalPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            objectToMove.transform.position = finalPos.position;
        }
    }
}
