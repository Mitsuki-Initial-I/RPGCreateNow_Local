using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCreateNow_Local.UseCase;

namespace RPGCreateNow_Local.Data
{
    public class StockPlayerData : MonoBehaviour,ISetPlayerData
    {
        PlayerStatus_Structure playerStatusData;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        void ISetPlayerData.SetPlayerStatusData(PlayerStatus_Structure setData)
        {
            playerStatusData = setData;
            Debug.Log(playerStatusData.playerName);
        }
    }
}
