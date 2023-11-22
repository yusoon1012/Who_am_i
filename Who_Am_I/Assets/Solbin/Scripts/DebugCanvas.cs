using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugCanvas : MonoBehaviour
{
    Dictionary<string, int> debugLogs = new Dictionary<string, int>();

    public TMP_Text display;

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog; // 이 부분 수정
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Warning)
        {
            string[] splitString = logString.Split(char.Parse(":"));
            string debugKey = splitString[0];
            string debugValue = splitString.Length > 1 ? splitString[1] : "";

            if (debugLogs.ContainsKey(debugKey))
                debugLogs[debugKey]++;
            else
                debugLogs.Add(debugKey, 1);
        }

        string displayText = "";
        foreach (KeyValuePair<string, int> log in debugLogs)
        {
            if (log.Value == 1)
                displayText += log.Key + "\n";
            else
                displayText += log.Key + ": " + log.Value + "\n";
        }

        display.text = displayText;
    }
}
