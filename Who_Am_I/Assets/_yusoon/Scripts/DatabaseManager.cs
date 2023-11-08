using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    private static DatabaseManager _instance;
    
    public static DatabaseManager Instance { get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DatabaseManager>();
            }
            return _instance;
        }
        
    }
    DatabaseReference m_Reference;
    private string databaseUrl = "https://who-am-i-33994-default-rtdb.firebaseio.com/";

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri( databaseUrl);
        m_Reference = FirebaseDatabase.DefaultInstance.RootReference;
        ReadUserData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReadUserData()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result; 
                for (int i = 0; i < snapshot.ChildrenCount; i++)
                {
                    Debug.Log(snapshot.Child(i.ToString()).Child("username").Value);
                }
            }
        }
        );
    }
    public void WriteUserData(string userId_, string userName_)
    {
        var data=new UserData(userId_, userName_);
        string jsondata=JsonUtility.ToJson(data);
        m_Reference.Child("users").Child(userId_).SetRawJsonValueAsync(jsondata)
        .ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("WriteUserData 실패: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log(jsondata + " 전송완료");
            }
        });
    }
    
}
public class UserData
{
    public string userId;
    public string userName;

    public UserData(string userId_, string userName_)
    {
        this.userId = userId_;
        this.userName = userName_;
    }
}
