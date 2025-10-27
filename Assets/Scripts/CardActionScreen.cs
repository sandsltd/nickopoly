using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CardActionScreen : MonoBehaviour
{
    [Header("Action Screen Settings")]
    public Vector3 actionScreenPosition = new Vector3(2, 0, 0);
    public float actionCardScale = 0.5f; // Smaller action card display
    
    private GameObject currentActionCard;
    private GameObject actionCardDisplay;
    private GameObject closeButton;
    private GameObject buttonBackground;
    private List<GameObject> actionButtons = new List<GameObject>();
    private Card originalCard;
    private bool isActionScreenActive = false;
    private float actionScreenOpenTime;
    
    public void ShowActionScreen(GameObject selectedCard)
    {
        if (isActionScreenActive)
        {
            HideActionScreen();
        }
        
        originalCard = selectedCard.GetComponent<Card>();
        
        // Create a copy of the card for the action screen
        actionCardDisplay = new GameObject("ActionCardDisplay");
        actionCardDisplay.transform.position = actionScreenPosition;
        actionCardDisplay.transform.localScale = Vector3.one * actionCardScale;
        
        // Copy the sprite renderer
        SpriteRenderer originalRenderer = selectedCard.GetComponent<SpriteRenderer>();
        SpriteRenderer actionRenderer = actionCardDisplay.AddComponent<SpriteRenderer>();
        actionRenderer.sprite = originalRenderer.sprite;
        actionRenderer.sortingOrder = 4000; // On top of everything
        
        // Create white background box for buttons
        CreateButtonBackground();
        
        // Create close button
        CreateCloseButton();
        
        // Create action buttons based on card type
        CreateActionButtons(selectedCard);
        
        currentActionCard = selectedCard;
        isActionScreenActive = true;
        actionScreenOpenTime = Time.time;
        
        Debug.Log($"Showing action screen for card: {selectedCard.name} at position: {actionScreenPosition}");
        Debug.Log($"Action card created with sprite: {actionRenderer.sprite.name}");
    }
    
    public void HideActionScreen()
    {
        Debug.Log("HideActionScreen called - closing action screen");
        
        if (actionCardDisplay != null)
        {
            Destroy(actionCardDisplay);
            actionCardDisplay = null;
        }
        
        if (closeButton != null)
        {
            Destroy(closeButton);
            closeButton = null;
        }
        
        if (buttonBackground != null)
        {
            Destroy(buttonBackground);
            buttonBackground = null;
        }
        
        // Destroy all action buttons
        foreach (GameObject button in actionButtons)
        {
            if (button != null)
            {
                Destroy(button);
            }
        }
        actionButtons.Clear();
        
        currentActionCard = null;
        originalCard = null;
        isActionScreenActive = false;
        
        Debug.Log("Action screen hidden");
    }
    
    public bool IsActionScreenActive()
    {
        return isActionScreenActive;
    }
    
    public GameObject GetCurrentActionCard()
    {
        return currentActionCard;
    }
    
    void CreateButtonBackground()
    {
        buttonBackground = new GameObject("ButtonBackground");
        Vector3 backgroundPosition = actionScreenPosition + new Vector3(-4.5f, 0, 0);
        buttonBackground.transform.position = backgroundPosition;
        
        // Create white background box
        SpriteRenderer backgroundRenderer = buttonBackground.AddComponent<SpriteRenderer>();
        backgroundRenderer.sprite = CreateBackgroundSprite();
        backgroundRenderer.sortingOrder = 3999; // Behind buttons but above card
        
        buttonBackground.transform.localScale = Vector3.one * 2.5f;
        
        Debug.Log($"Button background created at position: {backgroundPosition}");
    }
    
    void CreateCloseButton()
    {
        closeButton = new GameObject("CloseButton");
        Vector3 buttonPosition = actionScreenPosition + new Vector3(-4.5f, 2.5f, 0); // Move close button higher up
        closeButton.transform.position = buttonPosition;
        
        // Create background sprite for close button
        SpriteRenderer buttonBackgroundSprite = closeButton.AddComponent<SpriteRenderer>();
        buttonBackgroundSprite.sprite = CreateButtonSprite(new Color(0.6f, 0.6f, 0.6f, 1f)); // Gray
        buttonBackgroundSprite.sortingOrder = 4001;
        
        // Create text object as child
        GameObject textObj = new GameObject("ButtonText");
        textObj.transform.SetParent(closeButton.transform);
        textObj.transform.localPosition = Vector3.zero;
        
        TextMesh textMesh = textObj.AddComponent<TextMesh>();
        textMesh.text = "CLOSE";
        textMesh.fontSize = 40;
        textMesh.color = Color.white;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        
        // Set text sorting order above background
        MeshRenderer textRenderer = textObj.GetComponent<MeshRenderer>();
        textRenderer.sortingOrder = 4002;
        
        textObj.transform.localScale = Vector3.one * 0.015f;
        closeButton.transform.localScale = Vector3.one * 1.6f; // Bigger close button
        
        Debug.Log($"Close button created at position: {buttonPosition}");
    }
    
    CardType GetCardType(GameObject card)
    {
        string cardName = card.name.ToLower();
        Debug.Log($"Checking card type for: '{cardName}'");
        
        // Get the actual card data to determine type more accurately
        Card cardComponent = card.GetComponent<Card>();
        if (cardComponent != null && cardComponent.GetCardData() != null)
        {
            string filename = cardComponent.GetCardData().filename.ToLower();
            Debug.Log($"Using filename for detection: '{filename}'");
            
            // Check for money cards
            if (filename.Contains("money") || filename.Contains("pass_go"))
            {
                Debug.Log($"Detected as Money card via filename: {filename}");
                return CardType.Money;
            }
            
            // Check for action cards - using filename which is more reliable
            if (filename.Contains("dealbreaker") || filename.Contains("debt_collector") || 
                filename.Contains("forced_deal") || filename.Contains("sly_deal") || 
                filename.Contains("just_say_no") || filename.Contains("double_the_rent") || 
                filename.Contains("its_my_birthday") || filename.Contains("hotel") || 
                filename.Contains("house") || filename.Contains("rent_all") ||
                filename.Contains("_rent.png")) // All rent cards
            {
                Debug.Log($"Detected as Action card via filename: {filename}");
                return CardType.Action;
            }
        }
        
        // Fallback to card name if no card data
        // Check for money cards
        if (cardName.Contains("money") || cardName.Contains("pass_go"))
        {
            Debug.Log($"Detected as Money card via cardName: {cardName}");
            return CardType.Money;
        }
        
        // Check for action cards
        if (cardName.Contains("dealbreaker") || cardName.Contains("debt_collector") || 
            cardName.Contains("forced_deal") || cardName.Contains("sly_deal") || 
            cardName.Contains("rent") || cardName.Contains("just_say_no") || 
            cardName.Contains("double_the_rent") || cardName.Contains("birthday") ||
            cardName.Contains("hotel") || cardName.Contains("house"))
        {
            Debug.Log($"Detected as Action card via cardName: {cardName}");
            return CardType.Action;
        }
        
        // Everything else is property (including wild cards)
        Debug.Log($"Detected as Property card: {cardName}");
        return CardType.Property;
    }
    
    void CreateActionButtons(GameObject selectedCard)
    {
        CardType cardType = GetCardType(selectedCard);
        
        // Create buttons based on card type (no cancel button)
        switch (cardType)
        {
            case CardType.Money:
                CreateActionButton(CardAction.Bank, 0); // Slot 0
                CreateEmptySlot(1); // Empty slot 1
                CreateEmptySlot(2); // Empty slot 2
                break;
            case CardType.Property:
                CreateActionButton(CardAction.Place, 0); // Slot 0
                CreateEmptySlot(1); // Empty slot 1
                CreateEmptySlot(2); // Empty slot 2
                break;
            case CardType.Action:
                CreateActionButton(CardAction.Play, 0); // Slot 0
                CreateActionButton(CardAction.Bank, 1); // Slot 1
                CreateEmptySlot(2); // Empty slot 2
                break;
        }
        
        Debug.Log($"Created action buttons for {cardType} card with consistent layout (no cancel)");
        Debug.Log($"Total buttons in actionButtons list: {actionButtons.Count}");
        for (int i = 0; i < actionButtons.Count; i++)
        {
            if (actionButtons[i] != null)
            {
                Debug.Log($"  Button[{i}]: {actionButtons[i].name} at {actionButtons[i].transform.position}");
            }
        }
    }
    
    void CreateActionButton(CardAction action, int index)
    {
        GameObject button = new GameObject($"ActionButton_{action}");
        // Position buttons to the left of the card, vertically stacked
        Vector3 buttonPosition = actionScreenPosition + new Vector3(-4.5f, 1.2f - (index * 1.0f), 0); // More spacing between buttons
        button.transform.position = buttonPosition;
        
        // Create background sprite for button
        SpriteRenderer buttonBackgroundSprite = button.AddComponent<SpriteRenderer>();
        buttonBackgroundSprite.sprite = CreateButtonSprite(GetButtonColor(action));
        buttonBackgroundSprite.sortingOrder = 4001;
        
        // Create text object as child
        GameObject textObj = new GameObject("ButtonText");
        textObj.transform.SetParent(button.transform);
        textObj.transform.localPosition = Vector3.zero;
        
        TextMesh textMesh = textObj.AddComponent<TextMesh>();
        textMesh.text = action.ToString().ToUpper();
        textMesh.fontSize = 40;
        textMesh.color = Color.white;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        
        // Set text sorting order above background
        MeshRenderer textRenderer = textObj.GetComponent<MeshRenderer>();
        textRenderer.sortingOrder = 4002;
        
        textObj.transform.localScale = Vector3.one * 0.015f;
        button.transform.localScale = Vector3.one * 1.8f; // Much bigger buttons
        
        // Store button reference
        actionButtons.Add(button);
        
        Debug.Log($"Created {action} button at position: {buttonPosition}");
    }
    
    void CreateEmptySlot(int index)
    {
        // Create invisible placeholder to maintain consistent spacing
        GameObject emptySlot = new GameObject($"EmptySlot_{index}");
        Vector3 slotPosition = actionScreenPosition + new Vector3(-4.5f, 1.2f - (index * 1.0f), 0); // Match button spacing
        emptySlot.transform.position = slotPosition;
        
        // Store empty slot reference for cleanup
        actionButtons.Add(emptySlot);
    }
    
    Sprite CreateButtonSprite(Color color)
    {
        // Create a simple rounded rectangle sprite for buttons
        Texture2D texture = new Texture2D(200, 60);
        Color[] pixels = new Color[texture.width * texture.height];
        
        // Fill with rounded rectangle shape
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                float centerX = texture.width / 2f;
                float centerY = texture.height / 2f;
                float radiusX = texture.width / 2f - 8f;
                float radiusY = texture.height / 2f - 8f;
                
                float distX = Mathf.Abs(x - centerX);
                float distY = Mathf.Abs(y - centerY);
                
                bool isInside = false;
                
                if (distX <= radiusX && distY <= radiusY)
                {
                    isInside = true;
                }
                else if (distX <= radiusX)
                {
                    isInside = (distY - radiusY) <= 8f;
                }
                else if (distY <= radiusY)
                {
                    isInside = (distX - radiusX) <= 8f;
                }
                else
                {
                    float cornerDist = Mathf.Sqrt((distX - radiusX) * (distX - radiusX) + (distY - radiusY) * (distY - radiusY));
                    isInside = cornerDist <= 8f;
                }
                
                if (isInside)
                {
                    pixels[y * texture.width + x] = color;
                }
                else
                {
                    pixels[y * texture.width + x] = Color.clear;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
    
    Sprite CreateBackgroundSprite()
    {
        // Create a white background rectangle for the button area
        Texture2D texture = new Texture2D(120, 200);
        Color[] pixels = new Color[texture.width * texture.height];
        
        // Fill with white color and add a subtle border
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                // Create a border effect
                if (x < 3 || x >= texture.width - 3 || y < 3 || y >= texture.height - 3)
                {
                    pixels[y * texture.width + x] = new Color(0.8f, 0.8f, 0.8f, 1f); // Light gray border
                }
                else
                {
                    pixels[y * texture.width + x] = Color.white; // White background
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
    
    Color GetButtonColor(CardAction action)
    {
        switch (action)
        {
            case CardAction.Cancel:
                return new Color(0.8f, 0.2f, 0.2f, 1f); // Dark red
            case CardAction.Bank:
                return new Color(0.2f, 0.7f, 0.2f, 1f); // Dark green
            case CardAction.Place:
                return new Color(0.2f, 0.4f, 0.8f, 1f); // Dark blue
            case CardAction.Play:
                return new Color(0.9f, 0.7f, 0.1f, 1f); // Dark yellow/gold
            default:
                return new Color(0.5f, 0.5f, 0.5f, 1f); // Gray
        }
    }
    
    void Update()
    {
        // Close action screen when clicking elsewhere (but not immediately after opening)
        if (isActionScreenActive && Mouse.current.leftButton.wasPressedThisFrame && Time.time - actionScreenOpenTime > 0.1f)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mouseWorldPos.z = 0;
            
            // Check if clicked on close button
            if (closeButton != null)
            {
                float distanceToCloseButton = Vector3.Distance(mouseWorldPos, closeButton.transform.position);
                Debug.Log($"Click distance to close button: {distanceToCloseButton}, threshold: 1.0f");
                if (distanceToCloseButton < 1.0f) // Bigger close button click area
                {
                    Debug.Log("Closing action screen - close button clicked");
                    HideActionScreen();
                    return;
                }
            }
            
            // Check if clicked on any action button
            foreach (GameObject button in actionButtons)
            {
                if (button != null && button.name.StartsWith("ActionButton_"))
                {
                    float distanceToButton = Vector3.Distance(mouseWorldPos, button.transform.position);
                    // Debug.Log($"Checking button {button.name} at {button.transform.position}, distance: {distanceToButton}");
                    if (distanceToButton < 1.4f) // Larger click area for bigger buttons
                    {
                        Debug.Log($"Button {button.name} clicked!");
                        HandleActionButtonClick(button.name);
                        return;
                    }
                }
            }
            
            // Check if click is outside the action card area
            float distanceToActionCard = Vector3.Distance(mouseWorldPos, actionScreenPosition);
            Debug.Log($"Click distance to action card: {distanceToActionCard}, threshold: 1.5f");
            if (distanceToActionCard > 1.5f) // Outside the action card area
            {
                Debug.Log("Closing action screen - click too far from action card");
                HideActionScreen();
            }
        }
    }
    
    void HandleActionButtonClick(string buttonName)
    {
        string actionName = buttonName.Replace("ActionButton_", "");
        Debug.Log($"Clicked action button: {actionName} for card: {currentActionCard.name}");
        
        switch (actionName)
        {
            case "Bank":
                BankCard(currentActionCard);
                HideActionScreen();
                break;
            case "Place":
                Debug.Log($"Placing property card: {currentActionCard.name}");
                HideActionScreen();
                break;
            case "Play":
                Debug.Log($"Playing action card: {currentActionCard.name}");
                HideActionScreen();
                break;
        }
    }
    
    void BankCard(GameObject cardToBankObj)
    {
        Debug.Log($"BankCard called for GameObject: {cardToBankObj.name}");
        
        Card cardToBankComponent = cardToBankObj.GetComponent<Card>();
        if (cardToBankComponent == null)
        {
            Debug.LogError($"No Card component found on {cardToBankObj.name}");
            return;
        }
        
        CardData cardData = cardToBankComponent.GetCardData();
        if (cardData == null)
        {
            Debug.LogError($"No CardData found on {cardToBankObj.name}");
            return;
        }
        
        int bankValue = cardToBankComponent.GetBankValue();
        string cardName = cardData.cardName;
        
        Debug.Log($"Banking card: {cardName}, Bank Value: {bankValue}, Filename: {cardData.filename}");
        
        // Find the GameManager to call banking method
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            Debug.Log($"Calling GameManager.AddCardToPlayerBank for {cardName}");
            gameManager.AddCardToPlayerBank(cardToBankObj, bankValue);
            Debug.Log($"Successfully banked {cardName} for £{bankValue}M");
        }
        else
        {
            Debug.LogError("Could not find GameManager to bank card");
        }
    }
}