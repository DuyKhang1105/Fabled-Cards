using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    public GameCardConfig gameCardConfig;
    public List<TextMeshProUGUI> cardNameTexts;
    public List<TextMeshProUGUI> cardRarityTexts;
    public float[] rarityChances = { 0.5f, 0.4f, 0.1f };
    List<BaseCardConfig> selectedCards = new List<BaseCardConfig>();
    
    [ContextMenu("Roll")]
    public void Roll()
    {
        Debug.LogError("Rolling");
        GetRandomCards(3);
        
    }

    private void SetUI(int index, BaseCardConfig card)
    {
        cardNameTexts[index].text = card.cardName;
        cardRarityTexts[index].text = card.rarity.ToString();
    }

    public void GetRandomCards(int numberOfCards)
    {
        List<BaseCardConfig> selectedCards = new List<BaseCardConfig>();

        for (int i = 0; i < numberOfCards; i++)
        {
            BaseCardConfig card = GetRandomCard();
            SetUI(i, card);
            selectedCards.Add(card);
        }
    }

    private BaseCardConfig GetRandomCard()
    {
        float randomValue = Random.value;
        int rarity = GetRarityBasedOnChance(randomValue);
        List<BaseCardConfig> cardList = GetCardListByRarity(rarity);

        if (cardList.Count == 0)
        {
            Debug.LogWarning("No cards available for the given rarity.");
            return null;
        }

        int randomIndex = Random.Range(0, cardList.Count);
        return cardList[randomIndex];
    }

    private int GetRarityBasedOnChance(float randomValue)
    {
        float cumulative = 0f;
        for (int i = 0; i < rarityChances.Length; i++)
        {
            cumulative += rarityChances[i];
            if (randomValue < cumulative)
            {
                return i + 1;
            }
        }

        return 1; // Default to common if something goes wrong
    }

    private List<BaseCardConfig> GetCardListByRarity(int rarity)
    {
        switch (rarity)
        {
            case 1:
                return gameCardConfig.commonCards;
            case 2:
                return gameCardConfig.rareCards;
            case 3:
                return gameCardConfig.epicCards;
            default:
                return new List<BaseCardConfig>();
        }
    }
}
