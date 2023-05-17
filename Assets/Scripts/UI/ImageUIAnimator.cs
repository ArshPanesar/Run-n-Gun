using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageUIAnimator : MonoBehaviour
{
    public Image image;
    public SpriteRenderer spriteRenderer;
    
    void Update()
    {
        image.sprite = spriteRenderer.sprite;
    }
}
