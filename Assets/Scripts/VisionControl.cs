using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
using System;

public class VisionControl : MonoBehaviour
{

    public GameObject postprocessingObj;
    public GameObject StencilSphere;

    public Slider VignetteIntensitySlider;

    [HideInInspector]
    public bool SliderInput;

    private PostProcessVolume _volume;
    [HideInInspector]
    public float _VignetteIntensityValue;


    [SerializeField]
    private float _mouseScrollVignetteScale;

    [SerializeField]
    private float _mouseScrollSphereScale =5;
    
    
    private float vignetteControlVal;
    private float _sphereScaleValue;

  
    

    public void Update()
    {
        //VignetteIntensitySlider.value = _VignetteIntensityValue;
        ChangeVignetteIntensitySettings();

        ChnageVignetteValue();
    }

    private void ChnageVignetteValue()
    {
        if (SliderInput)
        {
            vignetteControlVal = VignetteIntensitySlider.value;
        }
        else
        {

            vignetteControlVal += -(Input.mouseScrollDelta.y * _mouseScrollVignetteScale);
            _sphereScaleValue += (Input.mouseScrollDelta.y * _mouseScrollSphereScale);

            VignetteIntensitySlider.value = _VignetteIntensityValue;

        }

        if (vignetteControlVal > 1)
        {
            _sphereScaleValue = 23;
            vignetteControlVal = 1;
            VignetteIntensitySlider.value = 1f;
        }
        else if (vignetteControlVal < 0.4f)
        {
            _sphereScaleValue = 40;
            vignetteControlVal = 0.4f;
            VignetteIntensitySlider.value = 0.4f;
        }
        vignetteControlVal = Mathf.Clamp(vignetteControlVal, 0.4f, 1f);
        _sphereScaleValue = Mathf.Clamp(_sphereScaleValue, 23f, 40f);

        DOTween.To(setvaluefortween, _VignetteIntensityValue, vignetteControlVal, 1f);
        if (StencilSphere != null)
        {
            StencilSphere.transform.DOScale(new Vector3(_sphereScaleValue, _sphereScaleValue, _sphereScaleValue), 1);
        }
    }

    void setvaluefortween(float tweenvalue)
    {
        _VignetteIntensityValue = tweenvalue;
    }

    private void ChangeVignetteIntensitySettings()
    {
        _volume = postprocessingObj.GetComponent<PostProcessVolume>();
        Vignette vignette;
        _volume.profile.TryGetSettings(out vignette);
        vignette.intensity.value = _VignetteIntensityValue;
    }

  
}
