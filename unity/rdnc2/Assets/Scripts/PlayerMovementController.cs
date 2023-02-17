using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var rightLeftKeys = Input.GetAxisRaw("Vertical");
        var upDownKeys = Input.GetAxisRaw("Horizontal");
        var movementTarget = new Vector3(rightLeftKeys, 0, upDownKeys)*0.05f;

        gameObject.transform.Translate(movementTarget);
    }
}
