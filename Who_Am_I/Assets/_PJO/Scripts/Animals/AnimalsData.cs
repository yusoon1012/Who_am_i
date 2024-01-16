using System;
using System.Collections.Generic;

//! 몬스터 유형
public enum AnimalsType
{
    Chicken,
    Duck,
    Buffalo,
    Goat,
    Boar,
    Bear,
    Reindeer
}       // AnimalsType

//! 몬스터 데이터

[Serializable]
public class AnimalData
{
    public string id;
    public int hp;
    public int speed;
    public int range;
    public int respawn;
    public string drop;
    public int count;
}

[Serializable]
public class AnimalBase
{
    public AnimalsType? animalsType;
    public AnimalData animalData;

    public virtual void Init
        (Dictionary<string, Dictionary<string, string>> _animalsTable, string _name)
    {
        animalData = new AnimalData
        {
            id = _animalsTable[_name]["ID"],
            hp = int.Parse(_animalsTable[_name]["HP"]),
            speed = int.Parse(_animalsTable[_name]["SPEED"]),
            range = int.Parse(_animalsTable[_name]["RANGE"]),
            respawn = int.Parse(_animalsTable[_name]["RESPAWN"]),
            drop = _animalsTable[_name]["DROP"],
            count = int.Parse(_animalsTable[_name]["COUNT"])
        };
    }
}

[Serializable]
public class Chicken : AnimalBase
{
    public override void Init
        (Dictionary<string, Dictionary<string, string>> _animalsTable, string _name)
    {
        base.Init(_animalsTable, _name);
    }
}

[Serializable]
public class Duck : AnimalBase
{
    public override void Init
        (Dictionary<string, Dictionary<string, string>> _animalsTable, string _name)
    {
        base.Init(_animalsTable, _name);
    }
}

[Serializable]
public class Buffalo : AnimalBase
{
    public override void Init
        (Dictionary<string, Dictionary<string, string>> _animalsTable, string _name)
    {
        base.Init(_animalsTable, _name);
    }
}

[Serializable]
public class Goat : AnimalBase
{
    public override void Init
        (Dictionary<string, Dictionary<string, string>> _animalsTable, string _name)
    {
        base.Init(_animalsTable, _name);
    }
}

[Serializable]
public class Boar : AnimalBase
{
    public override void Init
        (Dictionary<string, Dictionary<string, string>> _animalsTable, string _name)
    {
        base.Init(_animalsTable, _name);
    }
}

[Serializable]
public class Bear : AnimalBase
{
    public override void Init
        (Dictionary<string, Dictionary<string, string>> _animalsTable, string _name)
    {
        base.Init(_animalsTable, _name);
    }
}

[Serializable]
public class Reindeer : AnimalBase
{
    public override void Init
        (Dictionary<string, Dictionary<string, string>> _animalsTable, string _name)
    {
        base.Init(_animalsTable, _name);
    }
}