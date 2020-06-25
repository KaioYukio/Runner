using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldYPos : MonoBehaviour
{
    public Vector2 init;

    // Start is called before the first frame update
    void Start()
    {
        init = transform.position;  
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x, init.y);
    }
}
