using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾��� ������ ����
/// </summary>

public class ItemSystem : MonoBehaviour
{
    // ���� ������ ��ųʸ�
    private Dictionary<string, GameObject> itemDic = new Dictionary<string, GameObject>();
    // �÷��̾�
    [SerializeField] private Transform player = default;
    // (����) ��
    [SerializeField] private GameObject shavel = default;
    // (����) ���ô�
    [SerializeField] private GameObject fishingRod = default;
    // (����) ������ 
    [SerializeField] private GameObject nerfGun = default;

    private void Start()
    {
        Setting();
    }

    /// <summary>
    /// �ʱ� ����
    /// </summary>
    private void Setting()
    {
        itemDic["Shavel"] = shavel;
        itemDic["FishingRod"] = fishingRod;
        itemDic["NerfGun"] = nerfGun;
    }

    /// <summary>
    /// �÷��̾ �������� �����ϴ� �޼ҵ�
    /// </summary>
    /// <param name="_item">������ ������</param>
    public void MountingItem(GameObject _item)
    {
        GameObject item = _item;
    }
}
