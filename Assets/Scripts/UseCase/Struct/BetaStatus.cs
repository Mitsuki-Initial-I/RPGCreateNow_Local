[System.Serializable]
public class BetaStatus
{
    public string name;
    public int lv;
    public int currentHp;
    public int maxhp;
    public int attackpower;
    public int defensepower;
    public int speedpower;
    public int exp;
    public bool moveSuccess = false;
    public bool ImOnYourSide = false;

    public BattleCommandNames battleCommand;

    public virtual float NormalSpeed()
    {
        return (float)speedpower;
    }
    public virtual AttackData_Sruct NormalAttack()
    {
        AttackData_Sruct attackData = new AttackData_Sruct();
        attackData.atk = attackpower;
        attackData.attackType = AttackType.Slashing;
        attackData.attackAttribute = AttackAttribute.Physics;
        attackData.attackTargetArea = AttackTargetArea.Individual;
        return attackData; //attackpower;
    }
    public virtual int DamageProcess(AttackData_Sruct attackData)
    {
        var myDefensePower = defensepower;
        if (moveSuccess)
        {
            myDefensePower *= 2;
        }
        var damage = attackData.atk - myDefensePower;
        damage = damage < 0 ? 0 : damage;
        currentHp -= damage;
        
        return damage;
    }
}