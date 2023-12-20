using UnityEditor;
using UnityEngine;

// 에디터 스크립트로 선언하겠다는 어트리뷰트
[CustomEditor(typeof(MeshController))]
public class MeshControllerEditor : Editor
{
    // 변경할 오브젝트
    SerializedProperty targetObject;
    // 몇 등분할지를 정하는 변수
    SerializedProperty verticesValue;

    // Unity에서 기본적으로 제공하는 오브젝트의 메쉬
    private Mesh primitiveMesh;
    // target오브젝트의 메쉬
    private Mesh targetMesh;

    private void OnEnable()
    {
        // 프로퍼티 초기화
        targetObject = serializedObject.FindProperty("targetObject");
        verticesValue = serializedObject.FindProperty("verticesValue");
    }       // OnEnable()

    //! 유니티 에디터모드일때 바로바로 수정할 수 있는 플러그인
    public override void OnInspectorGUI()
    {
        // 인스펙터 창 업데이트
        serializedObject.Update();

        // 인스펙터에 변수를 편집 가능한 필드로 표시
        EditorGUILayout.PropertyField(targetObject, new GUIContent("해당 오브젝트"));
        EditorGUILayout.PropertyField(verticesValue, new GUIContent("등분 갯수"));

        // "Apply" 버튼 클릭시 정점을 늘림
        if (GUILayout.Button("Apply"))
        {
            SetMesh();
        }

        // "Reset" 버튼 클릭시 Unity에서 기본으로 제공하는 Plane Mesh를 적용
        if (GUILayout.Button("Reset"))
        {
            ResetMesh();
        }

        // SerializedProperty 변경사항 적용
        serializedObject.ApplyModifiedProperties();
    }       // OnInspectorGUI()

    private void PrimitiveMesh()
    {
        // { Unity에서 기본적으로 제공하는 메쉬 가져오기
        GameObject primitiveObject = GFuncE.PrimitiveObject(PrimitiveType.Plane);
        MeshFilter primitiveMeshFilter = GFuncE.SetComponent<MeshFilter>(primitiveObject);
        if (primitiveMeshFilter == null)
        {
            GFuncE.SubmitNonFindText(primitiveObject, typeof(MeshFilter));
            return;
        }
        primitiveMesh = primitiveMeshFilter.sharedMesh != null ? primitiveMeshFilter.sharedMesh : null;
        if (primitiveMesh == null)
        {
            GFuncE.SubmitNonFindText(primitiveObject, typeof(Mesh));
            return;
        }

        DestroyImmediate(primitiveObject);
        // } Unity에서 기본적으로 제공하는 메쉬 가져오기
    }

    private void ResetMesh()
    {
        // 기본 Mesh 초기화
        PrimitiveMesh();

        // { 타겟 오브젝트의 메쉬 가져오기
        MeshFilter targetMeshFilter = GFuncE.SetComponent<MeshFilter>(targetObject);
        if (targetMeshFilter == null)
        {
            GFuncE.SubmitNonFindText(targetObject, typeof(MeshFilter));
            return;
        }
        // } 타겟 오브젝트의 메쉬 가져오기

        // Unity에서 기본적으로 제공하는 메쉬 적용
        targetMeshFilter.sharedMesh = primitiveMesh;
    }

    private void SetMesh()
    {
        // 기본 Mesh 초기화
        PrimitiveMesh();

        // { 변화할 타겟 오브젝트의 메쉬 가져오기
        MeshFilter targetMeshFilter = GFuncE.SetComponent<MeshFilter>(targetObject);
        if (targetMeshFilter == null)
        {
            GFuncE.SubmitNonFindText(targetObject, typeof(MeshFilter));
            return;
        }
        targetMesh = targetMeshFilter.sharedMesh != null ? targetMeshFilter.sharedMesh : null;
        if (targetMesh == null)
        {
            GFuncE.SubmitNonFindText(targetObject, typeof(Mesh));
            return;
        }
        else { targetMesh = primitiveMesh; }
        // } 변화할 타겟 오브젝트의 메쉬 가져오기

        Mesh copyMesh = GFuncE.CopyMesh(targetMesh);

        // 초기의 Vertice 총 개수를 저장
        int originVerticesCount = copyMesh.vertices.Length;

        // 제곱근을 저장
        /*
         * Unity가 기본적으로 제공하는 Plane의 Vertice 총 개수는 11 * 11 = 121 이므로 loot 에는 11이 저장된다.
         */
        int loot = Mathf.FloorToInt(Mathf.Sqrt(originVerticesCount));

        // 새로 변화될 vertice 개수 설정
        /*
         * verticesValue에 2가 들어갔을 경우 loot(11) * 2 - 1 이므로 setValue 에는 21값이 저장된다.
         */
        int setValue = (loot - 1) * verticesValue.intValue + 1;

        // 새로 변화될 총 Vertice 갯수 만큼의 배열을 생성한다. 
        //Vector3[] setVertices = new Vector3[setValue * setValue];

        // 초기 메쉬 첫번째 정점위치와 마지막 정점 위치를 저장
        Vector3 startPos = copyMesh.vertices[0];
        Vector3 endPos = copyMesh.vertices[originVerticesCount - 1];

        /*
         * 1차원 배열: 기본적인 메쉬는 1차원 배열로 이루어져있기 때문에
         * 2차원 배열: 편리하게 정점을 저장하기 위해 
         */
        // 1차원 배열과 2차원 배열을 생성
        Vector3[] oneSetVertices = new Vector3[setValue * setValue];
        Vector3[,] twoSetVertices = new Vector3[setValue, setValue];

        // twoSetVertices
        for (int i = 0; i < setValue; i++)
        {
            for (int j = 0; j < setValue; j++)
            {
                // startPos, endPos를 이용해 좌표 생성
                twoSetVertices[j, i] =
                        new Vector3(Mathf.Lerp(startPos.x, endPos.x, (float)j / (float)(verticesValue.intValue * (loot - 1))),
                        0.0f,
                        Mathf.Lerp(startPos.z, endPos.z, (float)i / (float)(verticesValue.intValue * (loot - 1))));
            }
        }

        // 2차원 배열을 1차원 배열로 바꾸는 작업
        for (int i = 0; i < setValue; i++)
        {
            for (int j = 0; j < setValue; j++)
            {
                oneSetVertices[i * setValue + j] = twoSetVertices[j, i];
            }
        }

        // 새로 변화된 Vertice로 mesh 갱신
        copyMesh.vertices = oneSetVertices;

        // 새로 변화될 Mesh의 Triangle 총 개수 저장
        /*
         * 폴리곤이 사각형으로 이루어졌경우 * 1 이 되지만 삼각형으로 이루어져 있기 때문에 * 2
         */
        int SetTrianglesCount = (setValue - 1) * (setValue - 1) * 2;

        // 각 Triangle 은 3개의 정점으로 이루어져 있기 때문에 각 Triangle에 해당하는 정점을 저장할 배열 
        int[] triangles = new int[SetTrianglesCount * 3];

        // 정점들을  이용하여 만들어지는 각 Triangle의 Vertice 인덱스를 저장하기 위한 변수를 초기화
        int triangleIndex = 0;

        // 정점 인덱스를 사용하여 각 삼각형을 구성
        for (int y = 0; y < setValue - 1; y++)
        {
            for (int x = 0; x < setValue - 1; x++)
            {
                // { 정점 Position 설정
                int topLeft = y * setValue + x;
                int topRight = topLeft + 1;
                int bottomLeft = (y + 1) * setValue + x;
                int bottomRight = bottomLeft + 1;
                // } 정점 Position 설정

                // 첫 번째 삼각형
                triangles[triangleIndex] = topRight;
                triangles[triangleIndex + 1] = topLeft;
                triangles[triangleIndex + 2] = bottomRight;

                // 두 번째 삼각형
                triangles[triangleIndex + 3] = bottomLeft;
                triangles[triangleIndex + 4] = bottomRight;
                triangles[triangleIndex + 5] = topLeft;

                triangleIndex += 6; // 다음 삼각형 인덱스로 이동
            }
        }

        // 새로 변화될 Triangle로 mesh 갱신
        copyMesh.triangles = triangles;

        // 메쉬의 경계와 법선을 재계산
        copyMesh.RecalculateBounds();
        copyMesh.RecalculateNormals();

        targetMeshFilter.sharedMesh = copyMesh;
    }       // SetMesh()

    // Legacy:
    // 분할하게 될 두점 사이의 거리를 이용하여 새로 변화될 Vertice의 위치 생성
    //for (int y = 0; y < setValue; y++)
    //{
    //    for (int x = 0; x < setValue; x++)
    //    {
    //        setVertices[y * setValue + x] = mesh.vertices[0] + new Vector3(-lerpLength * x, 0.0f, -lerpLength * y);
    //    }
    //}

    // Legacy:
    //for (int w = 0; w < sqrt; w++)
    //{
    //    if (w == sqrt - 1)
    //    {
    //        int z = 0;

    //        for (int y = 0; y < sqrt; y++)
    //        {
    //            if (y == sqrt - 1)
    //            {
    //                setVertices[(sqrt - 1) * verticesValue.intValue * (w * verticesValue.intValue + z + 1) + w * verticesValue.intValue + z] =
    //                   new Vector3(mesh.vertices[w * sqrt + y].x, 0.0f, mesh.vertices[w * sqrt].z);
    //            }
    //            else
    //            {
    //                for (int x = 0; x < verticesValue.intValue; x++)
    //                {
    //                    setVertices[((sqrt - 1) * verticesValue.intValue * (w * verticesValue.intValue + z + 1) + w * verticesValue.intValue + z) - (setValue - 1) + y * verticesValue.intValue + x] =
    //                        new Vector3(Mathf.Lerp(mesh.vertices[w * sqrt + y].x, mesh.vertices[w * sqrt + y + 1].x, ((float)x / (float)verticesValue.intValue)), 0.0f,
    //                        mesh.vertices[w * sqrt].z);
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        for (int z = 0; z < verticesValue.intValue; z++)
    //        {
    //            for (int y = 0; y < sqrt; y++)
    //            {
    //                if (y == sqrt - 1)
    //                {
    //                    setVertices[(sqrt - 1) * verticesValue.intValue * (w * verticesValue.intValue + z + 1) + w * verticesValue.intValue + z] =
    //                    new Vector3(mesh.vertices[w * sqrt + y].x, 0.0f, Mathf.Lerp(mesh.vertices[w * sqrt].z, mesh.vertices[(w + 1) * sqrt].z, ((float)z / (float)verticesValue.intValue)));
    //                }
    //                else
    //                {
    //                    for (int x = 0; x < verticesValue.intValue; x++)
    //                    {
    //                        setVertices[((sqrt - 1) * verticesValue.intValue * (w * verticesValue.intValue + z + 1) + w * verticesValue.intValue + z) - (setValue - 1) + y * verticesValue.intValue + x] =
    //                            new Vector3(Mathf.Lerp(mesh.vertices[w * sqrt + y].x, mesh.vertices[w * sqrt + y + 1].x, ((float)x / (float)verticesValue.intValue)), 0.0f,
    //                            Mathf.Lerp(mesh.vertices[w * sqrt].z, mesh.vertices[(w + 1) * sqrt].z, ((float)z / (float)verticesValue.intValue)));
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
}
