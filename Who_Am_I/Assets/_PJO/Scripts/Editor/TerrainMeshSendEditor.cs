using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// ������ ��ũ��Ʈ�� �����ϰڴٴ� ��Ʈ����Ʈ
[CustomEditor(typeof(TerrainMeshSend))]
public class TerrainMeshSendEditor : Editor
{
    // Terrain������Ʈ�� �޽��� ������� ������Ʈ
    SerializedProperty targetObj;

    // TerrainMeshCopy��ũ��Ʈ�� �ִ� Terrain ������Ʈ ����
    private Terrain terrain;

    private void OnEnable()
    {
        // targetObj ������Ƽ �� terrain ���� �ʱ�ȭ
        targetObj = serializedObject.FindProperty("targetObj");
        terrain = ((TerrainMeshSend)target).GetComponent<Terrain>();
    }       // OnEnable()

    public override void OnInspectorGUI()
    {
        // �ν����� â ������Ʈ
        serializedObject.Update();

        // targetObj ������Ƽ�� �ν����� â�� ǥ��
        EditorGUILayout.PropertyField(targetObj);

        // �ν�����â�� �ش� ��ư�� �߰��Ͽ� Apply Ŭ���� CopyMesh() �޼��� ȣ��
        if (GUILayout.Button("Apply"))
        {
            CopyMesh();
        }

        // �ν����� â�� ������� ����
        serializedObject.ApplyModifiedProperties();
    }       // OnInspectorGUI()

    private void CopyMesh()
    {
        // GetTargetMeshFilter() �޼��带 ����Ͽ� target�� MeshFilter������Ʈ ��������
        MeshFilter targetMeshFilter = GetTargetMeshFilter();

        // targetMeshFilter üũ
        if (targetMeshFilter == null)
        {
            Debug.LogError("TargetObj�� MeshFilter�� ���ų� Mesh�� null");
            return;
        }

        // target�� �޽� ��������
        Mesh targetMesh = targetMeshFilter.sharedMesh;

        // targetMesh üũ
        if (targetMesh == null)
        {
            Debug.LogError("targetObj�� Mesh�� null�Դϴ�.");
            return;
        }

        // TerrainMesh�� �������� targetObj �޽��� ���� ���� 
        Bounds bounds = GetTargetMeshBounds();

        // ������ ������ ������ List
        List<Vector3> newVector = new List<Vector3>();

        // �� ������ �����Ͽ� newVector����Ʈ�� ����
        foreach (Vector3 vertices in targetMesh.vertices)
        {
            // ���� ��ǥ�� ��ȯ�� ���� ��ġ ���
            /*
             * �ڼ��� ���� �ּ� �ۼ�
             */
            Vector4 worldPos = targetMeshFilter.transform.localToWorldMatrix * vertices;

            // ���� ���纻 ����
            Vector3 newVertices = vertices;

            // ���纻 ������ y��ǥ�� �ش� ���� ��ǥ������ ���̷� ����
            newVertices.y = terrain.SampleHeight(worldPos);

            // ������ ������ newVector����Ʈ�� ����
            newVector.Add(newVertices);
        }

        // targetObj �޽��� ������ ������ �������� ����
        targetMesh.SetVertices(newVector.ToArray());

        // targetObj �޽��� ����, ����, ��踦 ����
        targetMesh.RecalculateNormals();
        targetMesh.RecalculateTangents();
        targetMesh.RecalculateBounds();
    }       // CopyMesh()

    // MeshFilter�� ������ Ȯ���ϱ� ���� �޼���
    private MeshFilter GetTargetMeshFilter()
    {
        // �ν����� â�� �Ҵ�� targetObj������Ƽ ��������
        GameObject targetObj_ = (GameObject)targetObj.objectReferenceValue;

        // _targetObj = _�� �ǹ�: ����ڰ� ������ �� private��� �Ϲ����� ���������� �ִ�.
        // tarfetObj = _�� �ڷ� ������ �������� ����ϴ� ������ ����

        // _targetObj�� null ���� Ȯ��
        // true : _targetObj�� MeshFilter ������Ʈ ��ȯ
        // false: null ��ȯ
        return targetObj_ != null ? targetObj_.GetComponent<MeshFilter>() : null;
    }       // GetTargetMeshFilter()

    // targetObj�� �ٿ�带 �������� ���� �޼���
    private Bounds GetTargetMeshBounds()
    {
        // targetObj�� MeshFilter ������Ʈ�� �������� ����
        TerrainMeshSend targetObj_ = (TerrainMeshSend)target;
        MeshFilter targetMeshFilter_ = targetObj_.GetComponent<MeshFilter>();

        // _targetMeshFilter ���� Ȯ��
        if (targetMeshFilter_ != null)
        {
            // �޽��� �������� ����
            Mesh targetMesh_ = targetMeshFilter_.sharedMesh;

            // _targetMesh ���� Ȯ��
            if (targetMesh_ != null)
            {
                // _targetMesh�� null�� �ƴ϶�� _targetMesh�� bounds ��ȯ
                return targetMesh_.bounds;
            }
        }

        // �� if���� �ش����� �ʴ´ٸ� �⺻���� (0, 0, 0)���� (1, 1, 1)������ �ٿ�� ��ȯ
        return new Bounds(Vector3.zero, Vector3.one);
    }       // GetTerrainMeshBounds()
}