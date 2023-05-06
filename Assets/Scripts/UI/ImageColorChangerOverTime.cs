using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageColorChangerOverTime : MonoBehaviour
{
    [SerializeField]
    private float totalDuration;
    [SerializeField]
    private Color startingColor, endColor;

    private float duration = 0;

    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        duration += Time.deltaTime;
        var currentColor = Color.Lerp(startingColor, endColor, duration / totalDuration);
        image.color = currentColor;
    }
}
