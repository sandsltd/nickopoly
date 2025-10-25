using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public Vector3 deckPosition = new Vector3(-6, 0, 0);
    public float cardOffset = 0.1f;
    public Vector3 opponentArea = new Vector3(-2, 2, 0);
    public Vector3 playerArea = new Vector3(-2, -2, 0);
    public float handSpacing = 1.5f;
    
    private List<CardData> cardDatabase = new List<CardData>();
    private List<GameObject> cardPile = new List<GameObject>();
    private List<GameObject> opponentHand = new List<GameObject>();
    private List<GameObject> playerHand = new List<GameObject>();
    private Sprite cardBackSprite;
    private bool cardsDealt = false;
    
    void Start()
    {
        deckPosition = new Vector3(-6, 0, 0);
        InitializeCardDatabase();
        LoadCardSprites();
        CreateCardBack();
        CreateCardPile();
    }
    
    void InitializeCardDatabase()
    {
        // Initialize all cards with their quantities based on XML
        cardDatabase.Add(new CardData("black_utility_rent.png", "Rent (Black Utilities)", 2));
        cardDatabase.Add(new CardData("bondstreet.png", "Bond Street", 1));
        cardDatabase.Add(new CardData("bowstreet.png", "Bow Street", 1));
        cardDatabase.Add(new CardData("brown_light_blue_rent.png", "Rent (Brown & Light Blue)", 2));
        cardDatabase.Add(new CardData("coventrystreet.png", "Coventry Street", 1));
        cardDatabase.Add(new CardData("dark_blue_orange_rent.png", "Rent (Dark Blue & Orange)", 1));
        cardDatabase.Add(new CardData("dealbreaker.png", "Deal Breaker", 2));
        cardDatabase.Add(new CardData("debt_collector.png", "Debt Collector", 3));
        cardDatabase.Add(new CardData("double_the_rent.png", "Double The Rent", 2));
        cardDatabase.Add(new CardData("electriccompany.png", "Electric Company", 1));
        cardDatabase.Add(new CardData("eustonroad.png", "Euston Road", 1));
        cardDatabase.Add(new CardData("fenchirchststation.png", "Fenchurch St. Station", 1));
        cardDatabase.Add(new CardData("fleetstreet.png", "Fleet Street", 1));
        cardDatabase.Add(new CardData("forced_deal.png", "Forced Deal", 3));
        cardDatabase.Add(new CardData("green_dark_blue_rent.png", "Rent (Green & Dark Blue)", 2));
        cardDatabase.Add(new CardData("green_darkblue_wild.png", "Green / Dark Blue Wild Card", 1));
        cardDatabase.Add(new CardData("hotel.png", "Hotel", 3));
        cardDatabase.Add(new CardData("house.png", "House", 3));
        cardDatabase.Add(new CardData("its_my_birthday.png", "It's My Birthday", 3));
        cardDatabase.Add(new CardData("just_say_no.png", "Just Say No", 3));
        cardDatabase.Add(new CardData("kingscrossstation.png", "Kings Cross Station", 1));
        cardDatabase.Add(new CardData("leicestersquare.png", "Leicester Square", 1));
        cardDatabase.Add(new CardData("light_blue_brown_wild.png", "Light Blue / Brown Wild Card", 1));
        cardDatabase.Add(new CardData("light_blue_pink_rent.png", "Rent (Light Blue & Pink)", 2));
        cardDatabase.Add(new CardData("light_blue_pink_wild.png", "Light Blue / Pink Wild Card", 1));
        cardDatabase.Add(new CardData("liverpoolststation.png", "Liverpool St. Station", 1));
        cardDatabase.Add(new CardData("marlboroughstreet.png", "Marlborough Street", 1));
        cardDatabase.Add(new CardData("marylebonstation.png", "Marylebone Station", 1));
        cardDatabase.Add(new CardData("mayfair.png", "Mayfair", 1));
        cardDatabase.Add(new CardData("money_1m.png", "Money £1M", 6));
        cardDatabase.Add(new CardData("money_2m.png", "Money £2M", 5));
        cardDatabase.Add(new CardData("money_3m.png", "Money £3M", 3));
        cardDatabase.Add(new CardData("money_4m.png", "Money £4M", 3));
        cardDatabase.Add(new CardData("money_5m.png", "Money £5M", 2));
        cardDatabase.Add(new CardData("money_10m.png", "Money £10M", 1));
        cardDatabase.Add(new CardData("northumberlandavenue.png", "Northumberland Avenue", 1));
        cardDatabase.Add(new CardData("oldkentroad.png", "Old Kent Road", 1));
        cardDatabase.Add(new CardData("orange_red_rent.png", "Rent (Orange & Red)", 2));
        cardDatabase.Add(new CardData("orange_red_wild.png", "Orange / Red Wild Card", 1));
        cardDatabase.Add(new CardData("oxfordstreet.png", "Oxford Street", 1));
        cardDatabase.Add(new CardData("pallmall.png", "Pall Mall", 1));
        cardDatabase.Add(new CardData("parklane.png", "Park Lane", 1));
        cardDatabase.Add(new CardData("pass_go.png", "Pass Go", 11));
        cardDatabase.Add(new CardData("pentonvilleroad.png", "Pentonville Road", 1));
        cardDatabase.Add(new CardData("piccadilly.png", "Piccadilly", 1));
        cardDatabase.Add(new CardData("pink_orange_rent.png", "Rent (Pink & Orange)", 2));
        cardDatabase.Add(new CardData("regentstreet.png", "Regent Street", 1));
        cardDatabase.Add(new CardData("rent_all.png", "Rent (All Colours)", 3));
        cardDatabase.Add(new CardData("rent_yellow_rent.png", "Rent (Yellow & Red)", 2));
        cardDatabase.Add(new CardData("sly_deal.png", "Sly Deal", 3));
        cardDatabase.Add(new CardData("strand.png", "Strand", 1));
        cardDatabase.Add(new CardData("theangelislington.png", "The Angel Islington", 1));
        cardDatabase.Add(new CardData("trafalgarsquare.png", "Trafalgar Square", 1));
        cardDatabase.Add(new CardData("vinestreet.png", "Vine Street", 1));
        cardDatabase.Add(new CardData("waterworks.png", "Water Works", 1));
        cardDatabase.Add(new CardData("whitechapelroad.png", "Whitechapel Road", 1));
        cardDatabase.Add(new CardData("whitehall.png", "Whitehall", 1));
        cardDatabase.Add(new CardData("wild_all_property.png", "Wild Card (All Properties)", 2));
        cardDatabase.Add(new CardData("yellow_green_wild.png", "Yellow / Green Wild Card", 1));
        cardDatabase.Add(new CardData("yellow_red_wild.png", "Yellow / Red Wild Card", 1));
    }
    
    void LoadCardSprites()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Cards");
        
        Debug.Log($"Available sprites: {string.Join(", ", System.Array.ConvertAll(sprites, s => s.name))}");
        
        // Match sprites to card data
        foreach (var cardData in cardDatabase)
        {
            string expectedName = cardData.filename.Replace(".png", "") + "_0";
            foreach (var sprite in sprites)
            {
                if (sprite.name == expectedName)
                {
                    cardData.sprite = sprite;
                    break;
                }
            }
            
            if (cardData.sprite == null)
            {
                Debug.LogWarning($"No sprite found for {cardData.filename} (looking for '{expectedName}')");
            }
        }
        
        Debug.Log($"Loaded sprites for {cardDatabase.Count} card types");
    }
    
    void CreateCardBack()
    {
        cardBackSprite = Resources.Load<Sprite>("card_back");
        if (cardBackSprite == null)
        {
            Debug.LogError("Could not load card_back.png from Resources folder");
            // Fallback to programmatic card back
            cardBackSprite = CardBack.CreateCardBackSprite();
        }
        else
        {
            Debug.Log("Loaded card_back.png successfully");
        }
    }
    
    void CreateCardPile()
    {
        int cardIndex = 0;
        
        // Create multiple copies of each card based on quantity
        foreach (var cardData in cardDatabase)
        {
            if (cardData.sprite == null)
            {
                Debug.LogWarning($"No sprite found for {cardData.filename}");
                continue;
            }
            
            for (int copy = 0; copy < cardData.quantity; copy++)
            {
                GameObject card = new GameObject($"{cardData.cardName}_{copy + 1}");
                card.transform.SetParent(this.transform);
                
                // Position cards in a pile with slight offset
                Vector3 position = deckPosition + new Vector3(0, 0, -cardIndex * cardOffset);
                card.transform.position = position;
                card.transform.localScale = Vector3.one * 0.35f;
                
                SpriteRenderer spriteRenderer = card.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = cardBackSprite;
                spriteRenderer.sortingOrder = 3000 + cardIndex;
                
                // Add the Card component and store the actual card sprite
                Card cardComponent = card.AddComponent<Card>();
                cardComponent.SetSprite(cardData.sprite);
                cardComponent.SetCardBack(cardBackSprite);
                cardComponent.SetFaceDown(true);
                
                // Add collider for clicking
                BoxCollider2D collider = card.AddComponent<BoxCollider2D>();
                collider.size = new Vector2(1, 1.4f);
                
                cardPile.Add(card);
                cardIndex++;
            }
        }
        
        // Shuffle the deck
        ShuffleDeck();
        
        Debug.Log($"Created deck with {cardPile.Count} total cards");
    }
    
    void ShuffleDeck()
    {
        for (int i = 0; i < cardPile.Count; i++)
        {
            int randomIndex = Random.Range(i, cardPile.Count);
            GameObject temp = cardPile[i];
            cardPile[i] = cardPile[randomIndex];
            cardPile[randomIndex] = temp;
            
            // Update positions and sorting orders after shuffle
            Vector3 position = deckPosition + new Vector3(0, 0, -i * cardOffset);
            cardPile[i].transform.position = position;
            cardPile[i].GetComponent<SpriteRenderer>().sortingOrder = 3000 + i;
        }
    }
    
    public void DealCards()
    {
        if (cardsDealt)
        {
            // Return all cards to deck and reshuffle
            ReturnCardsToDeck();
            ShuffleDeck();
            cardsDealt = false;
        }
        else
        {
            // Deal 5 cards to each player
            DealHandsToPlayers();
            cardsDealt = true;
        }
    }
    
    void DealHandsToPlayers()
    {
        // Deal 5 cards to opponent (top of screen)
        for (int i = 0; i < 5 && cardPile.Count > 0; i++)
        {
            GameObject card = cardPile[cardPile.Count - 1];
            cardPile.RemoveAt(cardPile.Count - 1);
            
            Card cardComponent = card.GetComponent<Card>();
            cardComponent.SetFaceDown(false);
            
            Vector3 cardPosition = opponentArea + new Vector3(i * handSpacing, 0, 0);
            card.transform.position = cardPosition;
            card.GetComponent<SpriteRenderer>().sortingOrder = 2000 + i;
            
            opponentHand.Add(card);
        }
        
        // Deal 5 cards to player (bottom of screen)
        for (int i = 0; i < 5 && cardPile.Count > 0; i++)
        {
            GameObject card = cardPile[cardPile.Count - 1];
            cardPile.RemoveAt(cardPile.Count - 1);
            
            Card cardComponent = card.GetComponent<Card>();
            cardComponent.SetFaceDown(false);
            
            Vector3 cardPosition = playerArea + new Vector3(i * handSpacing, 0, 0);
            card.transform.position = cardPosition;
            card.GetComponent<SpriteRenderer>().sortingOrder = 2000 + i;
            
            playerHand.Add(card);
        }
        
        Debug.Log($"Dealt 5 cards to each player. Deck has {cardPile.Count} cards remaining.");
    }
    
    void ReturnCardsToDeck()
    {
        // Return opponent cards
        foreach (GameObject card in opponentHand)
        {
            Card cardComponent = card.GetComponent<Card>();
            cardComponent.SetFaceDown(true);
            cardPile.Add(card);
        }
        opponentHand.Clear();
        
        // Return player cards
        foreach (GameObject card in playerHand)
        {
            Card cardComponent = card.GetComponent<Card>();
            cardComponent.SetFaceDown(true);
            cardPile.Add(card);
        }
        playerHand.Clear();
        
        // Reposition all cards back to deck
        for (int i = 0; i < cardPile.Count; i++)
        {
            Vector3 position = deckPosition + new Vector3(0, 0, -i * cardOffset);
            cardPile[i].transform.position = position;
            cardPile[i].GetComponent<SpriteRenderer>().sortingOrder = 3000 + i;
        }
        
        Debug.Log($"Returned all cards to deck. Deck has {cardPile.Count} cards.");
    }
    
    void Update()
    {
        // Simple click detection using new Input System
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mouseWorldPos.z = 0;
            
            // Check if click is on the deck (simple distance check)
            if (Vector3.Distance(mouseWorldPos, deckPosition) < 1.0f)
            {
                DealCards();
            }
        }
    }
}