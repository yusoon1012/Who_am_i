using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 에디터 스크립트로 선언하겠다는 어트리뷰트
[CustomEditor(typeof(TerrainMeshSend))]
public class TerrainMeshSendEditor : Editor
{
    // 정보를 보내는 오브젝트
    SerializedProperty pushObject;
    // 정보를 받을 오브젝트
    SerializedProperty pullObject;

    // 정보를 보낼 오브젝트의 Terrain
    private Terrain pushTerrain;

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

        // "Apply" 버튼 클릭시 매쉬 바꾸기
        if (GUILayout.Button("Apply"))
        {
            SetMesh();
        }

        // SerializedProperty 변경사항 적용
        serializedObject.ApplyModifiedProperties();
    }       // OnInspectorGUI()

    private void SetMesh()
    {
        // { 터레인 가져오기
        pushTerrain = GFuncE.SetComponent<Terrain>(pushObject);
        if (pushObject == null)
        {
            GFuncE.SubmitNonFindText(pushObject, typeof(Terrain));
            return;
        }
        // } 터레인 가져오기

        // { 저장할 pullObject 메쉬 가져오기
        MeshFilter pullMeshFilter = GFuncE.SetComponent<MeshFilter>(pullObject);
        if (pullMeshFilter == null)
        {
            GFuncE.SubmitNonFindText(pullObject, typeof(MeshFilter));
            return;
        }
        Mesh pullMesh = pullMeshFilter.sharedMesh != null ? pullMeshFilter.sharedMesh : null;
        if (pullMeshFilter.sharedMesh == null)
        {
            GFuncE.SubmitNonFindText(pullObject, typeof(Mesh));
            return;
        }
        // } 저장할 pullObject 메쉬 가져오기

        // Unity에서 기본으로 제공하는 메쉬를 보호
        Mesh copyMesh = GFuncE.CopyMesh(pullMesh);

        // 수정된 정점을 저장할 List
        List<Vector3> newVectors = new List<Vector3>();

        // 각 정점을 수정하여 newVectors리스트에 저장
        foreach (Vector3 vertice in copyMesh.vertices)
        {
            // 월드 좌표로 변환된 정점 위치 계산
            Vector4 worldPos = pullMeshFilter.transform.localToWorldMatrix * vertice;

            // 정점 복사본 생성
            Vector3 newVertices = vertice;

            // 복사본 정점의 y좌표를 해당 월드 좌표에서의 높이로 수정
            newVertices.y = pushTerrain.SampleHeight(worldPos);

            // 수정된 정점을 newVectors리스트에 저장
            newVectors.Add(newVertices);
        }

        // targetObj 메쉬의 정점을 수정된 정점으로 설정
        copyMesh.SetVertices(newVectors.ToArray());

        // { targetObj 메쉬의 법선, 접선, 경계를 재계산
        copyMesh.RecalculateNormals();
        copyMesh.RecalculateTangents();
        copyMesh.RecalculateBounds();
        // } targetObj 메쉬의 법선, 접선, 경계를 재계산

        pullMeshFilter.sharedMesh = copyMesh;
    }       // CopyMesh()
}