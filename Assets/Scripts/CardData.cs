using System;
using UnityEngine;

[Serializable]
public class CardData
{
    public string filename;
    public string cardName;
    public int quantity;
    public Sprite sprite;
    
    public CardData(string filename, string cardName, int quantity)
    {
        this.filename = filename;
        this.cardName = cardName;
        this.quantity = quantity;
    }
}