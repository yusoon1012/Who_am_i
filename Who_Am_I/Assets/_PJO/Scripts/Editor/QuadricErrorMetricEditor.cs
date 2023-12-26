using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(QuadricErrorMetric))]
public class QuadricErrorMetricEditor : Editor
{
    // 정점을 줄일 오브젝트
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

        // 인스펙터에 변수를 편집 가능한 필드로 표시
        EditorGUILayout.PropertyField(targetObject, new GUIContent("퀄리티를 낮출 오브젝트"));

        // "Apply" 버튼 클릭시 정점을 지움
        if (GUILayout.Button("Apply"))
        {
            QEM();
        }

        serializedObject.ApplyModifiedProperties();
    }       // OnInspectorGUI()

    private void QEM()
    {
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

        Mesh copyMesh = GFuncE.CopyMesh(targetMesh);

        Vector3[] vertices = copyMesh.vertices;
        int[] triangles = copyMesh.triangles;

        List<QEMVertex> qemVertices = new List<QEMVertex>();

        for (int i = 0; i < vertices.Length; i++)
        {
            qemVertices.Add(new QEMVertex(vertices[i], i));
        }

        QEMHeap qemHeap = new QEMHeap(qemVertices);
        while (qemHeap.Count > 2)
        {
            QEMVertexPair minErrorPair = qemHeap.PopMinErrorPair();

            if (minErrorPair.Error > 0.1f)
                break;

            // 정점을 병합하고 메시를 업데이트합니다.
            QEMVertex mergedVertex = minErrorPair.MergeVertices();
            qemVertices.Add(mergedVertex);

            // 삼각형을 업데이트합니다.
            triangles = QEMUtility.UpdateTriangles(triangles, minErrorPair.Vertex1.Index, minErrorPair.Vertex2.Index, mergedVertex.Index);

            // 병합된 정점의 QEM을 업데이트합니다.
            QEMUtility.UpdateQEM(qemVertices, triangles, mergedVertex);

            // 힙을 재정렬합니다.
            qemHeap.UpdateHeap();
        }

        // 간략화된 데이터로 새로운 메시를 생성합니다.
        Mesh simplifiedMesh = QEMUtility.CreateMeshFromQEMVertices(qemVertices);
        targetMeshFilter.sharedMesh = simplifiedMesh;

    }


    public class QEMVertex
    {
        public Vector3 Position { get; private set; }
        public int Index { get; private set; }
        public Matrix4x4 QEMMatrix { get; set; }

        public QEMVertex(Vector3 position, int index)
        {
            Position = position;
            Index = index;
            QEMMatrix = new Matrix4x4();
        }
    }

    public class QEMVertexPair
    {
        public QEMVertex Vertex1 { get; private set; }
        public QEMVertex Vertex2 { get; private set; }
        public float Error { get; private set; }

        public QEMVertexPair(QEMVertex vertex1, QEMVertex vertex2)
        {
            Vertex1 = vertex1;
            Vertex2 = vertex2;
            Error = CalculateError();
        }

        private float CalculateError()
        {
            return Vector3.Distance(Vertex1.Position, Vertex2.Position);
        }

        public QEMVertex MergeVertices()
        {
            Vector3 mergedPosition = (Vertex1.Position + Vertex2.Position) / 2f;
            QEMVertex mergedVertex = new QEMVertex(mergedPosition, Vertex1.Index);

            // 직접 구현한 AddOuterProduct 메서드를 사용하여 외적을 계산하고 더합니다.
            Matrix4x4Extensions.AddOuterProduct(mergedVertex.QEMMatrix, new Vector4(Vertex1.Position.x, Vertex1.Position.y, Vertex1.Position.z, 1.0f),
                                               new Vector4(Vertex2.Position.x, Vertex2.Position.y, Vertex2.Position.z, 1.0f));

            return mergedVertex;
        }
    }

    public class QEMHeap
    {
        private List<QEMVertexPair> heap;

        public int Count
        {
            get { return heap.Count; }
        }

        public QEMHeap(List<QEMVertex> vertices)
        {
            heap = new List<QEMVertexPair>();

            for (int i = 0; i < vertices.Count; i++)
            {
                for (int j = i + 1; j < vertices.Count; j++)
                {
                    heap.Add(new QEMVertexPair(vertices[i], vertices[j]));
                }
            }

            heap.Sort((pair1, pair2) => pair1.Error.CompareTo(pair2.Error));
        }

        public QEMVertexPair PopMinErrorPair()
        {
            QEMVertexPair minErrorPair = heap[0];
            heap.RemoveAt(0);
            return minErrorPair;
        }

        public void UpdateHeap()
        {
            heap.Sort((pair1, pair2) => pair1.Error.CompareTo(pair2.Error));
        }
    }

    public static class QEMUtility
    {
        public static Matrix4x4 UpdateQEM(List<QEMVertex> vertices, int[] triangles, QEMVertex mergedVertex)
        {
            Matrix4x4 qemMatrix = new Matrix4x4();

            for (int i = 0; i < triangles.Length; i += 3)
            {
                int v1 = triangles[i];
                int v2 = triangles[i + 1];
                int v3 = triangles[i + 2];

                if (v1 == mergedVertex.Index || v2 == mergedVertex.Index || v3 == mergedVertex.Index)
                {
                    Vector3 normal = Vector3.Cross(vertices[v2].Position - vertices[v1].Position, vertices[v3].Position - vertices[v1].Position).normalized;
                    Vector4 plane = new Vector4(normal.x, normal.y, normal.z, -Vector3.Dot(normal, mergedVertex.Position));

                    // 직접 구현한 AddOuterProduct 메서드를 사용하여 외적을 계산하고 더합니다.
                    Matrix4x4Extensions.AddOuterProduct(qemMatrix, plane, plane);
                }
            }

            return qemMatrix;
        }

        public static int[] UpdateTriangles(int[] triangles, int vertexIndex1, int vertexIndex2, int mergedVertexIndex)
        {
            List<int> updatedTriangles = new List<int>();

            for (int i = 0; i < triangles.Length; i += 3)
            {
                int v1 = triangles[i];
                int v2 = triangles[i + 1];
                int v3 = triangles[i + 2];

                if (!((v1 == vertexIndex1 && v2 == vertexIndex2) || (v1 == vertexIndex2 && v2 == vertexIndex1) ||
                      (v2 == vertexIndex1 && v3 == vertexIndex2) || (v2 == vertexIndex2 && v3 == vertexIndex1) ||
                      (v3 == vertexIndex1 && v1 == vertexIndex2) || (v3 == vertexIndex2 && v1 == vertexIndex1)))
                {
                    updatedTriangles.Add(v1);
                    updatedTriangles.Add(v2);
                    updatedTriangles.Add(v3);
                }
            }

            updatedTriangles.Add(vertexIndex1);
            updatedTriangles.Add(vertexIndex2);
            updatedTriangles.Add(mergedVertexIndex);

            return updatedTriangles.ToArray();
        }

        public static Mesh CreateMeshFromQEMVertices(List<QEMVertex> vertices)
        {
            Mesh mesh = new Mesh();
            List<Vector3> meshVertices = new List<Vector3>();

            for (int i = 0; i < vertices.Count; i++)
            {
                meshVertices.Add(vertices[i].Position);
            }

            mesh.vertices = meshVertices.ToArray();
            mesh.triangles = Enumerable.Range(0, meshVertices.Count).ToArray(); // Just a simple triangle fan

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}

public static class Matrix4x4Extensions
{
    public static void AddOuterProduct(Matrix4x4 matrix, Vector4 lhs, Vector4 rhs)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                matrix[i, j] += lhs[i] * rhs[j];
            }
        }
    }
}

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