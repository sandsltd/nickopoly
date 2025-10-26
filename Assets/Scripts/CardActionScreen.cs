using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CardActionScreen : MonoBehaviour
{
    [Header("Action Screen Settings")]
    public Vector3 actionScreenPosition = new Vector3(2, 0, 0);
    public float actionCardScale = 0.8f;
    
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
        Vector3 buttonPosition = actionScreenPosition + new Vector3(-4.5f, 1.8f, 0);
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
        
        textObj.transform.localScale = Vector3.one * 0.02f;
        closeButton.transform.localScale = Vector3.one * 1.0f;
        
        Debug.Log($"Close button created at position: {buttonPosition}");
    }
    
    CardType GetCardType(GameObject card)
    {
        string cardName = card.name.ToLower();
        
        // Check for money cards
        if (cardName.Contains("money") || cardName.Contains("pass_go"))
        {
            return CardType.Money;
        }
        
        // Check for action cards
        if (cardName.Contains("deal_breaker") || cardName.Contains("debt_collector") || 
            cardName.Contains("forced_deal") || cardName.Contains("sly_deal") || 
            cardName.Contains("rent") || cardName.Contains("just_say_no") || 
            cardName.Contains("double_the_rent") || cardName.Contains("birthday") ||
            cardName.Contains("hotel") || cardName.Contains("house"))
        {
            return CardType.Action;
        }
        
        // Everything else is property (including wild cards)
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
    }
    
    void CreateActionButton(CardAction action, int index)
    {
        GameObject button = new GameObject($"ActionButton_{action}");
        // Position buttons to the left of the card, vertically stacked
        Vector3 buttonPosition = actionScreenPosition + new Vector3(-4.5f, 0.8f - (index * 0.8f), 0);
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
        
        textObj.transform.localScale = Vector3.one * 0.02f;
        button.transform.localScale = Vector3.one * 1.0f;
        
        // Store button reference
        actionButtons.Add(button);
        
        Debug.Log($"Created {action} button at position: {buttonPosition}");
    }
    
    void CreateEmptySlot(int index)
    {
        // Create invisible placeholder to maintain consistent spacing
        GameObject emptySlot = new GameObject($"EmptySlot_{index}");
        Vector3 slotPosition = actionScreenPosition + new Vector3(-4.5f, 0.8f - (index * 0.8f), 0);
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
                if (distanceToCloseButton < 1.0f)
                {
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
                    if (distanceToButton < 1.0f) // Larger click area for buttons
                    {
                        HandleActionButtonClick(button.name);
                        return;
                    }
                }
            }
            
            // Check if click is outside the action card area
            float distanceToActionCard = Vector3.Distance(mouseWorldPos, actionScreenPosition);
            if (distanceToActionCard > 1.5f) // Outside the action card area
            {
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
                Debug.Log($"Banking card: {currentActionCard.name}");
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
}