using System.IO;
using System.Collections.Generic;
using UnityEngine;
using RPGCreateNow_Local.UseCase;

namespace RPGCreateNow_Local.System
{
    public class Access_DataBank
    {
        public SpecoesData_BankData_Structure[] specoesData_;
        public JobData_BankData_Structure[] jobData_;
        public SkillData_BankData_Structure[] skillData_;
        public TechniqueData_BankData_Structure[] techniqueData_;
        public ItemData_BankData_Structure[] itemData_;
        public MonsterData_BankData_Structure[] monsterData_;
        public Character_BankData_Structure[] character_;
        public MapData_BankData_Structure[] mapData_;
        public ShoopItemData_BankData_Structure[] shoopItemData_;
        public EnlargementData_BankData_Structure[] enlargementData_;

        #region SetData
        private void SetData_Specoes(List<string[]> getCsvStrs)
        {
            specoesData_ = new SpecoesData_BankData_Structure[getCsvStrs.Count];
            for (int i = 0; i < getCsvStrs.Count; i++)
            {
                specoesData_[i] = new SpecoesData_BankData_Structure();
                specoesData_[i].specoesId = int.Parse(getCsvStrs[i][0]);
                specoesData_[i].specoesName = getCsvStrs[i][1];
                specoesData_[i].hp = int.Parse(getCsvStrs[i][2]);
                specoesData_[i].mp = int.Parse(getCsvStrs[i][3]);
                specoesData_[i].ap = int.Parse(getCsvStrs[i][4]);
                specoesData_[i].dp = int.Parse(getCsvStrs[i][5]);
                specoesData_[i].map = int.Parse(getCsvStrs[i][6]);
                specoesData_[i].mdp = int.Parse(getCsvStrs[i][7]);
                specoesData_[i].sp = int.Parse(getCsvStrs[i][8]);
                specoesData_[i].luc = int.Parse(getCsvStrs[i][9]);
                specoesData_[i].des = int.Parse(getCsvStrs[i][10]);
                specoesData_[i].res = int.Parse(getCsvStrs[i][11]);
                specoesData_[i].app = int.Parse(getCsvStrs[i][12]);
                specoesData_[i].siz = int.Parse(getCsvStrs[i][13]);
            }
        }
        private void SetData_Job(List<string[]> getCsvStrs)
        {
            jobData_ = new JobData_BankData_Structure[getCsvStrs.Count];
            for (int i = 0; i < getCsvStrs.Count; i++)
            {
                jobData_[i] = new JobData_BankData_Structure();
                jobData_[i].jobId = int.Parse(getCsvStrs[i][0]);
                jobData_[i].jobName = getCsvStrs[i][1];
                jobData_[i].hp = int.Parse(getCsvStrs[i][2]);
                jobData_[i].mp = int.Parse(getCsvStrs[i][3]);
                jobData_[i].ap = int.Parse(getCsvStrs[i][4]);
                jobData_[i].dp = int.Parse(getCsvStrs[i][5]);
                jobData_[i].map = int.Parse(getCsvStrs[i][6]);
                jobData_[i].mdp = int.Parse(getCsvStrs[i][7]);
                jobData_[i].sp = int.Parse(getCsvStrs[i][8]);
                jobData_[i].luc = int.Parse(getCsvStrs[i][9]);
                jobData_[i].des = int.Parse(getCsvStrs[i][10]);
                jobData_[i].res = int.Parse(getCsvStrs[i][11]);
                jobData_[i].app = int.Parse(getCsvStrs[i][12]);
            }
        }
        private void SetData_Skill(List<string[]> getCsvStrs)
        {
            skillData_ = new SkillData_BankData_Structure[getCsvStrs.Count];
            for (int i = 0; i < getCsvStrs.Count; i++)
            {
                skillData_[i] = new SkillData_BankData_Structure();
                skillData_[i].skillNumber = int.Parse(getCsvStrs[i][0]);
                skillData_[i].skillRarity = int.Parse(getCsvStrs[i][1]);
                skillData_[i].skillId = int.Parse(getCsvStrs[i][2]);
                skillData_[i].skillName = getCsvStrs[i][3];
            }
        }
        private void SetData_Technique(List<string[]> getCsvStrs)
        {
            techniqueData_ = new TechniqueData_BankData_Structure[getCsvStrs.Count];
            for (int i = 0; i < getCsvStrs.Count; i++)
            {
                techniqueData_[i] = new TechniqueData_BankData_Structure();
                techniqueData_[i].techniqueNumber = int.Parse(getCsvStrs[i][0]);
                techniqueData_[i].techniqueType = int.Parse(getCsvStrs[i][1]);
                techniqueData_[i].techniqueId = int.Parse(getCsvStrs[i][2]);
                techniqueData_[i].techniqueName = getCsvStrs[i][3];
            }
        }
        private void SetData_Item(List<string[]> getCsvStrs)
        {
            itemData_ = new ItemData_BankData_Structure[getCsvStrs.Count];
            for (int i = 0; i < getCsvStrs.Count; i++)
            {
                itemData_[i] = new ItemData_BankData_Structure();
                itemData_[i].itemCategory01 = int.Parse(getCsvStrs[i][0]);
                itemData_[i].itemCategory02 = int.Parse(getCsvStrs[i][1]);
                itemData_[i].itemCategory03 = int.Parse(getCsvStrs[i][2]);
                itemData_[i].itemId = int.Parse(getCsvStrs[i][3]);
                itemData_[i].itemName = getCsvStrs[i][4];
            }
        }
        private void SetData_Monster(List<string[]> getCsvStrs)
        {
            monsterData_ = new MonsterData_BankData_Structure[getCsvStrs.Count];
            for (int i = 0; i < getCsvStrs.Count; i++)
            {
                monsterData_[i] = new MonsterData_BankData_Structure();
                monsterData_[i].mapId = int.Parse(getCsvStrs[i][0]);
                monsterData_[i].stageId = int.Parse(getCsvStrs[i][1]);
                monsterData_[i].monsterName = getCsvStrs[i][2];
                monsterData_[i].hp = int.Parse(getCsvStrs[i][3]);
                monsterData_[i].mp = int.Parse(getCsvStrs[i][4]);
                monsterData_[i].ap = int.Parse(getCsvStrs[i][5]);
                monsterData_[i].dp = int.Parse(getCsvStrs[i][6]);
                monsterData_[i].map = int.Parse(getCsvStrs[i][7]);
                monsterData_[i].mdp = int.Parse(getCsvStrs[i][8]);
                monsterData_[i].sp = int.Parse(getCsvStrs[i][9]);
                monsterData_[i].lv = int.Parse(getCsvStrs[i][10]);
                monsterData_[i].dropExp = int.Parse(getCsvStrs[i][11]);
            }
        }
        private void SetData_Character(List<string[]> getCsvStrs)
        {
            character_ = new Character_BankData_Structure[getCsvStrs.Count];
            for (int i = 0; i < getCsvStrs.Count; i++)
            {
                character_[i] = new Character_BankData_Structure();
                character_[i].mapNumber = int.Parse(getCsvStrs[i][0]);
                character_[i].charaType = int.Parse(getCsvStrs[i][1]);
                character_[i].charaId = int.Parse(getCsvStrs[i][2]);
                character_[i].charaName = getCsvStrs[i][3];
            }
        }
        private void SetData_Map(List<string[]> getCsvStrs)
        {
            mapData_ = new MapData_BankData_Structure[getCsvStrs.Count];
            for (int i = 0; i < getCsvStrs.Count; i++)
            {
                mapData_[i] = new MapData_BankData_Structure();
                mapData_[i].mapNumber = int.Parse(getCsvStrs[i][0]);
                mapData_[i].mapName = getCsvStrs[i][1];
                mapData_[i].mapType = int.Parse(getCsvStrs[i][2]);
                mapData_[i].mapStringData = getCsvStrs[i][3];
                mapData_[i].mapDepth = int.Parse(getCsvStrs[i][4]);
            }
        }
        private void SetData_ShopItem(List<string[]> getCsvStrs)
        {
            shoopItemData_ = new ShoopItemData_BankData_Structure[getCsvStrs.Count];
            for (int i = 0; i < getCsvStrs.Count; i++)
            {
                shoopItemData_[i] = new ShoopItemData_BankData_Structure();
                shoopItemData_[i].shoopId = int.Parse(getCsvStrs[i][0]);
                shoopItemData_[i].mapId = getCsvStrs[i][1];
                shoopItemData_[i].itemId = (getCsvStrs[i][2]);
            }
        }
        private void SetData_Enlargement(List<string[]> getCsvStrs)
        {
            enlargementData_ = new EnlargementData_BankData_Structure[getCsvStrs.Count];
            for (int i = 0; i < getCsvStrs.Count; i++)
            {
                enlargementData_[i] = new EnlargementData_BankData_Structure();
                enlargementData_[i].enlargementId = int.Parse(getCsvStrs[i][0]);
                enlargementData_[i].enlargementMoney = int.Parse(getCsvStrs[i][1]);
            }
        }
        
        #endregion

        private void UseData(DataBankFileNames nuber,string fileName)
        {
            TextAsset bankData = Resources.Load(fileName) as TextAsset;
            StringReader reader = new StringReader(bankData.text);
            List<string[]> csvStrs = new List<string[]>();
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                csvStrs.Add(line.Split(','));
            }
            reader.Close();
            switch (nuber)
            {
                case DataBankFileNames.SpecoesData:
                    SetData_Specoes(csvStrs);
                    break;
                case DataBankFileNames.JobData:
                    SetData_Job(csvStrs);
                    break;
                case DataBankFileNames.SkillData:
                    SetData_Skill(csvStrs);
                    break;
                case DataBankFileNames.TechniqueData:
                    SetData_Technique(csvStrs);
                    break;
                case DataBankFileNames.ItemData:
                    SetData_Item(csvStrs);
                    break;
                case DataBankFileNames.MonsterData:
                    SetData_Monster(csvStrs);
                    break;
                case DataBankFileNames.CharacterData:
                    SetData_Character(csvStrs);
                    break;
                case DataBankFileNames.MapData:
                    SetData_Map(csvStrs);
                    break;
                case DataBankFileNames.ShoopItemData:
                    SetData_ShopItem(csvStrs);
                    break;
                case DataBankFileNames.EnlargementData:
                    SetData_Enlargement(csvStrs);
                    break;
            }
        }

        public void Load_Resources(string[] resourcesDataFaileNames)
        {
            for (int i = 0; i < resourcesDataFaileNames.Length; i++)
            {
                UseData((DataBankFileNames)i, resourcesDataFaileNames[i]);
            }
        }
    }
}