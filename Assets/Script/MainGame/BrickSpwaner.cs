using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BrickSpwaner : MonoBehaviour
{
    [SerializeField] Transform MainCam;
    [SerializeField] GameObject MainControl;
    [SerializeField] GameObject Brick_Normal;
    [SerializeField] GameObject Brick_Safe;
    [SerializeField] GameObject Brick_Bumb;
    [SerializeField] GameObject Brick_Recycle_clone;
    GameObject Brick_Recycle;
    [SerializeField] GameObject Flag;
    [SerializeField] GameObject Num1;
    [SerializeField] GameObject Num2;
    [SerializeField] GameObject Num3;
    [SerializeField] GameObject Num4;
    [SerializeField] GameObject Num5;
    [SerializeField] GameObject Num6;
    [SerializeField] GameObject Num7;
    [SerializeField] GameObject Num8;
    GameObject FlagRecycler;
    private int BumbNum;
    private int length;
    Dictionary<(int x, int y), GameObject> ListBrick;
    Dictionary<(int x, int y), GameObject> ListSafeBrick;
    Dictionary<(int x, int y), GameObject> ListBumbBrick;
    Dictionary<(int x, int z ,int y), GameObject> ListFlag;
    private int[] BumbList = new int[99];
    private bool FirstPick;
    // Start is called before the first frame update

    private void Start()
    {
        Brick_Recycle = Instantiate(Brick_Recycle_clone);
        Brick_Recycle.gameObject.SetActive(false);
    }

    public void SizeSelect(string size)
    {
        switch (size)
        {
            case "small":
                length = 8;
                BumbNum = 10;
                break;
            case "medium":
                length = 16;
                BumbNum = 40;
                break;
            case "large":
                length = 22;
                BumbNum = 99;
                break;
        }
        ListBrick = new Dictionary<(int x, int y), GameObject>();
        ListSafeBrick = new Dictionary<(int x, int y), GameObject>();
        ListFlag = new Dictionary<(int x, int z, int y), GameObject>();
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

    public void FlagSet(Vector3 pos)
    {
        int x = Mathf.FloorToInt((pos.x + 1.2f) / 2.4f);
        int z = Mathf.FloorToInt((pos.z + 1.2f) / 2.4f);
        int y = 0;
        while (ListFlag.ContainsKey((x, z, y)))
        {
            y++;
        }
        GameObject flag = Instantiate(Flag);
        ListFlag.Add((x, z, y), flag);
        y++;
        flag.transform.position = new Vector3(x * 2.4f, 3f + 1f * y, z * 2.4f);
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

    int CheckBumbNum(int x, int z)
    {
        int num = 0;
        if (ListBumbBrick.ContainsKey((x + 1, z)))
        {
            num++;
        }
        if (ListBumbBrick.ContainsKey((x - 1, z)))
        {
            num++;
        }
        if (ListBumbBrick.ContainsKey((x, z + 1)))
        {
            num++;
        }
        if(ListBumbBrick.ContainsKey((x, z - 1)))
        {
            num++;
        }
        if(ListBumbBrick.ContainsKey((x + 1, z + 1)))
        {
            num++;
        }
        if (ListBumbBrick.ContainsKey((x - 1, z + 1)))
        {
            num++;
        }
        if (ListBumbBrick.ContainsKey((x + 1, z - 1)))
        {
            num++;
        }
        if (ListBumbBrick.ContainsKey((x - 1, z - 1)))
        {
            num++;
        }
        return num;
    }

    void Diffusion(int x, int z)
    {
        BrickChange(x + 1, z);
        BrickChange(x - 1, z);
        BrickChange(x, z + 1);
        BrickChange(x, z - 1);
        BrickChange(x + 1, z + 1);
        BrickChange(x + 1, z - 1);
        BrickChange(x - 1, z + 1);
        BrickChange(x - 1, z - 1);
    }

    void BrickChange(int x, int z)
    {
        if(ListBrick.TryGetValue((x, z), out GameObject now_brick) && !ListBumbBrick.ContainsKey((x, z)))
        {
            int num = CheckBumbNum(x, z);
            GameObject new_brick = Instantiate(Brick_Safe);
            ListSafeBrick.Add((x, z), new_brick);
            new_brick.SetActive(true);
            new_brick.transform.localPosition = now_brick.transform.localPosition;
            GameObject numtext;
            Vector3 pos = new Vector3(x * 2.4f, 1.1f, z * 2.4f);
            switch (num)
            {
                case 1:
                    numtext = Instantiate(Num1);
                    numtext.transform.position = pos;
                    numtext.SetActive(true);
                    break;
                case 2:
                    numtext = Instantiate(Num2);
                    numtext.transform.position = pos;
                    numtext.SetActive(true);
                    break;
                case 3:
                    numtext = Instantiate(Num3);
                    numtext.transform.position = pos;
                    numtext.SetActive(true);
                    break;
                case 4:
                    numtext = Instantiate(Num4);
                    numtext.transform.position = pos;
                    numtext.SetActive(true);
                    break;
                case 5:
                    numtext = Instantiate(Num5);
                    numtext.transform.position = pos;
                    numtext.SetActive(true);
                    break;
                case 6:
                    numtext = Instantiate(Num6);
                    numtext.transform.position = pos;
                    numtext.SetActive(true);
                    break;
                case 7:
                    numtext = Instantiate(Num7);
                    numtext.transform.position = pos;
                    numtext.SetActive(true);
                    break;
                case 8:
                    numtext = Instantiate(Num8);
                    numtext.transform.position = pos;
                    numtext.SetActive(true);
                    break;
            }

            ListBrick.Remove((x, z));
            now_brick.SetActive(false);
            if(num == 0) Diffusion(x, z);
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

    public void FRunShow()
    {
        Vector3 pos = Brick_Recycle.transform.position;
        int x = Mathf.FloorToInt((pos.x + 1.2f) / 2.4f);
        int z = Mathf.FloorToInt((pos.z + 1.2f) / 2.4f);
        if (ListBrick.TryGetValue((x, z), out GameObject brick))
        {
            brick.SetActive(true);
        }
        else if (ListSafeBrick.TryGetValue((x, z), out GameObject safebrick))
        {
            safebrick.SetActive(true);
        }
        Brick_Recycle.SetActive(false);
    }

    public void FlagRecycle(Vector3 pos)
    {
        int x = Mathf.FloorToInt((pos.x + 1.2f) / 2.4f);
        int z = Mathf.FloorToInt((pos.z + 1.2f) / 2.4f);

        if (ListBrick.TryGetValue((x, z), out GameObject brick))
        {
            brick.SetActive(false);
        }
        else if(ListSafeBrick.TryGetValue((x, z), out GameObject safebrick))
        {
            safebrick.SetActive(false);
        }
        Brick_Recycle.transform.position = new Vector3(x * 2.4f, 0, z * 2.4f);
        Brick_Recycle.SetActive(true);
    }
}
