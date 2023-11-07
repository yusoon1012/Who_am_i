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

        // �޽��� ���� �� Ʈ���̾ޱ� ���� �Լ�
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

        // �� ������ �� �� ���Ǳ� ������ 2���� �ﰢ�� ��
        int SetTrianglesCount = (setValue - 1) * (setValue - 1) * 2;

        // �� �ﰢ���� 3���� �������� �̷���� ����
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

                // ù ��° �ﰢ��
                triangles[triangleIndex] = topRight;
                triangles[triangleIndex + 1] = topLeft;
                triangles[triangleIndex + 2] = bottomRight;

                // �� ��° �ﰢ��
                triangles[triangleIndex + 3] = bottomLeft;
                triangles[triangleIndex + 4] = bottomRight;
                triangles[triangleIndex + 5] = topLeft;

                triangleIndex += 6; // ���� �ﰢ�� �ε����� �̵�
            }
        }

        mesh.triangles = triangles;

        // �޽��� ���� ������ ����
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
