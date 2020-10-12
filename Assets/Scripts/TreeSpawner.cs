using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private GameObject ground;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] float scale;

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        Vector3 spawnPosition;
        Vector3 spawnRotation;

        for (int x = (int)-ground.transform.localScale.x * 10 / 2; x < (int)ground.transform.localScale.x * 10 / 2; x++)
        {

            for (int z = (int)-ground.transform.localScale.z * 10 / 2; z < (int)ground.transform.localScale.z * 10 / 2; z++)
            {

                if (GenerateSpawnChance(x + (int)ground.transform.localScale.x / 2, z + (int)ground.transform.localScale.z / 2) < Random.Range(0, 1f))
                {
                    print("spawn");

                    spawnPosition = new Vector3(x, 0, z);
                    spawnRotation = new Vector3(0, Random.Range(0, 360f), 0);

                    Vector3 boxCastPosition = spawnPosition + new Vector3(0, treePrefab.GetComponent<CapsuleCollider>().bounds.size.y / 2);
                    Vector3 boxCastDim = treePrefab.GetComponent<CapsuleCollider>().bounds.size / 2;

                    bool colCheck = Physics.BoxCast(boxCastPosition, boxCastDim, Vector3.zero, Quaternion.identity, 0, groundLayer);

                    if (!colCheck)
                    {
                        Instantiate(treePrefab, spawnPosition, Quaternion.Euler(spawnRotation), this.transform);
                    }
                }
            }
        }
    }

    private float GenerateSpawnChance(int x, int y)
    {
        float xCoord = (float)x / 100 * scale;
        float yCoord = (float)y / 100 * scale;
        xCoord++;
        yCoord++;

        float spawnChance = Mathf.PerlinNoise(xCoord, yCoord);

        return spawnChance;
    }
}
