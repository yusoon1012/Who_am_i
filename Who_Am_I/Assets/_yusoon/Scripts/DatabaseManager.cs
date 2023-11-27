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
        m_Reference = FirebaseDatabase.DefaultInstance.GetReference("users");
        ReadUserData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReadUserData()
    {
        DatabaseReference userDB=FirebaseDatabase.DefaultInstance.GetReference("users");
        userDB.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            Debug.Log("����?");
            if (task.IsFaulted) 
            {
                Debug.LogError("READ ERROR"+task.Exception);
            }
            else if(task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log("ChildrenCount : "+snapshot.ChildrenCount);
                foreach(var user_ in snapshot.Children)
                {
                    Debug.Log(user_.Key + " " + user_.Value);
                    

                }
            }
        });
    }
    public void WriteUserData(string userId_, string userName_)
    {
        //UserData data =new UserData(userId_, userName_);
        //string jsondata=JsonUtility.ToJson(data);
        //DatabaseReference userDB = FirebaseDatabase.DefaultInstance.GetReference("users");

        //userDB.Push().SetRawJsonValueAsync(jsondata).ContinueWith(task =>
        //{
        //    if (task.IsFaulted)
        //    {
        //        Debug.LogError("WriteUserData ����: " + task.Exception);
        //    }
        //    else if (task.IsCompleted)
        //    {
        //        Debug.Log(jsondata + " ���ۿϷ�");
        //    }
        //});
        UserData data = new UserData(userId_, userName_);
        string jsondata = JsonUtility.ToJson(data);
        DatabaseReference userDB = FirebaseDatabase.DefaultInstance.GetReference("users");

        string userUID = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser.UserId; // ������� UID ��������
        DatabaseReference userRef = userDB.Child(userUID); // ������� UID�� ���۷��� ����

        userRef.SetRawJsonValueAsync(jsondata).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("WriteUserData ����: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log(jsondata + " ���ۿϷ�");
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
