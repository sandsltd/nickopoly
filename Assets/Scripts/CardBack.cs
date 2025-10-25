using UnityEngine;

public static class CardBack
{
    public static Sprite CreateCardBackSprite()
    {
        // Create a simple colored texture for card back
        Texture2D texture = new Texture2D(100, 140);
        Color backColor = new Color(0.2f, 0.2f, 0.6f, 1f); // Dark blue
        
        // Fill the texture with the back color
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                texture.SetPixel(x, y, backColor);
            }
        }
        
        // Add a border
        Color borderColor = Color.white;
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                if (x < 3 || x >= texture.width - 3 || y < 3 || y >= texture.height - 3)
                {
                    texture.SetPixel(x, y, borderColor);
                }
            }
        }
        
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}