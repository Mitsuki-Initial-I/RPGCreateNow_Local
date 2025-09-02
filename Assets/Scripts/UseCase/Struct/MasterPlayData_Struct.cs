[System.Serializable]
public struct MasterPlayData_Struct 
{
    public string gameVersion;
}
[System.Serializable]
public struct PrivatePlayData_Struct
{
    public string playerName;
    public int myMoney;
    public int playTime_Hours;
    public int playTime_Minutes;
}
[System.Serializable]
public struct CharacterData_Struct
{
    public int mapId;
    public int charaType;
    public int charaId;
    public string charaName;
}
[System.Serializable]
public struct EnlargementData_Struct
{
    public int enlargementId;
    public int enlargementCost;
}
[System.Serializable]
public struct ItemData_Struct
{
    public int itemCategory01;
    public int itemCategory02;
    public int itemCategory03;
    public int itemId;
    public string itemName;
}
[System.Serializable]
public struct JobData_Struct
{
    public int jobId;
    public string jobName;
    public int jobRank;
    public int hp;
    public int mp;
    public int ap;
    public int dp;
    public int map;
    public int mdp;
    public int sp;
    public int luc;
    public int des;
    public int res;
    public int app;
}
[System.Serializable]
public struct MapData_Struct
{
    public int mapId;
    public string mapName;
    public int mapType;
    public string mapStringData;
    public int mapDepth;
}
[System.Serializable]
public class MonsterData_BankData_Structure
{
    public int mapId;
    public int stageId;
    public string monsterName;
    public int hp;
    public int mp;
    public int ap;
    public int dp;
    public int map;
    public int mdp;
    public int sp;
    public int lv;
    public int dropExp;
}
[System.Serializable]
public struct ShoopItemData_BankData_Structure
{
    public int shoopId;
    public string mapId;
    public string itemId;
}
[System.Serializable]
public class SkillData_BankData_Structure
{
    public int skillNumber;
    public int skillRarity;
    public int skillId;
    public string skillName;
}
[System.Serializable]
public class SpecoesData_BankData_Structure
{
    public int specoesId;
    public string specoesName;
    public int specoesRarity;
    public int hp;
    public int mp;
    public int ap;
    public int dp;
    public int map;
    public int mdp;
    public int sp;
    public int luc;
    public int des;
    public int res;
    public int app;
    public int siz;
}
[System.Serializable]
public class TechniqueData_BankData_Structure
{
    public int techniqueNumber;
    public int techniqueType;
    public int techniqueId;
    public string techniqueName;
}