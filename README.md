# Monopoly Deal Card Game

A Unity project displaying all Monopoly Deal cards in a grid layout.

## Project Structure

- `Assets/Scripts/CardDisplayManager.cs` - Main script that handles loading and displaying all cards
- `Assets/Scripts/Card.cs` - Individual card component
- `Assets/Resources/Cards/` - Contains all 63 Monopoly Deal card PNG files
- `Assets/Scenes/CardDisplayScene.unity` - Scene configured to display all cards

## How to Use

1. Open the project in Unity
2. Open the `CardDisplayScene` scene
3. Press Play to see all Monopoly Deal cards displayed in a grid layout

## Card Display Features

- Automatically loads all cards from the Resources/Cards folder
- Displays cards in a configurable grid (default: 8 cards per row)
- Adjustable spacing between cards
- Camera positioned to show all cards at once

## Configuration

You can adjust the display settings in the `Card Display Manager` GameObject:
- `Cards Per Row`: Number of cards displayed in each row
- `Card Spacing`: Horizontal spacing between cards
- `Row Spacing`: Vertical spacing between rows