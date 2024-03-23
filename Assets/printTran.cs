using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class printTran : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print($"{transform.position.x}, {transform.position.y}, {transform.position.z}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
