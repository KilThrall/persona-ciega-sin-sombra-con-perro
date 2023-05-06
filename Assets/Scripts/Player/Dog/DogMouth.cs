using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DogMouth : MonoBehaviour
{
    //se puede hacer un script solo para guardar esto?, si lo hacía hijo por ahi era mas jodido de customizar donde tenia que estar exactamente la boca
    [SerializeField]
    private Transform mouthPosition;

    public Transform MouthPosition => mouthPosition;
}
