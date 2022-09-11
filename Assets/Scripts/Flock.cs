using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Flock : MonoBehaviour
{

    [Range(0, 5)]
    public float seperationBias = 1.0f;
    [Range(0, 5)]
    public float alignmentBias = 1.0f;
    [Range(0, 5)]
    public float cohesionBias = 1.0f;

    public int totalBoids;

    public float maxForce;
    public float maxSpeed;
    public float perceptionRadius;
    public float maxTravelDistance;

    public GameObject boid;

    List<GameObject> boids = new();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < totalBoids; i++)
        {
            boids.Add(Instantiate(boid, transform));
        }
    }

    // FixedUpdate is called once every 20ms
    void Update()
    {
        for (int i = 0; i < boids.Count; i++)
        {
            Boid _boid = boids[i].GetComponent<Boid>();

            _boid.seperationBias = seperationBias;
            _boid.alignmentBias = alignmentBias;
            _boid.cohesionBias = cohesionBias;

            _boid.maxForce = maxForce;
            _boid.maxSpeed = maxSpeed;
            _boid.perceptionRadius = perceptionRadius;
            _boid.maxTravelDistance = maxTravelDistance;

            _boid.Flocking(boids);
        }
    }

}
