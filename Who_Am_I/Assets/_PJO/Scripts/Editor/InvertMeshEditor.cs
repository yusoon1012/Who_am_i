using UnityEngine;
using UnityEditor;

// 에디터 스크립트로 선언하겠다는 어트리뷰트
[CustomEditor(typeof(InvertMesh))]
public class InvertMeshEditor : Editor
{
    // 메쉬를 뒤집을 오브젝트
    SerializedProperty targetObject;

    private void OnEnable()
    {
        // 프로퍼티 초기화
        targetObject = serializedObject.FindProperty("targetObject");
    }       // OnEnable()

    public override void OnInspectorGUI()
    {
        // 인스펙터 창 업데이트
        serializedObject.Update();

        // 인스펙터에 targetObject변수를 편집 가능한 필드로 표시
        EditorGUILayout.PropertyField(targetObject, new GUIContent("뒤집을 오브젝트"));

        // "Apply" 버튼 클릭시 메쉬를 뒤집음
        if (GUILayout.Button("Apply"))
        {
            InvertMesh();
        }

        // SerializedProperty 변경사항 적용
        serializedObject.ApplyModifiedProperties();
    }       // OnInspectorGUI()

    private void InvertMesh()
    {
        // { 타겟 오브젝트의 메쉬 가져오기
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
        // } 타겟 오브젝트의 메쉬 가져오기

        // Unity에서 기본으로 제공하는 메쉬를 보호
        Mesh copyMesh = GFuncE.CopyMesh(targetMesh);

        // { Mesh의 폴리곤 법선을 역으로 변경
        Vector3[] normals = copyMesh.normals;

        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }

        copyMesh.normals = normals;
        // } Mesh의 폴리곤 법선을 역으로 변경

        // 변경된 Mesh의 triangles을 저장할 배열
        int[] triangles = copyMesh.triangles;
        
        // { triangle의 세 점 중 가운데를 제외한 나머지 두 점을 Swap하여 뒤집음
        int tempTriangle = default;

        for (int i = 0; i < triangles.Length; i++)
        {
            if (i % 3 == 0)
            {
                tempTriangle = triangles[i];
                triangles[i] = triangles[i + 2];
                triangles[i + 2] = tempTriangle;
            }
        }
        // } triangle의 세 점 중 가운데를 제외한 나머지 두 점을 Swap하여 뒤집음

        // 변경된 삼각형을 Mesh에 적용
        copyMesh.triangles = triangles;

        targetMeshFilter.sharedMesh = copyMesh;
    }
}