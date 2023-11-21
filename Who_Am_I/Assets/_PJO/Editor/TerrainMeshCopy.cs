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
        // Terrain의 Bounds를 가져오는 부분을 수정
        // 기존에 사용한 terrain.terrainData.bounds는 테리언의 데이터 바운드를 나타냅니다.
        // 만약 테리언 메쉬의 시각적인 바운드를 얻으려면 다른 방법을 사용해야 할 수 있습니다.
        Bounds bounds = GetTerrainMeshBounds();

        // copyTerrain이 null인지, MeshFilter가 null인지 확인
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
                    Debug.LogError("Copy Terrain의 Mesh가 null입니다.");
                }
            }
            else
            {
                Debug.LogError("Copy Terrain에 MeshFilter가 없습니다.");
            }
        }
        else
        {
            Debug.LogError("Copy Terrain이 지정되지 않았거나 null입니다.");
        }
    }

    private Bounds GetTerrainMeshBounds()
    {
        // 테리언 메쉬의 바운드를 계산하는 코드 추가
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

        // 특별한 상황에서는 자체적인 바운드 계산을 사용할 수 있습니다.
        // 예를 들면, 테리언의 Collider.bounds를 사용하는 방법도 있습니다.
        // 여기서는 해당 코드를 제공하지 않았으므로 필요에 따라 수정이 필요할 수 있습니다.

        return new Bounds(Vector3.zero, Vector3.one);
    }
}
