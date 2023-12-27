using UnityEngine;
using UnityEditor;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using UnityEngine.ProBuilder;
using System.Collections.Generic;

[CustomEditor(typeof(TargetMeshSend))]
public class TargetMeshSendEditor : Editor
{
    // 정보를 보내는 오브젝트
    SerializedProperty pushObject;
    // 정보를 받을 오브젝트
    SerializedProperty pullObject;

    private int floorMask;

    private void OnEnable()
    {
        // 프로퍼티 초기화
        pushObject = serializedObject.FindProperty("pushObject");
        pullObject = serializedObject.FindProperty("pullObject");
    }       // OnEnable()

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
            SetMesh();
        }

        // SerializedProperty 변경사항 적용
        serializedObject.ApplyModifiedProperties();
    }       // OnInspectorGUI()

    private void SetMesh()
    {
        GameObject pushGameObject = GFuncE.SetGameObject(pushObject);
        MeshFilter pushMeshFilter = GFuncE.SetComponent<MeshFilter>(pushObject);
        if (pushMeshFilter == null) { GFuncE.SubmitNonFindText(pushObject, typeof(MeshFilter)); return; }
        Mesh pushMesh = pushMeshFilter.sharedMesh != null ? pushMeshFilter.sharedMesh : null;
        if (pushMesh == null) { GFuncE.SubmitNonFindText(pushObject, typeof(Mesh)); return; }

        MeshFilter pullMeshFilter = GFuncE.SetComponent<MeshFilter>(pullObject);
        if (pullMeshFilter == null) { GFuncE.SubmitNonFindText(pullObject, typeof(MeshFilter)); return; }
        Mesh pullMesh = pullMeshFilter.sharedMesh != null ? pullMeshFilter.sharedMesh : null;
        if (pullMesh == null) { GFuncE.SubmitNonFindText(pullObject, typeof(Mesh)); return; }

        Mesh copyMesh = GFuncE.CopyMesh(pullMesh);
        List<Vector3> modifiedVertices = new List<Vector3>(copyMesh.vertices);

        for (int i = 0; i < copyMesh.vertices.Length; i++)
        {
            Vector3 worldPos = pullMeshFilter.transform.TransformPoint(copyMesh.vertices[i]);
            float rayDistance = 10f;
            RaycastHit[] hits = Physics.RaycastAll(worldPos, Vector3.down, rayDistance);

            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.name == pushGameObject.name)
                {
                    // 대신 수정된 데이터를 리스트에 추가
                    Debug.Log(pushMeshFilter.transform.InverseTransformPoint(hit.point).y);

                    // y값만 갱신
                    modifiedVertices[i] = new Vector3(modifiedVertices[i].x, pushMeshFilter.transform.InverseTransformPoint(hit.point).y, modifiedVertices[i].z);

                    Debug.Log(modifiedVertices[i].y);
                    break;
                }
            }
        }

        // SetVertices 메서드를 사용하여 수정된 데이터를 적용
        copyMesh.SetVertices(modifiedVertices);

        pullMeshFilter.sharedMesh = copyMesh;
    }
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
}
