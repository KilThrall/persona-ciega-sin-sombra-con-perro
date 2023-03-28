using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Vector2 MinPosition, MaxPosition;

    public Transform target;

    private void FixedUpdate()
    {
        var pos = target.position;

        pos.x = Mathf.Clamp(pos.x, MinPosition.x, MaxPosition.x);
        pos.y = Mathf.Clamp(pos.y, MinPosition.y, MaxPosition.y);
        pos.z = -10;

        transform.position = pos;
    }
}
