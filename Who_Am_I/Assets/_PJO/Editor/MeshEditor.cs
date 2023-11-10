using UnityEditor;
using UnityEngine;

// Unity에서 제공하는 Editor 스크립팅 API를 사용하여 MeshController스크립트를 사용자 정의 에디터로 만들겠다는 선언
[CustomEditor(typeof(MeshController))]
public class MeshControllerEditor : Editor
{
    /*
     * SerializedProperty: Unity스크립트의 public및 [SerializeField]로 지정된 변수들이 저장되는 공간
     */

    // SerializedProperty를 선언하여 인스펙터창에 보이게 될 변수
    SerializedProperty verticesValue;

    // Unity에서 기본으로 제공하는 Mesh저장
    private Mesh originMesh = default;

    // 지속적으로 수정될 Mesh를 저장할 변수
    private Mesh mesh = default;

    //! 유니티 에디터모드를 호출할 때 한번 호출
    private void OnEnable()
    {
        // SerializedProperty 초기화
        verticesValue = serializedObject.FindProperty("verticesValue");

        // { Unity에서 기본적으로 제공하는 Plane Mesh를 originMesh변수에 할당
        GameObject planeObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
        originMesh = planeObject.GetComponent<MeshFilter>().sharedMesh;
        DestroyImmediate(planeObject);
        // } Unity에서 기본적으로 제공하는 Plane Mesh를 originMesh변수에 할당
    }       // OnEnable()

    //! 유니티 에디터모드일때 바로바로 수정할 수 있는 플러그인
    public override void OnInspectorGUI()
    {
        // SerializedProperty 업데이트
        serializedObject.Update();

        // target오브젝트의 MeshController스크립트 및 MeshFilter컴포넌트를 가져옴
        MeshController meshController = (MeshController)target;
        MeshFilter meshFilter = meshController.GetComponent<MeshFilter>();

        // 인스펙터에 verticesValue변수를 편집 가능한 필드로 표시
        EditorGUILayout.PropertyField(verticesValue, new GUIContent("Vertices Value"));

        // "Apply" 버튼 클릭시 Unity에서 기본으로 제공하는 Plane Mesh를 복사하여 적용
        if (GUILayout.Button("Apply"))
        {
            // 1. Unity에서 기본으로 제공하는 Plane Mesh를 적용
            /*
             * 이를 안할 경우 분할한 Mesh를 또 분할하는 현상이 일어남
             */
            meshFilter.sharedMesh = originMesh;

            // 2. Mesh 복사본 생성, 기존 Mesh 데이터 복사
            mesh = meshFilter.sharedMesh;
            Mesh copyMesh = new Mesh();
            copyMesh.vertices = mesh.vertices;
            copyMesh.triangles = mesh.triangles;
            copyMesh.normals = mesh.normals;
            copyMesh.uv = mesh.uv;
            copyMesh.name = "Copy" + originMesh.name;

            // 3. Mesh에 복사본 적용
            meshFilter.sharedMesh = copyMesh;
            mesh = meshFilter.sharedMesh;

            // Mesh의 Vertice및 Triangle설정 함수
            SetMesh();
        }

        // "Reset" 버튼 클릭시 Unity에서 기본으로 제공하는 Plane Mesh를 적용
        if (GUILayout.Button("Reset"))
        {
            meshFilter.sharedMesh = originMesh;
        }

        // SerializedProperty 변경사항 적용
        serializedObject.ApplyModifiedProperties();
    }       // OnInspectorGUI()

    private void SetMesh()
    {
        // 분할하게 될 두점 사이의 거리를 저장
        float lerpLength = Vector3.Distance(mesh.vertices[0], Vector3.Lerp(mesh.vertices[0], mesh.vertices[1], 1f / verticesValue.intValue));

        // 초기의 Vertice 총 개수를 저장
        int originVerticesCount = mesh.vertices.Length;

        // 제곱근을 저장
        /*
         * Unity가 기본적으로 제공하는 Plane의 Vertice 총 개수는 11 * 11 = 121 이므로 sqrt 에는 11이 저장된다.
         */
        int sqrt = Calculate(originVerticesCount);

        // 새로 변화될 vertice 개수 설정
        /*
         * verticesValue에 2가 들어갔을 경우 sqrt(11) * 2 - 1 이므로 setValue 에는 21값이 저장된다.
         */
        int setValue = sqrt * verticesValue.intValue - 1;

        // 새로 변화될 총 Vertice 갯수 만큼의 배열을 생성한다. 
        Vector3[] setVertices = new Vector3[setValue * setValue];

        // 분할하게 될 두점 사이의 거리를 이용하여 새로 변화될 Vertice의 위치 생성
        for (int y = 0; y < setValue; y++)
        {
            for (int x = 0; x < setValue; x++)
            {
                setVertices[y * setValue + x] = mesh.vertices[0] + new Vector3(-lerpLength * x, 0.0f, -lerpLength * y);
            }
        }

        // 새로 변화된 Vertice로 mesh 갱신
        mesh.vertices = setVertices;

        // 새로 변화될 Mesh의 Triangle 총 개수 저장
        /*
         * 폴리곤이 사각형으로 이루어졌경우 * 1 이 되지만 삼각형으로 이루어져 있기 때문에 * 2
         */
        int SetTrianglesCount = (setValue - 1) * (setValue - 1) * 2;

        // 각 Triangle 은 3개의 Vertice 로 이루어져 있기 때문에 각 Triangle에 해당하는 Vertice를 저장할 배열 
        int[] triangles = new int[SetTrianglesCount * 3];

        // Vertice들을  이용하여 만들어지는 각 Triangle의 Vertice 인덱스를 저장하기 위한 변수를 초기화
        int triangleIndex = 0;

        // Vertice 인덱스를 사용하여 각 삼각형을 구성
        for (int y = 0; y < setValue - 1; y++)
        {
            for (int x = 0; x < setValue - 1; x++)
            {
                // { Vertice 정점 Position 설정
                int topLeft = y * setValue + x;
                int topRight = topLeft + 1;
                int bottomLeft = (y + 1) * setValue + x;
                int bottomRight = bottomLeft + 1;
                // } Vertice 정점 Position 설정

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
        mesh.triangles = triangles;

        // 메쉬의 경계와 법선을 재계산
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }       // SetMesh()

    //! _num의 제곱근을 찾는 메서드
    private int Calculate(int _num)
    {
        int sqrt = 1;

        while (sqrt * sqrt < _num)
        {
            sqrt += 1;
        }

        return sqrt;
    }       // Calculate()
}
