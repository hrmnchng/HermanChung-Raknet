using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CTFGameManager : NetworkBehaviour {

    public int m_numPlayers = 2;
    public float m_gameTime = 5.0f;

    public GameObject m_flag = null;
	public GameObject m_powerup1 = null;
	public GameObject m_powerup2 = null;

	public bool powerup1present = false;
	public bool powerup2present = false ;


    public enum CTF_GameState
    {
        GS_WaitingForPlayers,
        GS_Ready,
        GS_InGame,
    }
	// Use this for initialization
	void Start () {
		
	}

    [SyncVar]
    CTF_GameState m_gameState = CTF_GameState.GS_WaitingForPlayers;

    public bool SpawnFlag()
    {
		Vector3 spawnPoint = new Vector3(0, 0, 0);;
		//ObjectSpawner.RandomPoint(new Vector3(0, 0, 0), 10.0f, out spawnPoint);
		GameObject flag = Instantiate(m_flag, spawnPoint, new Quaternion());
        NetworkServer.Spawn(flag);
        return true;
    }

	public bool SpawnPowerUp1()
	{
		if (powerup1present == false) {
			float num = Random.Range (15, 25);
			Vector3 spawnPoint2 = new Vector3 (num, 0, num);
			//ObjectSpawner.RandomPoint(new Vector3 (0, 0, 0), 50.0f, out spawnPoint2);
			GameObject powerUp1 = Instantiate (m_powerup1, spawnPoint2, new Quaternion ());
			NetworkServer.Spawn (powerUp1);
			powerup1present = true;
		}
		return true;
	}

	public bool SpawnPowerUp2()
	{
		if (powerup2present == false) {
			float num = Random.Range (15, 25);
			Vector3 spawnPoint3 = new Vector3 (-num, 0, -num);
			//ObjectSpawner.RandomPoint(new Vector3 (0, 0, 0), 50.0f, out spawnPoint2);
			GameObject powerUp2 = Instantiate (m_powerup2, spawnPoint3, new Quaternion ());
			NetworkServer.Spawn (powerUp2);
			powerup2present = true;
		}
		return true;
	}

    bool IsNumPlayersReached()
    {
        return CTFNetworkManager.singleton.numPlayers == m_numPlayers;
    }


	// Update is called once per frame
	void Update ()
    {
		
	    if(isServer)
        {
            if(m_gameState == CTF_GameState.GS_WaitingForPlayers && IsNumPlayersReached())
            {
                m_gameState = CTF_GameState.GS_Ready;
            }
        }

        UpdateGameState();
	}

    public void UpdateGameState()
    {
        if (m_gameState == CTF_GameState.GS_Ready)
        {
            //call whatever needs to be called
            if (isServer)
            {
                SpawnFlag();
				SpawnPowerUp1 ();
				SpawnPowerUp2 ();
                //change state to ingame
                m_gameState = CTF_GameState.GS_InGame;
            }
        }
        
    }
}
