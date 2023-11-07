using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class MeshController : MonoBehaviour
{
    private Mesh mesh = default;

    [SerializeField]
    private int verticesValue = default;

    private void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;

        // 메쉬의 정점 및 트라이앵글 설정 함수
        SetMesh();
    }

    private void SetMesh()
    {
        float lerpLength = Vector3.Distance(mesh.vertices[0], Vector3.Lerp(mesh.vertices[0], mesh.vertices[1], 1f / verticesValue));
        int originVerticesCount = mesh.vertices.Length;

        int sqrt = Calculate(originVerticesCount);

        int setValue = sqrt * verticesValue - 1;
        Vector3[] setVertices = new Vector3[setValue * setValue];

        for (int y = 0; y < setValue; y++)
        {
            for (int x = 0; x < setValue; x++)
            {
                setVertices[y * setValue + x] = mesh.vertices[0] + new Vector3(-lerpLength * x, 0.0f, -lerpLength * y);
            }
        }

        mesh.vertices = setVertices;

        // 각 정점이 두 번 사용되기 때문에 2배의 삼각형 수
        int SetTrianglesCount = (setValue - 1) * (setValue - 1) * 2;

        // 각 삼각형은 3개의 정점으로 이루어져 있음
        int[] triangles = new int[SetTrianglesCount * 3];

        int triangleIndex = 0;

        for (int y = 0; y < setValue - 1; y++)
        {
            for (int x = 0; x < setValue - 1; x++)
            {
                int topLeft = y * setValue + x;
                int topRight = topLeft + 1;
                int bottomLeft = (y + 1) * setValue + x;
                int bottomRight = bottomLeft + 1;

                // 첫 번째 삼각형
                triangles[triangleIndex] = topRight;
                triangles[triangleIndex + 1] = topLeft;
                triangles[triangleIndex + 2] = bottomRight;

                // 두 번째 삼각형
                triangles[triangleIndex + 3] = bottomLeft;
                triangles[triangleIndex + 4] = bottomRight;
                triangles[triangleIndex + 5] = topLeft;

                triangleIndex += 6; // 다음 삼각형 인덱스로 이동
            }
        }

        mesh.triangles = triangles;

        // 메쉬의 경계와 법선을 재계산
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    private int Calculate(int _num)
    {
        int sqrt = 1;
        while (sqrt * sqrt < _num)
        {
            sqrt += 1;
        }

        return sqrt;
    }
}
