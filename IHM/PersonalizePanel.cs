using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalizePanel : MonoBehaviour
{
    public GameObject colorPanel;
    public GameObject modelSelector;

    private GameObject selectedObject;

    private ColorPicker colorPicker;

    // Use this for initialization
    void Start()
    {
        for (int i = 0, j = transform.childCount; i < j; i++)
        {
            Transform childTransform = transform.GetChild(i);

            if (childTransform.name == "ColorPanel")
                if (colorPanel == null)
                    colorPanel = childTransform.gameObject;
        }

        if (colorPanel != null)
            colorPicker = colorPanel.transform.GetComponentInChildren<ColorPicker>();
        else
            Debug.LogError("ColorPanel introuvable");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setModelSelector(GameObject modelSelector)
    {
        this.modelSelector = modelSelector;
        reloadSelectedObject();
    }

    public void reloadSelectedObject()
    {
        selectedObject = modelSelector.GetComponent<ChooseModelAction>().getSelectedModel();
        colorPicker.setSelectedModel(selectedObject);
    }
}
