using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChanger : MonoBehaviour
{
    public void ChangePosition(GameObject newPosition)
    {
        transform.position = newPosition.transform.position;
    }

}
