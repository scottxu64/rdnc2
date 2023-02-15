using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GrassSpawnController : MonoBehaviour
{
    public LayerMask layerMask;
    public float spawnSpeed = 0.1f;
    public int maxInstanceCount = 1500;
    public List<GrassModel> prefabs;

    private ObjectPool<GrassModel> _pool;

    private void Start()
    {
        _pool = new ObjectPool<GrassModel>(
            () =>
            {
                var grass = Instantiate(prefabs[Random.Range(0, prefabs.Count)]);
                grass.Init(GetGroundSpawnPosition(), ReleaseGrass);
                return grass;
            },
            grass =>
            {
                grass.Init(GetGroundSpawnPosition());
                grass.gameObject.SetActive(true);
            },
            grass =>
            {
                grass.gameObject.SetActive(false);
            },
            grass =>
            {
                Destroy(grass.gameObject);
            },
            false, maxInstanceCount, maxInstanceCount+100
            );

        InvokeRepeating(nameof(GetGrass), 1, spawnSpeed);  // similar to FixedUpdate(), with finer control
    }



    private void GetGrass()
    {
        _pool.Get();
    }

    private void ReleaseGrass(GrassModel grass)
    {
        _pool.Release(grass);
    }


    private Vector3 GetGroundSpawnPosition(float range = float.MaxValue)
    {
        var x = Random.Range(0, Screen.width);
        var y = Random.Range(0, Screen.height);
        var screenPosition = new Vector3(x, y, 0);

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitTop, range) && Physics.Raycast(ray, out RaycastHit hitBottom, range, layerMask))
        {
            return hitTop.point == hitBottom.point ? hitBottom.point : GetGroundSpawnPosition();
        }

        return GetGroundSpawnPosition();
    }

}
