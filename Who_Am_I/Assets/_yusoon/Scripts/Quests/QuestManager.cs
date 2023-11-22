using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private static QuestManager instance;
    public static QuestManager Instance { get { return instance; } }
    int appleCount = 0;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        if (GameEventManager.instance != null && GameEventManager.instance.miscEvent != null)
        {
            GameEventManager.instance.miscEvent.onItemCollected += HandleItemCollect;
           // Debug.Log("성공");
        }
        else
        {
            Debug.LogWarning("GameEventManager.instance 또는 GameEventManager.instance.miscEvent가 null입니다.");
        }
    }

        // Update is called once per frame
        void Update()
    {
        
    }
    public void HandleItemCollect()
    {
        appleCount+=1;
        Debug.LogFormat("사과의 갯수 : {0}", appleCount);
    }
}
