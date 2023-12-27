using UnityEngine;

public class Dialogs001 : DialogsMain
{
    public Dialogs001()
    {
        Init();
    }     // Dialogs001()

    public override void Init()
    { 
        npcName = "연두";

        dialogs[0] = "안녕, 이방인! 나는 연두라고 해! 모든 것이 새롭고 낯설지?";
        dialogs[1] = "나의 역할은 이방인들이 이 세계에 잘 적응할 수 있도록 돕는 것이야.";
        dialogs[2] = "우선 나를 따라와! 사람들이 있는 곳으로 데려다줄게!";
        dialogs[3] = "어서 나를 따라와! 30 초만 기다려준다 ~";

        maxDialog = 3;
    }     // Init()
}
