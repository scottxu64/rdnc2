using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearbySpawnController : MonoBehaviour
{
    public List<GameObject> prefabs;
    public int minInstancesCount = 8;
    public int maxInstancesCount = 12;
    public int minDistanceBetweenInstances = 1;
    public int maxDistanceBetweenInstances = 8;

    void Start()
    {
        var spawnCount = Random.Range(minInstancesCount, maxInstancesCount);

        var i = 0;
        while (i < spawnCount)
        {
            var position = GetSpawnPosition() + transform.position;



            Instantiate(prefabs[Random.Range(0, prefabs.Count)], position, Quaternion.identity);
            i++;
        }
    }

    private Vector3 GetSpawnPosition()
    {
        var x = Random.Range(-1f, 1f);
        var z = Random.Range(-1f, 1f);
        var position = Vector3.ClampMagnitude(new Vector3(x, 0, z), 1f) * maxDistanceBetweenInstances;
        if (position.magnitude >= minDistanceBetweenInstances)
        {
            return position;
        }
        return GetSpawnPosition();





    }
}
