using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBarkSkill : BasePlayerSkill
{
    #region Serialized Variables
    [SerializeField]
    private GameObject lightPrefab;

    [SerializeField]
    private Transform mouthParent;
 
    [SerializeField][Tooltip("Position where the bark sound is emitted")]
    private Transform mouth;

    [SerializeField]
    private float cooldown = 0.5f;

    private float cooldownLeft = 0;
    #endregion

    private void FixedUpdate()
    {
        if (cooldownLeft > 0)
        {
            cooldownLeft -= Time.deltaTime;
        }
    }

    protected override void OnSkillUsed()
    {
        if (cooldownLeft > 0)
        {
            return;
        }
        cooldownLeft = cooldown;

        var lightInstance = Instantiate(lightPrefab, mouth.position, Quaternion.identity, mouth);
    }
}

