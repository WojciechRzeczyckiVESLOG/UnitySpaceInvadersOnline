using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject PlayerPrefab;
    public GameObject SceneCamera;

    public static int lives = 3;
    public static bool isGameRunning;

    public Text livesLabel;
    public Text endScreen;
    public Text ping;

    public GameObject DisconnectUI;
    private bool Off = false;
    public GameObject ServerMessage;
    public GameObject ServerMessageContainer;

    public float timeToLeave = 4f;
    public float leaveTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        isGameRunning = false;
        
    }


    private void Awake()
    {
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        ping.text = "Ping: " + PhotonNetwork.GetPing();
        livesLabel.text = "Lives: " + lives;

        CheckInput();

        //end label if Player has no lives left
        if(lives <=0)
        {
            endScreen.text = "YOU LOST!";
            leaveTimer += Time.deltaTime;
            if (Input.anyKey && leaveTimer >= timeToLeave)
            {
                //SceneManager.UnloadScene(1);
                lives = 3;
                //SceneManager.LoadScene(0);
                LeaveRoom();
            }

        }
        if (GameObject.FindGameObjectsWithTag("Enemy").Length<=0)
        {
            endScreen.text = "YOU WON!";
            leaveTimer += Time.deltaTime;
            if (Input.anyKey && leaveTimer >= timeToLeave)
            {
                //SceneManager.UnloadScene(1);
                lives = 3;
                //SceneManager.LoadScene(0);
                LeaveRoom();
            }

        }
    }

    public void SpawnPlayer()
    {
        float randomValue = Random.Range(-1f, 1f);
        //float randomValue = 0.5f;


        PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector2(this.transform.position.x * randomValue, -4), Quaternion.identity, 0);
        //public GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector2(this.transform.position.x * randomValue, this.transform.position.y), Quaternion.identity, 0);

        //GameCanvas.SetActive(false);
        //SceneCamera.SetActive(false);
    }


    public void CheckInput()
    {
        if (Off && Input.GetKeyDown(KeyCode.Escape))
        {
            DisconnectUI.SetActive(false);
            Off = false;
        }
        else if (!Off && Input.GetKeyDown(KeyCode.Escape)) {
            DisconnectUI.SetActive(true);
            Off = true;
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }

    private void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        GameObject message = Instantiate(ServerMessageContainer, new Vector2(0, 0), Quaternion.identity);
        message.transform.SetParent(ServerMessageContainer.transform, false);
        message.GetComponent<Text>().text = player.name + " joined the game";
        message.GetComponent<Text>().color = Color.green;
    }

    private void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        GameObject message = Instantiate(ServerMessageContainer, new Vector2(0, 0), Quaternion.identity);
        message.transform.SetParent(ServerMessageContainer.transform, false);
        message.GetComponent<Text>().text = player.name + " left the game";
        message.GetComponent<Text>().color = Color.red;
    }

}
