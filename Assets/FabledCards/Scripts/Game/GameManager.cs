using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KingCyber.Base;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private GameCardConfig gameBaseCardConfig;
    [SerializeField] private GameCardConfig gameMixCardConfig;
    [SerializeField] private GameRateConfigs gameRateConfigs;
    
    public GameCardConfig GameBaseCardConfig => gameBaseCardConfig;
    public GameCardConfig GameMixCardConfig => gameMixCardConfig;
    public GameRateConfigs GameRateConfigs => gameRateConfigs;
    
    
    const string SAVE_BASE_ID_CARD = "savedBaseIDCards";
    const string SAVE_MIX_ID_CARD = "savedMixIDCards";
    
    public void SaveIDCard(string id, bool isBaseType) {
        string key = GetKeySaveIDCardByType(isBaseType);
        List<string> ids = GetSavedIDCardsByType(isBaseType);
        
        ids.Add(id);
        SaveIdCardByKey(key, ids);
    }
    
    public void RemoveIDCard(string id, bool isBaseType) {
        string key = GetKeySaveIDCardByType(isBaseType);
        List<string> ids = GetSavedIDCardsByType(isBaseType);
        
        ids.Remove(id);
        SaveIdCardByKey(key, ids);
    }
    
    public List<string> GetSavedIDCardsByType(bool isBaseType) {
        string key = GetKeySaveIDCardByType(isBaseType);
        string savedIDs = PlayerPrefs.GetString(key, "");

        if (string.IsNullOrEmpty(savedIDs)) {
            return new List<string>();
        }
        return savedIDs.Split(',').ToList();
    }
    
    private string GetKeySaveIDCardByType(bool isBaseType) {
        return isBaseType ? SAVE_BASE_ID_CARD : SAVE_MIX_ID_CARD;
    } 
    
    private void SaveIdCardByKey(string key, List<string> ids) {
        PlayerPrefs.SetString(key, string.Join(",", ids));
        PlayerPrefs.Save();
    }
    
    public void RemoveAllSavedIDCards(bool isBaseType) {
        if (isBaseType)
        {
            PlayerPrefs.DeleteKey(SAVE_BASE_ID_CARD);
        }
        else
        {
            PlayerPrefs.DeleteKey(SAVE_MIX_ID_CARD);
        }
        PlayerPrefs.Save();
    }
}
