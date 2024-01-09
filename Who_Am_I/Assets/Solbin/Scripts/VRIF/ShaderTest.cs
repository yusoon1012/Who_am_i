using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderTest : MonoBehaviour
{
    private Material outline = default;
    private Renderer renderers = default;

    private List<Material> materialList = new List<Material>();

    private void Start()
    {
        outline = new Material(Shader.Find("Draw/OutlineShader"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y)) // 테스트 코드
        {
            TestOn();
        }    
        else if (Input.GetKeyDown(KeyCode.U))
        {
            TestOff();
        }
    }

    private void TestOn()
    {
        renderers = this.GetComponent<Renderer>();

        materialList.Clear();
        materialList.AddRange(renderers.sharedMaterials);
        materialList.Add(outline);

        renderers.materials = materialList.ToArray();
    }

    private void TestOff()
    {
        Renderer renderer = this.GetComponent<Renderer>();

        materialList.Clear();
        materialList.AddRange(renderer.sharedMaterials);
        materialList.Remove(outline);

        renderer.materials = materialList.ToArray();
    }
}
