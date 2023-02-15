using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GrassModel : MonoBehaviour
{
    private int health;
    private int lifeLength;
    private Action<GrassModel> releaseFn;  // c# way of passing methods, generic type is parameter type

    public void Init(Vector3 spawnPosition)
    {
        health = Random.Range(10, 20);
        lifeLength = Random.Range(60, 120); // averge lifeLength=90, with spawnSpeed=0.1, total instance=900
        gameObject.transform.position = spawnPosition;
    }

    public void Init(Vector3 spawnPosition, Action<GrassModel> rfn)
    {
        Init(spawnPosition);
        releaseFn = rfn;
    }

    private void Start()
    {
        Invoke(nameof(ReleaseGrass), lifeLength);
    }

    private void ReleaseGrass()
    {
        releaseFn?.Invoke(this);    // TODO: there are always inital 11 instance won't have release fn passed in
    }




    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.transform.CompareTag("Building"))
    //    {
    //        releaseFn?.Invoke(this);
    //    }
    //    else if (collision.transform.CompareTag("Animal"))
    //    {
    //        health--;
    //        if (health == 0)
    //        {
    //            releaseFn?.Invoke(this);
    //        }
    //    }
    //}

}
