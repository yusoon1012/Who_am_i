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
                    Debug.Log(emailField.text + "계정으로 로그인 하였습니다.");
                    isLogin = true;
                    UserInfo.userId=emailField.text;
                   
                   
                }
                else
                {
                    if (task.Exception != null)
                    {
                        Firebase.FirebaseException firebaseException = task.Exception.GetBaseException() as Firebase.FirebaseException;

                        if (firebaseException != null)
                        {
                            Firebase.Auth.AuthError errorCode = (Firebase.Auth.AuthError)firebaseException.ErrorCode;

                            switch (errorCode)
                            {
                                case Firebase.Auth.AuthError.WrongPassword:
                                    Debug.Log("잘못된 비밀번호입니다.");
                                    break;
                                case Firebase.Auth.AuthError.InvalidEmail:
                                    Debug.Log("등록되지않은 이메일입니다.");
                                    break;
                                case Firebase.Auth.AuthError.Failure:
                                    Debug.Log("로그인에 실패하였습니다. 오류 메시지: " + firebaseException.Message);
                                    break;
                                default:
                                    Debug.Log("로그인에 실패하였습니다. Error Code: " + errorCode);
                                    break;
                            }
                        }
                    }
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
                    Debug.Log(userName);
                    if (userName == null)
                    {
                        isUserNameValid = true;
                        Debug.Log("저장된 닉네임 없음");
                        userNameInput.SetActive(true);

                    }
                }
                else 
                {
                    if (task.Exception != null)
                    {
                        Firebase.FirebaseException firebaseException = task.Exception.GetBaseException() as Firebase.FirebaseException;

                        if (firebaseException != null)
                        {
                            Firebase.Auth.AuthError errorCode = (Firebase.Auth.AuthError)firebaseException.ErrorCode;

                            switch (errorCode)
                            {
                                case Firebase.Auth.AuthError.EmailAlreadyInUse:
                                    // Handle the case where the email is already in use
                                    Debug.Log("Email already in use.");
                                    break;
                                case Firebase.Auth.AuthError.WeakPassword:
                                    Debug.Log("비밀번호의 보안이 약합니다.7자리 이상으로 설정해 주십시오.");
                                    break;
                                // Add more cases for other error codes if needed

                                default:
                                    Debug.Log("회원가입에 실패하였습니다. Error Code: " + errorCode);
                                    break;
                            }
                        }
                    }
                    Debug.Log("회원가입에 실패하였습니다.");
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
        
        
    }
}
