using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickSpwaner : MonoBehaviour
{
    [SerializeField] Transform MainCam;
    private float CamY;
    private float CamZ;
    [SerializeField] GameObject Brick;
    [SerializeField] int small_length;
    [SerializeField] int medium_length;
    [SerializeField] int large_length;
    private int length = 8;
    private Dictionary<(int x, int y), GameObject> ListBrick;
    // Start is called before the first frame update
    void Start()
    {
        ListBrick = new Dictionary<(int x, int y), GameObject>();
        SizeSelect("large");
        MainCam.transform.position = new Vector3((length-1) * 2.4f / 2.0f, CamY, CamZ);
        Quaternion CamEuler = Quaternion.Euler(60.0f, 0.0f, 0.0f);
        MainCam.transform.rotation = CamEuler;
        for (int i = 0; i < length; i++)
        {
            for(int j = 0; j < length; j++)
            {
                var newBrick = Instantiate(Brick);
                ListBrick.Add((i, j), newBrick);
                newBrick.transform.position = new Vector3(i * 2.4f, 0.0f, j * 2.4f);
                newBrick.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SizeSelect(string size)
    {
        switch (size)
        {
            case "small":
                length = small_length;
                CamY = 20.0f;
                CamZ = -6.0f;
                break;
            case "medium":
                length = medium_length;
                CamY = 25.0f;
                CamZ = -8.0f;
                break;
            case "large":
                length = large_length;
                CamY = 30.0f;
                CamZ = -10.0f;
                break;
        }
    }

}
