using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class AuthManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TMP_InputField userNameField;
    [SerializeField] private GameObject userNameInput;

    FirebaseAuth auth;
    bool isUserNameValid;
    bool isLogin = false;
    string userName=default;

    // Start is called before the first frame update
    void Start()
    {
        
        auth = FirebaseAuth.DefaultInstance;
        userNameInput.SetActive(false);
        


    }

    public void Login()
    {
        auth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWith(
            task =>
            {
                if(task.IsCompleted&&!task.IsCanceled&&!task.IsFaulted)
                {
                    Debug.Log(emailField.text + "�������� �α��� �Ͽ����ϴ�.");
                    isLogin = true;
                    UserInfo.userId=emailField.text;
                   
                   
                }
                else
                {
                    Debug.Log("�α��ο� �����Ͽ����ϴ�.");
                }
            });
    }
    public void Register()
    {
        auth.CreateUserWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWith(
            task =>
            {
                if(!task.IsCanceled&&!task.IsFaulted)
                {
                    Debug.Log(emailField.text + "�������� ȸ�����ԵǾ����ϴ�.");
                    Debug.Log(userName);
                    if (userName == null)
                    {
                        isUserNameValid = true;
                        Debug.Log("����� �г��� ����");
                    }
                }
                else
                {
                    Debug.Log("ȸ�����Կ� �����Ͽ����ϴ�.");
                }
            }
            );
    }
    public void SetUserName()
    {
        userName = userNameField.text;
       
        isUserNameValid = false;
        userNameInput.SetActive(false);
        DatabaseManager.Instance.WriteUserData(emailField.text, userName);

    }
  
    // Update is called once per frame
    void Update()
    {
        if(isUserNameValid) 
        {
            userNameInput.SetActive(true);

        }
        if(isLogin)
        {
            SceneManager.LoadScene("NextScene");
        }
    }
}
