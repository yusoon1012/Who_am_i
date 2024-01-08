using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

/*      Decimation Algorithm
 *      
 *      폴리곤을 제거하여 메쉬의 크기를 줄이는 방법
 *      폴리곤의 면적을 기준으로 사용
 */

[CustomEditor(typeof(Decimation))]
public class DecimationEditor : Editor
{
    #region Property members
    SerializedProperty targetObject;        // 타겟 오브젝트
    SerializedProperty trianglePercent;     // 폴리곤 비율
    #endregion

    #region private members
    private MeshFilter targetMeshFilter;            // 타겟 오브젝트의 메쉬 필터
    private Mesh targetMesh;                        // 타겟 오브젝트의 메쉬
    private Mesh copyMesh;                          // 타겟 오브젝트의 복사본
    private int maxTriangleLength;                  // 비율에 기반한 최대 폴리곤 개수
    private List<Vector3> newVerties;               // 새로운 정점을 저장
    private List<int> newTriangles;                 // 새로운 폴리곤을 저장
    private Dictionary<int, float> trianglesValue;  // 폴리곤의 면적을 저장
    private List<Triangle> trianglesData;           // 폴리곤 데이터를 저장
    #endregion

    #region struct members
    private struct Triangle
    {
        public Vector3 v0;
        public Vector3 v1;
        public Vector3 v2;
        public int index;
    }       // 각 삼각형 데이터를 저장할 구조체
    #endregion

    #region Editor default
    private void OnEnable()
    {
        // 프로퍼티 초기화
        targetObject = serializedObject.FindProperty("targetObject");
        trianglePercent = serializedObject.FindProperty("trianglePercent");
    }       // OnEnable()

    public override void OnInspectorGUI()
    {
        // 인스펙터 창 업데이트
        serializedObject.Update();

        // 인스펙터에 변수를 편집 가능한 필드로 표시
        EditorGUILayout.PropertyField(targetObject, new GUIContent("타겟 오브젝트"));
        EditorGUILayout.PropertyField(trianglePercent, new GUIContent("폴리곤 퍼센트"));

        // "Apply" 버튼 클릭시 정점을 지움
        if (GUILayout.Button("Apply"))
        {
            EditorStart();
        }

        // SerializedProperty 변경사항 적용
        serializedObject.ApplyModifiedProperties();
    }       // OnInspectorGUI()
    #endregion

    #region Editor Initialization and Setup
    private void EditorStart()
    {
        InitializationComponents();
        ErrorCheck();
        InitializationNew();
        InitializationSetup();
        Decimation();
    }       // EditorStart()

    private void InitializationComponents()
    {
        targetMeshFilter = GFuncE.SetComponent<MeshFilter>(targetObject);
        targetMesh = targetMeshFilter.sharedMesh != null ? targetMeshFilter.sharedMesh : null;
    }       // InitializationComponents()

    private void InitializationNew()
    {
        newTriangles = new List<int>();
        newVerties = new List<Vector3>();
    }       // InitializationNew()

    private void ErrorCheck()
    {
        if (targetMeshFilter == null) { GFuncE.SubmitNonFindText(targetObject, typeof(MeshFilter)); return; }
        if (targetMesh == null) { GFuncE.SubmitNonFindText(targetObject, typeof(Mesh)); return; }
    }       // ErrorCheck()

    private void InitializationSetup()
    {
        copyMesh = GFuncE.CopyMesh(targetMesh);
        newVerties.AddRange(copyMesh.vertices);
        newTriangles.AddRange(copyMesh.triangles);
        maxTriangleLength = Mathf.FloorToInt(CalculateVertiesPercent());
        SetTriangleData(copyMesh, out trianglesData, out trianglesValue);
    }       // InitializationSetup()

    private float CalculateVertiesPercent()
    => copyMesh.triangles.Length * (trianglePercent.floatValue * 0.01f);

    // 트라이앵글 정보를 담은 List, Dictionary 초기화 메서드
    private void SetTriangleData(Mesh _mesh, out List<Triangle> _trianglesData, out Dictionary<int, float> _trianglesValue)
    {
        _trianglesData = new List<Triangle>();
        _trianglesValue = new Dictionary<int, float>();

        int[] triangles = _mesh.triangles;
        Vector3[] vertices = _mesh.vertices;

        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            int i0 = triangles[i];
            int i1 = triangles[i + 1];
            int i2 = triangles[i + 2];

            Vector3 v0 = vertices[i0];
            Vector3 v1 = vertices[i1];
            Vector3 v2 = vertices[i2];

            _trianglesData.Add(new Triangle { v0 = v0, v1 = v1, v2 = v2, index = j });
        }

        foreach (Triangle triangle in _trianglesData)
        {
            _trianglesValue[triangle.index] = CalculateTriangleArea(triangle.v0, triangle.v1, triangle.v2);
        }
    }

    private float CalculateTriangleArea(Vector3 _v0, Vector3 _v1, Vector3 _v2)
    {
        // 삼각형의 세 변의 길이
        float a = Vector3.Distance(_v0, _v1);
        float b = Vector3.Distance(_v1, _v2);
        float c = Vector3.Distance(_v2, _v0);

        // 삼각형의 반 둘레
        float s = (a + b + c) / 2;

        // 헤론의 공식을 사용하여 삼각형의 면적 계산
        float area = Mathf.Sqrt(s * (s - a) * (s - b) * (s - c));

        return area;
    }
    #endregion

    #region Editor function start
    private void Decimation()
    {
        for (int i = 0; maxTriangleLength <= copyMesh.triangles.Length; i++)
        {
            int minIndex = FindMinIndex(i);
            Vector3 newPos = CalculateTriangleCenter(trianglesData[minIndex].v0, trianglesData[minIndex].v1, trianglesData[minIndex].v2);

            MoveVertex(minIndex, newPos);

            copyMesh.triangles = RemoveTriangles();
        }

        // 갱신된 메시 정보 적용
        SetMesh();
    }

    private void SetMesh()
    {
        copyMesh.vertices = newVerties.ToArray();
        targetMeshFilter.sharedMesh = copyMesh;
    }

    private int FindMinIndex(int _nextCount)
        => trianglesValue.OrderBy(kv => kv.Value).Skip(_nextCount).First().Key;

    private void MoveVertex(int minIndex, Vector3 newPosition)
    {
        for (int i = 0; i < newTriangles.Count; i += 3)
        {
            int[] vertexIndices = { newTriangles[i], newTriangles[i + 1], newTriangles[i + 2] };

            foreach (int vertexIndex in vertexIndices)
            {
                Vector3 vertex = newVerties[vertexIndex];

                if (IsPointInsideTriangle(vertex, trianglesData[minIndex]))
                {
                    newVerties[vertexIndex] = newPosition;
                }
            }
        }
    }

    private int[] RemoveTriangles()
    {
        List<Vector3> newVerticesList = new List<Vector3>(newVerties);
        List<int> newTrianglesList = new List<int>(copyMesh.triangles);

        for (int i = 0; i < newTrianglesList.Count; i += 3)
        {
            int i0 = newTrianglesList[i];
            int i1 = newTrianglesList[i + 1];
            int i2 = newTrianglesList[i + 2];

            Vector3 v0 = newVerticesList[i0];
            Vector3 v1 = newVerticesList[i1];
            Vector3 v2 = newVerticesList[i2];

            float area = CalculateTriangleArea(v0, v1, v2);

            // 면적이 0이면 제거
            if (Mathf.Approximately(area, 0f))
            {
                newTrianglesList.RemoveAt(i);
                newTrianglesList.RemoveAt(i);
                newTrianglesList.RemoveAt(i);

                i -= 3; // 삼각형이 제거되었으므로 인덱스 조정
            }
        }

        return newTrianglesList.ToArray();
    }



    // 삼각형에 포함되는 정점이 있는지 확인하는 메서드
    private bool IsPointInsideTriangle(Vector3 _point, Triangle _triangle)
    => _point == _triangle.v0 || _point == _triangle.v1 || _point == _triangle.v2;

    // 삼각형의 중심 좌표를 알아내는 메서드
    private Vector3 CalculateTriangleCenter(Vector3 _v0, Vector3 _v1, Vector3 _v2)
    {
        float cX = (_v0.x + _v1.x + _v2.x) / 3f;
        float cY = (_v0.y + _v1.y + _v2.y) / 3f;
        float cZ = (_v0.z + _v1.z + _v2.z) / 3f;

        return new Vector3(cX, cY, cZ);
    }
    #endregion

    #region Lagacy
    //private Vector3 CalculateTriangleCenter(Vector3 _v0, Vector3 _v1, Vector3 _v2)
    //{
    //    float cX = (_v0.x + _v1.x + _v2.x) / 3f;
    //    float cY = (_v0.y + _v1.y + _v2.y) / 3f;
    //    float cZ = (_v0.z + _v1.z + _v2.z) / 3f;

    //    return new Vector3(cX, cY, cZ);
    //}

    //private void RemoveMisuseVertices(Mesh _mesh)
    //{
    //    HashSet<int> triangleIndices = new HashSet<int>(_mesh.triangles);
    //    HashSet<int> vertexIndices = new HashSet<int>();

    //    // 트라이앵글 인덱스를 vertexIndices에 추가
    //    for (int i = 0; i < _mesh.triangles.Length; i++)
    //    {
    //        vertexIndices.Add(_mesh.triangles[i]);
    //    }

    //    // 트라이앵글 인덱스에는 존재하지 않고 정점 인덱스에만 존재하는 정점을 식별하여 삭제
    //    List<Vector3> newVerticesList = new List<Vector3>(_mesh.vertices);
    //    foreach (int index in vertexIndices)
    //    {
    //        if (!triangleIndices.Contains(index))
    //        {
    //            Debug.Log("여기 몇번 들어오게 ?");
    //            newVerticesList[index] = Vector3.zero; // 혹은 다른 방법으로 정점을 삭제
    //        }
    //    }

    //    // 쓸모 없는 정점을 정리 (0 벡터로 설정된 정점들을 제거)
    //    newVerticesList.RemoveAll(vertex => vertex == Vector3.zero);

    //    // 갱신된 메시 정보 적용
    //    copyMesh.vertices = newVerticesList.ToArray();
    //}

    //private void RemoveVertex(int _minIndex)
    //{
    //    for (int i = 0; i < triangleLength; i += 3)
    //    {
    //        int i0 = copyMesh.triangles[i];
    //        int i1 = copyMesh.triangles[i + 1];
    //        int i2 = copyMesh.triangles[i + 2];

    //        Vector3 v0 = copyMesh.vertices[i0];
    //        Vector3 v1 = copyMesh.vertices[i1];
    //        Vector3 v2 = copyMesh.vertices[i2];

    //        if (IsPointInsideTriangle(v0, triangles[_minIndex]) && IsPointInsideTriangle(v1, triangles[_minIndex]) && IsPointInsideTriangle(v2, triangles[_minIndex]))
    //        {
    //            removedTriangles.Add(i);
    //            removedTriangles.Add(i + 1);
    //            removedTriangles.Add(i + 2);

    //            removedVertices.Add(i0);
    //            removedVertices.Add(i1);
    //            removedVertices.Add(i2);
    //        }
    //    }
    //}
    //private void MeshSimplification()
    //{
    //    Debug.LogFormat($"트라이앵글 초기 갯수 {triangleLength}");
    //    Debug.LogFormat($"버텍스 초기 갯수 {copyMesh.vertexCount}");

    //    int minIndex;

    //    Vector3 newPos;

    //    for (int k = 0; k < 100; k++)
    //    {
    //        // 딕셔너리에서 가장 작은 인덱스 찾기
    //        minIndex = triangleValue.OrderBy(kv => kv.Value).Skip(k).First().Key;
    //        // 가장 작은 인덱스에 해당하는 두 정점의 가운데 좌표
    //        newPos = CalculateTriangleCenter(triangles[minIndex].v0, triangles[minIndex].v1, triangles[minIndex].v2);

    //        for (int i = 0; i < triangleLength; i += 3)
    //        {
    //            int i0 = copyMesh.triangles[i];
    //            int i1 = copyMesh.triangles[i + 1];
    //            int i2 = copyMesh.triangles[i + 2];

    //            Vector3 v0 = copyMesh.vertices[i0];
    //            Vector3 v1 = copyMesh.vertices[i1];
    //            Vector3 v2 = copyMesh.vertices[i2];

    //            if (IsPointInsideTriangle(v0, triangles[minIndex]) && IsPointInsideTriangle(v1, triangles[minIndex]) && IsPointInsideTriangle(v2, triangles[minIndex]))
    //            {
    //                newVerties[i0] = newPos;
    //                newVerties[i1] = newPos;
    //                newVerties[i2] = newPos;
    //            }
    //            else if (IsPointInsideTriangle(v0, triangles[minIndex]) && IsPointInsideTriangle(v1, triangles[minIndex]))
    //            {
    //                newVerties[i0] = newPos;
    //                newVerties[i1] = newPos;
    //            }
    //            else if (IsPointInsideTriangle(v1, triangles[minIndex]) && IsPointInsideTriangle(v2, triangles[minIndex]))
    //            {
    //                newVerties[i1] = newPos;
    //                newVerties[i2] = newPos;
    //            }
    //            else if (IsPointInsideTriangle(v2, triangles[minIndex]) && IsPointInsideTriangle(v0, triangles[minIndex]))
    //            {
    //                newVerties[i2] = newPos;
    //                newVerties[i0] = newPos;
    //            }
    //            else if (IsPointInsideTriangle(v0, triangles[minIndex]))
    //            {
    //                newVerties[i0] = newPos;
    //            }
    //            else if (IsPointInsideTriangle(v1, triangles[minIndex]))
    //            {
    //                newVerties[i1] = newPos;
    //            }
    //            else if (IsPointInsideTriangle(v2, triangles[minIndex]))
    //            {
    //                newVerties[i2] = newPos;
    //            }
    //        }
    //    }



    //    Debug.Log(triangleLength);   // 3000
    //    Debug.Log(copyMesh.vertices.Length);    // 2646
    //    Debug.Log(newVerties.Count);          // 3000

    //    copyMesh.vertices = newVerties.ToArray();

    //    targetMeshFilter.sharedMesh = GFuncE.CopyMesh(copyMesh);

    //    Debug.LogFormat($"트라이앵글 이후 갯수 : {targetMeshFilter.sharedMesh.triangles.Length}");
    //}

    //private void MeshSimplification()
    //{
    //    Debug.LogFormat($"트라이앵글 초기 갯수 {triangleLength}");
    //    Debug.LogFormat($"버텍스 초기 갯수 {copyMesh.vertexCount}");

    //    int minIndex;
    //    Vector3 newPos;

    //    // Set to track removed vertices
    //    HashSet<int> removedVertices = new HashSet<int>();

    //    for (int k = 0; k < 10; k++)
    //    {
    //        // 딕셔너리에서 가장 작은 인덱스 찾기
    //        minIndex = triangleValue.OrderBy(kv => kv.Value).Skip(k).First().Key;
    //        // 가장 작은 인덱스에 해당하는 두 정점의 가운데 좌표
    //        newPos = CalculateTriangleCenter(triangles[minIndex].v0, triangles[minIndex].v1, triangles[minIndex].v2);

    //        Debug.Log($"반복: {k}, 정점 수: {newVerties.Count}");

    //        // Set to track removed triangles
    //        HashSet<int> removedTriangles = new HashSet<int>();

    //        for (int i = 0; i < triangleLength; i += 3)
    //        {
    //            int i0 = copyMesh.triangles[i];
    //            int i1 = copyMesh.triangles[i + 1];
    //            int i2 = copyMesh.triangles[i + 2];

    //            Vector3 v0 = copyMesh.vertices[i0];
    //            Vector3 v1 = copyMesh.vertices[i1];
    //            Vector3 v2 = copyMesh.vertices[i2];

    //            if (IsPointInsideTriangle(v0, triangles[minIndex]) && IsPointInsideTriangle(v1, triangles[minIndex]) && IsPointInsideTriangle(v2, triangles[minIndex]))
    //            {
    //                // Remove the triangle
    //                removedTriangles.Add(i);
    //                removedTriangles.Add(i + 1);
    //                removedTriangles.Add(i + 2);

    //                // Mark the vertices to be removed
    //                removedVertices.Add(i0);
    //                removedVertices.Add(i1);
    //                removedVertices.Add(i2);
    //            }
    //        }

    //        // Remove the vertices from newVerties list
    //        foreach (int removedVertexIndex in removedVertices)
    //        {
    //            newVerties[removedVertexIndex] = Vector3.zero;  // Or any other way to mark as removed
    //        }

    //        // Remove the triangles from copyMesh.triangles array
    //        copyMesh.triangles = copyMesh.triangles.Where((t, index) => !removedTriangles.Contains(index)).ToArray();
    //    }

    //    // Remove the vertices marked as removed
    //    newVerties = newVerties.Where(v => v != Vector3.zero).ToList();

    //    // Update Mesh.vertices array
    //    copyMesh.vertices = newVerties.ToArray();

    //    targetMeshFilter.sharedMesh = GFuncE.CopyMesh(copyMesh);

    //    Debug.LogFormat($"트라이앵글 이후 갯수 : {targetMeshFilter.sharedMesh.triangles.Length}");
    //}
    //private void QuadricErrorMetric()
    //{
    //    MeshFilter targetMeshFilter = GFuncE.SetComponent<MeshFilter>(targetObject);
    //    if (targetMeshFilter == null)
    //    {
    //        GFuncE.SubmitNonFindText(targetObject, typeof(MeshFilter));
    //        return;
    //    }
    //    Mesh targetMesh = targetMeshFilter.sharedMesh != null ? targetMeshFilter.sharedMesh : null;
    //    if (targetMesh == null)
    //    {
    //        GFuncE.SubmitNonFindText(targetObject, typeof(Mesh));
    //        return;
    //    }

    //    Mesh copyMesh = GFuncE.CopyMesh(targetMesh);
    //    copyMesh = SimplifyMesh(copyMesh, CalculateVertiesPercent());
    //    targetMeshFilter.sharedMesh = copyMesh;
    //}

    //// 각 삼각형의 각각의 정점에 대한 Quadric 행렬을 저장할 구조체
    //public struct Triangle
    //{
    //    public Matrix4x4 v0;
    //    public Matrix4x4 v1;
    //    public Matrix4x4 v2;
    //}

    //// Mesh의 각 정점에 대한 Quadric 행렬 계산
    //private Triangle[] CalculateQuadricMatrix(Mesh mesh)
    //{
    //    // 각 삼각형에 대한 Quadric 행렬을 저장할 배열
    //    Triangle[] triangles = new Triangle[mesh.triangles.Length / 3];

    //    for (int i = 0; i < triangles.Length; i++)
    //    {
    //        // 삼각형의 정점 인덱스 가져오기
    //        int index0 = mesh.triangles[i * 3];
    //        int index1 = mesh.triangles[i * 3 + 1];
    //        int index2 = mesh.triangles[i * 3 + 2];

    //        // 각 정점의 위치값 가져오기
    //        Vector3 vertex0 = mesh.vertices[index0];
    //        Vector3 vertex1 = mesh.vertices[index1];
    //        Vector3 vertex2 = mesh.vertices[index2];

    //        // 삼각형의 normal 계산
    //        Vector3 normal = Vector3.Cross(vertex1 - vertex0, vertex2 - vertex0).normalized;

    //        // 각 정점에 대한 Quadric 행렬을 계산하고 저장
    //        triangles[i] = new Triangle
    //        {
    //            v0 = CalculateQuadricMatrixForTriangle(vertex0, normal),
    //            v1 = CalculateQuadricMatrixForTriangle(vertex1, normal),
    //            v2 = CalculateQuadricMatrixForTriangle(vertex2, normal)
    //        };
    //    }

    //    return triangles;
    //}

    //// 삼각형의 정점에 대한 Quadric 행렬 계산
    //private Matrix4x4 CalculateQuadricMatrixForTriangle(Vector3 _vertex, Vector3 _normal)
    //{
    //    // normal 벡터 정규화
    //    Vector3 normalizedNormal = _normal.normalized;

    //    // Quadric 행렬의 계수
    //    float a = normalizedNormal.x;
    //    float b = normalizedNormal.y;
    //    float c = normalizedNormal.z;
    //    float d = -Vector3.Dot(normalizedNormal, _vertex);

    //    // 계수를 이용한 Quadirc 행렬 생성
    //    Matrix4x4 quadricMatrix = new Matrix4x4
    //        (
    //        new Vector4(a * a, a * b, a * c, a * d),
    //        new Vector4(b * a, b * b, b * c, b * d),
    //        new Vector4(c * a, c * b, c * c, c * d),
    //        new Vector4(d * a, d * b, d * c, d * d)
    //        );

    //    return quadricMatrix;
    //}

    //// QEM 행렬을 이용하여 에러 계산
    //private float CalculateError(Matrix4x4 quadricMatrix, Vector3 vertex)
    //{
    //    // 에러 계산식: E(v) = v^T * Q * v
    //    Vector4 vertex4 = new Vector4(vertex.x, vertex.y, vertex.z, 1.0f);
    //    return Vector4.Dot(vertex4, quadricMatrix * vertex4);
    //}

    //// 가장 에러가 작은 정점을 찾아 반환
    //private int FindOptimalVertex(Triangle triangle)
    //{
    //    float errorV0 = CalculateError(triangle.v0, triangle.v0.GetRow(3));
    //    float errorV1 = CalculateError(triangle.v1, triangle.v1.GetRow(3));
    //    float errorV2 = CalculateError(triangle.v2, triangle.v2.GetRow(3));

    //    if (errorV0 <= errorV1 && errorV0 <= errorV2)
    //    {
    //        return 0;
    //    }
    //    else if (errorV1 <= errorV0 && errorV1 <= errorV2)
    //    {
    //        return 1;
    //    }
    //    else
    //    {
    //        return 2;
    //    }
    //}

    //// 가장 에러가 작은 정점 쌍을 찾아 반환
    //private int[] FindOptimalVertexPair(Triangle[] triangles)
    //{
    //    float minError = float.MaxValue;
    //    int[] optimalPair = null;

    //    for (int i = 0; i < triangles.Length; i++)
    //    {
    //        Triangle triangle = triangles[i];

    //        for (int j = 0; j < 3; j++)
    //        {
    //            for (int k = j + 1; k < 3; k++)
    //            {
    //                float error = CalculateError(triangle.v0, triangle.v0.GetRow(3), j, triangle.v0.GetRow(3), k);
    //                error += CalculateError(triangle.v1, triangle.v1.GetRow(3), j, triangle.v1.GetRow(3), k);
    //                error += CalculateError(triangle.v2, triangle.v2.GetRow(3), j, triangle.v2.GetRow(3), k);

    //                if (error < minError)
    //                {
    //                    minError = error;
    //                    optimalPair = new int[] { j, k };
    //                }
    //            }
    //        }
    //    }

    //    return optimalPair;
    //}

    ////// 정점 쌍의 에러를 계산
    ////private float CalculateError(Matrix4x4 quadricMatrix, Vector3 vertex1, int index1, Vector3 vertex2, int index2)
    ////{
    ////    Vector4 vertex4 = new Vector4((vertex1.x + vertex2.x) * 0.5f, (vertex1.y + vertex2.y) * 0.5f, (vertex1.z + vertex2.z) * 0.5f, 1.0f);
    ////    float error = Vector4.Dot(vertex4, quadricMatrix * vertex4);
    ////    return error;
    ////}

    //// 가장 에러가 작은 정점 쌍을 찾아 반환
    //private int[] FindOptimalVertexPair(Triangle[] triangles, Mesh mesh)
    //{
    //    float minError = float.MaxValue;
    //    int[] optimalPair = null;

    //    for (int i = 0; i < triangles.Length; i++)
    //    {
    //        Triangle triangle = triangles[i];

    //        for (int j = 0; j < 3; j++)
    //        {
    //            for (int k = j + 1; k < 3; k++)
    //            {
    //                float error = CalculateError(triangle.v0, triangle.v0.GetRow(3), j, triangle.v0.GetRow(3), k);
    //                error += CalculateError(triangle.v1, triangle.v1.GetRow(3), j, triangle.v1.GetRow(3), k);
    //                error += CalculateError(triangle.v2, triangle.v2.GetRow(3), j, triangle.v2.GetRow(3), k);

    //                if (error < minError)
    //                {
    //                    minError = error;
    //                    optimalPair = new int[] { mesh.triangles[i * 3 + j], mesh.triangles[i * 3 + k] };
    //                }
    //            }
    //        }
    //    }

    //    return optimalPair;
    //}

    //// 메시 단순화 메서드
    //private Mesh SimplifyMesh(Mesh mesh, float targetPercentage)
    //{
    //    // Quadric 행렬 계산
    //    Triangle[] triangles = CalculateQuadricMatrix(mesh);

    //    // 각 삼각형의 곡률을 계산하고 인덱스와 함께 저장
    //    List<TriangleWithCurvature> trianglesWithCurvature = new List<TriangleWithCurvature>();
    //    for (int i = 0; i < triangles.Length; i++)
    //    {
    //        float curvature = CalculateCurvature(triangles[i]);
    //        trianglesWithCurvature.Add(new TriangleWithCurvature { Index = i, Curvature = curvature });
    //    }

    //    // 곡률이 낮은 순으로 정렬
    //    trianglesWithCurvature.Sort((a, b) => a.Curvature.CompareTo(b.Curvature));

    //    // 에러가 가장 작은 정점을 찾아서 제거하고 메시 업데이트
    //    int targetVertexCount = (int)(mesh.vertexCount * targetPercentage) + 1;
    //    List<int> newTriangles = new List<int>();

    //    while (mesh.vertexCount - newTriangles.Count / 3 > targetVertexCount)
    //    {
    //        // 가장 에러가 작은 정점 쌍을 찾기
    //        int[] optimalVertexPair = FindOptimalVertexPair(triangles, mesh);

    //        // 에러가 가장 작은 정점 쌍을 찾지 못하면 종료
    //        if (optimalVertexPair == null)
    //            break;

    //        // 정점을 병합하는 방식으로 변경
    //        newTriangles.Add(optimalVertexPair[1]);
    //        newTriangles.Add(optimalVertexPair[0]);
    //    }

    //    // 나머지 삼각형 인덱스 추가
    //    for (int i = 0; i < triangles.Length; i++)
    //    {
    //        newTriangles.Add(mesh.triangles[i * 3]);
    //        newTriangles.Add(mesh.triangles[i * 3 + 1]);
    //        newTriangles.Add(mesh.triangles[i * 3 + 2]);
    //    }

    //    // 새로운 트라이앵글 배열을 메시에 적용
    //    mesh.triangles = newTriangles.ToArray();
    //    mesh.Optimize();
    //    // 옵셔널: 메시 최적화
    //    return mesh;
    //}

    //private float CalculateError(Matrix4x4 quadricMatrix, Vector3 vertex1, int index1, Vector3 vertex2, int index2)
    //{
    //    // 두 정점의 가운데 값 계산
    //    Vector3 midpoint = (vertex1 + vertex2) * 0.5f;

    //    // 에러 계산식: E(v) = v^T * Q * v
    //    Vector4 vertex4 = new Vector4(midpoint.x, midpoint.y, midpoint.z, 1.0f);
    //    float error = Vector4.Dot(vertex4, quadricMatrix * vertex4);
    //    return error;
    //}

    //// 각 삼각형의 곡률을 계산
    //private float CalculateCurvature(Triangle triangle)
    //{
    //    // 각 정점의 Quadric 행렬 추출
    //    Matrix4x4 q0 = triangle.v0;
    //    Matrix4x4 q1 = triangle.v1;
    //    Matrix4x4 q2 = triangle.v2;

    //    // 각 Quadric 행렬의 트레이스 계산
    //    float traceQ0 = q0.m00 + q0.m11 + q0.m22 + q0.m33;
    //    float traceQ1 = q1.m00 + q1.m11 + q1.m22 + q1.m33;
    //    float traceQ2 = q2.m00 + q2.m11 + q2.m22 + q2.m33;

    //    // 삼각형의 곡률 계산
    //    float curvature = traceQ0 + traceQ1 + traceQ2;

    //    return curvature;
    //}

    //// 삼각형과 곡률 정보를 담는 구조체
    //private struct TriangleWithCurvature
    //{
    //    public int Index;
    //    public float Curvature;
    //}
}

//private void QuadricErrorMetric()
//{
//    MeshFilter targetMeshFilter = GFuncE.SetComponent<MeshFilter>(targetObject);
//    if (targetMeshFilter == null)
//    {
//        GFuncE.SubmitNonFindText(targetObject, typeof(MeshFilter));
//        return;
//    }
//    Mesh targetMesh = targetMeshFilter.sharedMesh != null ? targetMeshFilter.sharedMesh : null;
//    if (targetMesh == null)
//    {
//        GFuncE.SubmitNonFindText(targetObject, typeof(Mesh));
//        return;
//    }

//    Mesh copyMesh = GFuncE.CopyMesh(targetMesh);

//    copyMesh = SimplifyMesh(copyMesh);

//    copyMesh = UpdateMesh(copyMesh);

//    targetMeshFilter.sharedMesh = copyMesh;
//}

//private Mesh SimplifyMesh(Mesh _mesh)
//{
//    float value = CalculateVertiesPercent(_mesh);
//    Triangle[] triangles = CalculateQuadricMatrix(_mesh);
//    //_mesh = SortErrorMatric(_mesh, triangles);

//    //정점을 제거하는 작업을 수행합니다.
//    int targetVertexCount = Mathf.FloorToInt(value);
//    Debug.Log(targetVertexCount);
//    Debug.Log(_mesh.vertexCount);
//    while (_mesh.vertexCount > targetVertexCount)
//    {
//        // 정렬된 정점 배열을 가져옵니다.
//        int[] sortedIndices = SortIndicesByErrorMetric(CalculateVertexErrors(_mesh, triangles));

//        // 가장 마지막에 있는 정점을 제거합니다.
//        int lastVertexIndex = sortedIndices[sortedIndices.Length - 1];
//        RemoveVertex(_mesh, lastVertexIndex);
//    }

//    return _mesh;
//}

//private float[] CalculateVertexErrors(Mesh _mesh, Triangle[] _triangles)
//{
//    float[] errorMatrics = new float[_mesh.vertices.Length];

//    for (int i = 0; i < _mesh.vertices.Length; i++)
//    {
//        errorMatrics[i] = CalculateVertexErrorMatric(_mesh, _triangles, i);
//    }

//    return errorMatrics;
//}

//private void RemoveVertex(Mesh _mesh, int vertexIndex)
//{
//    List<Vector3> vertices = new List<Vector3>(_mesh.vertices);
//    List<Vector2> uv = new List<Vector2>(_mesh.uv);
//    List<Vector3> normals = new List<Vector3>(_mesh.normals);
//    List<Color> colors = new List<Color>(_mesh.colors);
//    List<Vector4> tangents = new List<Vector4>(_mesh.tangents);
//    List<BoneWeight> boneWeights = new List<BoneWeight>(_mesh.boneWeights);

//    // 정점 및 관련 데이터 제거
//    vertices.RemoveAt(vertexIndex);
//    uv.RemoveAt(vertexIndex);
//    normals.RemoveAt(vertexIndex);
//    colors.RemoveAt(vertexIndex);
//    tangents.RemoveAt(vertexIndex);
//    boneWeights.RemoveAt(vertexIndex);

//    // 메시 새로 만들기
//    _mesh.Clear();
//    _mesh.vertices = vertices.ToArray();
//    _mesh.uv = uv.ToArray();
//    _mesh.normals = normals.ToArray();
//    _mesh.colors = colors.ToArray();
//    _mesh.tangents = tangents.ToArray();
//    _mesh.boneWeights = boneWeights.ToArray();

//    // 삼각형 다시 계산
//    _mesh.triangles = RecalculateTriangles(_mesh.triangles, vertexIndex);

//    // 올바른 정점과 삼각형 정보로 업데이트
//    _mesh.RecalculateBounds();
//    _mesh.RecalculateNormals();
//}

//private int[] RecalculateTriangles(int[] triangles, int removedVertexIndex)
//{
//    List<int> newTriangles = new List<int>();

//    for (int i = 0; i < triangles.Length; i += 3)
//    {
//        int index0 = triangles[i];
//        int index1 = triangles[i + 1];
//        int index2 = triangles[i + 2];

//        if (index0 != removedVertexIndex && index1 != removedVertexIndex && index2 != removedVertexIndex)
//        {
//            // 제거된 정점을 포함하지 않는 삼각형만 추가
//            newTriangles.Add(index0);
//            newTriangles.Add(index1);
//            newTriangles.Add(index2);
//        }
//    }

//    return newTriangles.ToArray();
//}

//private Mesh UpdateMesh(Mesh _mesh)
//{
//    _mesh.RecalculateNormals();
//    _mesh.RecalculateBounds();

//    return _mesh;
//}





//// Quadric 행렬을 이용한 ErrorMatric 계산
//private float CalculateErrorMatric(Matrix4x4 _quadricMatrix, Vector3 _vertex)
//{
//    // 정점을 동차 좌표계로 나타내는 벡터 생성
//    Vector4 matrixValue = new Vector4(_vertex.x, _vertex.y, _vertex.z, 1);

//    // 정점 벡터를 Quadirc 행렬과 곱한 후 내적을 계산하여 ErrorMatric 계산
//    return Vector4.Dot(matrixValue, _quadricMatrix * matrixValue);
//}

//private Mesh SortErrorMatric(Mesh _mesh, Triangle[] _triangles)
//{
//    float[] errorMatrics = new float[_mesh.vertices.Length];
//    for (int i = 0; i < _mesh.vertices.Length; i++)
//    {
//        errorMatrics[i] = CalculateVertexErrorMatric(_mesh, _triangles, i);
//    }

//    int[] sortedIndices = SortIndicesByErrorMetric(errorMatrics);

//    Vector3[] sortedVertices = new Vector3[_mesh.vertices.Length];
//    for (int i = 0; i < _mesh.vertices.Length; i++)
//    {
//        sortedVertices[i] = _mesh.vertices[sortedIndices[i]];
//    }
//    _mesh.vertices = sortedVertices;

//    return _mesh;
//}

//private float CalculateVertexErrorMatric(Mesh _mesh, Triangle[] _triangles, int _vertexIndex)
//{
//    float totalError = 0f;

//    foreach (var triangle in _triangles)
//    {
//        if (Array.IndexOf(_mesh.triangles, _vertexIndex) != -1)
//        {
//            totalError += CalculateErrorMatric(triangle.v0, _mesh.vertices[_vertexIndex]);
//            totalError += CalculateErrorMatric(triangle.v1, _mesh.vertices[_vertexIndex]);
//            totalError += CalculateErrorMatric(triangle.v2, _mesh.vertices[_vertexIndex]);
//        }
//    }

//    return totalError;
//}

//private int[] SortIndicesByErrorMetric(float[] _errorMetrics)
//{
//    int[] indices = new int[_errorMetrics.Length];
//    for (int i = 0; i < indices.Length; i++)
//    {
//        indices[i] = i;
//    }
//    Array.Sort(indices, (a, b) => _errorMetrics[a].CompareTo(_errorMetrics[b]));

//    return indices;
//}




//private void QEM()
//{
//    MeshFilter targetMeshFilter = GFuncE.SetComponent<MeshFilter>(targetObject);
//    if (targetMeshFilter == null)
//    {
//        GFuncE.SubmitNonFindText(targetObject, typeof(MeshFilter));
//        return;
//    }
//    Mesh targetMesh = targetMeshFilter.sharedMesh != null ? targetMeshFilter.sharedMesh : null;
//    if (targetMesh == null)
//    {
//        GFuncE.SubmitNonFindText(targetObject, typeof(Mesh));
//        return;
//    }

//    Mesh copyMesh = GFuncE.CopyMesh(targetMesh);

//    Vector3[] vertices = copyMesh.vertices;
//    int[] triangles = copyMesh.triangles;

//    List<QEMVertex> qemVertices = new List<QEMVertex>();

//    for (int i = 0; i < vertices.Length; i++)
//    {
//        qemVertices.Add(new QEMVertex(vertices[i], i));
//    }

//    QEMHeap qemHeap = new QEMHeap(qemVertices);
//    while (qemHeap.Count > 2)
//    {
//        QEMVertexPair minErrorPair = qemHeap.PopMinErrorPair();

//        if (minErrorPair.Error > 0.1f)
//            break;

//        // 정점을 병합하고 메시를 업데이트합니다.
//        QEMVertex mergedVertex = minErrorPair.MergeVertices();
//        qemVertices.Add(mergedVertex);

//        // 삼각형을 업데이트합니다.
//        triangles = QEMUtility.UpdateTriangles(triangles, minErrorPair.Vertex1.Index, minErrorPair.Vertex2.Index, mergedVertex.Index);

//        // 병합된 정점의 QEM을 업데이트합니다.
//        QEMUtility.UpdateQEM(qemVertices, triangles, mergedVertex);

//        // 힙을 재정렬합니다.
//        qemHeap.UpdateHeap();
//    }

//    // 간략화된 데이터로 새로운 메시를 생성합니다.
//    Mesh simplifiedMesh = QEMUtility.CreateMeshFromQEMVertices(qemVertices);
//    targetMeshFilter.sharedMesh = simplifiedMesh;

//}


//public class QEMVertex
//{
//    public Vector3 Position { get; private set; }
//    public int Index { get; private set; }
//    public Matrix4x4 QEMMatrix { get; set; }

//    public QEMVertex(Vector3 position, int index)
//    {
//        Position = position;
//        Index = index;
//        QEMMatrix = new Matrix4x4();
//    }
//}

//public class QEMVertexPair
//{
//    public QEMVertex Vertex1 { get; private set; }
//    public QEMVertex Vertex2 { get; private set; }
//    public float Error { get; private set; }

//    public QEMVertexPair(QEMVertex vertex1, QEMVertex vertex2)
//    {
//        Vertex1 = vertex1;
//        Vertex2 = vertex2;
//        Error = CalculateError();
//    }

//    private float CalculateError()
//    {
//        return Vector3.Distance(Vertex1.Position, Vertex2.Position);
//    }

//    public QEMVertex MergeVertices()
//    {
//        Vector3 mergedPosition = (Vertex1.Position + Vertex2.Position) / 2f;
//        QEMVertex mergedVertex = new QEMVertex(mergedPosition, Vertex1.Index);

//        // 직접 구현한 AddOuterProduct 메서드를 사용하여 외적을 계산하고 더합니다.
//        Matrix4x4Extensions.AddOuterProduct(mergedVertex.QEMMatrix, new Vector4(Vertex1.Position.x, Vertex1.Position.y, Vertex1.Position.z, 1.0f),
//                                           new Vector4(Vertex2.Position.x, Vertex2.Position.y, Vertex2.Position.z, 1.0f));

//        return mergedVertex;
//    }
//}

//public class QEMHeap
//{
//    private List<QEMVertexPair> heap;

//    public int Count
//    {
//        get { return heap.Count; }
//    }

//    public QEMHeap(List<QEMVertex> vertices)
//    {
//        heap = new List<QEMVertexPair>();

//        for (int i = 0; i < vertices.Count; i++)
//        {
//            for (int j = i + 1; j < vertices.Count; j++)
//            {
//                heap.Add(new QEMVertexPair(vertices[i], vertices[j]));
//            }
//        }

//        heap.Sort((pair1, pair2) => pair1.Error.CompareTo(pair2.Error));
//    }

//    public QEMVertexPair PopMinErrorPair()
//    {
//        QEMVertexPair minErrorPair = heap[0];
//        heap.RemoveAt(0);
//        return minErrorPair;
//    }

//    public void UpdateHeap()
//    {
//        heap.Sort((pair1, pair2) => pair1.Error.CompareTo(pair2.Error));
//    }
//}

//public static class QEMUtility
//{
//    public static Matrix4x4 UpdateQEM(List<QEMVertex> vertices, int[] triangles, QEMVertex mergedVertex)
//    {
//        Matrix4x4 qemMatrix = new Matrix4x4();

//        for (int i = 0; i < triangles.Length; i += 3)
//        {
//            int v1 = triangles[i];
//            int v2 = triangles[i + 1];
//            int v3 = triangles[i + 2];

//            if (v1 == mergedVertex.Index || v2 == mergedVertex.Index || v3 == mergedVertex.Index)
//            {
//                Vector3 normal = Vector3.Cross(vertices[v2].Position - vertices[v1].Position, vertices[v3].Position - vertices[v1].Position).normalized;
//                Vector4 plane = new Vector4(normal.x, normal.y, normal.z, -Vector3.Dot(normal, mergedVertex.Position));

//                // 직접 구현한 AddOuterProduct 메서드를 사용하여 외적을 계산하고 더합니다.
//                Matrix4x4Extensions.AddOuterProduct(qemMatrix, plane, plane);
//            }
//        }

//        return qemMatrix;
//    }

//    public static int[] UpdateTriangles(int[] triangles, int vertexIndex1, int vertexIndex2, int mergedVertexIndex)
//    {
//        List<int> updatedTriangles = new List<int>();

//        for (int i = 0; i < triangles.Length; i += 3)
//        {
//            int v1 = triangles[i];
//            int v2 = triangles[i + 1];
//            int v3 = triangles[i + 2];

//            if (!((v1 == vertexIndex1 && v2 == vertexIndex2) || (v1 == vertexIndex2 && v2 == vertexIndex1) ||
//                  (v2 == vertexIndex1 && v3 == vertexIndex2) || (v2 == vertexIndex2 && v3 == vertexIndex1) ||
//                  (v3 == vertexIndex1 && v1 == vertexIndex2) || (v3 == vertexIndex2 && v1 == vertexIndex1)))
//            {
//                updatedTriangles.Add(v1);
//                updatedTriangles.Add(v2);
//                updatedTriangles.Add(v3);
//            }
//        }

//        updatedTriangles.Add(vertexIndex1);
//        updatedTriangles.Add(vertexIndex2);
//        updatedTriangles.Add(mergedVertexIndex);

//        return updatedTriangles.ToArray();
//    }

//    public static Mesh CreateMeshFromQEMVertices(List<QEMVertex> vertices)
//    {
//        Mesh mesh = new Mesh();
//        List<Vector3> meshVertices = new List<Vector3>();

//        for (int i = 0; i < vertices.Count; i++)
//        {
//            meshVertices.Add(vertices[i].Position);
//        }

//        mesh.vertices = meshVertices.ToArray();
//        mesh.triangles = Enumerable.Range(0, meshVertices.Count).ToArray(); // Just a simple triangle fan

//        mesh.RecalculateNormals();
//        mesh.RecalculateBounds();

//        return mesh;
//    }
//}
//}

//public static class Matrix4x4Extensions
//{
//    public static void AddOuterProduct(Matrix4x4 matrix, Vector4 lhs, Vector4 rhs)
//    {
//        for (int i = 0; i < 4; i++)
//        {
//            for (int j = 0; j < 4; j++)
//            {
//                matrix[i, j] += lhs[i] * rhs[j];
//            }
//        }
//    }
//}

//private float CalculateCurvature()

//private List<Triangle> GetTriangles(Mesh mesh)
//{
//    int[] triangles = mesh.triangles;
//    Vector3[] vertices = mesh.vertices;

//    int triangleCount = triangles.Length / 3;
//    List<Triangle> triangleArray = new List<Triangle>;

//    for (int i = 0; i < triangleCount; i++)
//    {
//        int v1Index = triangles[i * 3];
//        int v2Index = triangles[i * 3 + 1];
//        int v3Index = triangles[i * 3 + 2];

//        // 정점 인덱스가 올바르지 않으면 오류 출력 후 함수 종료
//        if (v1Index < 0 || v1Index >= vertices.Length ||
//            v2Index < 0 || v2Index >= vertices.Length ||
//            v3Index < 0 || v3Index >= vertices.Length)
//        {
//            Debug.LogError("올바르지 않은 정점 인덱스");
//            return null;
//        }

//        Vector3 v1 = vertices[v1Index];
//        Vector3 v2 = vertices[v2Index];
//        Vector3 v3 = vertices[v3Index];

//        triangleArray.Add(new Triangle { v1 = v1, v2 = v2, v3 = v3 });
//    }

//    return triangleArray;
//}

//struct Triangle
//{
//    public Vector3 v1;
//    public Vector3 v2;
//    public Vector3 v3;
//}

//struct Quadric
//{
//    public Matrix4x4 A;
//    public Vector3 b;
//    public float c;
//}

//private void SimplifyMesh(Mesh _copyMesh)
//{
//    Vector3[] vertices = _copyMesh.vertices;
//    int[] triangles = _copyMesh.triangles;

//    Quadric[] quadrics = new Quadric[triangles.Length];

//    InitializeQuadrics(vertices, triangles, quadrics);

//    EdgeCollapse(vertices, triangles, quadrics);


//}

//private void InitializeQuadrics(Vector3[] _vertices, int[] _triangles, Quadric[] _quadrics)
//{
//    for (int i = 0; i < _triangles.Length; i += 3)
//    {
//        int v1 = _triangles[i];
//        int v2 = _triangles[i + 1];
//        int v3 = _triangles[i + 2];

//        Quadric quadric = new Quadric();

//        Vector3 vertex1 = _vertices[v1];
//        Vector3 vertex2 = _vertices[v2];
//        Vector3 vertex3 = _vertices[v3];

//        Vector3 normal = Vector3.Cross(vertex2 - vertex1, vertex3 - vertex1).normalized;

//        quadric.A = CalculateQuadricMatrix(normal);
//        quadric.b = CalculateQuadricVector(normal, vertex1);
//        quadric.c = CalculateQuadricScalar(normal, vertex1);

//        _quadrics[i / 3] = quadric;
//    }

//}

//private Matrix4x4 CalculateQuadricMatrix(Vector3 normal)
//{
//    Matrix4x4 quadricMatrix = new Matrix4x4();

//    // Quadric 행렬 A 계산
//    quadricMatrix.SetRow(0, new Vector4(normal.x * normal.x, normal.x * normal.y, normal.x * normal.z, 0));
//    quadricMatrix.SetRow(1, new Vector4(normal.y * normal.x, normal.y * normal.y, normal.y * normal.z, 0));
//    quadricMatrix.SetRow(2, new Vector4(normal.z * normal.x, normal.z * normal.y, normal.z * normal.z, 0));
//    quadricMatrix.SetRow(3, new Vector4(0, 0, 0, 0)); // 마지막 행은 0으로 초기화

//    return quadricMatrix;
//}

//private Vector3 CalculateQuadricVector(Vector3 normal, Vector3 vertex)
//{
//    // Quadric 벡터 b 계산
//    return -2 * Vector3.Scale(normal, Vector3.Scale(normal, vertex));
//}

//private float CalculateQuadricScalar(Vector3 normal, Vector3 vertex)
//{
//    // Quadric 스칼라 c 계산
//    return vertex.sqrMagnitude;
//}

//private void EdgeCollapse(Vector3[] _vertices, int[] _triangles, Quadric[] _quadrics)
//{
//    // 에지를 축약할 횟수 또는 원하는 정점 개수에 도달할 때까지 반복
//    for (int collapseCount = 0; collapseCount < errorTolerance.intValue; collapseCount++)
//    {
//        float minError = float.MaxValue;
//        int bestEdgeIndex = -1;

//        // 각 에지에 대해 에러를 계산하고 최소 에러를 가지는 에지 선택
//        for (int i = 0; i < _triangles.Length; i += 3)
//        {
//            int v1 = _triangles[i];
//            int v2 = _triangles[i + 1];
//            int v3 = _triangles[i + 2];

//            float error = CalculateEdgeCollapseError(v1, v2, _quadrics[i / 3], _vertices);

//            if (error < minError)
//            {
//                minError = error;
//                bestEdgeIndex = i;
//            }

//            // 반대 방향의 에지에 대해서도 에러 계산
//            error = CalculateEdgeCollapseError(v2, v3, _quadrics[i / 3], _vertices);

//            if (error < minError)
//            {
//                minError = error;
//                bestEdgeIndex = i + 1;
//            }

//            error = CalculateEdgeCollapseError(v3, v1, _quadrics[i / 3], _vertices);

//            if (error < minError)
//            {
//                minError = error;
//                bestEdgeIndex = i + 2;
//            }
//        }

//        // 선택된 에지를 축약하고 메쉬를 업데이트
//        if (bestEdgeIndex != -1)
//        {
//            PerformEdgeCollapse(bestEdgeIndex, _triangles, _vertices, _quadrics);
//        }
//    }
//}

//private float CalculateEdgeCollapseError(int v1, int v2, Quadric quadric, Vector3[] vertices)
//{
//    if (v1 < 0 || v1 >= vertices.Length || v2 < 0 || v2 >= vertices.Length)
//    {
//        Debug.LogError("Invalid vertex indices in CalculateEdgeCollapseError");
//        return float.MaxValue;
//    }

//    // 축약 후의 정점 위치 계산
//    Vector3 collapsedVertex = CalculateCollapsedVertex(vertices[v1], vertices[v2], quadric);

//    // 에러 계산
//    float error = CalculateQuadricError(collapsedVertex, quadric);

//    return error;
//}

//private Vector3 CalculateCollapsedVertex(Vector3 v1, Vector3 v2, Quadric quadric)
//{
//    return (v1 + v2) / 2;
//}

//private float CalculateQuadricError(Vector3 vertex, Quadric quadric)
//{
//    // Quadric 에러 계산
//    return vertex.x * (quadric.A.m00 * vertex.x + quadric.A.m01 * vertex.y + quadric.A.m02 * vertex.z +
//                     quadric.b.x) +
//           vertex.y * (quadric.A.m10 * vertex.x + quadric.A.m11 * vertex.y + quadric.A.m12 * vertex.z +
//                     quadric.b.y) +
//           vertex.z * (quadric.A.m20 * vertex.x + quadric.A.m21 * vertex.y + quadric.A.m22 * vertex.z +
//                     quadric.b.z) +
//           quadric.c;
//}

//private void PerformEdgeCollapse(int edgeIndex, int[] triangles, Vector3[] vertices, Quadric[] quadrics)
//{
//    int v1 = triangles[edgeIndex];
//    int v2 = triangles[edgeIndex + 1];
//    int v3 = triangles[edgeIndex + 2];

//    // 선택된 에지를 축약하고 새로운 정점 위치 계산
//    Vector3 collapsedVertex = CalculateCollapsedVertex(vertices[v1], vertices[v2], quadrics[edgeIndex / 3]);

//    // 에지 축약
//    vertices[v1] = collapsedVertex;

//    // 에지에 연결된 인접한 삼각형들 업데이트
//    UpdateAdjacentTriangles(edgeIndex, triangles, vertices, quadrics);

//    // 축약된 에지와 연결된 정점들 업데이트
//    UpdateAdjacentVertices(v1, v2, triangles, vertices, quadrics);

//    // 축약된 에지를 사용한 인접한 삼각형들의 메쉬에서 제거
//    RemoveCollapsedEdgeTriangles(edgeIndex, triangles);
//}

//private void UpdateAdjacentTriangles(int edgeIndex, int[] triangles, Vector3[] vertices, Quadric[] quadrics)
//{
//    // 선택된 에지에 연결된 인접한 삼각형들의 인덱스 가져오기
//    int adjacentTriangleIndex1 = FindAdjacentTriangle(edgeIndex, triangles);
//    int adjacentTriangleIndex2 = FindAdjacentTriangle(edgeIndex + 1, triangles);
//    int adjacentTriangleIndex3 = FindAdjacentTriangle(edgeIndex + 2, triangles);

//    // 각 인접한 삼각형의 정점 정보 및 Quadric 정보 업데이트
//    UpdateTriangle(adjacentTriangleIndex1, triangles, vertices, quadrics);
//    UpdateTriangle(adjacentTriangleIndex2, triangles, vertices, quadrics);
//    UpdateTriangle(adjacentTriangleIndex3, triangles, vertices, quadrics);
//}

//private void UpdateTriangle(int triangleIndex, int[] triangles, Vector3[] vertices, Quadric[] quadrics)
//{
//    if (triangleIndex != -1)
//    {
//        // 삼각형의 정점 인덱스 가져오기
//        int v1 = triangles[triangleIndex];
//        int v2 = triangles[triangleIndex + 1];
//        int v3 = triangles[triangleIndex + 2];

//        // Quadric 업데이트
//        quadrics[triangleIndex / 3] = CalculateQuadric(vertices[v1], vertices[v2], vertices[v3]);

//        // 인접한 정점들의 Quadric 정보 업데이트
//        UpdateVertexQuadric(v1, triangles, vertices, quadrics);
//        UpdateVertexQuadric(v2, triangles, vertices, quadrics);
//        UpdateVertexQuadric(v3, triangles, vertices, quadrics);
//    }
//}

//private Quadric CalculateQuadric(Vector3 v1, Vector3 v2, Vector3 v3)
//{
//    Quadric quadric = new Quadric();

//    Vector3 normal = Vector3.Cross(v2 - v1, v3 - v1).normalized;

//    quadric.A = CalculateQuadricMatrix(normal);
//    quadric.b = CalculateQuadricVector(normal, v1);
//    quadric.c = CalculateQuadricScalar(normal, v1);

//    return quadric;
//}

//private void UpdateVertexQuadric(int vertexIndex, int[] triangles, Vector3[] vertices, Quadric[] quadrics)
//{
//    // 주어진 정점에 연결된 인접한 삼각형들의 Quadric을 합하여 업데이트
//    Quadric updatedQuadric = new Quadric();

//    for (int i = 0; i < triangles.Length; i += 3)
//    {
//        if (triangles[i] == vertexIndex || triangles[i + 1] == vertexIndex || triangles[i + 2] == vertexIndex)
//        {
//            updatedQuadric.A.SetColumn(0, updatedQuadric.A.GetColumn(0) + quadrics[i / 3].A.GetColumn(0));
//            updatedQuadric.A.SetColumn(1, updatedQuadric.A.GetColumn(1) + quadrics[i / 3].A.GetColumn(1));
//            updatedQuadric.A.SetColumn(2, updatedQuadric.A.GetColumn(2) + quadrics[i / 3].A.GetColumn(2));
//            updatedQuadric.A.SetColumn(3, updatedQuadric.A.GetColumn(3) + quadrics[i / 3].A.GetColumn(3));
//            updatedQuadric.b += quadrics[i / 3].b;
//            updatedQuadric.c += quadrics[i / 3].c;
//        }
//    }

//    // 업데이트된 Quadric으로 정점의 Quadric 업데이트
//    quadrics[vertexIndex] = updatedQuadric;
//}

//private void UpdateAdjacentVertices(int v1, int v2, int[] triangles, Vector3[] vertices, Quadric[] quadrics)
//{
//    // 축약된 에지와 연결된 인접한 정점들의 Quadric 정보 업데이트
//    UpdateVertexQuadric(v1, triangles, vertices, quadrics);
//    UpdateVertexQuadric(v2, triangles, vertices, quadrics);
//}

//private void RemoveCollapsedEdgeTriangles(int edgeIndex, int[] triangles)
//{
//    // 축약된 에지를 사용한 인접한 삼각형들을 triangles 배열에서 제거
//    int triangleIndex1 = edgeIndex / 3;
//    int triangleIndex2 = FindAdjacentTriangle(edgeIndex + 1, triangles) / 3;
//    int triangleIndex3 = FindAdjacentTriangle(edgeIndex + 2, triangles) / 3;

//    triangles[triangleIndex1 * 3] = -1;
//    triangles[triangleIndex1 * 3 + 1] = -1;
//    triangles[triangleIndex1 * 3 + 2] = -1;

//    if (triangleIndex2 != -1)
//    {
//        triangles[triangleIndex2 * 3] = -1;
//        triangles[triangleIndex2 * 3 + 1] = -1;
//        triangles[triangleIndex2 * 3 + 2] = -1;
//    }

//    if (triangleIndex3 != -1)
//    {
//        triangles[triangleIndex3 * 3] = -1;
//        triangles[triangleIndex3 * 3 + 1] = -1;
//        triangles[triangleIndex3 * 3 + 2] = -1;
//    }
//}

//private int FindAdjacentTriangle(int edgeIndex, int[] triangles)
//{
//    // 주어진 에지에 인접한 다른 삼각형을 찾아 반환
//    int v1 = triangles[edgeIndex];
//    int v2 = triangles[edgeIndex + 1];
//    int v3 = triangles[edgeIndex + 2];

//    for (int i = 0; i < triangles.Length; i += 3)
//    {
//        if (i != edgeIndex && (triangles[i] == v1 || triangles[i] == v2 || triangles[i] == v3 ||
//                               triangles[i + 1] == v1 || triangles[i + 1] == v2 || triangles[i + 1] == v3 ||
//                               triangles[i + 2] == v1 || triangles[i + 2] == v2 || triangles[i + 2] == v3))
//        {
//            return i;
//        }
//    }

//    return -1; // 인접한 다른 삼각형이 없는 경우
//}
//}

//public class Triangle
//{
//    public Vector3[] vertices;
//    public Vector3 center;

//    public Triangle(Vector3[] _vertices)
//    {
//        this.vertices = _vertices;
//        this.center = GetCenter();
//    }

//    public Vector3 GetCenter()
//    {
//        return (vertices[0] + vertices[1] + vertices[2]) / 3;
//    }
//}       // Class Triangle
//private void LowerObject()
//{
//    MeshFilter targetMeshFilter = GFuncE.SetComponent<MeshFilter>(targetObject);
//    if (targetMeshFilter == null)
//    {
//        GFuncE.SubmitNonFindText(targetObject, typeof(MeshFilter));
//        return;
//    }
//    Mesh targetMesh = targetMeshFilter.sharedMesh != null ? targetMeshFilter.sharedMesh : null;
//    if (targetMesh == null)
//    {
//        GFuncE.SubmitNonFindText(targetObject, typeof(Mesh));
//        return;
//    }

//    Mesh copyMesh = GFuncE.CopyMesh(targetMesh);

//    List<Triangle> triangles = GetTriangles(copyMesh);

//    triangles = QEM(triangles, errorTolerance.floatValue);

//    Vector3[] newVertices = UpdateMesh(copyMesh, triangles);

//    // { targetObj 메쉬의 법선, 접선, 경계를 재계산
//    copyMesh.SetVertices(newVertices);
//    copyMesh.RecalculateNormals();
//    copyMesh.RecalculateTangents();
//    copyMesh.RecalculateBounds();
//    // } targetObj 메쉬의 법선, 접선, 경계를 재계산

//    targetMeshFilter.sharedMesh = copyMesh;
//}       // LowerObject()

//// 기울기 행렬 Q를 계산
//private Matrix4x4 CalculateInclination(Vector3[] _vertices)
//{
//    Matrix4x4 Q_ = new Matrix4x4();
//    for (int i = 0; i < _vertices.Length; i++)
//    {
//        Vector3 n = _vertices[(i + 1) % _vertices.Length] - _vertices[i];
//        Q_[i, 0] = n.x;
//        Q_[i, 1] = n.y;
//        Q_[i, 2] = n.z;
//    }
//    return Q_;
//}       // CalculateInclination()

//// 상수 c를 계산
//private float CalculateConstant(Vector3[] _vertices, Matrix4x4 _Q, Vector3 _center)
//{
//    float c = 0;
//    for (int i = 0; i < _vertices.Length; i++)
//    {
//        c += _center.x * _Q[i, 0] + _center.y * _Q[i, 1] + _center.z * _Q[i, 2];
//    }
//    return -c;
//}       // CalculateConstant()

//// 삼각형의 오차 E를 계산
//private float CalculateError(Vector3[] _vertices, Matrix4x4 _Q, float _c, Vector3 _v)
//{
//    return Vector3.Dot(_v, _Q * _v) + _c;
//}       // CalculateError()

//private List<Triangle> QEM(List<Triangle> _triangles, float _errorTolerance)
//{
//    //if (_triangles.Count == 300)
//    //{
//    //    return _triangles;
//    //}

//    // 모든 삼각형의 기울기 행렬 Q와 상수 c를 계산
//    Matrix4x4[] q_ = new Matrix4x4[_triangles.Count];
//    float[] c_ = new float[_triangles.Count];

//    for (int i = 0; i < _triangles.Count; i++)
//    {
//        q_[i] = CalculateInclination(_triangles[i].vertices);
//        c_[i] = CalculateConstant(_triangles[i].vertices, q_[i], _triangles[i].center);
//    }

//    // 모든 삼각형의 오차 E를 계산
//    float[] errors_ = new float[_triangles.Count];

//    for (int i = 0; i < _triangles.Count; i++)
//    {
//        errors_[i] = CalculateError(_triangles[i].vertices, q_[i], c_[i], _triangles[i].center);
//    }

//    // 오차 E가 가장 작은 삼각형을 제거하거나 병합
//    int minIndex_ = 0;
//    float minError_ = errors_[0];

//    Debug.Log(minError_);
//    for (int i = 0; i < errors_.Length; i++)
//    {
//        if (errors_[i] < minError_)
//        {
//            minIndex_ = i;
//            minError_ = errors_[i];
//        }
//    }

//    //if (minError_ < errorTolerance.floatValue)
//    //{
//    //    // 삼각형 제거
//    //    _triangles.RemoveAt(minIndex_);
//    //}
//    if (minError_ < errorTolerance.floatValue)
//    {
//        // 삼각형 병합
//        int otherIndex_ = minIndex_;
//        for (int i = 0; i < _triangles.Count; i++)
//        {
//            if (i != minIndex_ && Overlaps(_triangles[i], _triangles[minIndex_]))
//            {
//                otherIndex_ = i;
//                break;
//            }
//        }

//        _triangles[minIndex_] = MergeTriangle(_triangles[minIndex_], _triangles[otherIndex_]);
//        _triangles.RemoveAt(otherIndex_);

//        return QEM(_triangles, errorTolerance.floatValue);
//    }

//    return _triangles;

//}       // QEM()

//private bool Overlaps(Triangle _t1, Triangle _t2)
//{
//    if (!IsCoplanar(_t1, _t2))
//    {
//        return false;
//    }

//    float[] distances = new float[3];
//    for (int i = 0; i < 3; i++)
//    {
//        distances[i] = Vector3.Distance(_t1.vertices[i], _t2.vertices[i]);
//    }

//    for (int i = 0; i < 3; i++)
//    {
//        if (_t1.vertices[i] != _t2.vertices[i])
//        {
//            return false;
//        }
//    }

//    return true;
//}       // Overlaps()

//private Triangle MergeTriangle(Triangle _t1, Triangle _t2)
//{
//    Vector3 v1 = (_t1.vertices[0] + _t2.vertices[0]) * 0.5f;
//    Vector3 v2 = (_t1.vertices[1] + _t2.vertices[1]) * 0.5f;
//    Vector3 v3 = (_t1.vertices[2] + _t2.vertices[2]) * 0.5f;

//    Triangle mergedTriangle_ = new Triangle(new Vector3[] { v1, v2, v3 });
//    mergedTriangle_.vertices = OrderVertices(mergedTriangle_.vertices);

//    return mergedTriangle_;
//}       // MergeTriangle()

//private Vector3[] OrderVertices(Vector3[] _vertices)
//{
//    // Order vertices to ensure correct winding order
//    Vector3 normal = Vector3.Cross(_vertices[1] - _vertices[0], _vertices[2] - _vertices[0]).normalized;

//    if (Vector3.Dot(normal, Vector3.Cross(_vertices[1] - _vertices[0], _vertices[2] - _vertices[1])) < 0)
//    {
//        // Swap vertices to maintain correct winding order
//        Vector3 temp = _vertices[1];
//        _vertices[1] = _vertices[2];
//        _vertices[2] = temp;
//    }

//    return _vertices;
//}       // OrderVertices()

//private bool IsCoplanar(Triangle _t1, Triangle _t2)
//{
//    Vector3 normal1 = Normal(_t1);
//    Vector3 normal2 = Normal(_t2);

//    float dot = Vector3.Dot(normal1, normal2);

//    // 허용 가능한 오차 범위 내에서 dot product가 1에 가까우면 공평면에 있다고 판단
//    return dot >= 0.99999f && dot <= 1.00001f;
//}       // IsCoplanar()

//private Vector3 Normal(Triangle _triangle)
//{
//    // 삼각형의 노말 벡터를 계산하는 방법은 여러 가지가 있습니다.
//    // 여기서는 간단하게 삼각형의 두 변의 외적을 계산해서 노말 벡터를 구합니다.
//    Vector3 edge1 = _triangle.vertices[1] - _triangle.vertices[0];
//    Vector3 edge2 = _triangle.vertices[2] - _triangle.vertices[0];

//    return Vector3.Cross(edge1, edge2).normalized;
//}       // Normal()

//private List<Triangle> GetTriangles(Mesh _mesh)
//{
//    List<Triangle> triangles = new List<Triangle>();

//    int[] indices = _mesh.triangles;
//    Vector3[] vertices = _mesh.vertices;

//    for (int i = 0; i < indices.Length; i += 3)
//    {
//        Triangle triangle = new Triangle(new Vector3[]
//        {
//        vertices[indices[i]],
//        vertices[indices[i + 1]],
//        vertices[indices[i + 2]]
//        });

//        triangles.Add(triangle);
//    }

//    return triangles;
//}       // GetTriangles()

//private Vector3[] UpdateMesh(Mesh _mesh, List<Triangle> _triangles)
//{
//    List<Vector3> newVertices = new List<Vector3>();
//    List<int> newIndices = new List<int>();

//    int vertexIndex = 0;

//    foreach (var triangle in _triangles)
//    {
//        newVertices.AddRange(triangle.vertices);

//        // Add valid indices for the new vertices
//        for (int i = 0; i < triangle.vertices.Length; i++)
//        {
//            newIndices.Add(vertexIndex++);
//        }
//    }

//    _mesh.Clear();
//    _mesh.SetVertices(newVertices);
//    _mesh.SetTriangles(newIndices, 0);
//    _mesh.RecalculateNormals();
//    _mesh.RecalculateTangents();
//    _mesh.RecalculateBounds();

//    return newVertices.ToArray();
//}       // UpdateMesh()
#endregion