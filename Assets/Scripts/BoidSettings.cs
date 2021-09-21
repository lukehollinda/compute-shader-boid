using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class BoidSettings : ScriptableObject
{

    public int boidSpawnCount = 65;
    
    // Boid Settings
    [Header ("Boid Settings")]
    public float minSpeed = 0.5f;
    public float maxSpeed = 1;
    public float perceptionRadius = 100;
    public float avoidanceRadius = 1;
    
    //public float maxSteerForce = 3;
    public float maxSteerForce = 30;

    public float alignWeight = 0;
    public float cohesionWeight = 0;
    public float seperateWeight = 0;
    
    
    [Header ("Collisions")]
    public LayerMask obstacleMask;
    public float boundsRadius = .27f;
    public float avoidCollisionWeight = 10;
    public float collisionAvoidDst = 0.1f;
    
}
