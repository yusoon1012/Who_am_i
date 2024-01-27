using UnityEngine;
using UnityEditor;

// 에디터 스크립트로 선언하겠다는 어트리뷰트
[CustomEditor(typeof(InvertMesh))]
public class InvertMeshEditor : Editor
{
    #region members

    #region Property members
    SerializedProperty propertyTargetObject;    // 메쉬를 뒤집을 오브젝트
    #endregion

    #region private members
    private GameObject targetObject;            // 메쉬를 뒤집을 오브젝트
    private MeshFilter targetMeshFilter;        // 메쉬를 뒤집을 오브젝트의 메쉬 필터
    private Mesh targetMesh;                    // 메쉬를 뒤집을 오브젝트의 메쉬
    private Mesh copyMesh;                      // 메쉬를 뒤집을 오브젝트의 메쉬 복사본
    #endregion

    #endregion

    #region Editor default
    private void OnEnable()
    {
        // 프로퍼티 초기화
        propertyTargetObject = serializedObject.FindProperty("propertyTargetObject");
    }

    public override void OnInspectorGUI()
    {
        // 인스펙터 창 업데이트
        serializedObject.Update();

        // 인스펙터에 변수를 편집 가능한 필드로 표시
        EditorGUILayout.PropertyField(propertyTargetObject, new GUIContent("메쉬를 뒤집을 오브젝트"));

        // "Apply" 버튼 클릭시 메쉬를 뒤집음
        if (GUILayout.Button("Apply"))
        {
            EditorStart();
        }

        // SerializedProperty 변경사항 적용
        serializedObject.ApplyModifiedProperties();
    }
    #endregion

    #region Editor Initialization and Setup
    // 초기 데이터 초기화 메서드
    private void EditorStart()
    {
        InitializationObjects();
        InitializationComponents();
        if (HasNullReference()) { return; }
        InitializationSetup();

        EditorInvertMesh();
    }

    // 초기 오브젝트 초기화 메서드
    private void InitializationObjects()
    {
        targetObject = GEFunc.GetPropertyGameObject(propertyTargetObject);
    }

    // 초기 컴포넌트 초기화 메서드
    private void InitializationComponents()
    {
        targetMeshFilter = targetObject.GetComponent<MeshFilter>() ? targetObject.GetComponent<MeshFilter>() : null;
        targetMesh = targetMeshFilter.sharedMesh != null ? targetMeshFilter.sharedMesh : null;
    }

    // Null 체크
    private bool HasNullReference()
    {
        if (targetObject == null) { GEFunc.DebugNonFind(propertyTargetObject, SerializedPropertyType.ObjectReference); return true; }
        if (targetMeshFilter == null) { GEFunc.DebugNonFindComponent(propertyTargetObject, typeof(MeshFilter)); return true; }
        if (targetMesh == null) { GEFunc.DebugNonFindComponent(propertyTargetObject, typeof(Mesh)); return true; }
        
        return false;
    }

    // 초기 설정 메서드
    private void InitializationSetup()
    {
        copyMesh = GFunc.CopyMesh(targetMesh);
    }
    #endregion

    #region Editor function start
    // 메쉬를 뒤집는 메서드
    private void EditorInvertMesh()
    {
        copyMesh.normals = InvertNormals();
        copyMesh.triangles = SwapTriangles();

        SetMesh();
    }

    // 메쉬의 법선을 역으로 변경하는 메서드
    private Vector3[] InvertNormals()
    {
        Vector3[] normals = copyMesh.normals;

        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }

        return normals;
    }

    // 트라이앵글을 뒤집는 메서드
    private int[] SwapTriangles()
    {
        int[] triangles = copyMesh.triangles;
        int tempTriangle = default;

        for (int i = 0; i < triangles.Length; i++)
        {
            if (i % 3 == 0)
            {
                tempTriangle = triangles[i];
                triangles[i] = triangles[i + 2];
                triangles[i + 2] = tempTriangle;
            }
        }

        return triangles;
    }

    // 변경된 메쉬 적용
    private void SetMesh()
    {
        targetMeshFilter.sharedMesh = copyMesh;
    }
    #endregion
}