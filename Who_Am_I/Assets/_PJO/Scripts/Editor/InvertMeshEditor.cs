using UnityEngine;
using UnityEditor;

// 에디터 스크립트로 선언하겠다는 어트리뷰트
[CustomEditor(typeof(InvertMesh))]
public class InvertMeshEditor : Editor
{
    #region Property members
    SerializedProperty targetObject;            // 메쉬를 뒤집을 오브젝트
    #endregion

    #region private members
    private MeshFilter targetMeshFilter;        // 해당 오브젝트의 메쉬 필터
    private Mesh targetMesh;                    // 해당 오브젝트의 메쉬
    private Mesh copyMesh;                      // targetObject의 메쉬 복사본
    #endregion

    #region Editor default
    private void OnEnable()
    {
        // 프로퍼티 초기화
        targetObject = serializedObject.FindProperty("targetObject");
    }

    public override void OnInspectorGUI()
    {
        // 인스펙터 창 업데이트
        serializedObject.Update();

        // 인스펙터에 변수를 편집 가능한 필드로 표시
        EditorGUILayout.PropertyField(targetObject, new GUIContent("뒤집을 오브젝트"));

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
        InitializationComponents();
        if (HasNullReference()) { return; }
        InitializationSetup();

        EditorInvertMesh();
    }

    // 초기 컴포넌트 초기화 메서드
    private void InitializationComponents()
    {
        targetMeshFilter = GFuncE.SetComponent<MeshFilter>(targetObject);
        targetMesh = targetMeshFilter.sharedMesh != null ? targetMeshFilter.sharedMesh : null;  
    }

    // Null 체크
    private bool HasNullReference()
    {
        if (targetMeshFilter == null) { GFuncE.SubmitNonFindText(targetObject, typeof(MeshFilter)); return true; }
        if (targetMesh == null) { GFuncE.SubmitNonFindText(targetObject, typeof(Mesh)); return true; }

        return false;
    }

    // 초기 값 설정 메서드
    private void InitializationSetup()
    {
        copyMesh = GFuncE.CopyMesh(targetMesh);
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

    // Mesh의 폴리곤 법선을 역으로 변경하는 메서드
    private Vector3[] InvertNormals()
    {
        Vector3[] normals = copyMesh.normals;

        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }

        return normals;
    }

    // 폴리곤을 뒤집는 메서드
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