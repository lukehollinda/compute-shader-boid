using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{

    public BoidSettings settings;
    public float spawnRadius = 10;
    public Boid prefab;
    
    void Awake()
    {
        
        for (int i = 0; i < settings.boidSpawnCount; i++)
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            Boid boid = Instantiate(prefab);
            boid.transform.position = pos;
            boid.transform.forward = Random.insideUnitSphere;

        }
    }
    
}
