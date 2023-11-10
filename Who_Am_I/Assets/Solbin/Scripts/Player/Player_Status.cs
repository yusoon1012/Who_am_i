using Oculus.Interaction;
using OVRSimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// �÷��̾� ���� ����, ����
/// </summary>

[Serializable]
public struct PlayerStat
{
    public float fullness; // ������ �ִ�ġ
    public float poo; // ���� �ִ�ġ
    public float speed; // �÷��̾� �̵� ���ǵ�
    public float teleportDistance; // �ڷ���Ʈ ���� �Ÿ�(������ �Ÿ�)
    public float jumpForce; // ������
    public float hungerTime; // ����� Ÿ�̸�
    public float getHunger; // ����� ��ġ
}

public class Player_Status : MonoBehaviour
{
    #region �ʵ�
    public static PlayerStat playerStat;

    public static float m_fullness = default; // ���� ������
    public static float m_poo = default; // ���� ���⵵

    public static float m_speed = default;
    public static float m_teleportDistance = default;
    public static float m_jumpForce = default;

    private bool metabolism = false; // ��ȭ�� Ȱ��ȭ

    public static event EventHandler playerPoo; // ���� �̺�Ʈ
    public static event EventHandler playerDefeat; // �÷��̾� ��� �̺�Ʈ
    #endregion

    private void Awake()
    {
        var playerStatJson = Resources.Load<TextAsset>("Json/PlayerStat");
        playerStat = JsonUtility.FromJson<PlayerStat>(playerStatJson.ToString());

        // �ʱ� ���� ����
        SetStartStat();
        // ��ȭ ����
        StartCoroutine(GetHunger());
    }

    /// <summary>
    /// static �ʵ� �ʱ�ȭ
    /// </summary>
    private void SetStartStat() 
    {
        m_fullness = playerStat.fullness; // ������ ���� �� ���·� ����
        m_poo = 0; // ���� 0���� ����
        m_speed = playerStat.speed; // �̵� �ӵ�
        m_jumpForce = playerStat.jumpForce; // ������
    }


    #region ����, ������
    /// <summary>
    /// �ð��� ������ ���� �������� ��´�
    /// </summary>
    private IEnumerator GetHunger()
    {
        metabolism = true;

        float _getHunger = playerStat.getHunger; // ����� Ÿ�̸�
        float _hungerTime = playerStat.hungerTime; // _getHunger�ʴ� ������ �϶�

        while(metabolism)
        {
            m_fullness -= _getHunger;
            yield return new WaitForSeconds(_hungerTime);
        }
    } 

    /// <summary>
    /// Ư�� �̺�Ʈ �߻��� ������ �϶�
    /// </summary>
    /// <param name="getHunger">��� ����� ��ġ</param>
    public void HungerEvent(float getHunger) { m_fullness -= getHunger; }

    /// <summary>
    /// ������ ������ ������, ���� ��ġ�� ��������
    /// </summary>
    /// <param name="getFull">��� ������ ��ġ</param>
    /// <param name="getPoo">��� ���� ��ġ</param>
    public void GetFood(float getFull, float getPoo) { m_fullness += getFull; m_poo += getPoo; }

    /// <summary>
    /// �÷��̾��� ���� ���¸� üũ
    /// </summary>
    private void StatCheck()
    {
        if (m_fullness <= 0) // ������ 0 ����
        {
            playerDefeat?.Invoke(this, EventArgs.Empty); // �÷��̾� ������ �̺�Ʈ
            metabolism = false;
        }
        else if (m_fullness >= playerStat.fullness) // ������ �Ѱ�ġ���� ���� ����
        {
            m_fullness = playerStat.fullness; // ������ ��ġ ����
        }

        if (m_poo >= playerStat.poo) // ���� �Ѱ�ġ���� ���� ����
        {
            playerPoo?.Invoke(this, EventArgs.Empty); // ���� �̺�Ʈ
        }
    }
    #endregion

    private void Update()
    {
        StatCheck();
    }
}
