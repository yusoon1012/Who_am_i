using UnityEngine;

public class Dialogs002 : DialogsMain
{
    public Dialogs002()
    {
        Init();
    }     // Dialogs002()

    public override void Init()
    {
        npcName = "빨강";

        dialogs[0] = "안녕! 난 빨강이라고 해!";

        maxDialog = 0;
    }     // Init()
}
