using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    public GameObject picker;
    private PointerAction pointerAction;

    public GameObject luminositeSlider;
    private LuminoSlider luminoSlider;

    public float minPointerPos = -125;
    public float maxPointerPos = 125;

    public GameObject selectedModel;

	// Use this for initialization
	void Start ()
    {
        for (int i = 0, j = transform.childCount; i < j; i++)
        {
            Transform childTransform = transform.GetChild(i);

            if (childTransform.name == "Picker")
                if(picker == null)
                    picker = childTransform.gameObject;

            if(childTransform.name == "LuminositeSlider")
                if (luminositeSlider == null)
                    luminositeSlider = childTransform.gameObject;
        }

        if (pointerAction == null)
        {
            PointerAction colorPickerPointerAction = picker.GetComponent<PointerAction>();

            if (colorPickerPointerAction == null)
                colorPickerPointerAction = picker.AddComponent<PointerAction>();

            pointerAction = colorPickerPointerAction;

            if (pointerAction == null)
                Debug.LogError("Aucun PointerAction spécifié !");

            pointerAction.setLimitValues(minPointerPos, maxPointerPos);
        }

        if(luminoSlider == null)
        {
            LuminoSlider luminoiteSliderLuminoSlider = luminositeSlider.GetComponent<LuminoSlider>();

            if (luminoiteSliderLuminoSlider == null)
                luminoiteSliderLuminoSlider = luminositeSlider.AddComponent<LuminoSlider>();

            luminoSlider = luminoiteSliderLuminoSlider;

            if (luminoSlider == null)
                Debug.LogError("Aucun PointerAction spécifié !");

            luminoSlider.setLuminosite(1.0f);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void setLuminosite(float luminosite)
    {
        pointerAction.setLuminosite(luminosite);
        luminoSlider.setLuminosite(luminosite);
    }
    public void setPickerLuminosite()
    {
        pointerAction.setLuminosite(luminoSlider.getLuminosite());
    }

    public void changeModelColor()
    {
        if (selectedModel != null)
            selectedModel.GetComponent<Renderer>().material.color = pointerAction.getColor();
    }

    public void setSelectedModel(GameObject selectedObject)
    {
        selectedModel = selectedObject;
        changeSelectedColor();
    }

    public void changeSelectedColor()
    {
        if (selectedModel != null)
        {
            Color color = selectedModel.GetComponent<Renderer>().material.color;
            pointerAction.setColor(color);
            setLuminosite((float)pointerAction.getLuminosite());
        }
    }
}
