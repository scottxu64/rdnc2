using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBillboardController : MonoBehaviour
{

    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0f, mainCamera.transform.rotation.eulerAngles.y, 0f);
    }
}
