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
        MeshFilter meshFilter = GetNormalMesh();

        if (meshFilter == null)
        {
            Debug.LogError("Object�� MeshFilter�� ���ų� Mesh�� null");
            return;
        }

        Mesh mesh = meshFilter.sharedMesh;

        if (mesh == null)
        {
            Debug.LogError("Object�� Mesh�� Null");
            return;
        }

        string path = "Assets/Meshes/YourMeshName.asset";

        AssetDatabase.CreateAsset(mesh, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Mesh�� ���������� ����Ǿ����ϴ�. ���: " + path);
    }

    private MeshFilter GetNormalMesh()
    {
        GameObject object_ = (GameObject)thisObject;

        return object_ != null ? object_.GetComponent<MeshFilter>() : null;
    }
}
