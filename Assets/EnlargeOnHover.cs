using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnlargeOnHover : MonoBehaviour
{
    // Store the sprite's original scale
    private Vector3 originalScale;

    void Start()
    {
        // Save the sprite's original scale
        originalScale = transform.localScale;
    }

    // This method is called when the mouse is hovering over the sprite
    void OnMouseEnter()
    {
        // Animate the sprite's scale to a larger size
        transform.DOScale(originalScale * 1.5f, 1.0f).SetEase(Ease.OutBounce);
    }

    // This method is called when the mouse is no longer hovering over the sprite
    void OnMouseExit()
    {
        // Animate the sprite's scale back to its original size
        transform.DOScale(originalScale, 1.0f).SetEase(Ease.OutBounce);;
    }
}
