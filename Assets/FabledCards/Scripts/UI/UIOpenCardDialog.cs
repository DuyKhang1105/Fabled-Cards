using System;
using System.Collections;
using System.Collections.Generic;
using KingCyber.Base.UI;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIOpenCardDialog : UIBaseDialog
{
    [SerializeField] private GameCardConfig gameCardConfig;
    [SerializeField] private float[] rarityChances = { 0.5f, 0.4f, 0.1f };
    [SerializeField] private List<ItemCardOpen> itemCardOpens;
    List<BaseCardConfig> selectedCards = new List<BaseCardConfig>();
    
    [SerializeField] private GameObject rollButton;
    [SerializeField] private GameObject GroupCard;

    private void OnEnable()
    {
        SetUI(false);
    }

    public void Roll()
    {
        Debug.LogError("Rolling");
        GetRandomCards();
        SetUI(true);
    }

    public void SetUI(bool isRolled)
    {
        rollButton.SetActive(!isRolled);
        GroupCard.SetActive(isRolled);
    }
    
    public void GetRandomCards()
    {
        List<BaseCardConfig> selectedCards = new List<BaseCardConfig>();

        for (int i = 0; i < itemCardOpens.Count; i++)
        {
            BaseCardConfig card = GetRandomCard();
            itemCardOpens[i].SetUI(card);
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
