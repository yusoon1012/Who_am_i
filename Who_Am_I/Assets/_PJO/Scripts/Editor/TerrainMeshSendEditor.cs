using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

// 에디터 스크립트로 선언하겠다는 어트리뷰트
[CustomEditor(typeof(TerrainMeshSend))]
public class TerrainMeshSendEditor : Editor
{
    #region members

    #region Property members
    SerializedProperty propertyPushObject;      // 정보를 보내는 오브젝트
    SerializedProperty propertyPullObject;      // 정보를 받을 오브젝트
    #endregion

    #region private members
    private GameObject pushObject;              // 정보를 보내는 오브젝트
    private Terrain pushTerrain;                // 정보를 보내는 오브젝트의 터레인
    private GameObject pullObject;              // 정보를 받을 오브젝트
    private MeshFilter pullMeshFilter;          // 정보를 받을 오브젝트의 메쉬 필터
    private Mesh pullMesh;                      // 정보를 받을 오브젝트의 메쉬
    private Mesh copyMesh;                      // 정보를 받을 오브젝트의 메쉬 복사본
    private List<Vector3> newVerties;           // 수정된 새로운 정점을 저장할 리스트
    #endregion

    #endregion

    #region Editor default
    private void OnEnable()
    {
        // 프로퍼티 초기화
        propertyPushObject = serializedObject.FindProperty("propertyPushObject");
        propertyPullObject = serializedObject.FindProperty("propertyPullObject");
    }

    public override void OnInspectorGUI()
    {
        // 인스펙터 창 업데이트
        serializedObject.Update();

        // 인스펙터에 변수를 편집 가능한 필드로 표시
        EditorGUILayout.PropertyField(propertyPushObject, new GUIContent("정보를 보내는 오브젝트"));
        EditorGUILayout.PropertyField(propertyPullObject, new GUIContent("정보를 받을 오브젝트"));

        // "Apply" 버튼 클릭시 매쉬 바꾸기
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
        if (HasNullReference())
        { GFunc.DebugError(typeof(TargetMeshSendEditor)); return; }
        InitializationInstance();
        InitializationSetup();

        EditorTerrainMeshSend();
    }

    // 초기 오브젝트 초기화 메서드
    private void InitializationObjects()
    {
        pushObject = GEFunc.GetPropertyGameObject(propertyPushObject);
        pullObject = GEFunc.GetPropertyGameObject(propertyPullObject);
    }

    // 초기 컴포넌트 초기화 메서드
    private void InitializationComponents()
    {
        pushTerrain = pushObject.GetComponent<Terrain>() ? pushObject.GetComponent<Terrain>() : null;
        pullMeshFilter = pullObject.GetComponent<MeshFilter>() ? pullObject.GetComponent<MeshFilter>() : null;
        pullMesh = pullMeshFilter.sharedMesh != null ? pullMeshFilter.sharedMesh : null;
    }

    // Null 체크
    private bool HasNullReference()
    {
        if (pushObject == null) { GEFunc.DebugNonFind(propertyPushObject, SerializedPropertyType.ObjectReference); return true; }
        if (pushTerrain == null) { GEFunc.DebugNonFindComponent(propertyPushObject, typeof(Terrain)); return true; }
        if (pullObject == null) { GEFunc.DebugNonFind(propertyPullObject, SerializedPropertyType.ObjectReference); return true; }
        if (pullMeshFilter == null) { GEFunc.DebugNonFindComponent(propertyPullObject, typeof(MeshFilter)); return true; }
        if (pullMesh == null) { GEFunc.DebugNonFindComponent(propertyPullObject, typeof(Mesh)); return true; }

        return false;
    }

    // 초기 인스턴스 생성 메서드
    private void InitializationInstance()
    {
        newVerties = new List<Vector3>();
    }

    // 초기 설정 메서드
    private void InitializationSetup()
    {
        copyMesh = GFunc.CopyMesh(pullMesh);
    }
    #endregion

    #region Editor function start
    // 특정 오브젝트의 Terrain의 높이맵을 이용해 메쉬를 적용하는 메서드
    private void EditorTerrainMeshSend()
    {
        copyMesh.vertices = SetVertices();

        SetMesh();
    }

    //새로운 Vertice의 위치를 설정하는 메서드
    private Vector3[] SetVertices()
    {
        foreach (Vector3 vertice in copyMesh.vertices)
        {
            // 월드 좌표로 변환된 정점 위치 계산
            Vector4 worldPos = pullMeshFilter.transform.localToWorldMatrix * vertice;

            Vector3 newVertices = vertice;

            // 복사본 정점의 y좌표를 해당 월드 좌표에서의 높이로 수정
            newVertices.y = pushTerrain.SampleHeight(worldPos);

            newVerties.Add(newVertices);
        }

        return newVerties.ToArray();
    }

    // 변경된 메쉬 적용
    private void SetMesh()
    {
        copyMesh.RecalculateNormals();
        copyMesh.RecalculateTangents();
        copyMesh.RecalculateBounds();

        pullMeshFilter.sharedMesh = copyMesh;
    }
    #endregion
}