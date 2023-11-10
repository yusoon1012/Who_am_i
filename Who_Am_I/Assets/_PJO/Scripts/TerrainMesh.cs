using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMesh : MonoBehaviour
{
    [SerializeField]
    private GameObject copyTerrain = default;

    private Terrain terrain = default;

    private void Awake()
    {
        terrain = GetComponent<Terrain>();
    }       // Awake()

    private void Start()
    {
        /*
         * Bounds: 3D�������� ��ü ������ �ѷ��ΰ� �ִ� ���� ���� ���ĵ� ����
         */
        // ���� Terrain�� Bounds �ڽ��� ������
        Bounds bounds = terrain.terrainData.bounds;

        // { copy�� ������Ʈ�� Mesh�� ������
        MeshFilter copyMeshFilter = copyTerrain.GetComponent<MeshFilter>();
        Mesh copyMesh = copyMeshFilter.mesh;
        // } copy�� ������Ʈ�� Mesh�� ������

        // ���ο� Vertice�� ����� ����
        List<Vector3> newVector = new List<Vector3>();

        // copy�� ������Ʈ�� ������ Vertice�� Terrain ���̷� �����ش�.
        foreach (Vector3 vertices in copyMesh.vertices)
        {
            // copyTerrain�� ������ǥ�� ���� ��ǥ�� ��ȯ
            Vector4 wPos = copyTerrain.transform.localToWorldMatrix * vertices;
            Vector3 newVertices = vertices;

            // Terrain���� �ش� ���� ��ǥ�� ���̸� �����ͼ� ������ y ���� �Ҵ�
            newVertices.y = terrain.SampleHeight(wPos);

            // ���ο� Vertice�� List�� ����
            newVector.Add(newVertices);
        }

        // Mesh�� Vertice�� ���� ���� Vertice�� ��ü
        copyMesh.SetVertices(newVector.ToArray());

        // { Mesh�� ����, ����, ��踦 ����
        copyMesh.RecalculateNormals();
        copyMesh.RecalculateTangents();
        copyMesh.RecalculateBounds();
        // } Mesh�� ����, ����, ��踦 ����
    }       // Start()
}
