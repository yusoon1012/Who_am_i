using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReader : MonoBehaviour
{
    Dictionary<string, string> questNpcScripts;
    public QuestMainTable[] table;
    public QuestProccessTable[] proccessTables1;
    private void Start()
    {
        table= MainQuestParse();
        //proccessTables1= QuestProccessParse("Quest_Main_1");
        //LoadMainQuests();
        //QuestAdvance();
        //LoadString();
    }
    QuestMainTable[] MainQuestParse()
    {
        List<QuestMainTable> mainQuestList= new List<QuestMainTable>();
        TextAsset csvData=Resources.Load<TextAsset>("QuestMainTable");
        string[] data = csvData.text.Split(new char[] { '\n' });
        for (int i = 1; i < data.Length;i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            QuestMainTable questMainTable = new QuestMainTable();
            for (int j=0;j<row.Length;j++)
            {
               // Debug.Log("Row " + i + ", Column " + j + ": " + row[j]);
                switch(j)
                {
                    case 1:
                        questMainTable.questName = row[j];
                        break;
                    case 2:
                        questMainTable.questTarget = row[j];
                        break;
                        case 3:
                        questMainTable.questRocation = row[j];
                        break;
                        case 4:
                        int parsedValue;
                        if (!string.IsNullOrEmpty(row[j]))
                        {
                            if (int.TryParse(row[j], out parsedValue))
                            {
                                // 정수로 성공적으로 변환되면 parsedValue를 사용
                                questMainTable.previousQuest = parsedValue;
                            }
                            else
                            {
                                // 변환에 실패한 경우 처리 (예: 경고 로그 기록)
                                Debug.LogWarning($"{i}행, {j}열에서 {row[j]}를 정수로 변환하는 데 실패했습니다.");
                            }
                        }
                        else
                        {
                            // 문자열이 비어 있거나 null인 경우 처리
                            Debug.LogWarning($"{i}행, {j}열에서 빈 값 또는 null을 발견했습니다.");
                        }
                        break;
                    case 5:
                        questMainTable.proccessCode = row[j];
                        questMainTable.questProccessTables = QuestProccessParse(row[j]);
                        break;
                    case 7:
                        questMainTable.questType = row[j];
                        break;
                        case 8:
                     
                        if (!string.IsNullOrEmpty(row[j]))
                        {
                            if (int.TryParse(row[j], out parsedValue))
                            {
                                // 정수로 성공적으로 변환되면 parsedValue를 사용
                                questMainTable.valueCount = parsedValue;
                            }
                            else
                            {
                                // 변환에 실패한 경우 처리 (예: 경고 로그 기록)
                                Debug.LogWarning($"{i}행, {j}열에서 {row[j]}를 정수로 변환하는 데 실패했습니다.");
                            }
                        }
                        else
                        {
                            // 문자열이 비어 있거나 null인 경우 처리
                            Debug.LogWarning($"{i}행, {j}열에서 빈 값 또는 null을 발견했습니다.");
                        }
                        break;
                    case 9:
                        questMainTable.valueName = row[j];
                        break;
                        case 10:
                        questMainTable.resultName = row[j];
                        break;
                    case 11:
                       
                        if (!string.IsNullOrEmpty(row[j]))
                        {
                            if (int.TryParse(row[j], out parsedValue))
                            {
                                // 정수로 성공적으로 변환되면 parsedValue를 사용
                                questMainTable.resultCount = parsedValue;
                            }
                            else
                            {
                                // 변환에 실패한 경우 처리 (예: 경고 로그 기록)
                                Debug.LogWarning($"{i}행, {j}열에서 {row[j]}를 정수로 변환하는 데 실패했습니다.");
                            }
                        }
                        else
                        {
                            // 문자열이 비어 있거나 null인 경우 처리
                            Debug.LogWarning($"{i}행, {j}열에서 빈 값 또는 null을 발견했습니다.");
                        }
                        break;
                    default:
                        break;
                }
            }
            mainQuestList.Add(questMainTable);

        }
        return mainQuestList.ToArray();
    }


    QuestProccessTable[] QuestProccessParse(string questCode)
    {
        List<QuestProccessTable> proccessTables = new List<QuestProccessTable>();
        TextAsset csvData = Resources.Load<TextAsset>("QuestProccessTable");
        string[] data = csvData.text.Split(new char[] { '\n' });
        for(int i=1;i<data.Length;i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            QuestProccessTable questProccessTable = new QuestProccessTable();

            if (row[3]!=questCode)
            {
                continue;
            }
            else
            {

            
                int parsedValue;
                if (int.TryParse(row[0], out parsedValue))
                {
                questProccessTable.questId = parsedValue;
                   
                }
                if (int.TryParse(row[2], out parsedValue))
                {
                    questProccessTable.questRocation = parsedValue;

                }
                questProccessTable.questGroup = row[3];
                if (int.TryParse(row[4], out parsedValue))
                {
                    questProccessTable.proccessGroup = parsedValue;

                }
                if (int.TryParse(row[5], out parsedValue))
                {
                    questProccessTable.proccessCount = parsedValue;

                }
                questProccessTable.scriptType = row[6];
                questProccessTable.npcName = row[7];
                questProccessTable.scriptCode = row[8];
                questProccessTable.scriptCode= LoadString(questProccessTable.scriptCode);
                if (int.TryParse(row[12], out parsedValue))
                {
                    questProccessTable.condition1_Count = parsedValue;

                }
                questProccessTable.condition1_Mbti = row[13];
                if (int.TryParse(row[14], out parsedValue))
                {
                    questProccessTable.condition1_Value = parsedValue;

                }
                if (int.TryParse(row[17], out parsedValue))
                {
                    questProccessTable.condition2_Count = parsedValue;

                }
                questProccessTable.condition2_Mbti = row[18];
                if (int.TryParse(row[19], out parsedValue))
                {
                    questProccessTable.condition2_Value = parsedValue;

                }
            }
            
            for (int j=0;j<row.Length;j++)
            {
                

            }
            proccessTables.Add(questProccessTable);
        }
        return proccessTables.ToArray();
    }

    public string LoadString(string code)
    {
        TextAsset csvData = Resources.Load<TextAsset>("QuestStringTable");
        string[] data = csvData.text.Split(new char[] { '\n' });
        string talkScript;
        for (int i = 0; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });


            for (int j = 0; j < row.Length; j++)
            {
                row[j] = row[j].Replace("*", ",");
               // Debug.Log("Row " + i + ", Column " + j + ": " + row[j]);
                if (row[0]==code)
                {
                    talkScript= row[4];
                    return talkScript;
                }

            }
        }
        return default;
    }
}
