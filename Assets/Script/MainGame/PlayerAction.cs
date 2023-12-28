using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    float MovingSpeed = 15f;
    private float face;
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
            transform.localRotation = Quaternion.Euler(0, face * 180 / Mathf.PI, 0);
            transform.localPosition += MovingSpeed * Time.deltaTime * new Vector3(Mathf.Sin(face), 0, Mathf.Cos(face));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.localRotation = Quaternion.Euler(0, face * 180 / Mathf.PI - 90, 0);
            transform.localPosition += MovingSpeed * Time.deltaTime * new Vector3(-1 * Mathf.Cos(face), 0, Mathf.Sin(face));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.localRotation = Quaternion.Euler(0, face * 180 / Mathf.PI + 180, 0);
            transform.localPosition += MovingSpeed * Time.deltaTime * new Vector3(-1 * Mathf.Sin(face), 0, -1 * Mathf.Cos(face));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.localRotation = Quaternion.Euler(0, face * 180 / Mathf.PI + 90, 0);
            transform.localPosition += MovingSpeed * Time.deltaTime * new Vector3(Mathf.Cos(face), 0, -1 * Mathf.Sin(face));
        }
    }

    [System.Obsolete]
    public void TransCam(Transform Cam)
    {
        face = Cam.transform.eulerAngles.y * Mathf.PI / 180;
    }

}
