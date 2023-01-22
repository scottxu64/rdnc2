using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTouchController : MonoBehaviour
{
    public int touchRemainingCount = 1;
    public LayerMask layerMask;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask))
            {
                if (hit.transform.gameObject.name == gameObject.name)
                {
                    touchRemainingCount--;
                    if (touchRemainingCount == 0)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }

    }
}
