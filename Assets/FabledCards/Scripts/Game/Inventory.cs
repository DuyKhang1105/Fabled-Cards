using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour
{
    [SerializeField] private ItemCard itemCard;
    [SerializeField] private GameObject content;
    
    private Dictionary<string, ItemCard> spawnedCards = new Dictionary<string, ItemCard>();
    public Dictionary<string, ItemCard> SpawnedCards => spawnedCards; 

    public void SpawnSaveCard(List<string> saveIdCards, GameCardConfig gameCardConfig , UnityAction<BaseCardConfig> actionSelectCardInInventory = null)
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
                BaseCardConfig card = gameCardConfig.GetCardById(id);
                ItemCard cardItem = Instantiate(itemCard, content.transform);
            
                cardItem.Init(card);
                cardItem.SetOnClick(actionSelectCardInInventory);

                // Thêm GameObject vào từ điển
                spawnedCards[id] = cardItem;
            }
        }
    }
}
