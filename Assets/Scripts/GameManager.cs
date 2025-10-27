using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public Vector3 deckPosition = new Vector3(-6, 0, 0);
    public float cardOffset = 0.1f;
    public Vector3 opponentArea = new Vector3(-2, 3, 0); // Higher up for opponent
    public Vector3 playerArea = new Vector3(-2, -3, 0); // Lower down for player
    public Vector3 playerBankArea = new Vector3(4, -2, 0);
    public Vector3 opponentBankArea = new Vector3(4, 2, 0);
    public float handSpacing = 0.4f; // Much closer spacing for dealt cards
    public float dealSpeed = 0.3f;
    public float dealDelay = 0.08f;
    public float arcHeight = 2f;
    
    private List<CardData> cardDatabase = new List<CardData>();
    private List<GameObject> cardPile = new List<GameObject>();
    private List<GameObject> opponentHand = new List<GameObject>();
    private List<GameObject> playerHand = new List<GameObject>();
    private List<GameObject> playerBank = new List<GameObject>();
    private List<GameObject> opponentBank = new List<GameObject>();
    private int playerBankValue = 0;
    private int opponentBankValue = 0;
    private GameObject playerBankTotal;
    private GameObject opponentBankTotal;
    private Sprite cardBackSprite;
    private bool cardsDealt = false;
    private CardActionScreen actionScreen;
    
    void Start()
    {
        deckPosition = new Vector3(-6, 0, 0);
        
        // Create action screen
        GameObject actionScreenObj = new GameObject("CardActionScreen");
        actionScreenObj.transform.SetParent(this.transform);
        actionScreen = actionScreenObj.AddComponent<CardActionScreen>();
        
        InitializeCardDatabase();
        LoadCardSprites();
        CreateCardBack();
        CreateCardPile();
        CreateBankTotalDisplays();
    }
    
    void InitializeCardDatabase()
    {
        // Initialize all cards with their quantities, bank values, and color groups based on XML
        cardDatabase.Add(new CardData("black_utility_rent.png", "Rent (Black Utilities)", 2, 1, "Utilities"));
        cardDatabase.Add(new CardData("bondstreet.png", "Bond Street", 1, 4, "Green"));
        cardDatabase.Add(new CardData("bowstreet.png", "Bow Street", 1, 2, "Orange"));
        cardDatabase.Add(new CardData("brown_light_blue_rent.png", "Rent (Brown & Light Blue)", 2, 1, "Brown / Light Blue"));
        cardDatabase.Add(new CardData("coventrystreet.png", "Coventry Street", 1, 3, "Yellow"));
        cardDatabase.Add(new CardData("dealbreaker.png", "Deal Breaker", 2, 5, "None"));
        cardDatabase.Add(new CardData("debt_collector.png", "Debt Collector", 3, 3, "None"));
        cardDatabase.Add(new CardData("double_the_rent.png", "Double The Rent", 2, 1, "None"));
        cardDatabase.Add(new CardData("electriccompany.png", "Electric Company", 1, 2, "Utility"));
        cardDatabase.Add(new CardData("eustonroad.png", "Euston Road", 1, 1, "Light Blue"));
        cardDatabase.Add(new CardData("fenchurchststation.png", "Fenchurch St. Station", 1, 2, "Station"));
        cardDatabase.Add(new CardData("fleetstreet.png", "Fleet Street", 1, 3, "Red"));
        cardDatabase.Add(new CardData("forced_deal.png", "Forced Deal", 3, 3, "None"));
        cardDatabase.Add(new CardData("green_dark_blue_rent.png", "Rent (Green & Dark Blue)", 2, 1, "Green / Dark Blue"));
        cardDatabase.Add(new CardData("green_darkblue_wild.png", "Green / Dark Blue Wild Card", 1, 2, "Green / Dark Blue"));
        cardDatabase.Add(new CardData("hotel.png", "Hotel", 3, 4, "None"));
        cardDatabase.Add(new CardData("house.png", "House", 3, 3, "None"));
        cardDatabase.Add(new CardData("its_my_birthday.png", "It's My Birthday", 3, 2, "None"));
        cardDatabase.Add(new CardData("just_say_no.png", "Just Say No", 3, 4, "None"));
        cardDatabase.Add(new CardData("kingscrossstation.png", "Kings Cross Station", 1, 2, "Station"));
        cardDatabase.Add(new CardData("leicestersquare.png", "Leicester Square", 1, 3, "Yellow"));
        cardDatabase.Add(new CardData("light_blue_brown_wild.png", "Light Blue / Brown Wild Card", 1, 2, "Light Blue / Brown"));
        cardDatabase.Add(new CardData("light_blue_pink_rent.png", "Rent (Light Blue & Pink)", 2, 1, "Light Blue / Pink"));
        cardDatabase.Add(new CardData("light_blue_pink_wild.png", "Light Blue / Pink Wild Card", 1, 2, "Light Blue / Pink"));
        cardDatabase.Add(new CardData("liverpoolststation.png", "Liverpool St. Station", 1, 2, "Station"));
        cardDatabase.Add(new CardData("marlboroughstreet.png", "Marlborough Street", 1, 2, "Orange"));
        cardDatabase.Add(new CardData("marylebonstation.png", "Marylebone Station", 1, 2, "Station"));
        cardDatabase.Add(new CardData("mayfair.png", "Mayfair", 1, 4, "Dark Blue"));
        cardDatabase.Add(new CardData("money_1m.png", "Money £1M", 6, 1, "None"));
        cardDatabase.Add(new CardData("money_2m.png", "Money £2M", 5, 2, "None"));
        cardDatabase.Add(new CardData("money_3m.png", "Money £3M", 3, 3, "None"));
        cardDatabase.Add(new CardData("money_4m.png", "Money £4M", 3, 4, "None"));
        cardDatabase.Add(new CardData("money_5m.png", "Money £5M", 2, 5, "None"));
        cardDatabase.Add(new CardData("money_10m.png", "Money £10M", 1, 10, "None"));
        cardDatabase.Add(new CardData("northumberlandavenue.png", "Northumberland Avenue", 1, 2, "Pink"));
        cardDatabase.Add(new CardData("oldkentroad.png", "Old Kent Road", 1, 1, "Brown"));
        cardDatabase.Add(new CardData("orange_red_rent.png", "Rent (Orange & Red)", 2, 1, "Orange / Red"));
        cardDatabase.Add(new CardData("orange_red_wild.png", "Orange / Red Wild Card", 1, 2, "Orange / Red"));
        cardDatabase.Add(new CardData("oxfordstreet.png", "Oxford Street", 1, 4, "Green"));
        cardDatabase.Add(new CardData("pallmall.png", "Pall Mall", 1, 2, "Pink"));
        cardDatabase.Add(new CardData("parklane.png", "Park Lane", 1, 4, "Dark Blue"));
        cardDatabase.Add(new CardData("pass_go.png", "Pass Go", 10, 1, "None"));
        cardDatabase.Add(new CardData("pentonvilleroad.png", "Pentonville Road", 1, 1, "Light Blue"));
        cardDatabase.Add(new CardData("piccadilly.png", "Piccadilly", 1, 3, "Yellow"));
        cardDatabase.Add(new CardData("pink_orange_rent.png", "Rent (Pink & Orange)", 2, 1, "Pink / Orange"));
        cardDatabase.Add(new CardData("regentstreet.png", "Regent Street", 1, 4, "Green"));
        cardDatabase.Add(new CardData("rent_all.png", "Rent (All Colours)", 3, 1, "All Colours"));
        cardDatabase.Add(new CardData("rent_yellow_rent.png", "Rent (Yellow & Red)", 2, 1, "Yellow / Red"));
        cardDatabase.Add(new CardData("sly_deal.png", "Sly Deal", 3, 3, "None"));
        cardDatabase.Add(new CardData("strand.png", "Strand", 1, 3, "Red"));
        cardDatabase.Add(new CardData("theangelislington.png", "The Angel Islington", 1, 1, "Light Blue"));
        cardDatabase.Add(new CardData("trafalgarsquare.png", "Trafalgar Square", 1, 3, "Red"));
        cardDatabase.Add(new CardData("vinestreet.png", "Vine Street", 1, 2, "Orange"));
        cardDatabase.Add(new CardData("waterworks.png", "Water Works", 1, 2, "Utility"));
        cardDatabase.Add(new CardData("whitechapelroad.png", "Whitechapel Road", 1, 1, "Brown"));
        cardDatabase.Add(new CardData("whitehall.png", "Whitehall", 1, 2, "Pink"));
        cardDatabase.Add(new CardData("wild_all_property.png", "Wild Card (All Properties)", 2, 0, "All Colours"));
        cardDatabase.Add(new CardData("yellow_green_wild.png", "Yellow / Green Wild Card", 1, 2, "Yellow / Green"));
        cardDatabase.Add(new CardData("yellow_red_wild.png", "Yellow / Red Wild Card", 1, 2, "Yellow / Red"));
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
                card.transform.localScale = Vector3.one * 0.25f; // Bigger deck cards
                
                SpriteRenderer spriteRenderer = card.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = cardBackSprite;
                spriteRenderer.sortingOrder = 3000 + cardIndex;
                
                // Add the Card component and store the actual card sprite and data
                Card cardComponent = card.AddComponent<Card>();
                cardComponent.SetSprite(cardData.sprite);
                cardComponent.SetCardBack(cardBackSprite);
                cardComponent.SetCardData(cardData);
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
            // Deal 5 cards to each player with animation
            StartCoroutine(DealHandsToPlayersAnimated());
            cardsDealt = true;
        }
    }
    
    IEnumerator DealHandsToPlayersAnimated()
    {
        // Deal 5 cards alternating between player and opponent
        for (int cardIndex = 0; cardIndex < 5; cardIndex++)
        {
            // Deal to player first
            if (cardPile.Count > 0)
            {
                StartCoroutine(DealSingleCard(true, cardIndex));
                yield return new WaitForSeconds(dealDelay);
            }
            
            // Then deal to opponent
            if (cardPile.Count > 0)
            {
                StartCoroutine(DealSingleCard(false, cardIndex));
                yield return new WaitForSeconds(dealDelay);
            }
        }
        
        Debug.Log($"Finished dealing. Deck has {cardPile.Count} cards remaining.");
    }
    
    IEnumerator DealSingleCard(bool toPlayer, int handIndex)
    {
        GameObject card = cardPile[cardPile.Count - 1];
        cardPile.RemoveAt(cardPile.Count - 1);
        
        Vector3 startPosition = card.transform.position;
        Vector3 targetPosition;
        
        if (toPlayer)
        {
            targetPosition = playerArea + new Vector3(handIndex * handSpacing, 0, 0);
            playerHand.Add(card);
        }
        else
        {
            targetPosition = opponentArea + new Vector3(handIndex * handSpacing, 0, 0);
            opponentHand.Add(card);
        }
        
        // Calculate control points for realistic card arc
        Vector3 midPoint = (startPosition + targetPosition) / 2f;
        midPoint.y += arcHeight; // Add height for the arc
        
        // Add some sideways curve for more realistic flight
        float sideOffset = toPlayer ? -1f : 1f;
        midPoint.x += sideOffset;
        
        // Animate the card with realistic motion
        float elapsedTime = 0;
        float startRotation = 0f;
        float targetRotation = UnityEngine.Random.Range(-15f, 15f); // Random spin
        
        while (elapsedTime < dealSpeed)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / dealSpeed;
            
            // Use quadratic bezier curve for smooth arc
            Vector3 pos1 = Vector3.Lerp(startPosition, midPoint, progress);
            Vector3 pos2 = Vector3.Lerp(midPoint, targetPosition, progress);
            card.transform.position = Vector3.Lerp(pos1, pos2, progress);
            
            // Add rotation during flight
            card.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(startRotation, targetRotation, progress));
            
            // Scale effect - slightly bigger during flight
            float scale = 1f + (Mathf.Sin(progress * Mathf.PI) * 0.1f);
            card.transform.localScale = Vector3.one * 0.18f * scale; // Bigger dealt cards
            
            yield return null;
        }
        
        // Ensure final position and rotation
        card.transform.position = targetPosition;
        card.transform.rotation = Quaternion.identity;
        card.transform.localScale = Vector3.one * 0.18f; // Bigger hand cards
        
        // Flip card face up and set sorting order
        Card cardComponent = card.GetComponent<Card>();
        cardComponent.SetFaceDown(false);
        card.GetComponent<SpriteRenderer>().sortingOrder = 2000 + handIndex;
        
        // Add a little "landing" effect
        StartCoroutine(CardLandingEffect(card));
    }
    
    IEnumerator CardLandingEffect(GameObject card)
    {
        // Quick scale bounce when card lands
        float bounceTime = 0.1f;
        float elapsedTime = 0;
        
        while (elapsedTime < bounceTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / bounceTime;
            float bounce = 1f + (Mathf.Sin(progress * Mathf.PI) * 0.05f);
            card.transform.localScale = Vector3.one * 0.18f * bounce; // Bigger bounce effect
            yield return null;
        }
        
        card.transform.localScale = Vector3.one * 0.18f; // Bigger final size
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
            
            // Check if action screen should handle the click
            if (actionScreen.IsActionScreenActive())
            {
                return; // Let action screen handle it
            }
            
            // Check if click is on a player card
            GameObject clickedCard = GetClickedPlayerCard(mouseWorldPos);
            if (clickedCard != null)
            {
                actionScreen.ShowActionScreen(clickedCard);
                return;
            }
            
            // Check if click is on the deck (simple distance check)
            if (Vector3.Distance(mouseWorldPos, deckPosition) < 0.7f) // Bigger click area for bigger deck
            {
                DealCards();
            }
        }
    }
    
    GameObject GetClickedPlayerCard(Vector3 mouseWorldPos)
    {
        foreach (GameObject card in playerHand)
        {
            float distance = Vector3.Distance(mouseWorldPos, card.transform.position);
            if (distance < 0.6f) // Bigger click area for bigger cards
            {
                return card;
            }
        }
        return null;
    }
    
    public void AddCardToPlayerBank(GameObject cardToBank, int bankValue)
    {
        Debug.Log($"AddCardToPlayerBank called - Card: {cardToBank.name}, Value: {bankValue}");
        Debug.Log($"Player hand contains {playerHand.Count} cards");
        Debug.Log($"Is card in player hand? {playerHand.Contains(cardToBank)}");
        
        // Remove card from player's hand
        if (playerHand.Contains(cardToBank))
        {
            Debug.Log($"Removing {cardToBank.name} from player hand");
            playerHand.Remove(cardToBank);
            
            // Add to player's bank
            playerBank.Add(cardToBank);
            int oldBankValue = playerBankValue;
            playerBankValue += bankValue;
            
            Debug.Log($"Added to bank - Old value: £{oldBankValue}M, Adding: £{bankValue}M, New total: £{playerBankValue}M, Bank count: {playerBank.Count}");
            
            // Position card in bank area (stack them slightly offset)
            Vector3 bankPosition = playerBankArea + new Vector3(playerBank.Count * 0.05f, 0, -playerBank.Count * 0.01f);
            cardToBank.transform.position = bankPosition;
            cardToBank.transform.localScale = Vector3.one * 0.08f; // Much smaller for bank
            
            // Set sorting order for bank cards
            SpriteRenderer spriteRenderer = cardToBank.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 1000 + playerBank.Count;
            
            // Reorganize remaining hand cards
            ReorganizePlayerHand();
            
            // Update bank total display
            UpdateBankTotalDisplays();
            
            Debug.Log($"Player bank now has £{playerBankValue}M ({playerBank.Count} cards)");
        }
        else
        {
            Debug.LogError($"Card {cardToBank.name} not found in player hand! Cannot bank it.");
            Debug.Log("Current player hand cards:");
            for (int i = 0; i < playerHand.Count; i++)
            {
                Debug.Log($"  Hand[{i}]: {playerHand[i].name}");
            }
        }
    }
    
    void ReorganizePlayerHand()
    {
        for (int i = 0; i < playerHand.Count; i++)
        {
            Vector3 handPosition = playerArea + new Vector3(i * handSpacing, 0, 0);
            playerHand[i].transform.position = handPosition;
            
            SpriteRenderer spriteRenderer = playerHand[i].GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 2000 + i;
        }
    }
    
    void CreateBankTotalDisplays()
    {
        // Create player bank total display
        playerBankTotal = new GameObject("PlayerBankTotal");
        playerBankTotal.transform.SetParent(this.transform);
        Vector3 playerBankTotalPosition = deckPosition + new Vector3(0, -1.6f, 0); // Adjusted for bigger deck
        playerBankTotal.transform.position = playerBankTotalPosition;
        
        TextMesh playerBankText = playerBankTotal.AddComponent<TextMesh>();
        playerBankText.text = "Bank: £0M";
        playerBankText.fontSize = 30;
        playerBankText.color = Color.yellow;
        playerBankText.anchor = TextAnchor.MiddleCenter;
        playerBankText.alignment = TextAlignment.Center;
        
        MeshRenderer playerBankRenderer = playerBankTotal.GetComponent<MeshRenderer>();
        playerBankRenderer.sortingOrder = 5000;
        
        playerBankTotal.transform.localScale = Vector3.one * 0.15f; // Made bigger
        
        // Create opponent bank total display
        opponentBankTotal = new GameObject("OpponentBankTotal");
        opponentBankTotal.transform.SetParent(this.transform);
        Vector3 opponentBankTotalPosition = deckPosition + new Vector3(0, 1.6f, 0); // Adjusted for bigger deck
        opponentBankTotal.transform.position = opponentBankTotalPosition;
        
        TextMesh opponentBankText = opponentBankTotal.AddComponent<TextMesh>();
        opponentBankText.text = "Bank: £0M";
        opponentBankText.fontSize = 30;
        opponentBankText.color = Color.yellow;
        opponentBankText.anchor = TextAnchor.MiddleCenter;
        opponentBankText.alignment = TextAlignment.Center;
        
        MeshRenderer opponentBankRenderer = opponentBankTotal.GetComponent<MeshRenderer>();
        opponentBankRenderer.sortingOrder = 5000;
        
        opponentBankTotal.transform.localScale = Vector3.one * 0.15f; // Made bigger
        
        Debug.Log($"Created bank total displays at player: {playerBankTotalPosition}, opponent: {opponentBankTotalPosition}");
    }
    
    void UpdateBankTotalDisplays()
    {
        Debug.Log($"Updating bank displays - Player: £{playerBankValue}M, Opponent: £{opponentBankValue}M");
        
        if (playerBankTotal != null)
        {
            TextMesh playerBankText = playerBankTotal.GetComponent<TextMesh>();
            playerBankText.text = $"Bank: £{playerBankValue}M";
            Debug.Log($"Updated player bank display to: {playerBankText.text}");
        }
        else
        {
            Debug.LogError("Player bank total display is null!");
        }
        
        if (opponentBankTotal != null)
        {
            TextMesh opponentBankText = opponentBankTotal.GetComponent<TextMesh>();
            opponentBankText.text = $"Bank: £{opponentBankValue}M";
            Debug.Log($"Updated opponent bank display to: {opponentBankText.text}");
        }
        else
        {
            Debug.LogError("Opponent bank total display is null!");
        }
    }
}