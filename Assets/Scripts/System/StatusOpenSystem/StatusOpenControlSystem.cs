using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPGCreateNow_Local.UseCase;

namespace RPGCreateNow_Local.System
{
    public class StatusOpenControlSystem : MonoBehaviour
    {
        [SerializeField]
        Text[] StatusTextes = new Text[10];
        PlayerStatus_Structure playerStatusData;
        IStockData stockData;

        const int OPENNUM = 362;
        const int CLAUSENUM = 620;
        public void Start()
        {
            stockData = GameObject.Find("StockPlayerData").GetComponent<IStockData>();
            playerStatusData = stockData.GetPlayerStatusData();
            StatusTextes[0].text = playerStatusData.playerName;
            StatusTextes[1].text = playerStatusData.lv.ToString();
            StatusTextes[2].text = playerStatusData.hp.ToString();
            StatusTextes[3].text = playerStatusData.mp.ToString();
            StatusTextes[4].text = playerStatusData.ap.ToString();
            StatusTextes[5].text = playerStatusData.dp.ToString();
            StatusTextes[6].text = playerStatusData.map.ToString();
            StatusTextes[7].text = playerStatusData.mdp.ToString();
            StatusTextes[8].text = playerStatusData.sp.ToString();
            StatusTextes[9].text = playerStatusData.exp.ToString();
        }

        public void ButtonControl()
        {
            Vector3 pos = transform.localPosition;
            pos.x = pos.x == OPENNUM ? CLAUSENUM : OPENNUM;
            transform.localPosition = pos;
        }
    }
}