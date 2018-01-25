using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LuminoSlider : MonoBehaviour
{
    public ColorPicker colorPicker;
    public Slider sliderComponent;

	// Use this for initialization
	void Start ()
    {
        if(colorPicker == null)
        {
            colorPicker = transform.parent.GetComponent<ColorPicker>();
            Debug.LogWarning("ColorPicker non spécifié");
        }

        sliderComponent = GetComponent<Slider>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	}

    public float getLuminosite()
    {
        return sliderComponent.value;
    }
    public void setLuminosite(float luminosite)
    {
        if(luminosite >= 0.0f && luminosite <= 1.0f)
            sliderComponent.value = luminosite;
        else
            sliderComponent.value = 1.0f;
    }
}
