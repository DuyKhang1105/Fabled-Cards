using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KingCyber.Base.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UICombineDialog : UIBaseDialog
{
    [SerializeField] private GameObject buttonCombine;
    [SerializeField] private GameObject buttonReset;
    [SerializeField] private ItemCardComponent cardMixComponent;
    
    [SerializeField] private Inventory inventory;
    [SerializeField] private BoardRateCombine boardRateCombine;
    
    [SerializeField] private List<string> saveIdCards;
    [SerializeField] private List<ItemCardComponent> cardBaseComponents;
    
    private float[] rarityChances;
    private Dictionary<string, ItemCardOpen> spawnedCards = new Dictionary<string, ItemCardOpen>();
    
    private GameManager gameManager;
    
    private void OnEnable()
    {
        gameManager = GameManager.Instance;
        saveIdCards = gameManager.GetSavedIDCardsByType();
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        CheckReset(false);
        inventory.SpawnSaveCard(saveIdCards, ActionSelectCardInInventory);
        spawnedCards = inventory.SpawnedCards;
    }
    
    public void ActionSelectCardInInventory(BaseCardConfig cardData)
    {
        foreach (var component in cardBaseComponents)
        {
            bool isSelectd = component.IsSelected;
            bool isIdAvailable = IsAvailableIdInComponent(cardData.id);
            
            if (isIdAvailable)
            {
                Debug.Log("You can't merge cards of the same type.Please select another card");
                return;
            }
            
            if (!isSelectd)
            {
                ItemCardOpen cardSelected = spawnedCards[cardData.id];
                cardSelected.DecreaseUICount();
                
                component.SetSelected(cardData);
                component.SetActionResetComponent(() => ActionResetComponent(cardSelected));
                ShowRateCombine();
                return;
            }
        }
    }
    
    private bool IsAvailableIdInComponent(string id)
    {
        foreach (var component in cardBaseComponents)
        {
            if (component.IdCard == id)
            {
                return true;
            }
        }

        return false;
    }

    private void ActionResetComponent(ItemCardOpen cardSelected)
    {
        cardSelected.IncreaseUICount();
        ShowRateCombine();
    }
    
    private void ShowRateCombine()
    {
        //TODO show rate combine
        rarityChances = GetRateCombine();

        boardRateCombine.ShowRateCombine(rarityChances);
    }
    
    private float[] GetRateCombine()
    {
        int score = GetAptitudeScore();
        return GameManager.Instance.GameRateConfigs.GetCombineRateConfigByScore(score);
    }
    
    public void Combine()
    {
        List<string> baseIds = GetBaseIds();
        if (baseIds == null)
        {
            return;
        }
        
        string mixID = string.Join("_", baseIds);
        
        ShowMixCard(mixID);
        //TODO save mix card and remove base card
    }

    private List<string> GetBaseIds()
    {
        if (!IsSelectedAll())
        {
            Debug.Log("Please select all cards");
            return null;
        }

        List<string> baseIds = new List<string>();
        foreach (var card in cardBaseComponents)
        {
            baseIds.Add(card.IdCard);
        }

        return baseIds;
    }
    
    private int GetAptitudeScore()
    {
        List<string> baseIds = GetBaseIds();
        if (baseIds == null)
        {
            return 0;
        }

        int score = 0;
        foreach (var id in baseIds)
        {
            score += int.Parse(id.Substring(0, 1));
        }

        return score;
    }
    
    private void ShowMixCard(string mixID)
    {
        //TODO update add rarity to id
        int aptitudeID = GetAptitudeOnChance();
        string rarityMixID = $"{aptitudeID}&{mixID}";
        //TODO show mix card
        BaseCardConfig card = GameManager.Instance.GameMixCardConfig.GetCardById(rarityMixID);

        if (card == null)
        {
            Debug.LogError("Mix card not found id: " + rarityMixID);
            return;
        }
        cardMixComponent.SetUI(card);
        CheckReset(true);
    }

    private int GetAptitudeOnChance()
    {
        float randomValue = Random.value;
        
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
    
    private bool IsSelectedAll()
    {
        foreach (var component in cardBaseComponents)
        {
            if (!component.IsSelected)
            {
                return false;
            }
        }

        return true;
    }

    private void CheckReset(bool canReset)
    {
        buttonReset.SetActive(canReset);
        buttonCombine.SetActive(!canReset);
    }
    
    public void ResetCombine()
    {
        foreach (var component in cardBaseComponents)
        {
            component.ResetComponent();
        }
        
        cardMixComponent.ResetComponent();
        CheckReset(false);
    }
}
