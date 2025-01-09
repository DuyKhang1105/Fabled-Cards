using System;
using System.Collections;
using System.Collections.Generic;
using KingCyber.Base.UI;
using UnityEngine;

public class UIBattleDialog : UIBaseDialog
{
    [SerializeField] private Inventory inventory;

    private List<string> saveIdCards;
    private GameManager gameManager;
    private GameCardConfig gameCardConfig;
    private void OnEnable()
    {
        gameManager = GameManager.Instance;
        gameCardConfig = GameManager.Instance.GameMixCardConfig;
        saveIdCards = gameManager.GetSavedIDCardsByType(false);
    }
    
    private void Start()
    {
        Init();
    }
    
    public void Init()
    {
        inventory.SpawnSaveCard(saveIdCards, gameCardConfig, ActionSelectCardInInventory);
    }
    
    public void ActionSelectCardInInventory(BaseCardConfig cardData)
    {
        Debug.Log("Card selected: " + cardData.id);
    }
}
