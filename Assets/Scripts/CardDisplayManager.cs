using System.Collections.Generic;
using UnityEngine;

public class CardDisplayManager : MonoBehaviour
{
    [Header("Card Display Settings")]
    public int cardsPerRow = 8;
    public float cardSpacing = 2.0f;
    public float rowSpacing = 2.5f;
    
    private List<Sprite> cardSprites = new List<Sprite>();
    
    void Start()
    {
        LoadCardSprites();
        DisplayAllCards();
    }
    
    void LoadCardSprites()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Cards");
        cardSprites.AddRange(sprites);
        Debug.Log($"Loaded {cardSprites.Count} card sprites");
    }
    
    void DisplayAllCards()
    {
        for (int i = 0; i < cardSprites.Count; i++)
        {
            CreateCard(cardSprites[i], i);
        }
    }
    
    void CreateCard(Sprite cardSprite, int index)
    {
        GameObject card = new GameObject($"Card_{cardSprite.name}");
        card.transform.SetParent(this.transform);
        
        int row = index / cardsPerRow;
        int col = index % cardsPerRow;
        
        float xPos = col * cardSpacing - (cardsPerRow * cardSpacing) / 2f;
        float yPos = -row * rowSpacing + 5f;
        
        card.transform.localPosition = new Vector3(xPos, yPos, 0);
        
        SpriteRenderer spriteRenderer = card.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = cardSprite;
        spriteRenderer.sortingOrder = 1;
        
        // Scale the cards to be much smaller
        card.transform.localScale = Vector3.one * 0.2f;
    }
}