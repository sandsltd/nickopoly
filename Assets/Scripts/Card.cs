using UnityEngine;

public class Card : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite cardFrontSprite;
    private Sprite cardBackSprite;
    private bool isFaceDown = true;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
    }
    
    public void SetSprite(Sprite sprite)
    {
        cardFrontSprite = sprite;
        UpdateDisplay();
    }
    
    public void SetCardBack(Sprite backSprite)
    {
        cardBackSprite = backSprite;
        UpdateDisplay();
    }
    
    public void SetFaceDown(bool faceDown)
    {
        isFaceDown = faceDown;
        UpdateDisplay();
    }
    
    public void FlipCard()
    {
        isFaceDown = !isFaceDown;
        UpdateDisplay();
    }
    
    private void UpdateDisplay()
    {
        if (spriteRenderer != null)
        {
            if (isFaceDown && cardBackSprite != null)
            {
                spriteRenderer.sprite = cardBackSprite;
            }
            else if (!isFaceDown && cardFrontSprite != null)
            {
                spriteRenderer.sprite = cardFrontSprite;
            }
        }
    }
}