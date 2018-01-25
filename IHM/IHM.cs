using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IHM : MonoBehaviour
{
    //Mode voulu
    private CursorLockMode wantedMode;

    public GameObject personalizePanel;
    public GameObject wishlistPanel;

    GameObject modelSelector;

    // Use this for initialization
    void Start()
    {
        wantedMode = CursorLockMode.Locked;

        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = transform.GetChild(i);

            if (childTransform.name == "PersonalizePanel")
                if (personalizePanel == null)
                    personalizePanel = childTransform.gameObject;

            if (childTransform.name == "WishlistPanel")
                if (wishlistPanel == null)
                    wishlistPanel = childTransform.gameObject;
        }

        modelSelector = GameObject.Find("ModelSelector");

        if (personalizePanel == null)
            Debug.LogWarning("No Personalize Panel");
        else
        {
            personalizePanel.GetComponent<PersonalizePanel>().setModelSelector(modelSelector);
            personalizePanel.SetActive(false);
        }

        if (wishlistPanel == null)
            Debug.LogWarning("No Wishlist Panel");
        else
            wishlistPanel.SetActive(false);

        if (modelSelector == null)
            Debug.LogWarning("No ModelSelector");
    }

    void OnGUI()
    {
        //Commence affichage vertical
        GUILayout.BeginHorizontal();
        
        if(GUILayout.Button("Afficher/Masquer PersonalizePanel"))
        {
            if (personalizePanel != null)
            {
                bool shown = personalizePanel.activeSelf;
                //Debug.Log("PersonalizePanel is actually: " + shown);

                showPersonalizePanel(!shown);
            }
        }
        if (GUILayout.Button("Afficher/Masquer WishlistPanel"))
        {
            if (wishlistPanel != null)
            {
                bool shown = wishlistPanel.activeSelf;
                //Debug.Log("WishlistPanel is actually: " + shown);

                showWishlistPanel(!shown);
            }
        }

        if(GUILayout.Button("Change Selected Cube"))
        {
            ChooseModelAction chooseModelAction = modelSelector.GetComponent<ChooseModelAction>();
            GameObject selectedModel = chooseModelAction.getSelectedModel();
            
            if(selectedModel.name == "Cube")
            {
                GameObject newSelectedModel = GameObject.Find("Cubi");

                if (newSelectedModel != null)
                    chooseModelAction.setSelectedModel(newSelectedModel);
                else
                    Debug.LogError("Cubi doesn't find");
            }
            else
            {
                GameObject newSelectedModel = GameObject.Find("Cube");

                if (newSelectedModel != null)
                    chooseModelAction.setSelectedModel(newSelectedModel);
                else
                    Debug.LogError("Cube doesn't find");
            }

            personalizePanel.GetComponent<PersonalizePanel>().reloadSelectedObject();
        }

        string labelStr = "Mouse x: " + Input.mousePosition.x.ToString() + "; Mouse y: " + Input.mousePosition.y.ToString();
        GUILayout.Label(labelStr);

        //Termine affichage vertical
        GUILayout.EndHorizontal();

        //SetCursorState();
    }

    //Change l'état du bouton
    void SetCursorState()
    {
        Cursor.lockState = wantedMode;
        Cursor.visible = (CursorLockMode.Locked != wantedMode);
    }

    void showPersonalizePanel(bool shown)
    {
        if (wishlistPanel.activeSelf)
            showWishlistPanel(false);

        personalizePanel.SetActive(shown);
    }
    void showWishlistPanel(bool shown)
    {
        if (personalizePanel.activeSelf)
            showPersonalizePanel(false);

        wishlistPanel.SetActive(shown);
    }
}
