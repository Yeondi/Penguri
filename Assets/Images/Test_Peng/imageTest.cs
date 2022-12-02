using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class imageTest : MonoBehaviour
{
    [SerializeField]
    [Description("Peng's Portrait")]
    List<Sprite> Sprites;

    Button btn;

    int nData = 0;

    [SerializeField]
    Sprite CurrentSprite;

    Image SpriteSize;

    private void Start()
    {
        CurrentSprite = gameObject.GetComponent<Image>().sprite;
        btn = gameObject.GetComponent<Button>();
        SpriteSize = gameObject.GetComponent<Image>();
    }

    public void changeImage()
    {
        switch (nData)
        {
            case 1:
                gameObject.GetComponent<Image>().sprite = Sprites[0];
                CurrentSprite = Sprites[0];
                break;
            case 2:
                gameObject.GetComponent<Image>().sprite = Sprites[1];
                CurrentSprite = Sprites[1];
                break;
            case 3:
                gameObject.GetComponent<Image>().sprite = Sprites[2];
                CurrentSprite = Sprites[2];
                SpriteSize.SetNativeSize();
                break;
            case 4:
                gameObject.GetComponent<Image>().sprite = Sprites[3];
                CurrentSprite = Sprites[3];
                SpriteSize.SetNativeSize();
                break;
            case 5:
                gameObject.GetComponent<Image>().sprite = Sprites[4];
                CurrentSprite = Sprites[4];
                SpriteSize.SetNativeSize();
                break;
        }
    }

    public void AddData()
    {
        if (nData == 5)
            nData = 1;
        else
            nData++;

        changeImage();
    }

    private void Update()
    {
        Debug.Log("Current Size Width : " + CurrentSprite.rect.width + "\n Current Size Height : " + CurrentSprite.rect.height);
    }
}
