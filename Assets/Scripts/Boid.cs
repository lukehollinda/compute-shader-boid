using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Boid : MonoBehaviour
{

    public Vector3 position;
    public Vector3 forward;
    public Vector3 velocity;

    // To update:
    public Vector3 acceleration;
    public Vector3 avgFlockHeading;
    public Vector3 avgAvoidanceHeading;
    public Vector3 centreOfFlockmates;
    public int flockmatesInRange;

    // Cached
    public Transform cachedTransform;
    private BoidSettings settings;


    private void Awake()
    {
        cachedTransform = transform;
    }

    public void Initialize(BoidSettings settings)
    {
        this.settings = settings;

        position = cachedTransform.position;
        forward = cachedTransform.forward;

        float startSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
        velocity = forward * startSpeed;
    }

    bool IsHeadingForCollision () 
    {
        RaycastHit hit;
        if (Physics.SphereCast (position, settings.boundsRadius, forward, out hit, settings.collisionAvoidDst, settings.obstacleMask)) 
        {
            return true;
        } 
        return false;
    }
    
    //Returns the direction of a ray that does not collide with the environment, else forward
    Vector3 ObstacleRays () 
    {
        Vector3[] rayDirections = ObstacleRayTracingHelper.directions;

        for (int i = 0; i < rayDirections.Length; i++) 
        {
            Vector3 dir = cachedTransform.TransformDirection (rayDirections[i]);
            Ray ray = new Ray (position, dir);
            if (!Physics.SphereCast (ray, settings.boundsRadius, settings.collisionAvoidDst, settings.obstacleMask)) 
            {
                return dir;
            }
        }
        
        return forward;
    }

    public void UpdateBoid()
    {
        
        acceleration = Vector3.zero;
        
        if (flockmatesInRange != 0)
        {
            UpdateAccelerationWithAlignment();
            UpdateAccelerationWithFlockAvoidance();
            UpdateAccelerationWithCohesion();
        }
        
        if (IsHeadingForCollision ()) 
        {
            UpdateAccelerationWithCollisionAvoidance();
        }
        
        UpdateVelocity();
        UpdatePosition();
    }

    void PrintComputedValues()
    {
        Debug.Log("Acceleration: " + acceleration);
        Debug.Log("FlockHeading: " + avgFlockHeading);
        Debug.Log("AvoidHeading: " + avgAvoidanceHeading);
        Debug.Log("FlockCenter: " + centreOfFlockmates);
        
    }
    
    
    Vector3 SteerTowards (Vector3 vector) {
        Vector3 v = vector.normalized * settings.maxSpeed - velocity;
        return Vector3.ClampMagnitude (v, settings.maxSteerForce);
    }

    void UpdateAccelerationWithCollisionAvoidance()
    {
        Vector3 collisionAvoidDir = ObstacleRays ();
        Vector3 collisionAvoidForce = SteerTowards (collisionAvoidDir) * settings.avoidCollisionWeight;
        acceleration += collisionAvoidForce;
    }
    
    void UpdateAccelerationWithAlignment()
    {
        acceleration += SteerTowards(avgFlockHeading) * settings.alignWeight;
    }
    
    void UpdateAccelerationWithFlockAvoidance()
    {
        acceleration += SteerTowards(avgAvoidanceHeading) * settings.seperateWeight;
    }
    
    void UpdateAccelerationWithCohesion()
    {
        Vector3 force = SteerTowards((centreOfFlockmates / flockmatesInRange) - position)  * settings.cohesionWeight;
        acceleration += force;
    }
    
    void UpdateVelocity()
    {
        velocity += acceleration * Time.deltaTime;
        float speed = velocity.magnitude;
        Vector3 dir = velocity / speed;
        speed = Mathf.Clamp (speed, settings.minSpeed, settings.maxSpeed);
        velocity = dir * speed;
    }

    void UpdatePosition()
    {
        cachedTransform.position += velocity * Time.deltaTime;
        cachedTransform.forward = velocity.normalized;
        position = cachedTransform.position;
        forward = velocity.normalized;
    }

}
