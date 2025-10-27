using System;
using UnityEngine;

[Serializable]
public class CardData
{
    public string filename;
    public string cardName;
    public int quantity;
    public int bankValue;
    public string colourGroup;
    public Sprite sprite;
    
    public CardData(string filename, string cardName, int quantity, int bankValue, string colourGroup)
    {
        this.filename = filename;
        this.cardName = cardName;
        this.quantity = quantity;
        this.bankValue = bankValue;
        this.colourGroup = colourGroup;
    }
}