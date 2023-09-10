using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[DisallowMultipleComponent]
public class SizeCriterion : MonoBehaviour
{
    public int size = 0;
    public float meshVolume = 0;
    // Volume includes scale on Object.
    public float Volume = 0;

    void Start()
    {

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null) return;

        // メッシュの体積を計算する
        Mesh mesh = meshFilter.sharedMesh;
        meshVolume = CalculateMeshVolume(mesh);

        Volume = CalculateVolume(meshVolume);

        // size 0 ~ 1 ~ 14  ~ 400 ~ 1000

        if(Volume < 1)
        {
            size = 1;
        }
        else if(Volume < 14)
        {
            size = 2;
        }
        else if(Volume < 400)
        {
            size = 3;
        }
        else if(Volume < 2500)
        {
            size = 4;
        }
        else if(Volume >= 2500)
        {
            size = 5;
        } 
    }

    float CalculateVolume(float meshVolume)
    {

        // メッシュの体積に現在のオブジェクトのスケールを掛け合わせる
        Vector3 scale = transform.lossyScale;
        return meshVolume * scale.x * scale.y * scale.z;

        // 丸めて表示する
        //Debug.Log(System.Math.Round(volume, 3));
    }

    float CalculateMeshVolume(Mesh mesh)
    {
        if (mesh == null) return 0;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        float volume = 0;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += Vector3.Dot(p1, Vector3.Cross(p2, p3)) / 6.0f;
        }
        return Mathf.Abs(volume);
    }

    private void Reset()
    {
        Start();
    }
}
