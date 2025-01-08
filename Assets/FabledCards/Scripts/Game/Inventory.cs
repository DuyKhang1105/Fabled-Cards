using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [SerializeField] private ItemCardOpen itemCardOpen;
    [SerializeField] private GameObject content;
    
    private Dictionary<string, ItemCardOpen> spawnedCards = new Dictionary<string, ItemCardOpen>();
    public Dictionary<string, ItemCardOpen> SpawnedCards => spawnedCards; 

    public void SpawnSaveCard(List<string> saveIdCards, UnityAction<BaseCardConfig> actionSelectCardInInventory = null)
    {
        foreach (var id in saveIdCards)
        {
            if (spawnedCards.ContainsKey(id))
            {
                // Tăng số lượng của GameObject đã tồn tại
                spawnedCards[id].IncreaseUICount();
            }
            else
            {
                // Tạo mới GameObject nếu ID chưa được sử dụng trước đó
                BaseCardConfig card = GameManager.Instance.GameBaseCardConfig.GetCardById(id);
                ItemCardOpen cardItem = Instantiate(itemCardOpen, content.transform);
            
                cardItem.Init(card);
                cardItem.SetOnClick(actionSelectCardInInventory);

                // Thêm GameObject vào từ điển
                spawnedCards[id] = cardItem;
            }
        }
    }
}
