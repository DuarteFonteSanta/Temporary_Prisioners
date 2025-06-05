using Unity.Cinemachine;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    Vector3 player1Spawn;
    [SerializeField]
    Vector3 player2Spawn;
    
    [SerializeField]
    GameObject player1;
    [SerializeField]
    GameObject player2;

    public CinemachineCamera cam1;
    public CinemachineCamera cam2;


    void Start()
    {
        Instantiate(player1, player1Spawn, Quaternion.identity);
        Instantiate(player2, player2Spawn, Quaternion.identity);

        cam1.Follow = GameObject.FindGameObjectWithTag("Player").transform;
        cam2.Follow = GameObject.FindGameObjectWithTag("Player2").transform;

    }


    public void RespawnPlayer(int playerNumber)
    {
        
        if (playerNumber == 1)
        {
            player1 = GameObject.FindGameObjectWithTag("Player");
            
            player1.transform.position = player1Spawn;
            
            
        }
        else
        {
            player2 = GameObject.FindGameObjectWithTag("Player2");
            player2.transform.position = player2Spawn;
        }
        Timer timerManager = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
        timerManager.PlayerCaught();
        
    }
    
    

    public void SpawnPlayer1()
    {
        Instantiate(player1, player1Spawn, Quaternion.identity);
    }

    public void SpawnPlayer2()
    {
        Instantiate(player2, player2Spawn, Quaternion.identity);
    }
}
