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
using UnityEngine.UI;
using Unity.VisualScripting;

public class AuthManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TMP_InputField userNameField;
    [SerializeField] private TMP_Text errorMessage;
    [SerializeField] private GameObject userNameInput;
    [SerializeField] private Image seePassword;
    [SerializeField] private Image cantSeePassword;
    [SerializeField] private GameObject loginUI;
    [SerializeField] private GameObject door;
    FirebaseAuth auth;
    bool isUserNameValid;
    bool isLogin = false;
    bool isError = false;
    Vector3 doorOpenPos = new Vector3(3.43f, 0.17f, 13.83f);
    private bool isShowPassword = false;
    string userName=default;
    Firebase.Auth.AuthError authError;
    // Start is called before the first frame update
    void Start()
    {
        seePassword.enabled = false;
        cantSeePassword.enabled = true;
        auth = FirebaseAuth.DefaultInstance;
        userNameInput.SetActive(false);
        isShowPassword = false;
        errorMessage.enabled = false;
        //StartCoroutine(DoorOpen());
    }

    public void ToggleShowPassword()
    {
        isShowPassword = !isShowPassword; 
       
        if (isShowPassword)
        {
           

            passwordField.contentType = TMP_InputField.ContentType.Standard;
            passwordField.Select();
            passwordField.caretPosition = passwordField.text.Length;
            seePassword.enabled = true;
            cantSeePassword.enabled = false;
        }
        else
        {
      

            passwordField.contentType = TMP_InputField.ContentType.Password;
            passwordField.Select();
            passwordField.caretPosition = passwordField.text.Length;

            seePassword.enabled = false;
            cantSeePassword.enabled = true;
        }
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
                    isError = false;


                }
                else
                {
                    if (task.Exception != null)
                    {
                        Firebase.FirebaseException firebaseException = task.Exception.GetBaseException() as Firebase.FirebaseException;

                        if (firebaseException != null)
                        {
                            authError = (Firebase.Auth.AuthError)firebaseException.ErrorCode;
                            isError = true;

                        }

                      
                        //errorMessage.text = "로그인에 실패하였습니다. 이메일 혹은 비밀번호를 확인해주세요.";

                    }

                }
                   
            });
         if(isError)
        {
            

            StartCoroutine(ErrorChange());
        }
         else
        {
            errorMessage.text = "로그인중...";
            errorMessage.enabled = true;
            loginUI.SetActive(false);
            StartCoroutine(DoorOpen());
        }

    }
    public IEnumerator DoorOpen()
    {
        Vector3 initPos = door.transform.position;
        float elapsedTime = 0f;
        float duration = 3f; // 열리는 데 걸리는 시간 (조절 가능)

        while (elapsedTime < duration)
        {
            yield return null;

            // 시간에 따라 현재 위치를 업데이트
            elapsedTime += Time.deltaTime;

            // Lerp를 사용하여 현재 위치를 업데이트
            door.transform.position = Vector3.Lerp(initPos, doorOpenPos, elapsedTime / duration);
        }

        // 열리는 동안의 보정
        door.transform.position = doorOpenPos;
    }
    private IEnumerator ErrorChange()
    {
        yield return new WaitForSeconds(0.5f);
        
        switch (authError)
        {
            case Firebase.Auth.AuthError.WrongPassword:
                Debug.Log("잘못된 비밀번호입니다.");
                break;
            case Firebase.Auth.AuthError.InvalidEmail:

                errorMessage.text = "등록되지않은 이메일입니다.";
                Debug.Log("등록되지않은 이메일입니다.");
                break;
            case Firebase.Auth.AuthError.MissingPassword:

                errorMessage.text = "비밀번호를 입력해주세요.";
                break;
            case Firebase.Auth.AuthError.Failure:
                //Debug.Log("로그인에 실패하였습니다. 오류 메시지: " + firebaseException.Message);

                errorMessage.text = "로그인에 실패하였습니다.\n 이메일 혹은 비밀번호를 확인해주세요.";
                break;
            default:
                errorMessage.text = "로그인에 실패하였습니다.\n 이메일 혹은 비밀번호를 확인해주세요.";

                Debug.Log("로그인에 실패하였습니다. Error Code: " + authError);
                break;
            case Firebase.Auth.AuthError.EmailAlreadyInUse:
                // Handle the case where the email is already in use
                errorMessage.text = "이미 사용중인 이메일 입니다.";

                Debug.Log("사용중인 이메일입니다.");
                break;
            case Firebase.Auth.AuthError.WeakPassword:
                Debug.Log("비밀번호의 보안이 약합니다.7자리 이상으로 설정해 주십시오.");
                errorMessage.text = "비밀번호의 보안이 약합니다.\n7자리 이상으로 설정해 주십시오.";

                break;
            // Add more cases for other error codes if needed

        }
        errorMessage.enabled = true;
    }

    public void Register()
    {
        auth.CreateUserWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWith(
            task =>
            {
                if(!task.IsCanceled&&!task.IsFaulted)
                {
                    isError = false;
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
                            isError = true;
                            authError = (Firebase.Auth.AuthError)firebaseException.ErrorCode;

                        }
                    }
                    Debug.Log("회원가입에 실패하였습니다.");
                }
            }
            );
        if (isError)
        {


            StartCoroutine(ErrorChange());
        }
    }
    public void SetUserName()
    {
        userName = userNameField.text;
       
        isUserNameValid = false;
        userNameInput.SetActive(false);
        DatabaseManager.Instance.WriteUserData(emailField.text, userName);

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SceneManager.LoadScene("MainTitle");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        
    }
}
