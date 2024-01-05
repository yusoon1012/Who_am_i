using UnityEngine;
using UnityEditor;

// 에디터 스크립트로 선언하겠다는 어트리뷰트
[CustomEditor(typeof(AddVertexObject))]
public class AddVertexObjectEditor : Editor
{
    #region Property members
    SerializedProperty targetObject;        // 해당 오브젝트
    SerializedProperty saveParent;          // 저장될 위치
    SerializedProperty height;              // 예외 높이
    #endregion

    #region private members
    private Transform saveTransform;                                // 저장될 위치
    private MeshFilter targetMeshFilter;                            // 해당 오브젝트의 메쉬 필터
    private Mesh targetMesh;                                        // 해당 오브젝트의 메쉬
    private Vector3 SIDE_SCALE = new Vector3(0.1f, 0.1f, 0.1f);     // 박스 콜라이더 크기 상수
    #endregion

    #region Editor default
    private void OnEnable()
    {
        // 프로퍼티 초기화
        targetObject = serializedObject.FindProperty("targetObject");
        saveParent = serializedObject.FindProperty("saveParent");
        height = serializedObject.FindProperty("height");
    }       // OnEnable()

    public override void OnInspectorGUI()
    {
        // 인스펙터 창 업데이트
        serializedObject.Update();

        // 인스펙터 창 업데이트
        EditorGUILayout.PropertyField(targetObject, new GUIContent("해당 오브젝트"));
        EditorGUILayout.PropertyField(saveParent, new GUIContent("저장될 위치"));
        EditorGUILayout.PropertyField(height, new GUIContent("예외 높이"));

        // "Apply" 버튼 클릭시 맵 오브젝트의 정점위치에 박스 콜라이더를 생성
        if (GUILayout.Button("Apply"))
        {
            EditorStart();
        }

        // SerializedProperty 변경사항 적용
        serializedObject.ApplyModifiedProperties();
    }       // OnInspectorGUI()
    #endregion

    #region Editor Initialization and Setup
    private void EditorStart()
    {
        InitializationComponents();             // 컴포넌트 초기화
        if (HasNullReference()) { return; }     // Null 

        AddObject();
    }

    private void InitializationComponents()
    {
        saveTransform = GFuncE.SetTransform(saveParent);
        targetMeshFilter = GFuncE.SetComponent<MeshFilter>(targetObject);
        targetMesh = targetMeshFilter.sharedMesh != null ? targetMeshFilter.sharedMesh : null;
    }

    private bool HasNullReference()
    {
        if (saveTransform == null) { GFuncE.SubmitNonFindText(saveParent, typeof(Transform)); return true; }
        if (targetMeshFilter == null) { GFuncE.SubmitNonFindText(targetObject, typeof(MeshFilter)); return true; }
        if (targetMesh == null) { GFuncE.SubmitNonFindText(targetObject, typeof(Mesh)); return true; }

        return false;
    }
    #endregion

    #region Editor function start
    private void AddObject()
    {
        // 박스 콜라이더를 생성할 빈 GameObject 생성
        GameObject newObject = new GameObject("pointObject");

        // 메쉬의 각 정점에 박스 콜라이더가있는 빈 오브젝트 생성
        foreach (Vector3 vertice in targetMesh.vertices)
        {
            // 정점의 월드 좌표 계산
            Vector3 worldPosition = targetMeshFilter.transform.TransformPoint(vertice);

            // 정점의 높이가 예외 높이보다 낮으면 continue
            if (worldPosition.y < height.floatValue)
            {
                continue;
            }
            else
            {
                // 박스 콜라이더 생성 및 크기 설정
                BoxCollider newCollider = Instantiate
                    (
                    GFunc.AddComponent<BoxCollider>(newObject),
                    worldPosition,
                    Quaternion.identity,
                    saveTransform
                    );

                newCollider.size = SIDE_SCALE;
            }
        }

        // 빈 GameObject 제거
        DestroyImmediate(newObject);
    }       // AddObject()
    #endregion
}