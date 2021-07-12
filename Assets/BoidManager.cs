using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{

    public BoidSettings settings;
    
    Boid[] boids;
    
    
    void Start()
    {
        //Find and initialize all Boids
        boids = FindObjectsOfType<Boid> ();
        foreach (Boid boid in boids)
        {
            boid.Initialize(settings);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Boid boid in boids)
        {
            boid.UpdateBoid();
        }
    }
}
