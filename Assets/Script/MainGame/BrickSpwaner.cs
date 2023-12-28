using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class BrickSpwaner : MonoBehaviour
{
    [SerializeField] Transform MainCam;
    [SerializeField] GameObject MainControl;
    private float CamY;
    private float CamZ;
    [SerializeField] GameObject Brick_Normal;
    [SerializeField] GameObject Brick_Safe;
    [SerializeField] GameObject Brick_Bumb;
    private int BumbNum;
    private int length;
    Dictionary<(int x, int y), GameObject> ListBrick;
    Dictionary<(int x, int y), GameObject> ListSafeBrick;
    Dictionary<(int x, int y), GameObject> ListBumbBrick;
    private int[] BumbList = new int[25];
    private bool FirstPick;
    // Start is called before the first frame update

    private void Start()
    {

    }

    public void SizeSelect(string size)
    {
        switch (size)
        {
            case "small":
                length = 8;
                CamY = 20.0f;
                CamZ = -5.0f;
                BumbNum = 10;
                break;
            case "medium":
                length = 10;
                CamY = 25.0f;
                CamZ = -7.0f;
                BumbNum = 18;
                break;
            case "large":
                length = 12;
                CamY = 30.0f;
                CamZ = -9.0f;
                BumbNum = 25;
                break;
        }
        ListBrick = new Dictionary<(int x, int y), GameObject>();
        ListSafeBrick = new Dictionary<(int x, int y), GameObject>();
        MainCam.transform.position = new Vector3((length - 1) * 2.4f / 2.0f, CamY, CamZ);
        Quaternion CamEuler = Quaternion.Euler(60.0f, 0.0f, 0.0f);
        MainCam.transform.rotation = CamEuler;
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                var newBrick = Instantiate(Brick_Normal);
                ListBrick.Add((i, j), newBrick);
                newBrick.transform.position = new Vector3(i * 2.4f, 0.0f, j * 2.4f);
                newBrick.SetActive(true);
            }
        }
        PlantBumb();
    }

    void PlantBumb()
    {
        ListBumbBrick = new Dictionary<(int x, int y), GameObject>();
        for (int i = 0; i < BumbNum; i++)
        {
            BumbList[i] = Random.Range(0, length * length);

            for (int j = 0; j < i; j++)
            {
                while(BumbList[j] == BumbList[i])
                {
                    j = 0;
                    BumbList[i] = Random.Range(0, length * length);
                }
            }
            GameObject bumbbrick = Instantiate(Brick_Bumb);
            bumbbrick.transform.localPosition = new Vector3(BumbList[i] / length * 2.4f, 0, BumbList[i] % length * 2.4f);
            ListBumbBrick.Add((BumbList[i] / length, BumbList[i] % length), bumbbrick);
        }
        FirstPick = true;
    }

    public void BrickCheck(Vector3 pos)
    {
        int x = Mathf.FloorToInt((pos.x + 1.2f) / 2.4f);
        int z = Mathf.FloorToInt((pos.z + 1.2f) / 2.4f);

        if (ListBumbBrick.ContainsKey((x, z)))
        {
            if (FirstPick)
            {
                FirstPick = false;
                PlantBumb();
                BrickCheck(pos);
            }
            else
            {
                MainControl.gameObject.SendMessage("EndGame");
                ShowBumb();
            }
        }
        else
        {
            FirstPick = false;
            BrickChange(x, z);
        }
    }

    bool CheckDiff(int x, int z)
    {
        if (ListBumbBrick.ContainsKey((x + 1, z))
            || ListBumbBrick.ContainsKey((x-1,z))
            || ListBumbBrick.ContainsKey((x,z+1))
            || ListBumbBrick.ContainsKey((x, z - 1)))
        {
            return false;
        }
        else  return true;
    }

    void Diffusion(int x, int z)
    {
        BrickChange(x + 1, z);
        BrickChange(x - 1, z);
        BrickChange(x, z + 1);
        BrickChange(x, z - 1);
    }

    void BrickChange(int x, int z)
    {
        if(ListBrick.TryGetValue((x, z), out GameObject now_brick) && !ListBumbBrick.ContainsKey((x, z)))
        {
            GameObject new_brick = Instantiate(Brick_Safe);
            ListSafeBrick.Add((x, z), new_brick);
            new_brick.SetActive(true);
            new_brick.transform.localPosition = now_brick.transform.localPosition;

            ListBrick.Remove((x, z));
            now_brick.SetActive(false);
            if(CheckDiff(x, z)) Diffusion(x, z);
        }
        
    }

    void ShowBumb()
    {
        for (int i = 0; i < length; i++)
        {
            for(int j = 0; j < length; j++)
            {
                if(ListBumbBrick.TryGetValue((i,j), out GameObject bumb_brick))
                {
                    bumb_brick.SetActive(true);
                    if (ListBrick.TryGetValue((i, j), out GameObject brick))
                    {
                        brick.SetActive(false);
                    }
                }
            }
        }
    }
}
