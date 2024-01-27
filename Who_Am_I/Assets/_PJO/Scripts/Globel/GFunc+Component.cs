using UnityEngine;

public static partial class GFunc
{
    public static Mesh CopyMesh(this Mesh mesh_)
    {
        Mesh copyMesh = new Mesh();

        copyMesh.vertices = mesh_.vertices;
        copyMesh.triangles = mesh_.triangles;
        copyMesh.normals = mesh_.normals;
        copyMesh.uv = mesh_.uv;

        if (mesh_.name.StartsWith("Copy") == false)
        {
            copyMesh.name = "Copy" + mesh_.name;
        }
        else { copyMesh.name = mesh_.name; }

        return copyMesh;
    }
}