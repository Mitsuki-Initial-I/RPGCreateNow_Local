using System.IO;
using System.Collections.Generic;
using UnityEngine;
using RPGCreateNow_Local.UseCase;

namespace RPGCreateNow_Local.Data
{
    public class EnemyStatusDataAccess
    {
        string fileName= "MonserData";
        public void LoadMapData(string monsterStr)
        {
            TextAsset textAsset = Resources.Load(fileName) as TextAsset;
            StringReader reader = new StringReader(textAsset.text);
            List<string[]> csvStrs = new List<string[]>();
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                csvStrs.Add(line.Split(','));
            }
            reader.Close();
            string[] monsterSearchStrs = monsterStr.Split('-');
            int[] monsterIds = new int[2];
            monsterIds[0] = int.Parse(monsterSearchStrs[0]) - 1;
            monsterIds[1] = int.Parse(monsterSearchStrs[1]) - 1;
            for (int i = 0; i < csvStrs.Count; i++)
            {
                if (monsterIds[0] ==int.Parse(csvStrs[i][0])) 
                {
                    if(monsterIds[1] == int.Parse(csvStrs[i][1]))
                    {
                        IStockData stockData = GameObject.Find("StockPlayerData").GetComponent<IStockData>();
                        EnemyStatus_Structure setData = new EnemyStatus_Structure();
                        setData.enemyName = csvStrs[i][2];
                        setData.hp = int.Parse(csvStrs[i][3]);
                        setData.mp = int.Parse(csvStrs[i][4]);
                        setData.ap = int.Parse(csvStrs[i][5]);
                        setData.dp = int.Parse(csvStrs[i][6]);
                        setData.map = int.Parse(csvStrs[i][7]);
                        setData.mdp = int.Parse(csvStrs[i][8]);
                        setData.sp = int.Parse(csvStrs[i][9]);
                        setData.lv = int.Parse(csvStrs[i][10]);
                        setData.dropExp = int.Parse(csvStrs[i][11]);
                        stockData.SetEnemyStatusData(setData);
                        return;
                    }
                }
            }
        }
    }
}
