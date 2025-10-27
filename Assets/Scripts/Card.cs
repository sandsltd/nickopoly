using UnityEngine;

public class Card : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite cardFrontSprite;
    private Sprite cardBackSprite;
    private bool isFaceDown = true;
    private CardData cardData;
    
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
    
    public void SetCardData(CardData data)
    {
        cardData = data;
    }
    
    public CardData GetCardData()
    {
        return cardData;
    }
    
    public int GetBankValue()
    {
        return cardData?.bankValue ?? 0;
    }
    
    public string GetColourGroup()
    {
        return cardData?.colourGroup ?? "None";
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