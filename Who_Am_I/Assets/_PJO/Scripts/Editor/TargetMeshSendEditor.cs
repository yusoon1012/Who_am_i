using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(TargetMeshSend))]
public class TargetMeshSendEditor : Editor
{
    #region Property members
    SerializedProperty pushObject;      // 정보를 보내는 오브젝트
    SerializedProperty pullObject;      // 정보를 받을 오브젝트
    #endregion

    #region private members
    private MeshFilter pushMeshFilter;      // 정보를 보내는 오브젝트의 메쉬 필터
    private Mesh pushMesh;                  // 정보를 보내는 오브젝트의 메쉬
    private MeshFilter pullMeshFilter;      // 정보를 받을 오브젝트의 메쉬 필터
    private Mesh pullMesh;                  // 정보를 받을 오브젝트의 메쉬
    private Mesh copyMesh;                  // 정보를 받을 오브젝트의 메쉬 복사본
    private List<Vector3> newVerties;       // 수정된 새로운 정점을 저장할 리스트
    private float RAY_DISTANCE = 10f;       // 레이케스트 길이 상수
    #endregion

    #region Editor default
    private void OnEnable()
    {
        // 프로퍼티 초기화
        pushObject = serializedObject.FindProperty("pushObject");
        pullObject = serializedObject.FindProperty("pullObject");
    }

    public override void OnInspectorGUI()
    {
        // 인스펙터 창 업데이트
        serializedObject.Update();

        // 인스펙터에 변수를 편집 가능한 필드로 표시
        EditorGUILayout.PropertyField(pushObject, new GUIContent("보낼 오브젝트"));
        EditorGUILayout.PropertyField(pullObject, new GUIContent("받을 오브젝트"));

        // "Apply" 버튼 클릭시 메쉬 바꾸기
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

        EditorTargetMeshSend();
    }

    // 초기 컴포넌트 초기화 메서드
    private void InitializationComponents()
    {
        pushMeshFilter = GFuncE.SetComponent<MeshFilter>(pushObject);
        pushMesh = pushMeshFilter.sharedMesh != null ? pushMeshFilter.sharedMesh : null;
        pullMeshFilter = GFuncE.SetComponent<MeshFilter>(pullObject);
        pullMesh = pullMeshFilter.sharedMesh != null ? pullMeshFilter.sharedMesh : null;
        copyMesh = GFuncE.CopyMesh(pullMesh);
    }

    // Null 체크
    private bool HasNullReference()
    {
        if (pushMeshFilter == null) { GFuncE.SubmitNonFindText(pushObject, typeof(MeshFilter)); return true; }
        if (pushMesh == null) { GFuncE.SubmitNonFindText(pushObject, typeof(Mesh)); return true; }
        if (pullMeshFilter == null) { GFuncE.SubmitNonFindText(pullObject, typeof(MeshFilter)); return true; }
        if (pullMesh == null) { GFuncE.SubmitNonFindText(pullObject, typeof(Mesh)); return true; }

        return false;
    }

    // 초기 값 설정 메서드
    private void InitializationSetup()
    {
        newVerties = new List<Vector3>(copyMesh.vertices);
    }
    #endregion

    #region Editor function start
    // 특정 오브젝트의 메쉬를 적용하는 메서드
    private void EditorTargetMeshSend()
    {
        SetVertices();

        copyMesh.vertices = SetVertices();

        SetMesh();
    }

    //새로운 Vertice의 위치를 설정하는 메서드
    private Vector3[] SetVertices()
    {
        for (int i = 0; i < copyMesh.vertices.Length; i++)
        {
            Vector3 worldPos = pullMeshFilter.transform.TransformPoint(copyMesh.vertices[i]);

            RaycastHit[] hits = Physics.RaycastAll(worldPos, Vector3.down, RAY_DISTANCE);

            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.name == pushObject.SetGameObject().name)
                {
                    // 수정된 정점 데이터를 리스트에 추가
                    newVerties[i] = new Vector3(newVerties[i].x, pushMeshFilter.transform.InverseTransformPoint(hit.point).y, newVerties[i].z);

                    break;
                }
            }
        }

        return newVerties.ToArray();
    }

    // 변경된 메쉬 적용
    private void SetMesh()
    {
        pullMeshFilter.sharedMesh = copyMesh;
    }
    #endregion

    #region Legacy
    //foreach (Vector3 vertex in copyMesh.vertices)
    //{
    //    Vector3 worldPos = pullMeshFilter.transform.TransformPoint(vertex);

    //    float rayDistance = 10f;
    //    RaycastHit[] hits = Physics.RaycastAll(worldPos, Vector3.up, rayDistance);
    //    foreach (RaycastHit hit in hits)
    //    {
    //        if (hit.transform.name == pushGameObject.name)
    //        {
    //            vertex = hit.point;

    //            break;
    //        }
    //    }
    //}

    //private void SetMesh()
    //{
    //    MeshFilter pushMeshFilter = GFuncE.SetComponent<MeshFilter>(pushObject);
    //    if (pushMeshFilter == null) { GFuncE.SubmitNonFindText(pushObject, typeof(MeshFilter)); return; }
    //    Mesh pushMesh = pushMeshFilter.sharedMesh != null ? pushMeshFilter.sharedMesh : null;
    //    if (pushMesh == null) { GFuncE.SubmitNonFindText(pushObject, typeof(Mesh)); return; }

    //    MeshFilter pullMeshFilter = GFuncE.SetComponent<MeshFilter>(pullObject);
    //    if (pullMeshFilter == null) { GFuncE.SubmitNonFindText(pullObject, typeof(MeshFilter)); return; }
    //    Mesh pullMesh = pullMeshFilter.sharedMesh != null ? pullMeshFilter.sharedMesh : null;
    //    if (pullMesh == null) { GFuncE.SubmitNonFindText(pullObject, typeof(Mesh)); return; }

    //    Mesh copyMesh = GFuncE.CopyMesh(pullMesh);

    //    List<Vector3> verties = new List<Vector3>();

    //    foreach (Vector3 vertex in pushMesh.vertices)
    //    {
    //        Vector3 worldPos = pushMeshFilter.transform.TransformPoint(vertex);
    //        float rayDistance = 10f; // Ray의 최대 길이
    //        RaycastHit[] hits = Physics.RaycastAll(worldPos, Vector3.down, rayDistance);
    //        foreach (RaycastHit hit in hits)
    //        {
    //            if (hit.transform.name == "Plane")
    //            {
    //                verties.Add(vertex);

    //                break;
    //            }
    //        }
    //    }

    //    copyMesh = SortVerties(verties);


    //    //pullMeshFilter.sharedMesh = copyMesh;
    //}

    //private Mesh SortVerties(List<Vector3> _verties)
    //{
    //    Mesh setMesh = new Mesh();

    //    // 정점 정렬 (x, z 좌표 기준)
    //    Vector3[] sortedVertices = _verties.OrderBy(v => v.x).ThenBy(v => v.z).ToArray();

    //    // 정점 배열을 메시에 할당
    //    setMesh.SetVertices(sortedVertices);

    //    // 삼각형 인덱스 설정
    //    List<int> triangles = new List<int>();
    //    for (int i = 0; i < sortedVertices.Length - 2; i++)
    //    {
    //        triangles.Add(0);
    //        triangles.Add(i + 1);
    //        triangles.Add(i + 2);
    //    }

    //    // 삼각형 배열을 메시에 할당
    //    setMesh.SetTriangles(triangles.ToArray(), 0);

    //    // 정규화된 법선, 접선, 경계 재계산
    //    setMesh.RecalculateNormals();
    //    setMesh.RecalculateTangents();
    //    setMesh.RecalculateBounds();

    //    return setMesh;
    //}
    #endregion
}
