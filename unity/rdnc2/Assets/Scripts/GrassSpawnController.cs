
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GrassSpawnController : MonoBehaviour
{
    public LayerMask layerMask;
    public float spawnSpeed = 0.1f;
    public int maxInstanceCount = 1000;
    public List<GrassModel> grassPrefabs;
    public GameObject explosionPrefab;

    private ObjectPool<GrassModel> _grassPool;
    private ObjectPool<GameObject> _explosionPool;

    public int count;       // for debugging in unity inspector


    private void Start()
    {
        _explosionPool = new ObjectPool<GameObject>(
            () =>
            {
                return Instantiate(explosionPrefab);
            },
            explosion =>
            {
                explosion.SetActive(true);
            },
            explosion =>
            {
                explosion.SetActive(false);
            },
            explosion =>
            {
                Destroy(explosion);
            },
            false, 50, 100
         );


        _grassPool = new ObjectPool<GrassModel>(
            () =>
            {
                var grass = Instantiate(grassPrefabs[Random.Range(0, grassPrefabs.Count)]);
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
                GetExplosion(grass.gameObject.transform.position);
                grass.gameObject.SetActive(false);
            },
            grass =>
            {
                GetExplosion(grass.gameObject.transform.position);
                Destroy(grass.gameObject);
            },
            false, maxInstanceCount, maxInstanceCount + 100
            );


        InvokeRepeating(nameof(GetGrass), 1, spawnSpeed);  // similar to FixedUpdate(), with finer control
    }


    #region Grass
    private void GetGrass()
    {
        _grassPool.Get();
        count = _grassPool.CountActive;
    }

    private void ReleaseGrass(GrassModel grass)
    {
        _grassPool.Release(grass);
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
    #endregion

    #region Explosion
    private void GetExplosion(Vector3 spawnPosition)
    {
        var explosion = _explosionPool.Get();
        explosion.gameObject.transform.position = spawnPosition;

        StartCoroutine(ReleaseExplosionAfter5Seconds(explosion));
    }

    IEnumerator ReleaseExplosionAfter5Seconds(GameObject explosion)
    {
        yield return new WaitForSeconds(5);
        _explosionPool.Release(explosion);
    }
    #endregion
}
