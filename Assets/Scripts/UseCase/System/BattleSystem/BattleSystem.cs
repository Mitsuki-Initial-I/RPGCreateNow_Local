using System.Collections.Generic;
using UnityEngine;

public class BattleSystem
{
    // 開発用の変数なので後で消す
    const float CRITICALLINE = 10.0f;
    const float HITLINE = 80.0f;
    const float RANMAX = 100.0f;

    // 攻撃が当たったか判定
    public AttackResult AttackHitCheck()
    {
        var hitLine = HITLINE;
        hitLine = hitLine < 0 ? 0 : hitLine;
        var randomValue = Random.Range(0.0f, RANMAX);
        if (randomValue <= hitLine)
        {
            float criticalValue = Random.Range(0.0f, RANMAX);
            if (criticalValue <= CRITICALLINE)
            {
                return AttackResult.criticalHit;
            }
            return AttackResult.hit;
        }
        return AttackResult.noHit;
    }

    // どの相手に当てたか
    public int HitTargetIDs(AttackData_Sruct attackData, BetaStatus[] betaStatusLong, int attackNumber)
    {
        var battleCharaCount = betaStatusLong.Length;
        int retrueNum = 0;
        for (int i = 0; i < battleCharaCount; i++)
        {
            if (i != attackNumber && betaStatusLong[i].currentHp > 0)
            {
                retrueNum = i;
            }
        }
        return retrueNum;

        /*　
        　めんどい！いつかやる
        switch (attackData.attackTargetArea)
        {
            case AttackTargetArea.Individual:
                break;
            case AttackTargetArea.BothSides:
                break;
            case AttackTargetArea.Group:
                break;
            case AttackTargetArea.AllMembers:
                break;
            default:
                break;
        }
        */
    }

    // 逃げれるか対抗
    public bool EscapeChallenge(BetaStatus[] betaStatusLong, int attackNumber)
    {
        List<BetaStatus> betaStatuses = new List<BetaStatus>();
        for (int i = 0; i < betaStatusLong.Length; i++)
        {
            if (i != attackNumber && betaStatusLong[i].ImOnYourSide != betaStatusLong[attackNumber].ImOnYourSide)
            {
                betaStatuses.Add(betaStatusLong[i]);
            }
        }
        var ramNum = Random.Range(0, betaStatuses.Count-1);

        return betaStatusLong[attackNumber].speedpower * 2 > betaStatuses[ramNum].speedpower;
    }
}