using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GearsPuzzle : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onPuzzleFinishEvent;
    [SerializeField]
    private List<Gear> gears=new();

    private bool finishEventTriggered;

    private void Awake()
    {
        foreach (var gear in gears)
        {
            gear.OnGearActivation += FinishPuzzle;
        }
    }

    private void FinishPuzzle()
    {
        if (CheckActivatedGears() && !finishEventTriggered)
        {
            finishEventTriggered = true;
            onPuzzleFinishEvent?.Invoke();
        }
        else
        {
            foreach (var gear in gears)
            {
                if (gear.IsActivated)
                {
                    gear.Sync();

                }
            }
        }
    }

    private bool CheckActivatedGears()
    {
        foreach (var gear in gears)
        {
            if (!gear.IsActivated)
            {
                return false;
            }
        }
        return true;
    }
}
