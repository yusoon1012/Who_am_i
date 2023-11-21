using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainMesh))]
public class TerrainMeshCopy : Editor
{
    SerializedProperty copyTerrain;

    private Terrain terrain = default;

    private void OnEnable()
    {
        copyTerrain = serializedObject.FindProperty("copyTerrain");

        terrain = ((TerrainMesh)target).GetComponent<Terrain>();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(copyTerrain);

        if (GUILayout.Button("Apply"))
        {
            CopyMesh();
        }


        serializedObject.ApplyModifiedProperties();
    }

    private void CopyMesh()
    {
        // Terrain�� Bounds�� �������� �κ��� ����
        // ������ ����� terrain.terrainData.bounds�� �׸����� ������ �ٿ�带 ��Ÿ���ϴ�.
        // ���� �׸��� �޽��� �ð����� �ٿ�带 �������� �ٸ� ����� ����ؾ� �� �� �ֽ��ϴ�.
        Bounds bounds = GetTerrainMeshBounds();

        // copyTerrain�� null����, MeshFilter�� null���� Ȯ��
        if (copyTerrain != null && copyTerrain.objectReferenceValue != null)
        {
            MeshFilter copyMeshFilter = ((GameObject)copyTerrain.objectReferenceValue).GetComponent<MeshFilter>();

            if (copyMeshFilter != null)
            {
                Mesh copyMesh = copyMeshFilter.sharedMesh;

                if (copyMesh != null)
                {
                    List<Vector3> newVector = new List<Vector3>();

                    foreach (Vector3 vertices in copyMesh.vertices)
                    {
                        Vector4 wPos = ((GameObject)copyTerrain.objectReferenceValue).transform.localToWorldMatrix * vertices;
                        Vector3 newVertices = vertices;

                        newVertices.y = terrain.SampleHeight(wPos);

                        newVector.Add(newVertices);
                    }

                    copyMesh.SetVertices(newVector.ToArray());
                    copyMesh.RecalculateNormals();
                    copyMesh.RecalculateTangents();
                    copyMesh.RecalculateBounds();
                }
                else
                {
                    Debug.LogError("Copy Terrain�� Mesh�� null�Դϴ�.");
                }
            }
            else
            {
                Debug.LogError("Copy Terrain�� MeshFilter�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("Copy Terrain�� �������� �ʾҰų� null�Դϴ�.");
        }
    }

    private Bounds GetTerrainMeshBounds()
    {
        // �׸��� �޽��� �ٿ�带 ����ϴ� �ڵ� �߰�
        TerrainMesh terrainMesh = (TerrainMesh)target;
        MeshFilter terrainMeshFilter = terrainMesh.GetComponent<MeshFilter>();

        if (terrainMeshFilter != null)
        {
            Mesh terrainMeshMesh = terrainMeshFilter.sharedMesh;
            if (terrainMeshMesh != null)
            {
                return terrainMeshMesh.bounds;
            }
        }

        // Ư���� ��Ȳ������ ��ü���� �ٿ�� ����� ����� �� �ֽ��ϴ�.
        // ���� ���, �׸����� Collider.bounds�� ����ϴ� ����� �ֽ��ϴ�.
        // ���⼭�� �ش� �ڵ带 �������� �ʾ����Ƿ� �ʿ信 ���� ������ �ʿ��� �� �ֽ��ϴ�.

        return new Bounds(Vector3.zero, Vector3.one);
    }
}
