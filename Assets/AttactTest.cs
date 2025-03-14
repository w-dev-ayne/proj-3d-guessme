using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttactTest : MonoBehaviour
{
    public float force = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Rigidbody>().AddForce(Vector3.forward * force, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
