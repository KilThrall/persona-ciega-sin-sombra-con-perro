using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DogMouth : MonoBehaviour
{
    //se puede hacer un script solo para guardar esto?
    [SerializeField]
    private Transform mouthPosition;

    public Transform MouthPosition => mouthPosition;
}
