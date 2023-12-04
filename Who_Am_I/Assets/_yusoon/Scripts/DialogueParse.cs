using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParse : MonoBehaviour
{
   public Dialogue[] Parse(string _csvFileName)
    {
        
        
        List<Dialogue> dialogueList = new List<Dialogue>();
        TextAsset csvData = Resources.Load<TextAsset>(_csvFileName);//csv 파일 가져옴
        string[] data = csvData.text.Split(new char[] {'\n'});//엔터로 줄나눔

        for (int i = 0; i < data.Length;)
        {
            string[] row = data[i].Split(new char[] { ',' });

            Debug.Log(row[2]);
            Dialogue dialogue = new Dialogue();//대사 리스트 생성

            dialogue.name = row[2];
            List<string> contextList= new List<string>();
            List<string> eventNumberList = new List<string>();
            //List<string> jumpList = new List<string>();
            do
            {
                contextList.Add(row[3]);
                eventNumberList.Add(row[4]);
                //jumpList.Add(row[5]);
                Debug.Log(row[3]);// 스크립트

                if (++i < data.Length)
                {
                    row = data[i].Split(new char[] { ',' });
                }
                else
                {
                    break;
                }
            } while (row[0].ToString()=="");
            dialogue.number = eventNumberList.ToArray();
            //dialogue.jumpNumber = jumpList.ToArray();
            dialogue.contexts = contextList.ToArray();
            dialogueList.Add(dialogue);
           
            



        }
        return dialogueList.ToArray();
    }       //Parse()

    private void Start()
    {
        Parse("Tutorial_01");
    }
}
