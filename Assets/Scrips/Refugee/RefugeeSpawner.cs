using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefugeeSpawner : MonoBehaviour
{
    public Vector2 worldSize;
    public float gridSize;
    public List<Quad> quads;

    private void Start()
    {
        InitQuads();
    }

    private void InitQuads()
    {
        GameObject quadParent = new GameObject("Quads");
        quadParent.transform.position = new Vector3(0, 0, 0);

        float width = (worldSize.x / 10);
        float height = (worldSize.y / 10);

        float w = width;
        float h = height;

        for (int i = 0; i < 20; i++)
        {
            CreateQuad(w, h, quadParent);
            for (int x = 0; x < 19; x++)
            {
                w = w + width;
                CreateQuad(w, h, quadParent);
            }
            w = width;
            h = h + height;
        }
    }

    private void CreateQuad(float w, float h, GameObject parent)
    {
        GameObject q = new GameObject("Quad" + w + " " + h);
        q.transform.parent = parent.transform;
        q.transform.position = new Vector3(w, 0, h);
        Quad qw = q.AddComponent<Quad>();
        qw.quadPosition = new Vector2(w, h);
        quads.Add(q.GetComponent<Quad>());
    }
}
