using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using JetBrains.Annotations;
using System;
using Unity.VisualScripting;

public class ItemDatabase : MonoBehaviour
{
    private static ItemDatabase _instance = default;
    public static ItemDatabase Instance
    {
        get
        {

            if (_instance == null)
            {
                _instance = FindObjectOfType<ItemDatabase>();
            }
            return _instance;
        }
    }
    DatabaseReference m_Reference;
    FirebaseAuth auth;
    int appleCount;
    public Dictionary<string, int> equipmentDict = new Dictionary<string, int>();
    public Dictionary<string, int> foodDict = new Dictionary<string, int>();
    public Dictionary<string, int> stuffDict = new Dictionary<string, int>();
    List<string> testList = new List<string>();
    IDictionary foodInfo;
    int tempCount = 0;
    string itemTempName = default;
    int itemTempCount = 0;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy( gameObject );
        }
    }
    private void Start()
    {

        auth = FirebaseAuth.DefaultInstance;
        appleCount = 10;
    }
    public void AddApple()
    {

        WriteItemInfo("Food", "Apple", appleCount);

        WriteItemInfo("Food", "Pie", 3);
        WriteItemInfo("Equipment", "Sword", 1);
        WriteItemInfo("Stuff", "Wood", 20);
    }

    public void RemoveItem(string itemType_, string itemcode_)
    {
        m_Reference = FirebaseDatabase.DefaultInstance.GetReference("users");
        m_Reference.Child(auth.CurrentUser.UserId).Child("Inventory").Child(itemType_).Child(itemcode_).RemoveValueAsync();
    }

    public void WriteItemInfo(string itemType_, string itemcode_, int itemCount_)
    {
        m_Reference = FirebaseDatabase.DefaultInstance.GetReference("users");
        Inventory inventory = new Inventory(itemcode_, itemCount_);
        string addItem = JsonUtility.ToJson(inventory);
        m_Reference.Child(auth.CurrentUser.UserId).Child("Inventory").Child(itemType_).Child(itemcode_).SetRawJsonValueAsync(addItem);
    }
    public void ReadItemInfo(string itemType)
    {

        m_Reference = FirebaseDatabase.DefaultInstance.GetReference("users");

        m_Reference.Child(auth.CurrentUser.UserId).Child("Inventory").Child(itemType).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("¿–æÓø¿±‚ Ω«∆–");
            }
            else if (task.IsCanceled)
            {
                Debug.Log("task √Îº“µ ");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.LogFormat("childrencount {1}", task, snapshot.ChildrenCount);
                foreach (var item in snapshot.Children)
                {
                    Debug.Log(item.Key);


                    foreach (var snapshotChild in item.Children)
                    {
                        switch (itemType)
                        {
                            case "Food":


                                if (snapshotChild.Value is string)
                                {

                                    string value = (string)snapshotChild.Value;
                                    itemTempName = (string)snapshotChild.Value;
                                    Debug.LogFormat("itemTempName : {0}", itemTempName);

                                }
                                else if (snapshotChild.Value is Int64)
                                {

                                    long tempLong = (long)snapshotChild.Value;

                                    itemTempCount = (int)tempLong;
                                    Debug.LogFormat("itemTempCount : {0}", itemTempCount);
                                }

                                tempCount += 1;
                                if (tempCount == 2)
                                {
                                    foodDict.Add(itemTempName, itemTempCount);
                                    tempCount = 0;
                                }

                                break;
                            case "Stuff":
                                if (snapshotChild.Value is string)
                                {

                                    string value = (string)snapshotChild.Value;
                                    itemTempName = (string)snapshotChild.Value;
                                    Debug.LogFormat("itemTempName : {0}", itemTempName);

                                }
                                else if (snapshotChild.Value is Int64)
                                {

                                    long tempLong = (long)snapshotChild.Value;

                                    itemTempCount = (int)tempLong;
                                    Debug.LogFormat("itemTempCount : {0}", itemTempCount);
                                }

                                tempCount += 1;
                                if (tempCount == 2)
                                {
                                    stuffDict.Add(itemTempName, itemTempCount);
                                    tempCount = 0;
                                }
                                break;
                            case "Equipment":
                                if (snapshotChild.Value is string)
                                {

                                    string value = (string)snapshotChild.Value;
                                    itemTempName = (string)snapshotChild.Value;
                                    Debug.LogFormat("itemTempName : {0}", itemTempName);

                                }
                                else if (snapshotChild.Value is Int64)
                                {

                                    long tempLong = (long)snapshotChild.Value;

                                    itemTempCount = (int)tempLong;
                                    Debug.LogFormat("itemTempCount : {0}", itemTempCount);
                                }

                                tempCount += 1;
                                if (tempCount == 2)
                                {
                                    equipmentDict.Add(itemTempName, itemTempCount);
                                    tempCount = 0;
                                }

                                break;
                            default: break;
                        }
                        //Debug.Log(snapshotChild.Key + " " + snapshotChild.Value);

                    }
                }

                Debug.Log("Adding items to foodDict completed.");
            }
        });
    }
    public void ShowInventory()
    {
        // Food µÒº≈≥ ∏Æ
        Debug.Log("Food µÒº≈≥ ∏Æ:");
        foreach (KeyValuePair<string, int> item in foodDict)
        {
            Debug.LogFormat("æ∆¿Ã≈€: {0}, ∞≥ºˆ: {1}", item.Key, item.Value);
        }

        // Stuff µÒº≈≥ ∏Æ
        Debug.Log("Stuff µÒº≈≥ ∏Æ:");
        foreach (KeyValuePair<string, int> item in stuffDict)
        {
            Debug.LogFormat("æ∆¿Ã≈€: {0}, ∞≥ºˆ: {1}", item.Key, item.Value);
        }

        // Equipment µÒº≈≥ ∏Æ
        Debug.Log("Equipment µÒº≈≥ ∏Æ:");
        foreach (KeyValuePair<string, int> item in equipmentDict)
        {
            Debug.LogFormat("æ∆¿Ã≈€: {0}, ∞≥ºˆ: {1}", item.Key, item.Value);
        }
    }
}


public class Inventory
{
    public string itemCode;
    public int itemCount;
    public Inventory(string itemCode, int itemCount)
    {
        this.itemCode = itemCode;
        this.itemCount = itemCount;
    }
}
