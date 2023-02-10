using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCreateController : MonoBehaviour
{
    public LayerMask layerMask;

    public int spawnSpeed = 1;
    public int maxInstanceCount = 100;
    public List<GameObject> prefabs;


    private void FixedUpdate()
    {
        // trigger conditions
        var position = GetGroundSpawnPosition();
        CreateInstance(prefabs[Random.Range(0, prefabs.Count)], position);
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

    private void CreateInstance(GameObject prefab, Vector3 position)
    {
        Instantiate(prefab, position, Quaternion.identity);
    }
}
