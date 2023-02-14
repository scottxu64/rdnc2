using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GrassModel : MonoBehaviour
{
    public int health = 10;
    public int lifeLength = 3;  // TODO: random range
    private float createTime;

    private Action<GrassModel> releaseFn;  // c# way of passing methods, generic type is parameter type

    public void Init()
    {
        health = 10;
        gameObject.transform.position = GetGroundSpawnPosition();
        createTime = Time.time;
    }

    public void Init(Action<GrassModel> r)
    {
        Init();
        releaseFn = r;
    }

    public void FixedUpdate()
    {
        if (Time.time > createTime + lifeLength)
        {
            releaseFn?.Invoke(this);
        }
    }


    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.transform.CompareTag("Building"))
    //    {
    //        ReleaseFn(this);
    //    }
    //    else if (collision.transform.CompareTag("Animal"))
    //    {
    //        health--;
    //        if (health == 0)
    //        {
    //            ReleaseFn(this);
    //        }
    //    }
    //}

    private Vector3 GetGroundSpawnPosition(float range = float.MaxValue)
    {
        var x = Random.Range(0, Screen.width);
        var y = Random.Range(0, Screen.height);
        var screenPosition = new Vector3(x, y, 0);

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        Physics.Raycast(ray, out RaycastHit hit, range);

        return hit.point;




    }
}
