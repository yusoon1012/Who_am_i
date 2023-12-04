using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//! 몬스터 유형
public enum AnimalsType
{
    Rabbit,
    Chicken
}       // AnimalsType

//! 몬스터 데이터
public class AnimalData
{
    public int id;
    public string description;
    public string modelinfo;
    public string type;
    public int hp;
    public float speed;
    public int range_Rec;
    public int drop_Item;
}       // AnimalData

public class AnimalBase
{
    public AnimalsType? animalsType;
    public AnimalData animalData;

    public virtual void Init()
    {
        /* NoEvent */
    }       // Init()
}       // AnimalBase

public class Rabbit : AnimalBase
{
    public override void Init()
    {
        base.Init();
        animalData = new AnimalData
        {
            id = 4001,
            description = "토끼",
            modelinfo = "TX_Minion_Rabbit",
            type = "1",
            hp = 1,
            speed = 3.0f,
            range_Rec = 10,
            drop_Item = 1
        };
    }       // Init()
}      // Rabbit

public class Chicken : AnimalBase
{
    public override void Init()
    {
        base.Init();
        animalData = new AnimalData
        {
            id = 4002,
            description = "닭",
            modelinfo = "TX_Minion_Chicken",
            type = "1",
            hp = 1,
            speed = 3.0f,
            range_Rec = 10,
            drop_Item = 1
        };
    }       // Init()
}       // Chicken
