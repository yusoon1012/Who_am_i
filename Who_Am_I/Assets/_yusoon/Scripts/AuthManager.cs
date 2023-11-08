using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using Firebase.Extensions;

public class AuthManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TMP_InputField userNameField;
    [SerializeField] private GameObject userNameInput;

    FirebaseAuth auth;
    bool isUserNameValid;
    string userName=default;
    // Start is called before the first frame update
    void Start()
    {
        userName = PlayerPrefs.GetString("USERNAME");
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
                    Debug.Log(emailField.text + "계정으로 로그인 하였습니다.");
                    if (userName == "")
                    {
                        isUserNameValid = true;
                        Debug.Log("저장된 닉네임 없음");
                    }

                   
                }
                else
                {
                    Debug.Log("로그인에 실패하였습니다.");
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
                    Debug.Log(emailField.text + "계정으로 회원가입되었습니다.");
                }
                else
                {
                    Debug.Log("회원가입에 실패하였습니다.");
                }
            }
            );
    }
    public void SetUserName()
    {
        userName = userNameField.text;
        PlayerPrefs.SetString("USERNAME", userName);
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
    }
}
