using UnityEngine;
using UnityEditor;

// 에디터 스크립트로 선언하겠다는 어트리뷰트
[CustomEditor(typeof(AddVertexObject))]
public class AddVertexObjectEditor : Editor
{
    // 맵 오브젝트
    SerializedProperty targetObject;
    // 저장될 위치
    SerializedProperty saveParent;
    // 예외 높이
    SerializedProperty height;

    // 박스 콜라이더 크기 상수
    private Vector3 SIDE_SCALE = new Vector3(0.1f, 0.1f, 0.1f);

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
        // { 인스펙터에서 변수를 편집 가능한 필드로 표시
        EditorGUILayout.PropertyField(targetObject, new GUIContent("해당 오브젝트"));
        EditorGUILayout.PropertyField(saveParent, new GUIContent("저장할 오브젝트"));
        EditorGUILayout.PropertyField(height, new GUIContent("예외 높이"));
        // } 인스펙터에서 변수를 편집 가능한 필드로 표시

        // "Apply" 버튼 클릭시 맵 오브젝트의 정점위치에 박스 콜라이더를 생성
        if (GUILayout.Button("Apply"))
        {
            AddObject();
        }

        // SerializedProperty 변경사항 적용
        serializedObject.ApplyModifiedProperties();
    }       // OnInspectorGUI()

    private void AddObject()
    {
        // { 저장할 위치의 Transform 가져오기
        Transform saveTransform = GFuncE.SetTransform(saveParent);

        if (GFuncE.HasChild(saveTransform) == false) { return; }
        // } 저장할 위치의 Transform 가져오기

        // { 맵의 메쉬 가져오기
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
        // } 맵의 메쉬 가져오기

        // 박스 콜라이더를 생성할 빈 GameObject 생성
        GameObject newObject = new GameObject("pointObject");

        // 메쉬의 각 정점에 박스 콜라이더가있는 빈 오브젝트 생성
        foreach (Vector3 vertice in targetMesh.vertices)
        {
            // 정점의 월드 좌표 계산
            Vector3 worldPos = targetMeshFilter.transform.TransformPoint(vertice);

            // 정점의 높이가 예외 높이보다 낮으면 continue
            if (worldPos.y < height.floatValue)
            {
                continue;
            }
            else
            {
                // 박스 콜라이더 생성 및 크기 설정
                BoxCollider newCol = Instantiate
                    (
                    GFunc.AddComponent<BoxCollider>(newObject),
                    worldPos,
                    Quaternion.identity,
                    saveTransform
                    );

                newCol.size = SIDE_SCALE;
            }
        }

        // 빈 GameObject 제거
        DestroyImmediate(newObject);
    }       // AddObject()
}