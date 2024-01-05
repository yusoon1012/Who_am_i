using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;

public class SaveDatabase : MonoBehaviour
{


    public GameObject player;
    SaveUserData userData=new SaveUserData();
    DatabaseReference m_reference;
    private string sceneName;
    private float posX;
    private float posY;
    private float posZ;
    private Vector3 userPos;
    private bool isOnLoad=false;
    private bool fireBaseLoad = false;
    private void Awake()
    {
    
     
        m_reference= FirebaseDatabase.DefaultInstance.GetReference("users");     
        Load();
    }

   
    private void Update()
    {
        if(isOnLoad==false&&sceneName==SceneManager.GetActiveScene().name)
        {
            isOnLoad = true;
            Debug.Log("씬 로드완료");
            SetUserPosition();
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            Save();
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            SetUserPosition();
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            SetUserScene();
        }
    }
    public void Save()
    {
        userData.sceneName = SceneManager.GetActiveScene().name;
        userData.lastPos = player.transform.position;
        string data = JsonUtility.ToJson(userData);
        m_reference.SetRawJsonValueAsync(data).ContinueWith(task =>
        { 
            if(task.IsFaulted)
            {
                Debug.Log("Save Faulted");
            }
            else if(task.IsCanceled)
            {
                Debug.Log("Save Canceled");
            }
            else if(task.IsCompleted)
            {
                Debug.Log("Save Complete");
                Debug.Log("SceneName : " + userData.sceneName);
                Debug.Log("lastPos : " + userData.lastPos);
            }
        });

    }
    public void Load()
    {
        m_reference.GetValueAsync().ContinueWith(task =>
        {
            if(task.IsFaulted)
            {
                Debug.LogWarning("Load task Faulted");
            }
            else if(task.IsCanceled)
            {
                Debug.LogWarning("Load task Canceled");

            }
            else if(task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var data in snapshot.Children)
                {
                    if (data.Key == "sceneName")
                    {
                        sceneName = (string)data.Value;
                        Debug.Log("SceneName : " + sceneName);
                    }
                    if (data.Key == "lastPos")
                    {
                        Debug.Log("in lastPos");
                        foreach (var pos in data.Children)
                        {
                            Debug.Log(pos.Key + ":" + pos.Value);
                            switch (pos.Key)
                            {
                                case "x":
                                    //Debug.Log("posX Type : " + pos.Value.GetType());
                                    double tempPosX = (double)pos.Value;
                                    posX = (float)tempPosX;
                                    Debug.Log("posX : " + posX);
                                    break;
                                case "y":
                                    // Debug.Log("posX Type : " + pos.Value.GetType());
                                    double tempPosY = (double)pos.Value;
                                    posY = (float)tempPosY;
                                    Debug.Log("posY : " + posY);
                                    break;
                                case "z":
                                    //Debug.Log("posX Type : " + pos.Value.GetType());
                                    double tempPosZ = (double)pos.Value;
                                    posZ = (float)tempPosZ;
                                    Debug.Log("posZ : " + posZ);
                                    break;
                                default:
                                    break;

                            }

                        }


                    }
                }
                fireBaseLoad = true;

            }
        });
        Debug.Log("Load 종료");
    }

    public void SetUserPosition()
    {
        player = GameObject.Find("PlayerController");
        Debug.Log("SetUserPosition");
        userPos = new Vector3(posX, posY, posZ);
        Debug.Log("userPos : " + userPos);
        player.transform.position = userPos;
        Debug.Log("Player gameObject : " + player.gameObject.name);
    }

    public void SetUserScene()
    {
        if(sceneName==null||sceneName==default)
        {
            Load();
            SceneManager.LoadScene(sceneName);

        }
        SceneManager.LoadScene(sceneName);
    }
}
