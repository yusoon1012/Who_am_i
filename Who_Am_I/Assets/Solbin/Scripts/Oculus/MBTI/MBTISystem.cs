using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBTISystem : MonoBehaviour
{
    #region �ʵ�
    public static MBTISystem instance;

    //// ���� �� �ۼ�������
    //private int fullPercent = 100;
    //// ������ ����
    //private float energy_E = default; // ����
    //private float energy_I = default; // ����
    //// �ν� ����
    //private float recognize_S = default; // ����
    //private float recognize_N = default; // ����
    //// �Ǵ� ����
    //private float judgment_T = default; // ���
    //private float judgment_F = default; // ����
    //// ��Ȱ ���
    //private float lifeCycle_J = default; // �Ǵ�
    //private float lifeCycle_P = default; // �ν�
    #endregion

    public static MBTISystem Instance()
    {
        if (instance == null)
        {
            instance = new MBTISystem();
        }

        return instance;
    }

    // TODO: MBTI ��ġ ���� �޼ҵ� �߰� 

}
