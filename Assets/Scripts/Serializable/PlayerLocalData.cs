﻿using System;
using System.Collections.Generic;

[Serializable]
public class PlayerLocalData
{
    //Local Save Data

    //Money for unit unlock
    public int Money { get; set; }
    public int RemainedPoint { get; set; }
    public int StartGold { get; set; }
    public int MoreEarnGold { get; set; }
    public int MoreCastleHealth { get; set; }
    public int ReduceCooldown { get; set; }
    public HeroList[] HerosList { get; set; }

    public int Stage { get; set; }
    public string[] StageRace { get; set; }
    public float CastleMaxHealth { get; set; }
    public float CastleHealth { get; set; }
    public float CastleExtraHealth { get; set; }
    
    public Dictionary<string, int> UnitList { get; set; }

    public PlayerLocalData()
    {
        Money = 0; // 초기 값 설정
        RemainedPoint = 0;
        StartGold = 0;
        MoreEarnGold = 0;
        MoreCastleHealth = 0;
        ReduceCooldown = 0;
        HerosList = new HeroList[1]; // 크기를 1로 지정 (원하는 크기로 변경 가능)
        HerosList[0] = new HeroList(null, false, 0); // 배열의 첫 번째 요소 초기화
        Stage = 0;
        StageRace = new string[] { "Human", "DarkElf", "Orc", "Witch", "Skeleton" };
        CastleMaxHealth = 5000f;
        CastleHealth = 5000f;
        CastleExtraHealth = 0;
        UnitList = new Dictionary<string, int>();
    }

    public PlayerLocalData(int money, int remainedPoint, int startGold, int moreEarnGold, int moreCastleHealth,
        int reduceCooldown, HeroList[] herosList, int stage, string[] stageRace, float castleMaxHealth,
        float castleHealth, float castleExtraHealth, Dictionary<string,int> unitList)
    {
        Money = money;
        RemainedPoint = remainedPoint;
        StartGold = startGold;
        MoreEarnGold = moreEarnGold;
        MoreCastleHealth = moreCastleHealth;
        ReduceCooldown = reduceCooldown;
        HerosList = herosList;
        Stage = stage;
        StageRace = stageRace;
        CastleMaxHealth = castleMaxHealth;
        CastleHealth = castleHealth;
        CastleExtraHealth = castleExtraHealth;
        UnitList = unitList;
    }
}

[Serializable]
public class HeroList : Triple<string, bool, int>
{
    public HeroList(string heroName, bool unlocked, int selected) : base(heroName, unlocked, selected)
    {
    }
}

[Serializable]
public class Triple<T1, T2, T3>
{
    public T1 Item1 { get; set; }
    public T2 Item2 { get; set; }
    public T3 Item3 { get; set; }

    public Triple(T1 item1, T2 item2, T3 item3)
    {
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
    }
}