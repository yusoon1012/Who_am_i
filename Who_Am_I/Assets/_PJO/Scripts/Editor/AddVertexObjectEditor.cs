using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AddVertexObject))]
public class AddVertexObjectEditor : Editor
{
    SerializedProperty targetObj;
    SerializedProperty targetObjParent;
    SerializedProperty height;

    private Vector3 SIDE_SCALE = new Vector3(0.1f, 0.1f, 0.1f);

    private void OnEnable()
    {
        targetObj = serializedObject.FindProperty("targetObj");
        targetObjParent = serializedObject.FindProperty("targetObjParent");
        height = serializedObject.FindProperty("height");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(targetObj);
        EditorGUILayout.PropertyField(targetObjParent);
        EditorGUILayout.PropertyField(height, new GUIContent("Height"));

        if (GUILayout.Button("Apply"))
        {
            AddObject();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void AddObject()
    {
        Transform targetParent = GFuncE.PropertySetTransform(targetObjParent);

        if (targetParent == null)
        {
            Debug.LogError("targetObjParent의 초기화되지 않았습니다.");
            return;
        }
        if (targetParent.childCount != 0)
        {
            Debug.LogError("targetObjParent의 하위에 오브젝트가 존재합니다.");
            return;
        }

        MeshFilter targetMeshFilter = GFuncE.PropertySetComponent<MeshFilter>(targetObj);

        if (targetMeshFilter == null)
        {
            Debug.LogError("targetObj의 MeshFilter가 null입니다.");
            return;
        }

        Mesh targetMesh = targetMeshFilter.sharedMesh;

        if (targetMesh == null)
        {
            Debug.LogError("targetObj의 Mesh가 null입니다.");
            return;
        }

        GameObject newObject = new GameObject("pointObj");

        foreach (Vector3 vertice in targetMesh.vertices)
        {
            Vector3 worldPos = targetMeshFilter.transform.TransformPoint(vertice);

            if (worldPos.y < height.floatValue)
            {
                continue;
            }
            else
            {
                BoxCollider newCol = Instantiate(GFunc.AddComponent<BoxCollider>(newObject),
                    worldPos,
                    Quaternion.identity,
                    targetParent);

                newCol.size = SIDE_SCALE;
            }
        }

        DestroyImmediate(newObject);
    }
}