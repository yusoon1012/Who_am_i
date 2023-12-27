using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest000 : QuestsMain
{
    public Quest000()
    {
        Init();
    }

    public override void Init()
    {
        questNum = 0;

        questDialogs[0, 0] = "안녕!";
        questDialogs[0, 1] = "갑자기 고기가 먹고싶어!";
        questDialogs[0, 2] = "나에게 고기 10 개만 가져다 줄래?";

        questDialogs[1, 0] = "아직 고기 10 개는 구하지 못한거야?";

        questDialogs[2, 0] = "고마워! 얼마만에 먹어보는 고기야!";
        questDialogs[2, 1] = "넌 정말 착한애구나! 내가 너에게 선물을 줄게!";

        questDialogs[3, 0] = "저번에 고기는 정말 맛있었어! 고마워!";

        doneNpc = "QuestNPC";

        lootItem = "고기";

        lootCount = 10;

        completeItem = "딸기 우유";

        completeCount = 1;

        questType = QuestType.CONDITION;

        questState = QuestState.NONE;
    }
}
