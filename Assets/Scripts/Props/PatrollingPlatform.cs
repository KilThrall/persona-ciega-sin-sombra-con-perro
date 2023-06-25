using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingPlatform : MonoBehaviour
{
    [SerializeField]
    private Vector2 positionA, positionB;
    [SerializeField]
    private float initialSpeed;

    private float speed;
    private Vector3 targetPosition;

    private void Start()
    {
        SetSpeed(initialSpeed);
    }

    private void Update()
    {
        Vector3 dir = Vector3.zero;
        if (Vector3.Distance(transform.position, targetPosition) > 0.5f)
        {
            dir = positionB - positionA;
        }
        transform.Translate(dir.normalized * speed * Time.deltaTime);
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
        if (speed > 0)
        {
            targetPosition = positionB;
        }
        else
        {
            targetPosition = positionA;
        }
    }

    public void SwitchSpeed()
    {
        SetSpeed(-speed);
    }
}
