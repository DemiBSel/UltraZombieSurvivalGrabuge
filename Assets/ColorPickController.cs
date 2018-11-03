using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickController : MonoBehaviour {

    public Color pickedColor;
    public Slider RSlider;
    public Slider GSlider;
    public Slider BSlider;

    public Image displayPanel;

    // Use this for initialization
    void Start () {
        pickedColor = new Color(0, 0, 0);
        pickedColor.a = 1;
        displayPanel = transform.Find("DisplayPanel").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        displayPanel.color = pickedColor;
	}

    public void updateRed()
    {
        pickedColor.r = RSlider.value;
    }
    public void updateGreen()
    {
        pickedColor.g = GSlider.value;
    }
    public void updataBlue()
    {
        pickedColor.b = BSlider.value;
    }
}
