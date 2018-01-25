using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseModelAction : MonoBehaviour
{
    public GameObject selectedModel;
    public Camera selectedCamera;

    private TextureManager textureManager;

	// Use this for initialization
	void Start ()
    {
        if (selectedModel != null)
            setSelectedModel(selectedModel);
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    public void setSelectedModel(GameObject selectedModel)
    {
        if(this.selectedModel != null)
        {
            Transform selectedModelTransform = this.selectedModel.transform;
            int selectedModelChildCount = selectedModelTransform.childCount;
            
            for (int i = 0; i < selectedModelChildCount; i++)
                Destroy(selectedModelTransform.GetChild(i).gameObject);
        }

        this.selectedModel = selectedModel;
        Instantiate(selectedCamera.gameObject, selectedModel.transform);
    }

    public GameObject getSelectedModel()
    {
        return selectedModel;
    }
}
