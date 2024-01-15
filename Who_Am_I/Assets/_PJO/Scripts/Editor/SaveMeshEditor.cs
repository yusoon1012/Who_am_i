using UnityEditor;
using UnityEngine;

// 에디터 스크립트로 선언하겠다는 어트리뷰트
[CustomEditor(typeof(SaveMesh))]
public class SaveMeshEditor : Editor
{
    #region Property members
    SerializedProperty targetObject;        // 메쉬를 저장할 오브젝트
    SerializedProperty meshName;            // 저장할 때 이름
    #endregion

    #region private members
    private MeshFilter targetMeshFilter;    // 메쉬를 저장할 오브젝트의 메쉬 필터
    private Mesh targetMesh;                // 메쉬를 저장할 오브젝트의 메쉬
    private string PATH;                    // 저장 위치
    #endregion

    #region Editor default
    private void OnEnable()
    {
        // 프로퍼티 초기화
        targetObject = serializedObject.FindProperty("targetObject");
        meshName = serializedObject.FindProperty("meshName");
    }

    public override void OnInspectorGUI()
    {
        // 인스펙터 창 업데이트
        serializedObject.Update();

        // 인스펙터에 변수를 편집 가능한 필드로 표시
        EditorGUILayout.PropertyField(targetObject, new GUIContent("저장할 오브젝트"));
        EditorGUILayout.PropertyField(meshName, new GUIContent("저장될 메쉬 이름"));
        
        // "Apply" 버튼 클릭시 메쉬 저장
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

        EditorSaveMesh();
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
        PATH = $"Assets/_Meshes/{meshName.stringValue}.asset";
    }
    #endregion

    #region Editor function start
    // 메쉬를 저장하는 메서드
    private void EditorSaveMesh()
    {
        // targetMesh를 path에 지정된 경로에 새로운 에셋으로 생성
        AssetDatabase.CreateAsset(targetMesh, PATH);
        // 에셋 데이터베이스를 저장
        AssetDatabase.SaveAssets();
        // 에셋 데이터베이스를 설정
        AssetDatabase.Refresh();
        // 저장완료 표시
        Debug.Log("Mesh saved successfully. Path: " + PATH);
    }
    #endregion
}
