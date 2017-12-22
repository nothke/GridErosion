using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MeshXtensions;

public class Gridmaker : MonoBehaviour
{

    const int SIZE = 200;

    float[] heights;

    void Start()
    {
        heights = new float[SIZE * SIZE];

        for (int x = 0; x < SIZE; x++)
        {
            for (int y = 0; y < SIZE; y++)
            {
                //heights[y * SIZE + x] = Mathf.PerlinNoise(x * 0.543f, y * 0.543f) * 1;
                //heights[y * SIZE + x] += Mathf.PerlinNoise(x * 0.254f, y * 0.254f) * 2;
                heights[y * SIZE + x] += Mathf.PerlinNoise(x * 0.123f, y * 0.123f) * 4;
                heights[y * SIZE + x] += Mathf.PerlinNoise(x * 0.0332f, y * 0.0332f) * 20;
            }
        }

        Erode(ref heights);

        Grid g = Grid.Create(SIZE, SIZE, 1);
        g.AddHeights(heights);

        gameObject.InitMesh(g.ToMesh());
    }

    void RegenMesh()
    {
        Grid g = Grid.Create(SIZE, SIZE, 1);
        g.AddHeights(heights);
        gameObject.SetMesh(g.ToMesh());
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Erode(ref heights);
            RegenMesh();
        }
    }

    void Erode(ref float[] heights)
    {
        const int DROPLETS = 100;
        const int ENERGY = 3;


        for (int i = 0; i < DROPLETS; i++)
        {

            int x = Random.Range(0, SIZE);
            int y = Random.Range(0, SIZE);

            int energy = ENERGY;

            while (energy > 0)
            {

                energy--;
            }
        }
    }

    void AdjacentDepositionErode(ref float[] heights)
    {
        float[] temp = new float[SIZE * SIZE];

        for (int x = 0; x < SIZE; x++)
        {
            for (int y = 0; y < SIZE; y++)
            {
                // Get slope vector
                float xSlope = HeightAt(heights, x - 1, y) - HeightAt(heights, x + 1, y);
                float ySlope = HeightAt(heights, x, y - 1) - HeightAt(heights, x, y + 1);
                Vector2 slope = new Vector2(xSlope, ySlope);

                // remove some soil from this grid point and add to adjacent
                float amount = slope.magnitude * 0.01f;
                amount = Mathf.Clamp(amount, -0.1f, 0.1f);

                // find adjacent cell
                int adjX = Mathf.Clamp(Mathf.RoundToInt(slope.normalized.x), 0, SIZE - 1);
                int adjY = Mathf.Clamp(Mathf.RoundToInt(slope.normalized.y), 0, SIZE - 1);

                temp[y * SIZE + x] -= amount;
                temp[adjY * SIZE + adjX] += amount;

                //Debug.DrawRay(new Vector3(x, heights[y * SIZE + x], y), new Vector3(slope.x, 0, slope.y), Color.red, 5);
            }
        }

        // apply erosion
        for (int i = 0; i < SIZE * SIZE; i++)
        {
            heights[i] += temp[i];
        }
    }

    void SimpleErode(ref float[] heights)
    {
        float[] temp = new float[SIZE * SIZE];

        for (int x = 0; x < SIZE; x++)
        {
            for (int y = 0; y < SIZE; y++)
            {
                // Get slope vector
                float xSlope = HeightAt(heights, x - 1, y) - HeightAt(heights, x + 1, y);
                float ySlope = HeightAt(heights, x, y - 1) - HeightAt(heights, x, y + 1);
                Vector2 slope = new Vector2(xSlope, ySlope);

                // remove some soil according to steepness
                temp[y * SIZE + x] = -slope.magnitude * 0.1f;

                //Debug.DrawRay(new Vector3(x, heights[y * SIZE + x], y), new Vector3(slope.x, 0, slope.y), Color.red, 5);
            }
        }

        // apply erosion
        for (int i = 0; i < SIZE * SIZE; i++)
        {
            heights[i] += temp[i];
        }
    }

    Vector2 SlopeAt(int x, int y)
    {
        float xSlope = HeightAt(heights, x - 1, y) - HeightAt(heights, x + 1, y);
        float ySlope = HeightAt(heights, x, y - 1) - HeightAt(heights, x, y + 1);
        return new Vector2(xSlope, ySlope);
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
