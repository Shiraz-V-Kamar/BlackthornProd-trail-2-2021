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
    public float _VignetteIntensityValue= 1f;
    [HideInInspector]
    public float _GrainIntensityValue;


    [SerializeField]
    private float _mouseScrollVignetteScale;

    [SerializeField]
    private float _mouseScrollSphereScale =5;

    private float grainControlVal;
    private float vignetteControlVal;
    private float _sphereScaleValue;

    private AudioSource VisionSoundSource;
    private float VisionSoundControl;

    private void Start()
    {
        vignetteControlVal = 1;
        VisionSoundSource = GetComponent<AudioSource>();
    }
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
            VisionSoundControl += (Input.mouseScrollDelta.y * _mouseScrollVignetteScale);
            grainControlVal += -(Input.mouseScrollDelta.y * _mouseScrollVignetteScale);
            vignetteControlVal += -(Input.mouseScrollDelta.y * _mouseScrollVignetteScale);
            _sphereScaleValue += (Input.mouseScrollDelta.y * _mouseScrollSphereScale);

            //VignetteIntensitySlider.value = _VignetteIntensityValue;
            DOTween.To(() => VignetteIntensitySlider.value, X => VignetteIntensitySlider.value = X, _VignetteIntensityValue, .25f);
        }
        
        if (vignetteControlVal > 1)
        {
            VisionSoundControl = 0f;
            _sphereScaleValue = 17f;
            grainControlVal = 0.3f;
            vignetteControlVal = 1;
            VignetteIntensitySlider.value = 1f;


        }
        else if (vignetteControlVal < 0.4f)
        {
            VisionSoundControl = 0.8f;
            _sphereScaleValue = 32f;
            vignetteControlVal = 0.4f;
            grainControlVal= 0;
            VignetteIntensitySlider.value = 0.4f;
        }
      
        grainControlVal= Mathf.Clamp(grainControlVal, 0f, 0.3f);
        vignetteControlVal = Mathf.Clamp(vignetteControlVal, 0.4f, 1f);
        _sphereScaleValue = Mathf.Clamp(_sphereScaleValue, 17f, 32f);
        VisionSoundControl = Mathf.Clamp(VisionSoundControl, 0, 0.8f);
       
        DOTween.To(setvalueforVignettetween, _VignetteIntensityValue, Mathf.Clamp(vignetteControlVal,0.4f,3f), 0.5f);
        DOTween.To(setvalueforGraintween, _GrainIntensityValue, grainControlVal, 0.5f);
        VisionSoundSource.DOPitch(VisionSoundControl, 0.5f);
        
        if (StencilSphere != null)
        {
            StencilSphere.transform.DOScale(new Vector3(_sphereScaleValue, _sphereScaleValue, _sphereScaleValue), 1.5f);
        }
    }

    void  setvalueforGraintween(float tweenvalue)
    {
        _GrainIntensityValue = tweenvalue;
    }

    void setvalueforVignettetween(float tweenvalue)
    {
        _VignetteIntensityValue = tweenvalue;
    }

    private void ChangeVignetteIntensitySettings()
    {
        _volume = postprocessingObj.GetComponent<PostProcessVolume>();
        Vignette vignette;
        Grain grain;
        _volume.profile.TryGetSettings(out grain);
        _volume.profile.TryGetSettings(out vignette);
        vignette.intensity.value = _VignetteIntensityValue;
        grain.intensity.value = _GrainIntensityValue;
    }

  
}
