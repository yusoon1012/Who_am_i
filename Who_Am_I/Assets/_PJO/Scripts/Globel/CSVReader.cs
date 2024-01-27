using System.Collections.Generic;
using UnityEngine;

public static class CSVReader
{
    private const char DELIMITER = ',';      // CSV 파일에서 사용하는 구분자
    private const char LINE = '\n';          // CSV 파일에서 사용하는 라인 구분자

    /// <summary>
    /// CSV 파일을 읽어와 데이터를 아래 형식으로 저장하는 딕셔너리를 반환하는 메서드
    /// KEY     VALUE
    /// KEY     VALUE
    /// KEY     VALUE
    /// </summary>
    /// <param name="_csvFileName">CSV파일명</param>
    public static Dictionary<string, string> ReadCSVKeyString(string _csvFileName)
    {
        Dictionary<string, string> dataDictionary = new Dictionary<string, string>();

        TextAsset csvTextAsset = Resources.Load<TextAsset>(_csvFileName);

        if (csvTextAsset != null)
        {
            string[] lines = csvTextAsset.text.Split(LINE);

            if (lines.Length > 0)
            {
                foreach (string line in lines)
                {
                    string[] columns = line.Split(DELIMITER);

                    if (columns.Length >= 2)
                    {
                        string key = columns[0].Trim();
                        string value = columns[1].Trim();

                        dataDictionary[key] = value;
                    }
                }
            }
            else
            {
                Debug.Log("CSV file is empty.");
                return null;
            }
        }
        else
        {
            Debug.Log("CSV file not found: " + _csvFileName);
            return null;
        }

        return dataDictionary;
    }

    /// <summary>
    /// CSV 파일을 읽어와 데이터를 아래 형식으로 저장하는 딕셔너리를 반환하는 메서드
    /// KEY/KEY     KEY         KEY
    /// KEY         VALUE       VALUE
    /// KEY         VALUE       VALUE
    /// </summary>
    /// <param name="_csvFileName">CSV파일명</param>
    public static Dictionary<string, Dictionary<string, string>> ReadCSVKeyDictionary(string _csvFileName)
    {
        Dictionary<string, Dictionary<string, string>> dataDictionary = new Dictionary<string, Dictionary<string, string>>();

        TextAsset csvTextAsset = Resources.Load<TextAsset>(_csvFileName);

        if (csvTextAsset != null)
        {
            string[] lines = csvTextAsset.text.Split(LINE);

            if (lines.Length > 0)
            {
                string[] headers = lines[0].Trim().Split(DELIMITER);

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] values = lines[i].Trim().Split(DELIMITER);

                    if (values.Length == headers.Length)
                    {
                        string rowKey = values[0];

                        if (!dataDictionary.ContainsKey(rowKey))
                        {
                            dataDictionary[rowKey] = new Dictionary<string, string>();
                        }

                        for (int j = 1; j < values.Length; j++)
                        {
                            dataDictionary[rowKey][headers[j]] = values[j];
                        }
                    }
                }
            }
            else
            {
                Debug.Log("CSV file is empty.");
                return null;
            }
        }
        else
        {
            Debug.Log("CSV file not found: " + _csvFileName);
            return null;
        }

        return dataDictionary;
    }

    /// <summary>
    /// CSV 파일을 읽어와 데이터를 아래 형식으로 저장하는 딕셔너리를 반환하는 메서드
    /// KEY         KEY         KEY         ... INDEX(0)
    /// VALUE       VALUE       VALUE       ... INDEX(1)
    /// VALUE       VALUE       VALUE       ... INDEX(2)
    /// </summary>
    /// <param name="_csvFileName">CSV파일명</param>
    public static Dictionary<string, List<string>> ReadCSVKeyList(string _csvFileName)
    {
        Dictionary<string, List<string>> dataDictionary = new Dictionary<string, List<string>>();

        TextAsset csvTextAsset = Resources.Load<TextAsset>(_csvFileName);

        if (csvTextAsset != null)
        {
            string[] lines = csvTextAsset.text.Split(LINE);

            if (lines.Length > 0)
            {
                string[] headers = lines[0].Split(DELIMITER);

                foreach (string header in headers)
                {
                    dataDictionary.Add(header, new List<string>());
                }

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] values = line.Split(DELIMITER);

                    if (values.Length == headers.Length)
                    {
                        for (int j = 0; j < values.Length; j++)
                        {
                            dataDictionary[headers[j]].Add(values[j]);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("CSV file is empty.");
                return null;
            }
        }
        else
        {
            Debug.Log("CSV file not found: " + _csvFileName);
            return null;
        }

        return dataDictionary;
    }
}