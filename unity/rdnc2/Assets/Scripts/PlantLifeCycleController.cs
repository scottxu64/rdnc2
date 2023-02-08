using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlantLifeCycleController : MonoBehaviour
{
    public List<GameObject> prefabs;
    public float minLifeLengthInSeconds = 3;
    public float maxLifeLengthInSeconds = 5;

    private int currentIndex;
    private GameObject instance;
    private DateTime nextInstanceStartTime;

    private void FixedUpdate()
    {
        if (nextInstanceStartTime == DateTime.MinValue)     // first one
        {
            currentIndex = 0;
            instance = Instantiate(prefabs[currentIndex], transform.position, transform.rotation);
            nextInstanceStartTime = DateTime.Now.AddSeconds(Random.Range(minLifeLengthInSeconds, maxLifeLengthInSeconds));
        }
        else if (DateTime.Now > nextInstanceStartTime)
        {
            if (currentIndex + 1 < prefabs.Count)           // middle ones
            {
                Destroy(instance);

                currentIndex++;
                instance = Instantiate(prefabs[currentIndex], transform.position, transform.rotation);
                nextInstanceStartTime = DateTime.Now.AddSeconds(Random.Range(minLifeLengthInSeconds, maxLifeLengthInSeconds));
            }
            else                                            // last one
            {
                Destroy(gameObject);
            }
        }
    }
}