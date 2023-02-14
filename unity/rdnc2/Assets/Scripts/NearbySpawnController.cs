using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearbySpawnController : MonoBehaviour
{
    public List<GameObject> prefabs;
    public int minInstancesCount = 10;
    public int maxInstancesCount = 15;
    public int minDistanceBetweenInstances = 1;
    public int maxDistanceBetweenInstances = 10;

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

        var position = Random.insideUnitCircle * maxDistanceBetweenInstances;
        if (position.magnitude >= minDistanceBetweenInstances)
        {
            return new Vector3(position.x, 0, position.y);
        }
        return GetSpawnPosition();
    }


}
