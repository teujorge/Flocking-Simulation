using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{

    Vector3 position;
    Vector3 velocity;
    public Vector3 acceleration;

    public float seperationBias;
    public float alignmentBias;
    public float cohesionBias;

    public float maxForce;
    public float maxSpeed;
    public float perceptionRadius;
    public float maxTravelDistance;

    //public List<GameObject> boids;

    // Start is called before the first frame update
    void Start()
    {

        seperationBias = 1.0f;
        alignmentBias = 1.0f;
        cohesionBias = 1.0f;

        position = new Vector3(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f) * 100.0f;
        velocity = new Vector3(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f) / 5.0f;

    }

    // FixedUpdate is called once every 20ms
    void Update()
    {

        position += velocity;

        velocity += acceleration;
        velocity = ClampMagnitude(velocity, maxSpeed / 2.0f, maxSpeed);

        transform.position = position;

        WrapBoidInCubeVolume();

    }

    public static Vector3 ClampMagnitude(Vector3 v, float min, float max)
    {
        double sm = v.sqrMagnitude;
        if (sm > (double)max * (double)max) return v.normalized * max;
        else if (sm < (double)min * (double)min) return v.normalized * min;
        return v;
    }

    void WrapBoidInCubeVolume()
    {

        if (transform.position.x > maxTravelDistance)
        {
            position.x = -maxTravelDistance;
        }
        else if (transform.position.x < -maxTravelDistance)
        {
            position.x = maxTravelDistance;
        }

        if (transform.position.y > maxTravelDistance)
        {
            position.y = -maxTravelDistance;
        }
        else if (transform.position.y < -maxTravelDistance)
        {
            position.y = maxTravelDistance;
        }

        if (transform.position.z > maxTravelDistance)
        {
            position.z = -maxTravelDistance;
        }
        else if (transform.position.z < -maxTravelDistance)
        {
            position.z = maxTravelDistance;
        }

    }

    // separation: steer to aviod crowding local flockmates
    // alignment: steer towards the average heading of local flockmates
    // cohesion: steer to move towards the average position of local flockmates
    public void Flocking(List<GameObject> boids)
    {
        int boidsInPerception = 0;
        Vector3 separation = new(0, 0, 0);
        Vector3 alignment = new(0, 0, 0);
        Vector3 cohesion = new(0, 0, 0);

        for (int i = 0; i < boids.Count; i++)
        {
            Boid _boid = boids[i].GetComponent<Boid>();
            Vector3 boidToMe = position - _boid.position;

            if (boids[i] != this && boidToMe.magnitude <= perceptionRadius)
            {

                if (boidToMe.magnitude > 0)
                {
                    separation += (boidToMe / boidToMe.magnitude);
                }
                else
                {
                    separation += (boidToMe / 0.01f);
                }

                alignment += _boid.velocity;
                cohesion += _boid.position;
                boidsInPerception++;
            }
        }

        if (boidsInPerception > 0)
        {
            separation /= boidsInPerception;
            //separation = separation.normalized * maxSpeed;
            separation -= velocity;

            alignment /= boidsInPerception;
            //alignment = alignment.normalized * maxSpeed;
            alignment -= velocity;

            cohesion /= boidsInPerception;
            cohesion -= position;
            //cohesion = cohesion.normalized * maxSpeed;
            cohesion -= velocity;
        }

        Vector3 moveTo = new(0, 0, 0);
        moveTo += separation * seperationBias;
        moveTo += alignment * alignmentBias;
        moveTo += cohesion * cohesionBias;
        acceleration = ClampMagnitude(moveTo, maxForce / 2.0f, maxForce);
    }

}
