using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        AwakeStart();
    }

    private void AwakeStart()
    {
        InitializationMBTI();
        InitializationTable();
        SetAnimals();
    }

    #region MBTI
    private MBTI mbti;

    private void InitializationMBTI()
    {
        mbti = new MBTI(0, 0, 0, 0, 0, 0, 0, 0);
    }

    public void AddMBTI(string _type)
    {
        switch (_type)
        {
            case "E": mbti.E += 1; break;
            case "S": mbti.S += 1; break;
            case "T": mbti.T += 1; break;
            case "J": mbti.J += 1; break;
            case "I": mbti.I += 1; break;
            case "N": mbti.N += 1; break;
            case "F": mbti.F += 1; break;
            case "P": mbti.P += 1; break;
        }
    }

    public string SetMBTI()
    {
        string m = mbti.E > mbti.I ? "E" : "I";
        string b = mbti.S > mbti.N ? "S" : "N";
        string t = mbti.T > mbti.F ? "T" : "F";
        string i = mbti.J > mbti.P ? "J" : "P";

        return m + b + t + i;
    }
    #endregion

    #region Animals
    public Dictionary<AnimalsType, AnimalData> animals;
    public Dictionary<string, Dictionary<string, string>> animalsTable;
    public List<GameObject> items;

    private void InitializationTable()
    {
        animals = new Dictionary<AnimalsType, AnimalData>();
        animalsTable = new Dictionary<string, Dictionary<string, string>>();

        animalsTable = CSVReader.ReadCSVKeyDictionary("AnimalsTable");
    }

    private void SetAnimals()
    {
        // Chicken 데이터 저장
        Chicken chicken = new Chicken();
        chicken.Init(animalsTable, "Chicken");
        animals.Add(AnimalsType.Chicken, chicken.animalData);
        // Duck 데이터 저장
        Duck duck = new Duck();
        duck.Init(animalsTable, "Duck");
        animals.Add(AnimalsType.Duck, duck.animalData);
        // Buffalo 데이터 저장
        Buffalo buffalo = new Buffalo();
        buffalo.Init(animalsTable, "Buffalo");
        animals.Add(AnimalsType.Buffalo, buffalo.animalData);
        // Goat 데이터 저장
        Goat goat = new Goat();
        goat.Init(animalsTable, "Goat");
        animals.Add(AnimalsType.Goat, goat.animalData);
        // Boar 데이터 저장
        Boar boar = new Boar();
        boar.Init(animalsTable, "Boar");
        animals.Add(AnimalsType.Boar, boar.animalData);
        // Bear 데이터 저장
        Bear bear = new Bear();
        bear.Init(animalsTable, "Bear");
        animals.Add(AnimalsType.Bear, bear.animalData);
        // Reindeer 데이터 저장
        Reindeer reindeer = new Reindeer();
        reindeer.Init(animalsTable, "Reindeer");
        animals.Add(AnimalsType.Reindeer, reindeer.animalData);
    }
    #endregion
}