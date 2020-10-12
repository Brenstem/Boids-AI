using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScale : MonoBehaviour
{
    void Start()
    {
        float scale = Random.Range(0.75f, 2f);

        transform.localScale = new Vector3(scale, scale, scale);
    }
}
