using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSystem : MonoBehaviour
{
    #region �ʵ�
    // �ڷ���Ʈ Ȧ ��ǥ ��ųʸ�
    private Dictionary<string, Vector3> hallPos = new Dictionary<string, Vector3>();
    // �÷��̾� Ʈ������
    [SerializeField] private Transform player = default;
    // �ڷ���Ʈ UI
    public Canvas teleportUI = default;
    // �ӽ� Ȧ ���ӿ�����Ʈ
    [SerializeField] private Transform mainHall = default;
    [SerializeField] private Transform cityHall = default;
    [SerializeField] private Transform iceHall = default;
    [SerializeField] private Transform forestHall = default;
    [SerializeField] private Transform beachHall = default;
    #endregion

    private void Start()
    {
        GetHallPos(); // ��ǥ ����
    }

    /// <summary>
    /// (�ʱ� ����)�ڷ���ƮȦ ��ǥ ��ųʸ� ����
    /// </summary>
    private void GetHallPos()
    {
        // TODO: ���� �� Ȯ�� �� �˻� ������� ��ü
        hallPos["MainHall"] = mainHall.position;
        hallPos["CityHall"] = cityHall.position;
        hallPos["IceHall"] = iceHall.position;
        hallPos["ForestHall"] = forestHall.position;
        hallPos["BeachHall"] = beachHall.position;
    }

    #region �ڷ���Ʈ ����
    /// <summary>
    /// �ڷ���Ʈ ��ư Ŭ�� �� �ڷ���Ʈ Ȧ�� ��ǥ�� ��´�
    /// </summary>
    /// <param name="buttonName">�̵� ��ư ���ӿ�����Ʈ�� �̸�</param>
    /// <returns>�ڷ���Ʈ �� Ȧ�� ��ǥ</returns>
    public void TeleportPos(string buttonName)
    {
        Vector3 teleportPos = default;

        switch (buttonName)
        {
            case "MainHall":
                teleportPos = hallPos["MainHall"];
                break;

            case "CityHall": // ���ø� �̵�
                Debug.Log("���ø����� �̵�!");
                teleportPos = hallPos["CityHall"];
                break;

            case "IceHall": // ������ �̵�
                teleportPos = hallPos["IceHall"];
                break;

            case "ForestHall": // ���� �̵�
                teleportPos = hallPos["ForestHall"];
                break;

            case "BeachHall": // �ؾ����� �̵�
                teleportPos = hallPos["BeachHall"];
                break;

            default:
                Debug.LogWarning("��� Ȧ�� ã�� ����");
                teleportPos = Vector3.zero;
                break;
        }

        Teleport(teleportPos);
    }

    private void Teleport(Vector3 _teleportPos)
    {
        Vector3 teleportPos = _teleportPos;

        // TODO: ���� �ڷ���Ʈ ȿ�� �߰�
        player.position = teleportPos;
    }
    #endregion
}
