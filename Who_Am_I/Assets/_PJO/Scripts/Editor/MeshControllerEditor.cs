using UnityEditor;
using UnityEngine;

// 에디터 스크립트로 선언하겠다는 어트리뷰트
[CustomEditor(typeof(MeshController))]
public class MeshControllerEditor : Editor
{
    #region members

    #region Property members
    SerializedProperty propertyTargetObject;        // 변경할 오브젝트
    SerializedProperty propertyVerticesValue;       // 몇 등분할지를 정하는 변수
    #endregion

    #region private members
    private GameObject targetObject;                // 변경할 오브젝트
    private MeshFilter targetMeshFilter;            // 변경할 오브젝트의 메쉬 필터
    private Mesh targetMesh;                        // 변경할 오브젝트의 메쉬
    private Mesh copyMesh;                          // 변경할 오브젝트의 메쉬 복사본
    private GameObject primitiveObject;             // Unity에서 기본적으로 제공하는 오브젝트
    private MeshFilter primitiveMeshFilter;         // Unity에서 기본적으로 제공하는 오브젝트의 메쉬 필터
    private Mesh primitiveMesh;                     // Unity에서 기본적으로 제공하는 오브젝트의 메쉬
    private int verticesValue;                      // 몇 등분할지를 정하는 변수
    #endregion

    #endregion

    #region Editor default
    private void OnEnable()
    {
        // 프로퍼티 초기화
        propertyTargetObject = serializedObject.FindProperty("propertyTargetObject");
        propertyVerticesValue = serializedObject.FindProperty("propertyVerticesValue");
    }

    public override void OnInspectorGUI()
    {
        // 인스펙터 창 업데이트
        serializedObject.Update();

        // 인스펙터에 변수를 편집 가능한 필드로 표시
        EditorGUILayout.PropertyField(propertyTargetObject, new GUIContent("변경할 오브젝트"));
        EditorGUILayout.PropertyField(propertyVerticesValue, new GUIContent("몇 등분할지를 정하는 변수"));

        // "Apply" 버튼 클릭시 정점을 늘림
        if (GUILayout.Button("Apply"))
        {
            EditorStart(true);
        }

        // "Reset" 버튼 클릭시 Unity에서 기본으로 제공하는 Plane Mesh를 적용
        if (GUILayout.Button("Reset"))
        {
            EditorStart(false);
        }

        // SerializedProperty 변경사항 적용
        serializedObject.ApplyModifiedProperties();
    }
    #endregion

    #region Editor Initialization and Setup
    // 초기 데이터 초기화 메서드
    private void EditorStart(bool isApply)
    {
        InitializationObjects();
        InitializationComponents();
        InitializationValue();
        if (HasNullReference())
        { GFunc.DebugError(typeof(MeshControllerEditor)); return; }
        InitializationSetup();

        if (isApply) { EditorMeshController(); }
    }

    // 초기 오브젝트 초기화 메서드
    private void InitializationObjects()
    {
        targetObject = GEFunc.GetPropertyGameObject(propertyTargetObject);
        primitiveObject = GFunc.PrimitiveObject(PrimitiveType.Plane);
    }

    // 초기 컴포넌트 초기화 메서드
    private void InitializationComponents()
    {
        targetMeshFilter = targetObject.GetComponent<MeshFilter>() ? targetObject.GetComponent<MeshFilter>() : null;
        targetMesh = targetMeshFilter.sharedMesh != null ? targetMeshFilter.sharedMesh : null;
        primitiveMeshFilter = primitiveObject.GetComponent<MeshFilter>() ? primitiveObject.GetComponent<MeshFilter>() : null;
        primitiveMesh = primitiveMeshFilter.sharedMesh != null ? primitiveMeshFilter.sharedMesh : null;
    }

    // 초기 값 초기화 메서드
    private void InitializationValue()
    {
        verticesValue = GEFunc.GetPropertyInteger(propertyVerticesValue);
    }

    // Null 체크
    private bool HasNullReference()
    {
        if (targetObject == null) { GEFunc.DebugNonFind(propertyTargetObject, SerializedPropertyType.ObjectReference); return true; }
        if (targetMeshFilter == null) { GEFunc.DebugNonFindComponent(propertyTargetObject, typeof(MeshFilter)); return true; }
        if (targetMesh == null) { GEFunc.DebugNonFindComponent(propertyTargetObject, typeof(Mesh)); return true; }
        if (primitiveObject == null) { GFunc.DebugNonFindPrimitiveType(PrimitiveType.Plane); }
        if (primitiveMeshFilter == null) { GFunc.DebugNonFindComponent(primitiveObject, typeof(MeshFilter)); return true; }
        if (primitiveMesh == null) { GFunc.DebugNonFindComponent(primitiveObject, typeof(Mesh)); return true; }
        if (verticesValue == default) { GEFunc.DebugNonFind(propertyVerticesValue, SerializedPropertyType.Integer); return true; }

        return false;
    }

    // 초기 설정 메서드
    private void InitializationSetup()
    {
        targetMeshFilter.sharedMesh = primitiveMesh;
        copyMesh = GFunc.CopyMesh(primitiveMesh);
        DestroyImmediate(primitiveObject);
    }
    #endregion

    #region Editor function start
    // 정점을 특정 갯수로 나누는 메서드
    private void EditorMeshController()
    {
        // Unity에서 기본적으로 제공하는 Plane의 제곱근 저장
        // ex) Unity가 기본적으로 제공하는 Plane의 Vertice 총 개수는 11 * 11 = 121 이므로 root 에는 11이 저장된다.
        int root = Mathf.FloorToInt(Mathf.Sqrt(copyMesh.vertices.Length));
        // 새로 변화될 정점 개수 설정
        // ex) verticesValue에 2가 들어갔을 경우 root(11) * 2 - 1 이므로 setRoot 에는 21값이 저장된다.
        int setRoot = (root - 1) * verticesValue + 1;

        Vector3[,] doubleArrayVertices = SetDoubleArrayVertices(root, setRoot);

        copyMesh.vertices = ConvertArray2DTo1D(doubleArrayVertices, setRoot);
        copyMesh.triangles = SetTriangles(setRoot);

        SetMesh();
    }

    // 2차원 배열로 정의된 새로운 정점의 위치를 설정하는 메서드
    private Vector3[,] SetDoubleArrayVertices(int rootValue, int setRootValue)
    {
        Vector3[,] newDoubleArray = new Vector3[setRootValue, setRootValue];

        Vector3 startPosition = copyMesh.vertices[0];
        Vector3 endPosition = copyMesh.vertices[copyMesh.vertices.Length - 1];

        for (int i = 0; i < setRootValue; i++)
        {
            for (int j = 0; j < setRootValue; j++)
            {
                // startPosition, endPosition 이용해 좌표 생성
                newDoubleArray[j, i] =
                        new Vector3(Mathf.Lerp(startPosition.x, endPosition.x, (float)j / (float)(verticesValue * (rootValue - 1))),
                        0.0f,
                        Mathf.Lerp(startPosition.z, endPosition.z, (float)i / (float)(verticesValue * (rootValue - 1))));
            }
        }

        return newDoubleArray;
    }

    // 2차원 배열을 1차원 배열로 변환하는 메서드
    private Vector3[] ConvertArray2DTo1D(Vector3[,] doubleArray, int value)
    {
        Vector3[] newSingleArray = new Vector3[value * value];

        for (int i = 0; i < value; i++)
        {
            for (int j = 0; j < value; j++)
            {
                newSingleArray[i * value + j] = doubleArray[j, i];
            }
        }

        return newSingleArray;
    }

    // 새로운 트라이앵글 정보를 설정하는 메서드
    private int[] SetTriangles(int value)
    {
        int setTrianglesCount = (value - 1) * (value - 1) * 2;
        int[] setTriangles = new int[setTrianglesCount * 3];
        int currentIndex = 0;

        for (int y = 0; y < value - 1; y++)
        {
            for (int x = 0; x < value - 1; x++)
            {
                // 정점 Position 설정
                int topLeft = y * value + x;
                int topRight = topLeft + 1;
                int bottomLeft = (y + 1) * value + x;
                int bottomRight = bottomLeft + 1;

                // 첫 번째 삼각형
                setTriangles[currentIndex] = topRight;
                setTriangles[currentIndex + 1] = topLeft;
                setTriangles[currentIndex + 2] = bottomRight;

                // 두 번째 삼각형
                setTriangles[currentIndex + 3] = bottomLeft;
                setTriangles[currentIndex + 4] = bottomRight;
                setTriangles[currentIndex + 5] = topLeft;

                // 다음 삼각형 인덱스로 이동
                currentIndex += 6;
            }
        }

        return setTriangles;
    }

    // 변경된 메쉬 적용
    private void SetMesh()
    {
        copyMesh.RecalculateBounds();
        copyMesh.RecalculateNormals();

        targetMeshFilter.sharedMesh = copyMesh;
    }
    #endregion

    #region Legacy
    //private void EditorMeshControllerSet1()
    //{
    //    targetMeshFilter.sharedMesh = primitiveMesh;

    //    // 초기의 Vertice 총 개수를 저장
    //    int originVerticesCount = copyMesh.vertices.Length;

    //    // 제곱근을 저장
    //    /*
    //     * Unity가 기본적으로 제공하는 Plane의 Vertice 총 개수는 11 * 11 = 121 이므로 loot 에는 11이 저장된다.
    //     */
    //    int loot = Mathf.FloorToInt(Mathf.Sqrt(originVerticesCount));

    //    // 새로 변화될 vertice 개수 설정
    //    /*
    //     * verticesValue에 2가 들어갔을 경우 loot(11) * 2 - 1 이므로 setValue 에는 21값이 저장된다.
    //     */
    //    int setValue = (loot - 1) * verticesValue.intValue + 1;

    //    // 초기 메쉬 첫번째 정점위치와 마지막 정점 위치를 저장
    //    Vector3 startPos = copyMesh.vertices[0];
    //    Vector3 endPos = copyMesh.vertices[originVerticesCount - 1];

    //    /*
    //     * 1차원 배열: 기본적인 메쉬는 1차원 배열로 이루어져있기 때문에
    //     * 2차원 배열: 편리하게 정점을 저장하기 위해 
    //     */
    //    // 1차원 배열과 2차원 배열을 생성
    //    Vector3[] oneSetVertices = new Vector3[setValue * setValue];
    //    Vector3[,] twoSetVertices = new Vector3[setValue, setValue];

    //    // twoSetVertices
    //    for (int i = 0; i < setValue; i++)
    //    {
    //        for (int j = 0; j < setValue; j++)
    //        {
    //            // startPos, endPos를 이용해 좌표 생성
    //            twoSetVertices[j, i] =
    //                    new Vector3(Mathf.Lerp(startPos.x, endPos.x, (float)j / (float)(verticesValue.intValue * (loot - 1))),
    //                    0.0f,
    //                    Mathf.Lerp(startPos.z, endPos.z, (float)i / (float)(verticesValue.intValue * (loot - 1))));
    //        }
    //    }

    //    // 2차원 배열을 1차원 배열로 바꾸는 작업
    //    for (int i = 0; i < setValue; i++)
    //    {
    //        for (int j = 0; j < setValue; j++)
    //        {
    //            oneSetVertices[i * setValue + j] = twoSetVertices[j, i];
    //        }
    //    }

    //    // 새로 변화된 Vertice로 mesh 갱신
    //    copyMesh.vertices = oneSetVertices;

    //    // 새로 변화될 Mesh의 Triangle 총 개수 저장
    //    /*
    //     * 폴리곤이 사각형으로 이루어졌경우 * 1 이 되지만 삼각형으로 이루어져 있기 때문에 * 2
    //     */
    //    int SetTrianglesCount = (setValue - 1) * (setValue - 1) * 2;

    //    // 각 Triangle 은 3개의 정점으로 이루어져 있기 때문에 각 Triangle에 해당하는 정점을 저장할 배열 
    //    int[] triangles = new int[SetTrianglesCount * 3];

    //    // 정점들을  이용하여 만들어지는 각 Triangle의 Vertice 인덱스를 저장하기 위한 변수를 초기화
    //    int triangleIndex = 0;

    //    // 정점 인덱스를 사용하여 각 삼각형을 구성
    //    for (int y = 0; y < setValue - 1; y++)
    //    {
    //        for (int x = 0; x < setValue - 1; x++)
    //        {
    //            // { 정점 Position 설정
    //            int topLeft = y * setValue + x;
    //            int topRight = topLeft + 1;
    //            int bottomLeft = (y + 1) * setValue + x;
    //            int bottomRight = bottomLeft + 1;
    //            // } 정점 Position 설정

    //            // 첫 번째 삼각형
    //            triangles[triangleIndex] = topRight;
    //            triangles[triangleIndex + 1] = topLeft;
    //            triangles[triangleIndex + 2] = bottomRight;

    //            // 두 번째 삼각형
    //            triangles[triangleIndex + 3] = bottomLeft;
    //            triangles[triangleIndex + 4] = bottomRight;
    //            triangles[triangleIndex + 5] = topLeft;

    //            triangleIndex += 6; // 다음 삼각형 인덱스로 이동
    //        }
    //    }

    //    // 새로 변화될 Triangle로 mesh 갱신
    //    copyMesh.triangles = triangles;

    //    // 메쉬의 경계와 법선을 재계산
    //    copyMesh.RecalculateBounds();
    //    copyMesh.RecalculateNormals();

    //    targetMeshFilter.sharedMesh = copyMesh;
    //}       // SetMesh()

    // 분할하게 될 두점 사이의 거리를 이용하여 새로 변화될 Vertice의 위치 생성
    //for (int y = 0; y < setValue; y++)
    //{
    //    for (int x = 0; x < setValue; x++)
    //    {
    //        setVertices[y * setValue + x] = mesh.vertices[0] + new Vector3(-lerpLength * x, 0.0f, -lerpLength * y);
    //    }
    //}
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
    #endregion
}