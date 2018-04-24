using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class SphereMaker : MonoBehaviour
{
    public Vector3 size = Vector3.one;
    MeshCreator mc = new MeshCreator();

    public int subdivisions = 0;
    public float radius = 5f;

    void Update()
    {
        // float height = 1 + 0.9f * Perlin.Noise(col * size.x * (float)1.2, row * size.z * (float)1.2, Random.Range(0.05f, 0.5f));
        float vertexChange = 1 + 0.9f * Perlin.Noise((float)1.2, radius * (float)1.2, Random.Range(1.0f, 2.5f));
        // Mesh sphereMesh = GetComponent<MeshFilter>().mesh = SphereCreator.Create(subdivisions, radius);
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = SphereCreator.Create(subdivisions, radius, vertexChange); ;


        mc.Clear(); // Clear internal lists and mesh
    }
}
