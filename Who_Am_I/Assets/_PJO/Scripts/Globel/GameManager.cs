using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Dictionary<AnimalsType, AnimalData> animals = new Dictionary<AnimalsType, AnimalData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        SetData();
    }


    private void SetData()
    {
        #region 동물 설정
        // 토끼 데이터 저장
        Rabbit rabbit = new Rabbit();
        rabbit.Init();
        animals.Add(AnimalsType.Rabbit, rabbit.animalData);
        // 닭 데이터 저장
        Chicken chicken = new Chicken();
        chicken.Init();
        animals.Add(AnimalsType.Chicken, chicken.animalData);
        // 멧돼지 데이터 저장
        Hog hog = new Hog();
        hog.Init();
        animals.Add(AnimalsType.Hog, hog.animalData);
        #endregion
    }
}
