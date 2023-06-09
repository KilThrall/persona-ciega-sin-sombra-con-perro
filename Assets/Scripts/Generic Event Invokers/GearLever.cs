using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearLever : GenericInteractionToggle
{
    [SerializeField]
    private List<GameObject> gears = new();

    readonly private List<IMustBeWaited> itemsToWaitFor = new();

    private void Awake()
    {
        foreach (var go in gears)
        {
            var gear = go.GetComponent<Gear>();
            itemsToWaitFor.Add(gear);

            OnEvent.AddListener(gear.ChangeState);
            OffEvent.AddListener(gear.ChangeState);
        }
    }

    protected override void OnPlayerInteraction(GameObject go)
    {
        //era necesario que las palancas no puedan usarse mientras los engranajes transicionaban!!1!
        //aguanten las interfaces
        if (itemsToWaitFor.Count > 0)
        {
            foreach (var item in itemsToWaitFor)
            {
                if (item.HasFinished())
                {
                    return;
                }
            }
        }

        if (isToggled)
        {
            OffEvent.Invoke();
        }
        else
        {
            OnEvent.Invoke();
        }
        isToggled = !isToggled;
    }
}
