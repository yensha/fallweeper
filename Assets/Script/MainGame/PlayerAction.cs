using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] GameObject Player1;
    float MoveingSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.localPosition += MoveingSpeed * Time.deltaTime * Vector3.forward;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.localRotation = Quaternion.Euler(0, 90, 0);
            transform.localPosition += MoveingSpeed * Time.deltaTime * Vector3.left;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            transform.localPosition += MoveingSpeed * Time.deltaTime * Vector3.back;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.localRotation = Quaternion.Euler(0, 270, 0);
            transform.localPosition += MoveingSpeed * Time.deltaTime * Vector3.right;
        }
    }
}
