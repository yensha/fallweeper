using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float distance = 20f;
    private float min_dis = 5f;
    private float max_dis = 35f;
    private float min_vert = 5f;
    private float max_vert = 90f;
    private Vector3 CameraPos;
    private Vector3 PlayerPos = new Vector3(0, 1, 0);
    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }
    private float rotationX = 0, rotationY = 0;
    public RotationAxes axes = RotationAxes.MouseXAndY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * 4, 0);
        }
        else if (axes == RotationAxes.MouseY)
        {
            rotationX -= Input.GetAxis("Mouse Y") * 4;
            rotationX = Mathf.Clamp(rotationX, min_vert, max_vert);

            float rotationY = transform.localEulerAngles.y;
            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
        }
        else
        {
            rotationX -= Input.GetAxis("Mouse Y") * 4;
            rotationX = Mathf.Clamp(rotationX, min_vert, max_vert);

            float delta = Input.GetAxis("Mouse X") * 4;
            rotationY = transform.localEulerAngles.y + delta;
            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
        }
        float mouseCenter = Input.GetAxis("Mouse ScrollWheel");
        if (mouseCenter < 0)
        {
            if (distance < max_dis) distance++;
        }
        else if (mouseCenter > 0)
        {
            if (distance > min_dis)   distance--;
        }
        CameraPos.x = Mathf.Sin(rotationY / 180 * Mathf.PI) * distance * Mathf.Cos(rotationX / 180 * Mathf.PI);
        CameraPos.y = -Mathf.Sin(rotationX / 180 * Mathf.PI) * distance;
        CameraPos.z = Mathf.Cos(rotationY / 180 * Mathf.PI) * distance * Mathf.Cos(rotationX / 180 * Mathf.PI);
        transform.position = PlayerPos - CameraPos;
    }

    public void TransPos(Vector3 pos)
    {
        PlayerPos = pos;
    }
}
