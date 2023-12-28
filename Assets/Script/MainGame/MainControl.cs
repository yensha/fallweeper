using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControl : MonoBehaviour
{
    [SerializeField] GameObject BrickSpwaner;
    GameObject Player;
    [SerializeField] GameObject Player1;

    // Start is called before the first frame update
    void Start()
    {
        StartGame("large", "player1");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            BrickSpwaner.SendMessage("BrickCheck", Player.transform.localPosition);
        }
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
