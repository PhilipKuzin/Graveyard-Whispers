using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSwap : MonoBehaviour
{
    public Sprite firstSprite;
    public Sprite secondSprite;

    private bool isToggle = false;
    [SerializeField]
    private Image buttonImage =>  GetComponent<Image>();

    void Start()
    {
        
        buttonImage.sprite = firstSprite;
    }

    public void ChangeSprite()
    {
        isToggle = !isToggle;
        buttonImage.sprite = isToggle ? secondSprite : firstSprite;
    }
}
