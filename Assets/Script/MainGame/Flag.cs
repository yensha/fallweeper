using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -.5f) gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "hell")
        {
            gameObject.layer = 7;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "hell")
        {
            gameObject.layer = 6;
        }
    }
}
