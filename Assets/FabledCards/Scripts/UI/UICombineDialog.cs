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
    [SerializeField] private ItemCardOpen itemCardOpen;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject buttonCombine;
    [SerializeField] private GameObject buttonReset;
    [SerializeField] private ItemCardComponent cardMixComponent;
    [SerializeField] private BoardRateCombine boardRateCombine;

    [SerializeField] private float[] rarityChances;
    
    [SerializeField] private List<string> saveIdCards;
    [SerializeField] private List<ItemCardComponent> cardBaseComponents;

    private void OnEnable()
    {
        saveIdCards = GameManager.Instance.GetSavedIDCards();
        Init();
    }

    public void Init()
    {
        CheckReset(false);
        SpawnSaveCard();
    }
    
    private void SpawnSaveCard()
    {
        foreach (var id in saveIdCards)
        {
            BaseCardConfig card = GameManager.Instance.GameBaseCardConfig.GetCardById(id);
            ItemCardOpen cardItem = Instantiate(itemCardOpen, content.transform);
            
            cardItem.Init(card);
            cardItem.SetOnClick(SelectCard);
        }
    }
    
    const int MAX_COMPONENT = 2;
    public void SelectCard(BaseCardConfig cardData)
    {
        foreach (var component in cardBaseComponents)
        {
            bool isSelectd = component.IsSelected;

            if (!isSelectd)
            {
                component.SetSelected(cardData);
                component.SetActionResetComponent(ShowRateCombine);
                ShowRateCombine();
                return;
            }
        }
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
            Debug.LogError("Please select all cards");
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
        Debug.LogError(baseIds.Count);

        int score = 0;
        foreach (var id in baseIds)
        {
            Debug.LogError($"id: {id}");
            score += int.Parse(id.Substring(0, 1));
        }

        return score;
    }
    
    private void ShowMixCard(string mixID)
    {
        //TODO update add rarity to id
        int aptitudeID = GetAptitudeOnChance();
        string rarityMixID = $"{aptitudeID}&{mixID}";
        Debug.LogError($"rarityMixID: {rarityMixID}"); 
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
