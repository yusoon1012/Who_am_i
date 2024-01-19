using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    public Transform mainObjTf;
    public GameObject dialogObj;
    public Text dialogNpcNameText;
    public Text dialogText;
    public Text nextText;

    public bool nextDialogCheck { get; set; } = false;

    private void Awake()
    {
        if (instance == null || instance == default)
        {
            instance = this; DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }     // Awake()

    #region Junoh: Refactor
    public void PrintText(string _name, string _dialog)
    {
        ActivePrint(true);

        // 대화 창 이름을 가져온 대화 정보에서 NPC 이름으로 출력함
        dialogNpcNameText.text = string.Format("{0}", _name);
        // 대화 창 내용을 가져온 대화 정보에서 대화 순서를 참고하여 출력함
        dialogText.text = string.Format("{0}", _dialog);
    }

    public void NonPrintText()
    {
        ActivePrint(false);
    }

    private void ActivePrint(bool _isActive)
    {
        dialogObj.gameObject.SetActive(_isActive);
    }
    #endregion

    public void PrintDialog(string name, string dialog)
    {
        if (nextDialogCheck == true) { return; }

        mainObjTf.GetComponent<UIController>().uiController = 12;

        // 대화 창 오브젝트를 활성화함
        dialogObj.gameObject.SetActive(true);
        // 대화 창 이름을 가져온 대화 정보에서 NPC 이름으로 출력함
        dialogNpcNameText.text = string.Format("{0}", name);
        // 대화 창 내용을 가져온 대화 정보에서 대화 순서를 참고하여 출력함
        dialogText.text = string.Format("{0}", dialog);

        StartCoroutine(PrintNextText());
    }     // PrintDialog()

    public void InputDialog()
    {
        if (nextDialogCheck == false) { return; }

        // 대화 창 이름을 가져온 대화 정보에서 NPC 이름으로 출력함
        dialogNpcNameText.text = string.Format(" ");
        // 대화 창 내용을 가져온 대화 정보에서 대화 순서를 참고하여 출력함
        dialogText.text = string.Format(" ");
        // 다음 표시 텍스트를 비활성화 시킴
        nextText.gameObject.SetActive(false);
        // 대화 창 오브젝트를 활성화함
        dialogObj.gameObject.SetActive(false);

        nextDialogCheck = false;
        mainObjTf.GetComponent<UIController>().uiController = 0;

        //* Feat : 대화 종료 정보를 전달해야함
    }     // InputDialog()

    IEnumerator PrintNextText()
    {
        yield return new WaitForSeconds(1f);

        // 다음 표시를 활성화 된 상태로 변경함
        nextDialogCheck = true;
        // 다음 표시 텍스트를 활성화 시킴
        nextText.gameObject.SetActive(true);
    }     // PrintNextText()
}
