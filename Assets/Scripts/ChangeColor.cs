using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    Color color = new Color(0.8313726f, 0.8509805f, 0.7137255f);
    Toggle myToggle;
    void Start()
    {
        myToggle = gameObject.GetComponent<Toggle>();
    }

    private void Awake()
    {
        myToggle = gameObject.GetComponent<Toggle>();
    }

    public void changeColor()
    {
        if(myToggle.isOn)
            gameObject.GetComponent<Image>().color = color;
        else
            gameObject.GetComponent<Image>().color = Color.white;

    }
}
