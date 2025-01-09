using UnityEngine;
using KingCyber.Base.UI;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class UICombineDialog : UIBaseDialog
{
    [SerializeField] private GameObject buttonCombine;
    [SerializeField] private GameObject buttonReset;
    [SerializeField] private ItemComponentInCombine mixComponentInCombine;
    
    [SerializeField] private Inventory inventory;
    [SerializeField] private BoardRateCombine boardRateCombine;
    
    [SerializeField] private List<ItemComponentInCombine> cardBaseComponents;
    
    private float[] rarityChances;
    private List<string> saveIdCards;
    private Dictionary<string, ItemCard> spawnedCards = new Dictionary<string, ItemCard>();
    
    private GameManager gameManager;
    
    private void OnEnable()
    {
        gameManager = GameManager.Instance;
        saveIdCards = gameManager.GetSavedIDCardsByType(true);
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        CheckReset(false);
        inventory.SpawnSaveCard(saveIdCards, gameManager.GameBaseCardConfig, ActionSelectCardInInventory);
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
                ItemCard cardSelected = spawnedCards[cardData.id];
                cardSelected.DecreaseUICount();
                
                component.SetSelected(cardData);
                component.RegistrationActionResetComponent(() => ActionResetComponent(cardSelected));
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

    private void ActionResetComponent(ItemCard cardSelected)
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
        
        string mixID = GetMixID(baseIds);
        
        //Save mix card and remove base cards
        
        BaseCardConfig card = gameManager.GameMixCardConfig.GetCardById(mixID);

        if (card == null)
        {
            Debug.LogError("Mix card not found id: " + mixID);
            return;
        }
        
        ShowMixCard(card);
        gameManager.SaveIDCard(mixID, false);
        RemoveBaseCards(baseIds);
        CheckReset(true);
        UnRegistrationActionResetComponents(); // Unregister action reset component to don't increase UI count
    }
    
    private string GetMixID(List<string> baseIds)
    {
        int aptitudeID = GetAptitudeOnChance();
        string parentID = string.Join("_", baseIds);
        return $"{aptitudeID}&{parentID}";
    }
    
    private void RemoveBaseCards(List<string> baseIds)
    {
        foreach (var id in baseIds)
        {
            gameManager.RemoveIDCard(id, true);
        }
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
    
    private void ShowMixCard(BaseCardConfig card)
    {
        mixComponentInCombine.SetUI(card);
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
    
    public void OnClickResetCombine()
    {
        
        foreach (var component in cardBaseComponents)
        {
            component.ResetComponent();
        }
        
        mixComponentInCombine.ResetComponent();
        CheckReset(false);
    }
    
    private void UnRegistrationActionResetComponents()
    {
        foreach (var component in cardBaseComponents)
        {
            component.UnRegistrationActionResetComponent();
        }
    }
}
