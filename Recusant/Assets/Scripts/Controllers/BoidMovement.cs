using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoidMovement : MonoBehaviour
{
    public float separationRadius = 2f;
    public float alignmentRadius = 5f;
    public float cohesionRadius = 5f;
    public float maxSpeed = 5f;
    public float maxForce = 1f;
    public float boundaryRadius = 10f;
    public int maxNeighbors = 5; // Number of neighbors to consider
    public float randomChangeInterval = 5f; 
    private float timeSinceLastRandomChange = 0f; 
    public float randomChangeDuration = 1f; 
    private float randomChangeCooldown = 0f;
    public bool goodToMove;
    private Rigidbody2D rb;

    private Vector2 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Initialize with a random velocity
        velocity = Random.insideUnitCircle.normalized * maxSpeed;
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (goodToMove)
        {
            BoidMove();
        }
    }

    public void BoidMove()
    {
        if (gameObject.activeSelf)
        {
            Vector2 separation = Separate();
            Vector2 alignment = Align();
            Vector2 cohesion = Cohere();
            Vector2 boundaryAvoidance = AvoidBoundary();

            // Combine the forces with weights (adjust these weights as needed)
            Vector2 combinedForce = separation * 1.5f + alignment + cohesion * 2f + boundaryAvoidance;
            combinedForce = Vector2.ClampMagnitude(combinedForce, maxForce);

            // Adjust the boid's velocity based on the combined force
            velocity += combinedForce * Time.deltaTime;
            velocity = Vector2.ClampMagnitude(velocity, maxSpeed);

            // Move the boid
            //transform.Translate(velocity * Time.deltaTime);
            rb.AddForce(combinedForce);
            

            // Randomly change direction occasionally
            timeSinceLastRandomChange += Time.deltaTime;
            if (timeSinceLastRandomChange >= randomChangeInterval && randomChangeCooldown <= 0f)
            {
                ApplyRandomChange();
                timeSinceLastRandomChange = 0f;
                randomChangeCooldown = randomChangeDuration;
            }

            // Update random change cooldown
            randomChangeCooldown = Mathf.Max(0f, randomChangeCooldown - Time.deltaTime);
        }
    }

    Vector2 Separate()
    {
        Vector2 separationForce = Vector2.zero;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, separationRadius);

        int neighborCount = 0;

        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider.gameObject != gameObject)
            {
                BoidMovement otherBoid = collider.GetComponent<BoidMovement>();
                if (otherBoid != null && neighborCount < maxNeighbors)
                {
                    Vector2 avoidVector = (transform.position - collider.transform.position).normalized;
                    separationForce += avoidVector / avoidVector.magnitude;
                    neighborCount++;
                }
            }
        }

        return separationForce;
    }

    Vector2 Align()
    {
        Vector2 averageDirection = Vector2.zero;
        int count = 0;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, alignmentRadius);

        int neighborCount = 0;

        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider.gameObject != gameObject)
            {
                BoidMovement otherBoid = collider.GetComponent<BoidMovement>();
                if (otherBoid != null && neighborCount < maxNeighbors)
                {
                    averageDirection += (Vector2)collider.transform.up;
                    count++;
                    neighborCount++;
                }
            }
        }

        if (count > 0)
            averageDirection /= count;

        return averageDirection.normalized;
    }

    Vector2 Cohere()
    {
        Vector2 averagePosition = Vector2.zero;
        int count = 0;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cohesionRadius);

        int neighborCount = 0;

        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider.gameObject != gameObject)
            {
                BoidMovement otherBoid = collider.GetComponent<BoidMovement>();
                if (otherBoid != null && neighborCount < maxNeighbors)
                {
                    averagePosition += (Vector2)collider.transform.position;
                    count++;
                    neighborCount++;
                }
            }
        }

        if (count > 0)
        {
            averagePosition /= count;
            Vector2 cohesionVector = (averagePosition - (Vector2)transform.position).normalized;
            return cohesionVector;
        }

        return Vector2.zero;
    }

    Vector2 AvoidBoundary()
    {
        Vector2 centerOffset = Vector2.zero - (Vector2)transform.position;
        float distanceToCenter = centerOffset.magnitude;

        if (distanceToCenter > boundaryRadius)
        {
            // If outside the boundary, steer back towards the center
            return centerOffset.normalized;
        }

        return Vector2.zero;
    }
    
    void ApplyRandomChange()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector2 randomForce = randomDirection * maxForce * 0.5f; // Adjust the force magnitude as needed

        // Apply the random force to the boid's velocity
        velocity += randomForce;
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
    }
}
