using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class GrassModel : MonoBehaviour
{
    private int health;
    private int lifeLength;
    private Action<GrassModel> releaseGrassFn;  // c# reflection, generic type is parameter type
    private DateTime activeStartTime;
    private SpriteRenderer mySpriteRenderer;


    // for pool get
    public void Init(Vector3 spawnPosition)
    {
        health = Random.Range(10, 20);
        lifeLength = Random.Range(60, 120); // averge lifeLength=90, with spawnSpeed=0.1, total instance=900
        gameObject.transform.position = spawnPosition;
        activeStartTime = DateTime.Now;
    }

    // for pool create
    public void Init(Vector3 spawnPosition, Action<GrassModel> rgfn)
    {
        Init(spawnPosition);
        releaseGrassFn = rgfn;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        if (DateTime.Now > activeStartTime.AddSeconds(lifeLength))
        {
            ReleaseGrass();
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Animal"))
        {
            Debug.Log("enter, health=" + health);
            InvokeRepeating(nameof(ShakeGrass), 0, 0.2f);

            health--;
            if (health == 0)
            {
                ReleaseGrass();
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        Debug.Log("exit");
        CancelInvoke(nameof(ShakeGrass));
    }

    private void ReleaseGrass()
    {
        CancelInvoke(nameof(ShakeGrass));
        releaseGrassFn?.Invoke(this);
    }

    private bool isFliped = false;
    private void ShakeGrass()
    {
        mySpriteRenderer.flipX = isFliped;
        isFliped = !isFliped;
    }

}
