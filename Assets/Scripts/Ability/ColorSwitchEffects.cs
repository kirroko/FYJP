using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class ColorSwitchEffects : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private ColorAdjustments colorAdjustments;

    [Header("Colors")]
    [SerializeField] private Color[] colors;

    // Private variables

    private void Awake()
    {
        Volume vol = gameObject.GetComponent<Volume>();
        ColorAdjustments tmp;

        if (vol.profile.TryGet<ColorAdjustments>(out tmp))
        {
            colorAdjustments = tmp;
        }
    }

    // 0 = white, 1 = red, 2 = yellow, 3 = Blue, 4 = orange, 5 = green, 6 = purple
    public void SwitchColor(int choice)
    {
        colorAdjustments.colorFilter.value = colors[choice];
    }
}
