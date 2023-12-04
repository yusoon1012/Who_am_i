using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ---------- 완성 ----------
 * 싱글턴을 이용한 맵의 정점배열을 저장
 * GetMapVertex.instance.vertices를 이용
 * ---------- 완성 ----------
 */
public class GetMapVertex : MonoBehaviour
{
    #region static
    public static GetMapVertex instance;
    #endregion

    #region [SerializeField]
    [SerializeField]
    [Header("맵 오브젝트")]
    private GameObject map = default;
    #endregion

    #region public || private
    // 맵의 정점을 저장할 배열
    public Vector3[] vertices = default;
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        // MeshFilter를 찾고 예외처리
        MeshFilter meshFilter = GetMeshFilter(map);
        if (meshFilter == null) { return; }

        // Mesh를 찾고 예외처리
        Mesh mesh = GetMesh(meshFilter);
        if (mesh == null) { return; }

        // 맵의 정점을 저장
        vertices = mesh.vertices;
    }

    //! MeshFilter를 찾고 예외처리
    private MeshFilter GetMeshFilter(GameObject obj_)
    {
        if (obj_.GetComponent<MeshFilter>() == null)
        {
            Debug.LogError("Object의 MeshFilter가 null입니다.");

            return null;
        }

        return obj_.GetComponent<MeshFilter>();
    }       // GetMeshFilter()

    //! Mesh를 찾고 예외처리
    private Mesh GetMesh(MeshFilter meshFilter_)
    {
        if (meshFilter_.sharedMesh == null)
        {
            Debug.LogError("Object의 Mesh가 null입니다.");

            return null;
        }

        return meshFilter_.sharedMesh;
    }
}       // GetMesh()
