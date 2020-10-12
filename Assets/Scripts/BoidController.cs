using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{
    private float perceptionRadius;
    private Collider[] _boids;
    private Rigidbody _rb;
    private float _speed;
    private bool newCheckPoint = true;
    private LayerMask boidLayerMask;

    private void Start()
    {
        perceptionRadius = FlockVars.Variables.PerceptionRadius;
        _rb = GetComponent<Rigidbody>();
        _speed = Random.Range(FlockVars.Variables.MinInitSpeed, FlockVars.Variables.MaxInitSpeed);

        if (!FlockVars.Variables.GenerateCheckPoint)
            _rb.AddForce(new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 0)) * _speed); // Random initial vector

        boidLayerMask = ~(1 << this.gameObject.layer);
    }

    private void Update()
    {
        _boids = Physics.OverlapSphere(this.transform.position, perceptionRadius);

        _rb.velocity = (_rb.velocity + Move()).normalized * _speed;
        Debug.DrawLine(this.transform.position, this.transform.position + _rb.velocity, Color.red);
    }

    private Vector3 Move()
    {
        Vector3 movementVec;

        movementVec = Vector3.zero;

        movementVec += Alignment() * FlockVars.Variables.AlignWeight;
        movementVec += Cohesion() * FlockVars.Variables.CohesionWeight;
        movementVec += Separation() * FlockVars.Variables.SeparationWeight;
        movementVec += ObjAvoidance() * FlockVars.Variables.ObjAvoidanceWeight;
        movementVec += GroundAvoidance() * FlockVars.Variables.GroundAvoidanceWeight;
        movementVec += Wandering() * FlockVars.Variables.WanderingWeight;

        return movementVec.normalized;
    }

    private Vector3 Alignment()
    {
        Vector3 steering = Vector3.zero;

        foreach (Collider other in _boids)
        {
            if (other.CompareTag("Boid") && this.name != other.name)
            {
                steering += other.GetComponent<Rigidbody>().velocity;
            }
        }

        if (steering != Vector3.zero)
            steering -= _rb.velocity;

        steering /= _boids.Length;
        return steering;
    }

    private Vector3 Cohesion()
    {
        Vector3 steering = Vector3.zero;

        foreach (Collider other in _boids)
        {
            if (other.CompareTag("Boid") && this.name != other.name)
            {   
                steering += other.transform.position;   
            }
        }

        if (steering != Vector3.zero)
            steering -= this.transform.position;

        steering /= _boids.Length;
        return steering;
    }

    private Vector3 Separation()
    {
        Vector3 steering = Vector3.zero;

        foreach (Collider other in _boids)
        {
            if (this.name != other.name)
            {
                Vector3 diff = this.transform.position - other.transform.position;
                diff /= diff.magnitude;

                steering += diff;
            }    
        }

        steering /= _boids.Length;

        return steering;
    }

    private Vector3 ObjAvoidance()
    {
        Vector3 steering = Vector3.zero;
        GameObject closest = null;

        foreach (Collider other in _boids)
        {
            if (other.CompareTag(FlockVars.Variables.ObjAvoidanceTag))
            {
                if (closest == null)
                {
                    closest = other.gameObject;
                }
                else if (Vector3.Distance(this.transform.position, other.transform.position) < Vector3.Distance(this.transform.position, closest.transform.position))
                {
                    closest = other.gameObject;
                }
            }
        }

        if (closest != null)
        {
            Vector3 rayDirection = new Vector3(closest.transform.position.x, this.transform.position.y, closest.transform.position.z);
            RaycastHit hit;
            Physics.Linecast(this.transform.position, rayDirection, out hit, boidLayerMask);

            if (hit.collider != null)
            {
                steering = Vector3.Reflect(_rb.velocity, hit.normal);
                steering /= steering.magnitude;
                // Debug.DrawLine(this.transform.position, hit.point, Color.blue);
            }
        }

        // Debug.DrawRay(this.transform.position, steering, Color.red);

        return steering;
    }

    private Vector3 Wandering()
    {
        if (newCheckPoint)
        {
            if (FlockVars.Variables.GenerateCheckPoint)
            {
                FlockVars.Variables.FlockCheckPoint = new Vector3(Random.Range(-400f, 400f), Random.Range(1f, 20f), Random.Range(-400f, 400f));
                newCheckPoint = false;
            }
        }

        return Seek(FlockVars.Variables.FlockCheckPoint, out newCheckPoint);
    }

    private Vector3 Seek(Vector3 target, out bool hitCheckPoint)
    {
        Vector3 steering = Vector3.zero;
        hitCheckPoint = false;

        steering = (target - transform.position).normalized;

        foreach (Collider col in Physics.OverlapSphere(target, 10f))
        {
            if (this.gameObject == col.gameObject)
            {
                hitCheckPoint = true;
            }
            else 
            {
                hitCheckPoint = false;
            }
        }

        return steering;
    }

    private Vector3 GroundAvoidance()
    {
        Vector3 steering = Vector3.zero;
        RaycastHit col;

        Ray ray = new Ray(this.transform.position, -Vector3.up);
        Physics.Raycast(ray, out col, FlockVars.Variables.PerceptionRadius, FlockVars.Variables.GroundMask);

        if (!object.ReferenceEquals(col.collider, null))
        {
            steering = this.transform.position - col.point;
            steering /= steering.magnitude;
        }

        return steering;
    }
}
