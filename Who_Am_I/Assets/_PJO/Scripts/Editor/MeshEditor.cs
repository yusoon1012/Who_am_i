using UnityEditor;
using UnityEngine;

// ������ ��ũ��Ʈ�� �����ϰڴٴ� ��Ʈ����Ʈ
[CustomEditor(typeof(MeshController))]
public class MeshControllerEditor : Editor
{
    // �� ��������� ���ϴ� ����
    SerializedProperty verticesValue;

    // Unity���� �⺻���� �����ϴ� Mesh����
    private Mesh originMesh = default;

    // ���������� ������ Mesh�� ������ ����
    private Mesh mesh = default;

    private void OnEnable()
    {
        // verticesValue ������Ƽ �ʱ�ȭ
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
        // �ν����� â ������Ʈ
        serializedObject.Update();

        // MeshController��ũ��Ʈ�� �ִ� ������Ʈ�� MeshFilter������Ʈ�� ������
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

            // Mesh�� Vertice�� Triangle���� �޼���
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
        // �ʱ��� Vertice �� ������ ����
        int originVerticesCount = mesh.vertices.Length;

        // �������� ����
        /*
         * Unity�� �⺻������ �����ϴ� Plane�� Vertice �� ������ 11 * 11 = 121 �̹Ƿ� loot ���� 11�� ����ȴ�.
         */
        int loot = LootCalculate(originVerticesCount);

        // ���� ��ȭ�� vertice ���� ����
        /*
         * verticesValue�� 2�� ���� ��� loot(11) * 2 - 1 �̹Ƿ� setValue ���� 21���� ����ȴ�.
         */
        int setValue = (loot - 1) * verticesValue.intValue + 1;

        // ���� ��ȭ�� �� Vertice ���� ��ŭ�� �迭�� �����Ѵ�. 
        //Vector3[] setVertices = new Vector3[setValue * setValue];

        // �ʱ� �޽� ù��° ������ġ�� ������ ���� ��ġ�� ����
        Vector3 startPos = mesh.vertices[0];
        Vector3 endPos = mesh.vertices[originVerticesCount - 1];

        /*
         * 1���� �迭: �⺻���� �޽��� 1���� �迭�� �̷�����ֱ� ������
         * 2���� �迭: ���ϰ� ������ �����ϱ� ���� 
         */
        // 1���� �迭�� 2���� �迭�� ����
        Vector3[] oneSetVertices = new Vector3[setValue * setValue];
        Vector3[,] twoSetVertices = new Vector3[setValue, setValue];

        // twoSetVertices
        for (int i = 0; i < setValue; i++)
        {
            for (int j = 0; j < setValue; j++)
            {
                // startPos, endPos�� �̿��� ��ǥ ����
                twoSetVertices[j, i] =
                        new Vector3(Mathf.Lerp(startPos.x, endPos.x, (float)j / (float)(verticesValue.intValue * (loot - 1))),
                        0.0f,
                        Mathf.Lerp(startPos.z, endPos.z, (float)i / (float)(verticesValue.intValue * (loot - 1))));
            }
        }

        // 2���� �迭�� 1���� �迭�� �ٲٴ� �۾�
        for (int i = 0; i < setValue; i++)
        {
            for (int j = 0; j < setValue; j++)
            {
                oneSetVertices[i * setValue + j] = twoSetVertices[j, i];
            }
        }

        // ���� ��ȭ�� Vertice�� mesh ����
        mesh.vertices = oneSetVertices;

        // ���� ��ȭ�� Mesh�� Triangle �� ���� ����
        /*
         * �������� �簢������ �̷������� * 1 �� ������ �ﰢ������ �̷���� �ֱ� ������ * 2
         */
        int SetTrianglesCount = (setValue - 1) * (setValue - 1) * 2;

        // �� Triangle �� 3���� �������� �̷���� �ֱ� ������ �� Triangle�� �ش��ϴ� ������ ������ �迭 
        int[] triangles = new int[SetTrianglesCount * 3];

        // ��������  �̿��Ͽ� ��������� �� Triangle�� Vertice �ε����� �����ϱ� ���� ������ �ʱ�ȭ
        int triangleIndex = 0;

        // ���� �ε����� ����Ͽ� �� �ﰢ���� ����
        for (int y = 0; y < setValue - 1; y++)
        {
            for (int x = 0; x < setValue - 1; x++)
            {
                // { ���� Position ����
                int topLeft = y * setValue + x;
                int topRight = topLeft + 1;
                int bottomLeft = (y + 1) * setValue + x;
                int bottomRight = bottomLeft + 1;
                // } ���� Position ����

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
    private int LootCalculate(int _num)
    {
        int loot_ = 1;

        while (loot_ * loot_ < _num)
        {
            loot_ += 1;
        }

        return loot_;
    }       // Calculate()

    // Legacy:
    // �����ϰ� �� ���� ������ �Ÿ��� �̿��Ͽ� ���� ��ȭ�� Vertice�� ��ġ ����
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
