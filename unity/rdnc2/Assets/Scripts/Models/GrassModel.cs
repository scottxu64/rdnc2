using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GrassModel : MonoBehaviour
{
    public GameObject explosionPrefab;

    private int health;
    private int lifeLength;
    private Action<GrassModel> releaseFn;  // c# way of passing methods, generic type is parameter type
    private DateTime activeStartTime;

    // for pool get
    public void Init(Vector3 spawnPosition)
    {
        health = Random.Range(10, 20);
        lifeLength = Random.Range(60, 120); // averge lifeLength=90, with spawnSpeed=0.1, total instance=900
        gameObject.transform.position = spawnPosition;
        activeStartTime = DateTime.Now;
    }

    // for pool create
    public void Init(Vector3 spawnPosition, Action<GrassModel> rfn)
    {
        Init(spawnPosition);
        releaseFn = rfn;
    }



    private void Update()
    {
        if (DateTime.Now > activeStartTime.AddSeconds(lifeLength))
        {
            DestroyGrass();
        }
    }


  



    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Animal"))
        {
            Debug.Log("enter, health="+health);
            InvokeRepeating(nameof(ShakeGrass), 0, 0.2f);
           

            health--;
            if (health == 0)
            {
                DestroyGrass();
            }
        }
    }



    private void OnTriggerExit(Collider collision)
    {
        Debug.Log("exit");
        CancelInvoke(nameof(ShakeGrass));
    }

    private void DestroyGrass()
    {
        CancelInvoke(nameof(ShakeGrass));

        if (releaseFn != null)
        {
            var explosion = Instantiate(explosionPrefab);   // TODO: avoid instantiate destroy
            explosion.transform.position = gameObject.transform.position;
            Destroy(explosion, 5);

            releaseFn.Invoke(this);
        }
    }

    private bool isFliped = false;
    private void ShakeGrass()
    {
        var mySpriteRenderer = GetComponent<SpriteRenderer>();      // TODO: optimize it out
        mySpriteRenderer.flipX = isFliped;

        isFliped = !isFliped;

        
    }

}
