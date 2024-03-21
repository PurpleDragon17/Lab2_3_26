using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapGen : MonoBehaviour
{
    //Code is based on Noise map generation in example scene 6
    [System.Serializable]
    public class Wave
    {
        public float seed;
        public float frequency;
        public float amplitude;
    }

    public float[,] noiseMap;
    public float[,] GenerateNoiseMap(int mapDepth, int mapWidth, float scale, float offsetX, float offsetZ, Wave[] waves)

    {
        float[,] noiseMap = new float[mapDepth, mapWidth];
        //This is the empty noise map that the following code will fill

        for (int zIndex = 0; zIndex < mapDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < mapWidth; xIndex++)
            {
                //Ajust map by scale 
                float sampleX = (xIndex + offsetX) / scale;
                float sampleZ = (zIndex + offsetZ) / scale;

                float noise = 0f;
                float normalization = 0f;
                //This is the Perlin Noise Equasion 
                foreach (Wave wave in waves)
                {
                    noise += wave.amplitude * Mathf.PerlinNoise(sampleX * wave.frequency + wave.seed, sampleZ * wave.frequency + wave.seed);
                    normalization += wave.amplitude;
                }
                noise /= normalization;
                noiseMap[zIndex, xIndex] = noise;
            }
        }
        return noiseMap;
    }
}
