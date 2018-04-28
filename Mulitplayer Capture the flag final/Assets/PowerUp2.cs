using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PowerUp2 : NetworkBehaviour {
	public enum State
	{
		Available,
		Dropped,
		Possessed
	};
	public float droppedTime = 1.0f;

	private float m_droppedTimer = 1.0f;

	[SyncVar]
	State m_state;

	public State GetState() {
		return m_state;
	}

	// Use this for initialization
	void Start () {
		m_state = State.Available;
		m_droppedTimer = droppedTime;
	}

	void OnTriggerEnter(Collider other)
	{
		if(!isServer || other.tag != "Player")
		{
			return;
		}

		//make this player drop the flag, start a cooldown for pickup
		if (m_state == State.Available) {

			m_state = State.Possessed;
			Destroy (this.gameObject);
			GameObject.Find ("CTFGameManager").GetComponent<CTFGameManager> ().powerup2present = false;
			GameObject.Find ("CTFGameManager").GetComponent<CTFGameManager> ().SpawnPowerUp2 ();


		}
	}

	// Update is called once per frame
	void Update () {
		if (m_state == State.Dropped)
		{
			this.transform.parent = null;
		}

		if (!isServer) {	
			return;
		}

		if (m_state == State.Dropped) {
			this.transform.parent = null;
			m_droppedTimer -= Time.deltaTime;
			if (m_droppedTimer < 0.0f) {
				m_state = State.Available;
				m_droppedTimer = droppedTime;
			}
		}
	}
}
