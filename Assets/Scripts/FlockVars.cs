using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockVars : MonoBehaviour
{
    private static FlockVars _flockVars;
    public static FlockVars Variables
    {
        get
        {
            if (_flockVars == null)
            {
                _flockVars = GameObject.FindObjectOfType<FlockVars>();
            }
            return _flockVars;
        }
    }

    [SerializeField] private GameObject checkPoint;
    [SerializeField] [Range(0, 15f)] private float perceptionRadius;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] [Range(0, 1f)] private float alignWeight;
    [SerializeField] [Range(0, 1f)] private float cohesionWeight;
    [SerializeField] [Range(0, 1f)] private float separationWeight;
    [SerializeField] [Range(0, 1f)] private float objAvoidanceWeight;
    [SerializeField] [Range(0, 1f)] private float groundAvoidanceWeight;
    [SerializeField] [Range(0, 1f)] private float wanderingWeight;
    [SerializeField] private bool generateCheckPoint;
    [SerializeField] private string objAvoidanceTag;
    [SerializeField] private LayerMask groundMask;

    public float PerceptionRadius { get { return perceptionRadius; } }
    public float MinInitSpeed { get { return minSpeed; } }
    public float MaxInitSpeed { get { return maxSpeed; } }
    public float AlignWeight { get { return alignWeight; } }
    public float CohesionWeight { get { return cohesionWeight; } }
    public float SeparationWeight { get { return separationWeight; } }
    public float ObjAvoidanceWeight { get { return objAvoidanceWeight; } }
    public float WanderingWeight { get { return wanderingWeight; } }
    public string ObjAvoidanceTag { get { return objAvoidanceTag; } }
    public float GroundAvoidanceWeight { get { return groundAvoidanceWeight; } }
    public bool GenerateCheckPoint { get { return generateCheckPoint; } }
    private Vector3 checkPointPos;
    public Vector3 FlockCheckPoint
    {
        get
        {
            if (!generateCheckPoint)
                return checkPoint.transform.position;
            else
                return checkPointPos;
        }
        set
        {
            if (generateCheckPoint)
                checkPointPos = value;
        }
    }
    public LayerMask GroundMask { get { return groundMask; } }
}
