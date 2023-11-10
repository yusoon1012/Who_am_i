using UnityEditor;
using UnityEngine;

// Unity���� �����ϴ� Editor ��ũ���� API�� ����Ͽ� MeshController��ũ��Ʈ�� ����� ���� �����ͷ� ����ڴٴ� ����
[CustomEditor(typeof(MeshController))]
public class MeshControllerEditor : Editor
{
    /*
     * SerializedProperty: Unity��ũ��Ʈ�� public�� [SerializeField]�� ������ �������� ����Ǵ� ����
     */

    // SerializedProperty�� �����Ͽ� �ν�����â�� ���̰� �� ����
    SerializedProperty verticesValue;

    // Unity���� �⺻���� �����ϴ� Mesh����
    private Mesh originMesh = default;

    // ���������� ������ Mesh�� ������ ����
    private Mesh mesh = default;

    //! ����Ƽ �����͸�带 ȣ���� �� �ѹ� ȣ��
    private void OnEnable()
    {
        // SerializedProperty �ʱ�ȭ
        verticesValue = serializedObject.FindProperty("verticesValue");

        // { Unity���� �⺻������ �����ϴ� Plane Mesh�� originMesh������ �Ҵ�
        GameObject planeObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
        originMesh = planeObject.GetComponent<MeshFilter>().sharedMesh;
        DestroyImmediate(planeObject);
        // } Unity���� �⺻������ �����ϴ� Plane Mesh�� originMesh������ �Ҵ�
    }       // OnEnable()

    //! ����Ƽ �����͸���϶� �ٷιٷ� ������ �� �ִ� �÷�����
    public override void OnInspectorGUI()
    {
        // SerializedProperty ������Ʈ
        serializedObject.Update();

        // target������Ʈ�� MeshController��ũ��Ʈ �� MeshFilter������Ʈ�� ������
        MeshController meshController = (MeshController)target;
        MeshFilter meshFilter = meshController.GetComponent<MeshFilter>();

        // �ν����Ϳ� verticesValue������ ���� ������ �ʵ�� ǥ��
        EditorGUILayout.PropertyField(verticesValue, new GUIContent("Vertices Value"));

        // "Apply" ��ư Ŭ���� Unity���� �⺻���� �����ϴ� Plane Mesh�� �����Ͽ� ����
        if (GUILayout.Button("Apply"))
        {
            // 1. Unity���� �⺻���� �����ϴ� Plane Mesh�� ����
            /*
             * �̸� ���� ��� ������ Mesh�� �� �����ϴ� ������ �Ͼ
             */
            meshFilter.sharedMesh = originMesh;

            // 2. Mesh ���纻 ����, ���� Mesh ������ ����
            mesh = meshFilter.sharedMesh;
            Mesh copyMesh = new Mesh();
            copyMesh.vertices = mesh.vertices;
            copyMesh.triangles = mesh.triangles;
            copyMesh.normals = mesh.normals;
            copyMesh.uv = mesh.uv;
            copyMesh.name = "Copy" + originMesh.name;

            // 3. Mesh�� ���纻 ����
            meshFilter.sharedMesh = copyMesh;
            mesh = meshFilter.sharedMesh;

            // Mesh�� Vertice�� Triangle���� �Լ�
            SetMesh();
        }

        // "Reset" ��ư Ŭ���� Unity���� �⺻���� �����ϴ� Plane Mesh�� ����
        if (GUILayout.Button("Reset"))
        {
            meshFilter.sharedMesh = originMesh;
        }

        // SerializedProperty ������� ����
        serializedObject.ApplyModifiedProperties();
    }       // OnInspectorGUI()

    private void SetMesh()
    {
        // �����ϰ� �� ���� ������ �Ÿ��� ����
        float lerpLength = Vector3.Distance(mesh.vertices[0], Vector3.Lerp(mesh.vertices[0], mesh.vertices[1], 1f / verticesValue.intValue));

        // �ʱ��� Vertice �� ������ ����
        int originVerticesCount = mesh.vertices.Length;

        // �������� ����
        /*
         * Unity�� �⺻������ �����ϴ� Plane�� Vertice �� ������ 11 * 11 = 121 �̹Ƿ� sqrt ���� 11�� ����ȴ�.
         */
        int sqrt = Calculate(originVerticesCount);

        // ���� ��ȭ�� vertice ���� ����
        /*
         * verticesValue�� 2�� ���� ��� sqrt(11) * 2 - 1 �̹Ƿ� setValue ���� 21���� ����ȴ�.
         */
        int setValue = sqrt * verticesValue.intValue - 1;

        // ���� ��ȭ�� �� Vertice ���� ��ŭ�� �迭�� �����Ѵ�. 
        Vector3[] setVertices = new Vector3[setValue * setValue];

        // �����ϰ� �� ���� ������ �Ÿ��� �̿��Ͽ� ���� ��ȭ�� Vertice�� ��ġ ����
        for (int y = 0; y < setValue; y++)
        {
            for (int x = 0; x < setValue; x++)
            {
                setVertices[y * setValue + x] = mesh.vertices[0] + new Vector3(-lerpLength * x, 0.0f, -lerpLength * y);
            }
        }

        // ���� ��ȭ�� Vertice�� mesh ����
        mesh.vertices = setVertices;

        // ���� ��ȭ�� Mesh�� Triangle �� ���� ����
        /*
         * �������� �簢������ �̷������� * 1 �� ������ �ﰢ������ �̷���� �ֱ� ������ * 2
         */
        int SetTrianglesCount = (setValue - 1) * (setValue - 1) * 2;

        // �� Triangle �� 3���� Vertice �� �̷���� �ֱ� ������ �� Triangle�� �ش��ϴ� Vertice�� ������ �迭 
        int[] triangles = new int[SetTrianglesCount * 3];

        // Vertice����  �̿��Ͽ� ��������� �� Triangle�� Vertice �ε����� �����ϱ� ���� ������ �ʱ�ȭ
        int triangleIndex = 0;

        // Vertice �ε����� ����Ͽ� �� �ﰢ���� ����
        for (int y = 0; y < setValue - 1; y++)
        {
            for (int x = 0; x < setValue - 1; x++)
            {
                // { Vertice ���� Position ����
                int topLeft = y * setValue + x;
                int topRight = topLeft + 1;
                int bottomLeft = (y + 1) * setValue + x;
                int bottomRight = bottomLeft + 1;
                // } Vertice ���� Position ����

                // ù ��° �ﰢ��
                triangles[triangleIndex] = topRight;
                triangles[triangleIndex + 1] = topLeft;
                triangles[triangleIndex + 2] = bottomRight;

                // �� ��° �ﰢ��
                triangles[triangleIndex + 3] = bottomLeft;
                triangles[triangleIndex + 4] = bottomRight;
                triangles[triangleIndex + 5] = topLeft;

                triangleIndex += 6; // ���� �ﰢ�� �ε����� �̵�
            }
        }

        // ���� ��ȭ�� Triangle�� mesh ����
        mesh.triangles = triangles;

        // �޽��� ���� ������ ����
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }       // SetMesh()

    //! _num�� �������� ã�� �޼���
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
