using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControl : MonoBehaviour
{
    [SerializeField] GameObject MainCamera;
    [SerializeField] GameObject BrickSpwaner;
    GameObject Player;
    private Vector3 pos;
    [SerializeField] GameObject Player1;
    private bool canFlag = true;

    // Start is called before the first frame update
    void Start()
    {
        StartGame("medium", "player1");
    }

    // Update is called once per frame
    void Update()
    {
        MainCamera.gameObject.SendMessage("TransPos", Player.transform.localPosition);
        Player.gameObject.SendMessage("TransCam", MainCamera.transform);
        if(Input.GetMouseButtonDown(0))
        {
            BrickSpwaner.SendMessage("BrickCheck", Player.transform.localPosition);
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            BrickSpwaner.SendMessage("FlagRecycle", Player.transform.localPosition);
            pos = Player.transform.position;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            BrickSpwaner.SendMessage("FRunShow");
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(Mathf.FloorToInt((pos.x +1.2f) / 2.4f) != Mathf.FloorToInt((Player.transform.position.x + 1.2f) / 2.4f)
                || Mathf.FloorToInt((pos.z + 1.2f) / 2.4f) != Mathf.FloorToInt((Player.transform.position.z + 1.2f) / 2.4f))
            {
                BrickSpwaner.SendMessage("FRunShow");
                BrickSpwaner.SendMessage("FlagRecycle", Player.transform.localPosition);
                pos = Player.transform.position;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1) && canFlag)
            {
                canFlag = false;
                Invoke("FlagCD", .5f);
                BrickSpwaner.SendMessage("FlagSet", Player.transform.localPosition);
            }
        }
    }

    void FlagCD()
    {
        canFlag = true;
    }

    public void StartGame(string size, string player)
    {
        BrickSpwaner.gameObject.SendMessage("SizeSelect", size);
        PlayerSelect(player);
        Player.transform.localPosition = new Vector3(0,1,0);
    }

    public void PlayerSelect(string player)
    {
        switch (player)
        {
            case "player1":
                Player = Instantiate(Player1);
                break;
            default:
                Player = Instantiate(Player1);
                break;
        }
    }

    public void EndGame()
    {
        Debug.Log("bumb");
    }
}
