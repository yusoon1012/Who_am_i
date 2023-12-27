using UnityEngine;

public class DialogsMain
{
    // 대화창의 최대 갯수
    public int maxDialog = default;
    // 출력할 NPC 이름
    public string npcName = default;
    // 출력할 대화 텍스트 목록
    public string[] dialogs = new string[30];

    public virtual void Init()
    {
        /* Empty */
    }     // Init()
}
