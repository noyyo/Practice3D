using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    private float time;
    public float fullDayLength;
    private float timeRate;
    public float inclination;
    private Vector3 referenceAxis;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionIntensityMultiplier;

    [Header("SkyBox")]
    public Gradient SkyTint;
    public Gradient Ground;
    public AnimationCurve Exposure;
    private Material skyBox;

    private void Start()
    {

        skyBox = RenderSettings.skybox;
        timeRate = 1.0f / fullDayLength;
        sun.transform.rotation = Quaternion.Euler(90, 0, 0);
        moon.transform.rotation = Quaternion.Euler(-90, 0, 0);
        sun.transform.Rotate(sun.transform.up, inclination, Space.World);
        moon.transform.Rotate(-moon.transform.up, inclination, Space.World);
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);
        UpdateSkyBox();

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
    }

    private void UpdateLighting(Light lightSource, Gradient colorGradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);

        lightSource.transform.Rotate(lightSource.transform.right, timeRate * Time.deltaTime * 360, Space.World);
        lightSource.color = colorGradient.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
            go.SetActive(false);
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
            go.SetActive(true);
    }
    private void UpdateSkyBox()
    {
        skyBox.SetColor("_SkyTint", SkyTint.Evaluate(time));
        skyBox.SetColor("_Ground", Ground.Evaluate(time));
        skyBox.SetFloat("_Exposure", Exposure.Evaluate(time));
        if (time <= 0.25f || time >= 0.75f)
        {
            RenderSettings.sun = sun;
            skyBox.SetFloat("_SunSize", 0.08f);
            skyBox.SetFloat("_SunSizeConvergence", 5f);
        }
        else
        {
            RenderSettings.sun = moon;
            skyBox.SetFloat("_SunSize", 0.04f);
            skyBox.SetFloat("_SunSizeConvergence", 8f);
        }
    }
}
