using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointerAction : MonoBehaviour
{
    public float minPos = -125;
    public float maxPos = 125;

    private Vector2 pointerPos = new Vector2(0, 0);
    private Vector2 cursorPos = new Vector2(0, 0);

    public GameObject pointer;
    public GameObject viewer;

    public bool cursorIsOn = false;
    public bool cursorIsPressed = false;

    public bool colorIsChanged = false;

    public double luminosite = 1.0f;
    
    public ColorPicker colorPicker;
    public Color currentColor;

    // Use this for initialization
    void Start ()
    {
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = transform.GetChild(i);

            if (childTransform.name == "Pointer")
                if (pointer == null)
                    pointer = childTransform.gameObject;

            if (childTransform.name == "Viewer")
                if (viewer == null)
                    viewer = childTransform.gameObject;
        }

        if (colorPicker == null)
        {
            colorPicker = transform.parent.GetComponent<ColorPicker>();
            Debug.LogWarning("ColorPicker non spécifié");
        }

        pointer.transform.localPosition = pointerPos;
        colorIsChanged = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Position du cuseur
        cursorPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        //Position minimale du curseur pour être sur la zone de couleur
        Vector2 minPosition = new Vector2(transform.position.x - maxPos, transform.position.y - maxPos);
        //Position maximale du curseur pour être sur la zone de couleur
        Vector2 maxPosition = new Vector2(transform.position.x + maxPos, transform.position.y + maxPos);

        //Si le curseur est sur la zone, on passe la variable cursorIsOn à true
        if (cursorPos.x >= minPosition.x && cursorPos.y >= minPosition.y && cursorPos.x <= maxPosition.x && cursorPos.y <= maxPosition.y)
            cursorIsOn = true;
        //Sinon à false
        else
            cursorIsOn = false;

        //Si le clique gauche est déclenché et que le curseur est sur la zone, on passe la variable cursorIsPressed à true
        if (Input.GetKeyDown(KeyCode.Mouse0) && cursorIsOn)
            cursorIsPressed = true;

        //Si le clique gauche est relâché, on passe la variable cursorIsPressed à false
        if (Input.GetKeyUp(KeyCode.Mouse0))
            cursorIsPressed = false;

        /*Si la variable cursorIsPressed est à true, le curseur est dans la zone et le clique gauche est déclenché
         * On peut donc changer la position du pointeur de couleur et afficher la couleur dans l'aperçu*/
        if (cursorIsPressed)
        {
            setPointerPosition();
            colorIsChanged = true;
        }

        if(colorIsChanged)
        {
            Color color = getColor();
            viewer.GetComponent<Image>().color = color;

            colorPicker.changeModelColor();
            colorIsChanged = false;

            //Debug.Log("Update - Color: " + currentColor + "; PointerPos: " + pointerPos);
        }
    }

    void setPointerPosition()
    {
        //On calcule la nouvelle position du pointeur à partir de la position du curseur et du positionnement du cadre
        pointerPos.x = Input.mousePosition.x - transform.position.x;
        pointerPos.y = Input.mousePosition.y - transform.position.y;

        pointerPos = verifyPointerPosition(pointerPos);

        //On applique la nouvelle position au pointeur
        pointer.transform.localPosition = pointerPos;
    }

    public void setPointerPosition(Vector2 pos)
    {
        pos = verifyPointerPosition(pos);

        pointerPos = pos;
        pointerPos = verifyPointerPosition(pointerPos);

        if(pointer != null)
        {
            //On applique la nouvelle position au pointeur
            pointer.transform.localPosition = pointerPos;
            colorIsChanged = true;
        }
    }

    Vector2 verifyPointerPosition(Vector2 pos)
    {
        //On empêche le pointeur de sortir du cadre en suivant le curseur (sécurité)
        if (pos.x > maxPos)
            pos.x = maxPos;

        if (pos.x < minPos)
            pos.x = minPos;

        if (pos.y > maxPos)
            pos.y = maxPos;

        if (pos.y < minPos)
            pos.y = minPos;

        return pos;
    }

    public Color getColor()
    {
        Color rgb = Color.black;

        //Algorithme de conversion TSL vers RGB: https://fr.wikipedia.org/wiki/Teinte_saturation_luminosit%C3%A9
        double teinte = pointerPos.x + maxPos;
        teinte = teinte * 360 / (maxPos * 2);

        if (teinte >= 360)
            teinte = 359.99f;

        double saturation = pointerPos.y + maxPos;
        saturation = saturation / (maxPos * 2);

        double chroma = luminosite * saturation;
        double teinteSeconde = teinte / 60;

        double x = chroma * (1 - Mathf.Abs((float)(teinteSeconde % 2 - 1)));

        Color rgbSeconde = new Color(0, 0, 0);

        if (teinteSeconde >= 0 && teinteSeconde < 1)
            rgbSeconde = new Color((float)chroma, (float)x, 0);

        if (teinteSeconde >= 1 && teinteSeconde < 2)
            rgbSeconde = new Color((float)x, (float)chroma, 0);

        if (teinteSeconde >= 2 && teinteSeconde < 3)
            rgbSeconde = new Color(0, (float)chroma, (float)x);

        if (teinteSeconde >= 3 && teinteSeconde < 4)
            rgbSeconde = new Color(0, (float)x, (float)chroma);

        if (teinteSeconde >= 4 && teinteSeconde < 5)
            rgbSeconde = new Color((float)x, 0, (float)chroma);

        if (teinteSeconde >= 5 && teinteSeconde < 6)
            rgbSeconde = new Color((float)chroma, 0, (float)x);

        double m = luminosite - chroma;
        rgb = new Color((float)(rgbSeconde.r + m), (float)(rgbSeconde.g + m), (float)(rgbSeconde.b + m));

        currentColor = rgb;

        return rgb;
    }

    public void setColor(Color color)
    {
        currentColor = color;
        //Debug.Log("SetColor - Color: " + color);

        double M = Mathf.Max(color.r, color.g, color.b);
        double m = Mathf.Min(color.r, color.g, color.b);

        double chroma = M - m;

        double teinteSeconde = -1;

        if (M == color.r)
            teinteSeconde = ((color.g - color.b) / chroma) % 6;
        else if (M == color.g)
            teinteSeconde = ((color.b - color.r) / chroma + 2) % 6;
        else if (M == color.b)
            teinteSeconde = ((color.r - color.g) / chroma + 4) % 6;

        if (teinteSeconde != -1)
        {
            double teinte = teinteSeconde * 60;
            if (teinte >= 360)
                teinte = 359.99f;

            luminosite = M;

            double saturation = chroma / M;

            if (M == 0)
                saturation = 0;

            float pointerPosX = (float)(teinte * (maxPos * 2) / 360 - maxPos);
            float pointerPosY = (float)(saturation * (maxPos * 2) - maxPos);

            if (pointerPosX > maxPos)
                pointerPosX -= maxPos * 2;

            if (pointerPosX < minPos)
                pointerPosX += maxPos * 2;

            if (pointerPosY > maxPos)
                pointerPosY -= maxPos * 2;

            if (pointerPosY < minPos)
                pointerPosY += maxPos * 2;

            //Debug.Log("SetColor - PointerPos: (" + pointerPosX + ", " + pointerPosY + ")");

            setPointerPosition(new Vector2(pointerPosX, pointerPosY));
        }
        else
            Debug.LogError("Teinte' = -1");
    }

    public double getLuminosite()
    {
        return luminosite;
    }

    public void setLuminosite(float luminosite)
    {
        this.luminosite = luminosite;
        Color color = new Color(luminosite, luminosite, luminosite);

        RawImage rawImage = GetComponent<RawImage>();
        rawImage.color = color;

        colorIsChanged = true;
    }

    public void setLimitValues(float min, float max)
    {
        minPos = min;
        maxPos = max;
    }
}
