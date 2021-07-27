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

    // Update is called once per frame
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
        computeShader.SetBool("flockmatesInRange", false);
        computeShader.SetInt("numBoids", numBoids);
        computeShader.SetFloat("viewRadius", settings.perceptionRadius);
        computeShader.SetFloat("avoidRadius", settings.avoidanceRadius);

        int threadGroups = Mathf.CeilToInt(numBoids / (float)threadGroupSize);
        
        computeShader.Dispatch(computeKernalID, threadGroups, 1, 1);
        computeBuffer.GetData(boidData);

        //Update boid data
        for(int i = 0; i < numBoids; ++i)
        {
            boids[i].avgFlockHeading = boidData[i].averageFlockHeading;
            boids[i].centreOfFlockmates = boidData[i].cohesionDirection;
            boids[i].avgAvoidanceHeading = boidData[i].collisionAvoidanceDirection;

            boids[i].flockmatesInRange = boidData[i].flockmatesInRange;

            Debug.Log("FlockHeading: " + boidData[i].averageFlockHeading);
            Debug.Log("AvoidHeading: " + boidData[i].collisionAvoidanceDirection);
            Debug.Log("FlockCenter: " + boidData[i].cohesionDirection);
            Debug.Log("Flock in range?: " +  (boidData[i].flockmatesInRange != 0));

            boids[i].UpdateBoid();
            
        }
    }
}

public struct BoidData
{
    //Input
    public Vector3 position;
    public Vector3 direction;
    
    //Ouput

    public Vector3 averageFlockHeading;
    public Vector3 collisionAvoidanceDirection;
    public Vector3 cohesionDirection;

    public int flockmatesInRange;
    public static int SizeInBytes()
    {
        return sizeof(float) * 3 * 5 + sizeof(int);
    }
    

}
