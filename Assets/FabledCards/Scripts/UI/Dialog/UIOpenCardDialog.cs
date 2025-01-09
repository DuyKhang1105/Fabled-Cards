using System;
using System.Collections;
using System.Collections.Generic;
using KingCyber.Base.UI;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class UIOpenCardDialog : UIBaseDialog
{
    [SerializeField] private float[] rarityChances = { 0.5f, 0.4f, 0.1f };
    [SerializeField] private List<ItemCard> itemCardOpens;
    
    [SerializeField] private GameObject rollButton;
    [SerializeField] private GameObject GroupCard;

    private GameManager gameManager;
    private GameCardConfig gameCardConfig;
    private UnityAction onOpenPack;
    
    private void OnEnable()
    {
        gameManager = GameManager.Instance;
        gameCardConfig = GameManager.Instance.GameBaseCardConfig;
        SetUI(false);
    }
    
    public void SetData(float[] rarityChances, UnityAction onOpenPack)
    {
        // Set data for the dialog
        this.rarityChances = rarityChances;
        this.onOpenPack = onOpenPack;
    }   

    public void OpenPack()
    {
        GetRandomCards();
        SetUI(true);
        onOpenPack?.Invoke();
    }

    public void SetUI(bool isRolled)
    {
        rollButton.SetActive(!isRolled);
        GroupCard.SetActive(isRolled);
    }
    
    public void GetRandomCards()
    {
        for (int i = 0; i < itemCardOpens.Count; i++)
        {
            BaseCardConfig card = GetRandomCard();
            itemCardOpens[i].Init(card);
            gameManager.SaveIDCard(card.id, true);
        }
    }

    private BaseCardConfig GetRandomCard()
    {
        float randomValue = Random.value;
        int idType = GetTypeBasedOnChance(randomValue);
        List<BaseCardConfig> cardList = gameCardConfig.GetCardListByType(idType);

        if (cardList.Count == 0)
        {
            Debug.LogWarning("No cards available for the given rarity.");
            return null;
        }

        int randomIndex = Random.Range(0, cardList.Count);
        return cardList[randomIndex];
    }

    private int GetTypeBasedOnChance(float randomValue)
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
}
