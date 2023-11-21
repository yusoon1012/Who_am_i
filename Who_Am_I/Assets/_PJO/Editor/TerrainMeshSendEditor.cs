using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 에디터 스크립트로 선언하겠다는 어트리뷰트
[CustomEditor(typeof(TerrainMeshSend))]
public class TerrainMeshSendEditor : Editor
{
    // Terrain오브젝트의 메쉬를 복사당할 오브젝트
    SerializedProperty targetObj;

    // TerrainMeshCopy스크립트가 있는 Terrain 컴포넌트 참조
    private Terrain terrain;

    private void OnEnable()
    {
        // targetObj 프로퍼티 및 terrain 변수 초기화
        targetObj = serializedObject.FindProperty("targetObj");
        terrain = ((TerrainMeshSend)target).GetComponent<Terrain>();
    }       // OnEnable()

    public override void OnInspectorGUI()
    {
        // 인스펙터 창 업데이트
        serializedObject.Update();

        // targetObj 프로퍼티를 인스펙터 창에 표시
        EditorGUILayout.PropertyField(targetObj);

        // 인스펙터창에 해당 버튼을 추가하여 Apply 클릭시 CopyMesh() 메서드 호출
        if (GUILayout.Button("Apply"))
        {
            CopyMesh();
        }

        // 인스펙터 창에 변경사항 적용
        serializedObject.ApplyModifiedProperties();
    }       // OnInspectorGUI()

    private void CopyMesh()
    {
        // GetTargetMeshFilter() 메서드를 사용하여 target의 MeshFilter컴포넌트 가져오기
        MeshFilter targetMeshFilter = GetTargetMeshFilter();

        // targetMeshFilter 체크
        if (targetMeshFilter == null)
        {
            Debug.LogError("TargetObj의 MeshFilter가 없거나 Mesh가 null");
            return;
        }

        // target의 메쉬 가져오기
        Mesh targetMesh = targetMeshFilter.sharedMesh;

        // targetMesh 체크
        if (targetMesh == null)
        {
            Debug.LogError("targetObj의 Mesh가 null입니다.");
            return;
        }

        // TerrainMesh를 기준으로 targetObj 메쉬의 정점 수정 
        Bounds bounds = GetTargetMeshBounds();

        // 수정된 정점을 저장할 List
        List<Vector3> newVector = new List<Vector3>();

        // 각 정점을 수정하여 newVector리스트에 저장
        foreach (Vector3 vertices in targetMesh.vertices)
        {
            // 월드 좌표로 변환된 정점 위치 계산
            Vector4 worldPos = targetMeshFilter.transform.localToWorldMatrix * vertices;

            // 정점 복사본 생성
            Vector3 newVertices = vertices;

            // 복사본 정점의 y좌표를 해당 월드 좌표에서의 높이로 수정
            newVertices.y = terrain.SampleHeight(worldPos);

            // 수정된 정점을 newVector리스트에 저장
            newVector.Add(newVertices);
        }

        // targetObj 메쉬의 정점을 수정된 정점으로 설정
        targetMesh.SetVertices(newVector.ToArray());

        // targetObj 메쉬의 법선, 접선, 경계를 재계산
        targetMesh.RecalculateNormals();
        targetMesh.RecalculateTangents();
        targetMesh.RecalculateBounds();
    }

    // MeshFilter의 유무를 확인하기 위한 메서드
    private MeshFilter GetTargetMeshFilter()
    {
        // 인스펙터 창에 할당된 targetObj프로퍼티 가져오기
        GameObject _targetObj = (GameObject)targetObj.objectReferenceValue;

        // _targetObj가 null 유무 확인
        // true : _targetObj의 MeshFilter 컴포넌트 반환
        // false: null 반환
        return _targetObj != null ? _targetObj.GetComponent<MeshFilter>() : null;
    }       // GetTargetMeshFilter()

    // targetObj의 바운드를 가져오기 위한 메서드
    private Bounds GetTargetMeshBounds()
    {
        // targetObj의 MeshFilter 컴포넌트를 가져오는 과정
        TerrainMeshSend _targetObj = (TerrainMeshSend)target;
        MeshFilter _targetMeshFilter = _targetObj.GetComponent<MeshFilter>();

        // _targetMeshFilter 유무 확인
        if (_targetMeshFilter != null)
        {
            // 메쉬를 가져오는 과정
            Mesh _targetMesh = _targetMeshFilter.sharedMesh;

            // _targetMesh 유뮤 확인
            if (_targetMesh != null)
            {
                // _targetMesh가 null이 아니라면 _targetMesh의 bounds 반환
                return _targetMesh.bounds;
            }
        }

        // 위 if문에 해당하지 않는다면 기본적인 (0, 0, 0)에서 (1, 1, 1)까지의 바운드 반환
        return new Bounds(Vector3.zero, Vector3.one);
    }       // GetTerrainMeshBounds()
}