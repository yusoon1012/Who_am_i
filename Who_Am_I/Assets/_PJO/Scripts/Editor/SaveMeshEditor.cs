using UnityEditor;
using UnityEngine;

// 에디터 스크립트로 선언하겠다는 어트리뷰트
[CustomEditor(typeof(SaveMesh))]
public class SaveMeshEditor : Editor
{
    #region members

    #region Property members
    SerializedProperty propertyTargetObject;    // 메쉬를 저장할 오브젝트
    SerializedProperty propertyMeshName;        // 저장할 때 이름
    #endregion

    #region private members
    private GameObject targetObject;            // 메쉬를 저장할 오브젝트
    private MeshFilter targetMeshFilter;        // 메쉬를 저장할 오브젝트의 메쉬 필터
    private Mesh targetMesh;                    // 메쉬를 저장할 오브젝트의 메쉬
    private string meshName;                    // 저장할 때 이름
    private string path;                        // 저장 위치
    #endregion

    #endregion

    #region Editor default
    private void OnEnable()
    {
        // 프로퍼티 초기화
        propertyTargetObject = serializedObject.FindProperty("propertyTargetObject");
        propertyMeshName = serializedObject.FindProperty("propertyMeshName");
    }

    public override void OnInspectorGUI()
    {
        // 인스펙터 창 업데이트
        serializedObject.Update();

        // 인스펙터에 변수를 편집 가능한 필드로 표시
        EditorGUILayout.PropertyField(propertyTargetObject, new GUIContent("메쉬를 저장할 오브젝트"));
        EditorGUILayout.PropertyField(propertyMeshName, new GUIContent("저장할 때 이름"));

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
        InitializationObjects();
        InitializationComponents();
        InitializationValue();
        if (HasNullReference())
        { GFunc.DebugError(typeof(SaveMeshEditor)); return; }
        InitializationSetup();

        EditorSaveMesh();
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

    // 초기 값 초기화 메서드
    private void InitializationValue()
    {
        meshName = GEFunc.GetPropertyString(propertyMeshName);
    }

    // Null 체크
    private bool HasNullReference()
    {
        if (targetObject == null) { GEFunc.DebugNonFind(propertyTargetObject, SerializedPropertyType.ObjectReference); return true; }
        if (targetMeshFilter == null) { GEFunc.DebugNonFindComponent(propertyTargetObject, typeof(MeshFilter)); return true; }
        if (targetMesh == null) { GEFunc.DebugNonFindComponent(propertyTargetObject, typeof(Mesh)); return true; }
        if (meshName == default) { GEFunc.DebugNonFind(propertyMeshName, SerializedPropertyType.String); return true; }

        return false;
    }

    // 초기 설정 메서드
    private void InitializationSetup()
    {
        path = $"Assets/_Meshes/{meshName}.asset";
    }
    #endregion

    #region Editor function start
    // 메쉬를 저장하는 메서드
    private void EditorSaveMesh()
    {
        // targetMesh를 path에 지정된 경로에 새로운 에셋으로 생성
        AssetDatabase.CreateAsset(targetMesh, path);
        // 에셋 데이터베이스를 저장
        AssetDatabase.SaveAssets();
        // 에셋 데이터베이스를 설정
        AssetDatabase.Refresh();
        // 저장완료 표시
        Debug.Log("Mesh saved successfully. Path: " + path);
    }
    #endregion
}
