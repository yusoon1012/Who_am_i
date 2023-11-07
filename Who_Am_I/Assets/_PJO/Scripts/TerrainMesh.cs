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
    }

    private void Start()
    {
        var bounds = terrain.terrainData.bounds;

        var copyMeshFilter = copyTerrain.GetComponent<MeshFilter>();
        var copyMesh = copyMeshFilter.mesh;

        List<Vector3> newVector = new List<Vector3>();

        foreach (var vertices in copyMesh.vertices)
        { 
        var wPos = copyTerrain.transform.localToWorldMatrix * vertices;
            var newVertices = vertices;

            newVertices.y = terrain.SampleHeight(wPos);
            newVector.Add(newVertices);
        }

        copyMesh.SetVertices(newVector.ToArray());

        copyMesh.RecalculateNormals();
        copyMesh.RecalculateTangents();
        copyMesh.RecalculateBounds();
    }

}
