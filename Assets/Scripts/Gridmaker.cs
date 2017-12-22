using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MeshXtensions;

public class Gridmaker : MonoBehaviour
{

    const int SIZE = 100;

    void Start()
    {
        float[] heights = new float[SIZE * SIZE];

        for (int x = 0; x < SIZE; x++)
        {
            for (int y = 0; y < SIZE; y++)
            {
                heights[y * SIZE + x] = Mathf.PerlinNoise(x * 0.543f, y * 0.543f) * 1;
                heights[y * SIZE + x] += Mathf.PerlinNoise(x * 0.254f, y * 0.254f) * 2;
                heights[y * SIZE + x] += Mathf.PerlinNoise(x * 0.123f, y * 0.123f) * 4;
                heights[y * SIZE + x] += Mathf.PerlinNoise(x * 0.0332f, y * 0.0332f) * 10;
            }
        }

        Erode(ref heights);

        Grid g = Grid.Create(SIZE, SIZE, 1);
        g.AddHeights(heights);

        gameObject.InitMesh(g.ToMesh());
    }

    void Erode(ref float[] heights)
    {
        float[] temp = new float[SIZE];

        for (int x = 0; x < SIZE; x++)
        {
            for (int y = 0; y < SIZE; y++)
            {
                // Get slope vector
                float xSlope = HeightAt(heights, x - 1, y) - HeightAt(heights, x + 1, y);
                float ySlope = HeightAt(heights, x, y - 1) - HeightAt(heights, x, y + 1);
                Vector2 slope = new Vector2(xSlope, ySlope);

                Debug.DrawRay(new Vector3(x, heights[y * SIZE + x], y), new Vector3(slope.x, 0, slope.y), Color.red, 5);
            }
        }
    }

    float HeightAt(float[] heights, int x, int y)
    {
        if (x < 0) x = 0;
        if (x >= SIZE) x = SIZE - 1;
        if (y < 0) y = 0;
        if (y >= SIZE) y = SIZE - 1;

        return heights[y * SIZE + x];
    }
}
