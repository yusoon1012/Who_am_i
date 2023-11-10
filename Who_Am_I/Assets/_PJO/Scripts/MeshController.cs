using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class MeshController : MonoBehaviour
{
    [SerializeField]
    private int verticesValue;
}
