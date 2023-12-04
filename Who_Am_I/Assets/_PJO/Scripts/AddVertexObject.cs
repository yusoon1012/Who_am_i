using UnityEngine;

public class AddVertexObject : MonoBehaviour
{
    [SerializeField]
    [Header("맵 오브젝트")]
    private GameObject targetObj;

    [SerializeField]
    [Header("저장될 위치")]
    private GameObject targetObjParent;

    [SerializeField]
    [Header("예외 높이")]
    private float height = 1.0f;
}