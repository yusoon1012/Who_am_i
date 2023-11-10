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
         * Bounds: 3D공간에서 객체 주위에 둘러싸고 있는 가장 작은 정렬된 상자
         */
        // 현재 Terrain의 Bounds 박스를 가져옴
        Bounds bounds = terrain.terrainData.bounds;

        // { copy할 오브젝트의 Mesh를 가져옴
        MeshFilter copyMeshFilter = copyTerrain.GetComponent<MeshFilter>();
        Mesh copyMesh = copyMeshFilter.mesh;
        // } copy할 오브젝트의 Mesh를 가져옴

        // 새로운 Vertice가 저장될 공간
        List<Vector3> newVector = new List<Vector3>();

        // copy할 오브젝트의 각각의 Vertice를 Terrain 높이로 맞춰준다.
        foreach (Vector3 vertices in copyMesh.vertices)
        {
            // copyTerrain의 로컬좌표를 월드 좌표로 변환
            Vector4 wPos = copyTerrain.transform.localToWorldMatrix * vertices;
            Vector3 newVertices = vertices;

            // Terrain에서 해당 월드 좌표의 높이를 가져와서 정점의 y 값을 할당
            newVertices.y = terrain.SampleHeight(wPos);

            // 새로운 Vertice를 List에 저장
            newVector.Add(newVertices);
        }

        // Mesh의 Vertice를 새로 계산된 Vertice로 교체
        copyMesh.SetVertices(newVector.ToArray());

        // { Mesh의 법선, 접선, 경계를 재계산
        copyMesh.RecalculateNormals();
        copyMesh.RecalculateTangents();
        copyMesh.RecalculateBounds();
        // } Mesh의 법선, 접선, 경계를 재계산
    }       // Start()
}
