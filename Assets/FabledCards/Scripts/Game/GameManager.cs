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
    
    public GameCardConfig GameBaseCardConfig => gameBaseCardConfig;
    public GameCardConfig GameMixCardConfig => gameMixCardConfig;
    
    
    const string SAVE_BASE_ID_CARD = "savedBaseIDCards";
    const string SAVE_MIX_ID_CARD = "savedMixIDCards";
    
    public void SaveIDCard(string id, bool isMix = false) {
        string key = isMix ? SAVE_MIX_ID_CARD : SAVE_BASE_ID_CARD;
        
        List<string> ids = GetSavedIDCards(isMix);
        ids.Add(id);
        PlayerPrefs.SetString(key, string.Join(",", ids));
        PlayerPrefs.Save();
    }
    
    public List<string> GetSavedIDCards(bool isMix = false) {
        string key = isMix ? SAVE_MIX_ID_CARD : SAVE_BASE_ID_CARD;

        string savedIDs = PlayerPrefs.GetString(key, "");
        if (string.IsNullOrEmpty(savedIDs)) {
            return new List<string>();
        }
        return savedIDs.Split(',').ToList();
    }
}
