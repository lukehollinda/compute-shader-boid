using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    private const int threadGroupSize = 1024;
    
    public ComputeShader computeShader;

    public BoidSettings settings;
    
    private Boid[] boids;

    private int computeKernalID;
    
    private BoidData[] boidData;
    
    
    void Start()
    {
        //Find and initialize all Boids
        boids = FindObjectsOfType<Boid> ();
        foreach (Boid boid in boids)
        {
            
            boid.Initialize(settings);
        }

        computeKernalID = computeShader.FindKernel("BoidComputation");
        boidData = new BoidData[settings.boidSpawnCount];
    }

    void Update()
    {
        
        // Initialize Buffer Input Data
        int numBoids = boids.Length;
        boidData = new BoidData[numBoids];

        for (int i = 0; i < numBoids; ++i)
        {
            boidData[i].position = boids[i].position;
            boidData[i].direction = boids[i].forward;
        }
        
        
        
        // Set up and dispatch compute shader
        var computeBuffer = new ComputeBuffer(numBoids, BoidData.SizeInBytes());
        computeBuffer.SetData(boidData);
        
        computeShader.SetBuffer(computeKernalID, "boids", computeBuffer);
        computeShader.SetInt("numBoids", numBoids);
        computeShader.SetFloat("viewRadius", settings.perceptionRadius);
        computeShader.SetFloat("avoidRadius", settings.avoidanceRadius);

        int threadGroups = Mathf.CeilToInt(numBoids / (float)threadGroupSize);
        
        computeShader.Dispatch(computeKernalID, threadGroups, 1, 1);
        computeBuffer.GetData(boidData);

        //Update boid data
        for(int i = 0; i < numBoids; ++i)
        {
            boids[i].avgFlockHeading = boidData[i].flockHeading;
            boids[i].centreOfFlockmates = boidData[i].flockCentre;
            boids[i].avgAvoidanceHeading = boidData[i].separationHeading;

            boids[i].flockmatesInRange = boidData[i].flockmatesInRange;
            
            boids[i].UpdateBoid();
            
        }
        
        computeBuffer.Release();
        
    }
}


//Structure passed to compute shader
public struct BoidData
{
    //Input
    public Vector3 position;
    public Vector3 direction;
    
    //Ouput

    public Vector3 flockHeading;
    public Vector3 flockCentre;
    public Vector3 separationHeading;

    public int flockmatesInRange;
    public static int SizeInBytes()
    {
        return sizeof(float) * 3 * 5 + sizeof(int);
    }
    

}
