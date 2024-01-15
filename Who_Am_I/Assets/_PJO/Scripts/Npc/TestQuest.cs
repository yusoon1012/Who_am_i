//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;

//public class TestQuest : MonoBehaviour
//{
//    #region Singleton
//    public static TestQuest instance = null;

//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//        }

//        DontDestroyOnLoad(this.gameObject);
//    }
//    #endregion

//    #region 뭉비타이에 온 걸 환영해
//    public string[] QuestStart0001(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "TamoWife":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0001"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0002"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0003"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0004"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0005"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0006"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0007"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestProgress0001(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "TamoWife":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0008"]);
//                break;
//            case "Buddle":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0009"]);
//                break;
//            case "Lita":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0010"]);
//                break;
//            case "Mokgu":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0011"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestComplete0001(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "TamoWife":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0012"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0013"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0014"]);
//                break;
//        }

//        return newList.ToArray();
//    }
//    #endregion

//    #region 뭉비타이에서 살아남는 방법
//    public string[] QuestStart0002(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Buddle":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0015"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0016"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0017"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0018"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0019"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0020"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0021"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0022"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestProgress0002(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Buddle":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0023"]);
//                break;
//            case "Bertturu":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0024"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0025"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0026"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0027"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0028"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0029"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestComplete0002(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Buddle":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0030"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0031"]);
//                break;
//        }

//        return newList.ToArray();
//    }
//    #endregion

//    #region 이번엔 고기 앞으로!
//    public string[] QuestStart0003(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Lita":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0032"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0033"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0034"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0035"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0036"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0037"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0038"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestProgress0003(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Lita":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0039"]);
//                break;
//            case "Ricota":        // 이거 맞아?
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0040"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0041"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestComplete0003(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Lita":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0042"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0043"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0044"]);
//                break;
//        }

//        return newList.ToArray();
//    }
//    #endregion

//    #region 오늘은 내가 뭉비타이 요리사~
//    public string[] QuestStart0004(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Mokgu":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0045"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0046"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0047"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0048"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0049"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0050"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestProgress0004(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Mokgu":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0051"]);
//                break;
//            case "Momoksu":        // 이거 맞아?
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0052"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0053"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0054"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0055"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0056"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0057"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestComplete0004(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Mokgu":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0058"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0059"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0060"]);
//                break;
//        }

//        return newList.ToArray();
//    }
//    #endregion

//    #region 구원의 시작
//    public string[] QuestStart0005(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Tatamu":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0061"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0062"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0063"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0064"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0065"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0066"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0067"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0068"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0069"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0070"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0071"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0072"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestProgress0005(string _name)
//    {
//        List<string> newList = new List<string>();

//        return newList.ToArray();
//    }

//    public string[] QuestComplete0005(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Tatamu":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0073"]);
//                break;
//        }

//        return newList.ToArray();
//    }
//    #endregion

//    #region 내가 구원자라고?
//    public string[] QuestStart0006(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Plterny":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0074"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0075"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0076"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0077"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0078"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0079"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0080"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0081"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0082"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestProgress0006(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Plterny":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0083"]);
//                break;
//            case "Piel":        // 이거 맞아?
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0084"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0085"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0086"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0087"]);
//                break;
//            case "Hanna":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0088"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0089"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0090"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0091"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestComplete0006(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Plterny":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0092"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0093"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0094"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0095"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0096"]);
//                break;
//        }

//        return newList.ToArray();
//    }
//    #endregion

//    #region 삐리뽀가 사라졌다!
//    public string[] QuestStart0007(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Tiring":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0097"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0098"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0099"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0100"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0101"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0102"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0103"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0104"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestProgress0007(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Tiring":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0105"]);
//                break;
//            case "Sana":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0106"]);
//                break;
//            case "Tina":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0107"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0108"]);
//                break;
//            case "Piribbo":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0109"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0110"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0111"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0112"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0113"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0114"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestComplete0007(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Tiring":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0115"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0116"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0117"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0118"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0119"]);
//                break;
//        }

//        return newList.ToArray();
//    }
//    #endregion

//    #region 실건실제
//    public string[] QuestStart0008(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Libi":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0120"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0121"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0122"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0123"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0124"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0125"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0126"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestProgress0008(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Libi":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0127"]);
//                break;
//            case "Jirak":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0128"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0129"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0130"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0131"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestComplete0008(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Libi":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0132"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0133"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0134"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0135"]);
//                break;
//        }

//        return newList.ToArray();
//    }
//    #endregion

//    #region 신나는 보물 찾기!
//    public string[] QuestStart0009(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Cripto":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0136"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0137"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0138"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0139"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0140"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0141"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0142"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0143"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestProgress0009(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Cripto":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0144"]);
//                break;
//            case "Masito":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0145"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0146"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0147"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0148"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0149"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0150"]);
//                break;
//            case "Pasito":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0151"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0152"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0153"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0154"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0155"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0156"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestComplete0009(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Cripto":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0157"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0158"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0159"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0160"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0161"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0162"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0163"]);
//                break;
//        }

//        return newList.ToArray();
//    }
//    #endregion

//    #region 밝혀진 진실
//    public string[] QuestStart0010(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Treete":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0164"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0165"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0166"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0167"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0168"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0169"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0170"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0171"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0172"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0173"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0174"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0175"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0176"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0177"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestProgress0010(string _name)
//    {
//        List<string> newList = new List<string>();

//        return newList.ToArray();
//    }

//    public string[] QuestComplete0010(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Treete":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0178"]);
//                break;
//        }

//        return newList.ToArray();
//    }
//    #endregion

//    #region 가을 타나봐
//    public string[] QuestStart0011(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Broli":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0179"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0180"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0181"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0182"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0183"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0184"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0185"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0186"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0187"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0188"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestProgress0011(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Roliroli":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0189"]);
//                break;
//            case "Puhihing":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0190"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestComplete0011(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Broli":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0191"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0192"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0193"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0194"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0195"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0196"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0197"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0198"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0199"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0200"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0201"]);
//                break;
//        }

//        return newList.ToArray();
//    }
//    #endregion

//    #region 고구마를 뽑아보자!
//    public string[] QuestStart0012(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Pungdana":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0202"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0203"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0204"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0205"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0206"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0207"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestProgress0012(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Pongdang":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0208"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0209"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0210"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0211"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0212"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0213"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestComplete0012(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Pungdana":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0214"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0215"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0216"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0217"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0218"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0219"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0220"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0221"]);
//                break;
//        }

//        return newList.ToArray();
//    }
//    #endregion

//    #region 풍요로운 단풍 마을
//    public string[] QuestStart0013(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Ssamung":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0222"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0223"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0224"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0225"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0226"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0227"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0228"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestProgress0013(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Moaong":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0229"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0230"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0231"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0232"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0233"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestComplete0013(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Ssamung":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0234"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0235"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0236"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0237"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0238"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0239"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0240"]);
//                break;
//        }

//        return newList.ToArray();
//    }
//    #endregion

//    #region 겨울을 책임 질게
//    public string[] QuestStart0014(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Brauning":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0241"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0242"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0243"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0244"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0245"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0246"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0247"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0248"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestProgress0014(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Laoni":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0249"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0250"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0251"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0252"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0253"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0254"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0255"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0256"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0257"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestComplete0014(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Brauning":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0258"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0259"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0260"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0261"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0262"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0263"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0264"]);
//                break;
//        }

//        return newList.ToArray();
//    }
//    #endregion

//    #region 집으로 가는 길
//    public string[] QuestStart0015(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Dandanmung":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0265"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0266"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0267"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0268"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0269"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0270"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0271"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0272"]);
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0273"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    public string[] QuestProgress0015(string _name)
//    {
//        List<string> newList = new List<string>();

//        return newList.ToArray();
//    }

//    public string[] QuestComplete0015(string _name)
//    {
//        List<string> newList = new List<string>();

//        switch (_name)
//        {
//            case "Dandanmung":
//                newList.Add(GameManager.instance.stringTable["Quest_Main_Npc_Script_0274"]);
//                break;
//        }

//        return newList.ToArray();
//    }

//    // 추가 꿀비, Ggulbi
//    // 추가 윈터닝, Winterning
//    // 추가 누니, Nuni
//    // 추가 울라워, Ulower
//    // 추가 눈눈뭉, Nunnunmung
//    #endregion
//}