using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

[CustomEditor(typeof(SaveMesh))]
public class SaveMeshEditor : Editor
{
    private GameObject thisObject;

    private void OnEnable()
    {
        thisObject = ((SaveMesh)target).gameObject;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (GUILayout.Button("Apply"))
        {
            SaveMesh();
        }
    }

    private void SaveMesh()
    {
        MeshFilter meshFilter = GFunc.SetComponent<MeshFilter>(thisObject);

        if (meshFilter == null)
        {
            Debug.LogError("Object의 MeshFilter가 없거나 Mesh가 null");
            return;
        }

        Mesh mesh = meshFilter.sharedMesh;

        if (mesh == null)
        {
            Debug.LogError("Object의 Mesh가 Null");
            return;
        }

        string path = "Assets/Meshes/YourMeshName.asset";

        AssetDatabase.CreateAsset(mesh, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Mesh가 성공적으로 저장되었습니다. 경로: " + path);
    }
}
