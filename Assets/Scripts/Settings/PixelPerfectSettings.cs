using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PixelPerfectSettings", menuName = "PixelPerfect/CreateNewSettings", order = 1)]
public class PixelPerfectSettings : ScriptableObject
{
    public int pixelPerUnit;
    public Vector2 referenceResolution;
    public bool useReferredScale;
    public int pixelScale;
    public static PixelPerfectSettings GetSettings()
    {
        return Resources.LoadAll<PixelPerfectSettings>("Settings").FirstOrDefault();
    }
}
