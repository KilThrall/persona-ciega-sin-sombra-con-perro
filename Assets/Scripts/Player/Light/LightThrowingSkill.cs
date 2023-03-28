using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightThrowingSkill : BasePlayerSkill
{
    [SerializeField]
    private GameObject lightPrefab;


    [SerializeField]
    private float throwSpeed = 5;
    [SerializeField]
    private float playerVelocityMultiplier;
    [SerializeField]
    private float cooldown = 0.5f;

    private float cooldownLeft = 0;


    private void FixedUpdate()
    {
        if (cooldownLeft > 0)
        {
            cooldownLeft -= Time.deltaTime;
        }
    }

    protected override void OnSkillUsed(Vector2 aimDir)
    {
        if (cooldownLeft > 0)
        {
            return;
        }
        cooldownLeft = cooldown;
        var velocity = aimDir.normalized * throwSpeed;
        velocity += rb.velocity * playerVelocityMultiplier;

        var lightInstance = Instantiate(lightPrefab, transform.position, Quaternion.identity);

        lightInstance.GetComponent<Rigidbody2D>().AddForce(velocity);
    }
}
