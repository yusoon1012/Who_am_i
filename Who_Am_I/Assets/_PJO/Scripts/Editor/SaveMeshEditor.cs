using UnityEditor;
using UnityEngine;

// 에디터 스크립트로 선언하겠다는 어트리뷰트
[CustomEditor(typeof(SaveMesh))]
public class SaveMeshEditor : Editor
{
    // 저장할 오브젝트
    SerializedProperty targetObject;
    SerializedProperty meshName;

    private void OnEnable()
    {
        // targetObject 프로퍼티 초기화
        targetObject = serializedObject.FindProperty("targetObject");
        meshName = serializedObject.FindProperty("meshName");
    }       // OnEnable()

    public override void OnInspectorGUI()
    {
        // 인스펙터 창 업데이트
        serializedObject.Update();

        // 인스펙터에 targetObject변수를 편집 가능한 필드로 표시
        EditorGUILayout.PropertyField(targetObject, new GUIContent("저장할 오브젝트"));
        EditorGUILayout.PropertyField(meshName, new GUIContent("저장될 메쉬 이름"));
        // "Apply" 버튼 클릭시 메쉬 저장
        if (GUILayout.Button("Apply"))
        {
            SaveMesh();
        }

        // SerializedProperty 변경사항 적용
        serializedObject.ApplyModifiedProperties();
    }       // OnInspectorGUI()

    private void SaveMesh()
    {
        // { 저장할 타겟 오브젝트 메쉬 가져오기
        MeshFilter targetMeshFilter = GFuncE.SetComponent<MeshFilter>(targetObject);
        if (targetMeshFilter == null)
        {
            GFuncE.SubmitNonFindText(targetObject, typeof(MeshFilter));
            return;
        }
        Mesh targetMesh = targetMeshFilter.sharedMesh != null ? targetMeshFilter.sharedMesh : null;
        if (targetMesh == null)
        {
            GFuncE.SubmitNonFindText(targetObject, typeof(Mesh));
            return;
        }
        // } 저장할 타겟 오브젝트 메쉬 가져오기

        // 저장 위치 설정
        string path = $"Assets/Meshes/{meshName.stringValue}.asset";

        // targetMesh를 path에 지정된 경로에 새로운 에셋으로 생성
        AssetDatabase.CreateAsset(targetMesh, path);
        // 에셋 데이터베이스를 저장
        AssetDatabase.SaveAssets();
        // 에셋 데이터베이스를 설정
        AssetDatabase.Refresh();

        // 저장완료 표시
        Debug.Log("Mesh saved successfully. Path: " + path);
    }       // SaveMesh()
}
