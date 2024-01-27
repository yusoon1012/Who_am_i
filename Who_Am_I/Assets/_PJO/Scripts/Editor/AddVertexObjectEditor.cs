using UnityEngine;
using UnityEditor;

// 에디터 스크립트로 선언하겠다는 어트리뷰트
[CustomEditor(typeof(AddVertexObject))]
public class AddVertexObjectEditor : Editor
{
    #region members

    #region Property members
    private SerializedProperty propertyTargetObject;                        // 해당 오브젝트
    private SerializedProperty propertySaveParent;                          // 저장될 위치
    private SerializedProperty propertyExceptionHeight;                     // 예외 높이
    #endregion

    #region readonly members
    private readonly Vector3 SIDE_SCALE = new Vector3(0.1f, 0.1f, 0.1f);    // 박스 콜라이더 크기 상수
    #endregion

    #region private members
    private GameObject targetObject;                                        // 해당 오브젝트
    private GameObject saveParentObject;                                    // 저장될 위치의 오브젝트
    private Transform saveParent;                                           // 저장될 위치
    private float exceptionHeight;                                          // 예외 높이
    private MeshFilter targetMeshFilter;                                    // 해당 오브젝트의 메쉬 필터
    private Mesh targetMesh;                                                // 해당 오브젝트의 메쉬
    #endregion

    #endregion

    #region Editor default
    private void OnEnable()
    {
        // 프로퍼티 초기화
        propertyTargetObject = serializedObject.FindProperty("propertyTargetObject");
        propertySaveParent = serializedObject.FindProperty("propertySaveParent");
        propertyExceptionHeight = serializedObject.FindProperty("propertyExceptionHeight");
    }

    public override void OnInspectorGUI()
    {
        // 인스펙터 창 업데이트
        serializedObject.Update();

        // 인스펙터에 변수를 편집 가능한 필드로 표시
        EditorGUILayout.PropertyField(propertyTargetObject, new GUIContent("해당 오브젝트"));
        EditorGUILayout.PropertyField(propertySaveParent, new GUIContent("저장될 위치"));
        EditorGUILayout.PropertyField(propertyExceptionHeight, new GUIContent("예외 높이"));

        // "Apply" 버튼 클릭시 맵 오브젝트의 정점위치에 박스 콜라이더를 생성
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
        { GFunc.DebugError(typeof(AddVertexObjectEditor)); return; }

        EditorAddVertexObject();
    }

    // 초기 오브젝트 초기화 메서드
    private void InitializationObjects()
    {
        targetObject = GEFunc.GetPropertyGameObject(propertyTargetObject);
        saveParentObject = GEFunc.GetPropertyGameObject(propertySaveParent);
    }

    // 초기 컴포넌트 초기화 메서드
    private void InitializationComponents()
    {
        saveParent = saveParentObject.GetComponent<Transform>() ? saveParentObject.GetComponent<Transform>() : null;
        targetMeshFilter = targetObject.GetComponent<MeshFilter>() ? targetObject.GetComponent<MeshFilter>() : null;
        targetMesh = targetMeshFilter.sharedMesh != null ? targetMeshFilter.sharedMesh : null;
    }

    // 초기 값 초기화 메서드
    private void InitializationValue()
    {
        exceptionHeight = GEFunc.GetPropertyFloat(propertyExceptionHeight);
    }

    // Null 체크
    private bool HasNullReference()
    {
        if (targetObject == null) { GEFunc.DebugNonFind(propertyTargetObject, SerializedPropertyType.ObjectReference); return true; }
        if (saveParentObject == null) { GEFunc.DebugNonFind(propertySaveParent, SerializedPropertyType.ObjectReference); return true; }
        if (saveParent == null) { GEFunc.DebugNonFindComponent(propertySaveParent, typeof(Transform)); return true; }
        if (targetMeshFilter == null) { GEFunc.DebugNonFindComponent(propertyTargetObject, typeof(MeshFilter)); return true; }
        if (targetMesh == null) { GEFunc.DebugNonFindComponent(propertyTargetObject, typeof(Mesh)); return true; }
        if (exceptionHeight == default) { GEFunc.DebugNonFind(propertyExceptionHeight, SerializedPropertyType.Float); return true; }

        return false;
    }
    #endregion

    #region Editor function start
    // 메쉬의 각 정점에 콜라이더가 있는 오브젝트를 배치하는 메서드
    private void EditorAddVertexObject()
    {
        // 박스 콜라이더를 가지고있는 빈 오브젝트 생성 및 크기 조절
        GameObject newObject = new GameObject("pointObject");
        newObject.AddComponent<BoxCollider>();
        newObject.GetComponent<BoxCollider>().size = SIDE_SCALE;

        // 메쉬의 각 정점에 박스 콜라이더가있는 빈 오브젝트 생성
        foreach (Vector3 vertice in targetMesh.vertices)
        {
            // 정점의 월드 좌표 계산
            Vector3 worldPosition = targetMeshFilter.transform.TransformPoint(vertice);

            // 정점의 높이가 예외 높이보다 낮으면 continue
            if (worldPosition.y < exceptionHeight)
            {
                continue;
            }
            else
            {
                // 오브젝트 생성
                Instantiate(newObject, worldPosition, Quaternion.identity, saveParent);
            }
        }

        // 사용한 GameObject 제거
        DestroyImmediate(newObject);
    }
    #endregion
}