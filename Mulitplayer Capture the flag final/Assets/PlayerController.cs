using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomMsgType
{
    public static short Transform = MsgType.Highest + 1;
};


public class PlayerController : NetworkBehaviour
{
	[SyncVar]
	[SerializeField] public float points;

    public float m_linearSpeed = 5.0f;
    public float m_angularSpeed = 3.0f;
    public float m_jumpSpeed = 5.0f;

    private Rigidbody m_rb = null;

	[SyncVar]
	public float powertime;

    [SyncVar]
    public bool m_hasFlag = false;
	[SyncVar]
	public bool m_hasP1 = false;
	[SyncVar]
	public bool m_hasP2 = false;

    public bool HasFlag() {
        return m_hasFlag;
    }
	public bool HasP1() {
		return m_hasP1;
	}
	public bool HasP2() {
		return m_hasP2;
	}

    [Command]
    public void CmdPickUpFlag()
    {
        m_hasFlag = true;
    }

    [Command]
    public void CmdDropFlag()
    {
        m_hasFlag = false;
    }

	[Command]
	public void CmdPickUpP2()
	{
		m_hasP2 = true;
	}

	[Command]
	public void CmdDropP2()
	{
		m_hasP2 = false;
	}

    bool IsHost()
    {
        return isServer && isLocalPlayer;
    }

    // Use this for initialization
    void Start() {
		

        m_rb = GetComponent<Rigidbody>();
        //Debug.Log("Start()");
        Vector3 spawnPoint;
        ObjectSpawner.RandomPoint(this.transform.position, 10.0f, out spawnPoint);
        this.transform.position = spawnPoint;

        TrailRenderer tr = GetComponent<TrailRenderer>();
        tr.enabled = false;

		points = 0;

    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        //Debug.Log("OnStartAuthority()");
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        //Debug.Log("OnStartClient()");
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        //Debug.Log("OnStartLocalPlayer()");
        GetComponent<MeshRenderer>().material.color = new Color(0.0f, 1.0f, 0.0f);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        //Debug.Log("OnStartServer()");
    }

    public void Jump()
    {
        Vector3 jumpVelocity = Vector3.up * m_jumpSpeed;
        m_rb.velocity += jumpVelocity;
        TrailRenderer tr = GetComponent<TrailRenderer>();
        tr.enabled = true;
    }


    [ClientRpc]
    public void RpcJump()
    {
        Jump();
    }

    [Command]
    public void CmdJump()
    {
        Jump();
        RpcJump();
    }

	public void getpowerup1()
	{
		this.transform.localScale = new  Vector3(3,3,3);
	}


	[ClientRpc]
	public void Rpcgetpowerup1()
	{
		getpowerup1();
	}

	[Command]
	public void Cmdgetpowerup1()
	{
		getpowerup1();
		Rpcgetpowerup1();
	}

	public void getpowerup2()
	{
		this.gameObject.transform.position = GameObject.FindGameObjectWithTag ("Flag").transform.position;
		//this.transform.localScale = new  Vector3(3,3,3);
	}


	[ClientRpc]
	public void Rpcgetpowerup2()
	{
		getpowerup2();
	}

	[Command]
	public void Cmdgetpowerup2()
	{
		getpowerup2();
		Rpcgetpowerup2();
	}
    // Update is called once per frame
    void Update() {
		

        if (!isLocalPlayer)
        {
            return;
        }

        if (m_rb.velocity.y < Mathf.Epsilon) {
            TrailRenderer tr = GetComponent<TrailRenderer>();
            tr.enabled = false;
        }

        float rotationInput = Input.GetAxis("Horizontal");
        float forwardInput = Input.GetAxis("Vertical");

        Vector3 linearVelocity = this.transform.forward * (forwardInput * m_linearSpeed);

		if (Input.GetKeyDown(KeyCode.Space)&&(m_hasFlag == true))
        {
            CmdJump();
        }
		if (Input.GetKey(KeyCode.Space)&&(m_hasFlag == false))
		{

			m_linearSpeed = 20;
			//CmdJump();
		}else{m_linearSpeed = 7.0f;}

        float yVelocity = m_rb.velocity.y;


        linearVelocity.y = yVelocity;
        m_rb.velocity = linearVelocity;

        Vector3 angularVelocity = this.transform.up * (rotationInput * m_angularSpeed);
        m_rb.angularVelocity = angularVelocity;

		if (m_hasFlag == true) {
			points = points + Time.deltaTime;
		} else {
			points = points;
		}

    }

    [Command]
    public void CmdPlayerDropFlag()
    {
        Transform childTran = this.transform.GetChild(this.transform.childCount - 1);
        Flag flag = childTran.gameObject.GetComponent<Flag>();
		if (flag) {
			flag.CmdDropFlag();
		}
    }
	public void OnCollisionEnter(Collision other)
	{
		if(!isLocalPlayer || other.collider.tag != "Player")
		{
			return;
		}
			
		if (HasFlag()) {
			Transform childTran = this.transform.GetChild (this.transform.childCount - 1);
			if (childTran.gameObject.tag == "Flag") {
                CmdPlayerDropFlag();
			
            }

		}

	}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "P1") 
		{
			
			getpowerup1 ();

			//Destroy (this.gameObject);
		}
		if (other.tag == "P2") 
		{

			getpowerup2 ();
			//Destroy (this.gameObject);

		}
	}
	private GUIStyle guiStyle1 = new GUIStyle();

	public void OnGUI(){
		if (isLocalPlayer) {
			guiStyle1.fontSize = 30;
			guiStyle1.normal.textColor = Color.black;

			GUI.Label (new Rect (100, 20, 500, 300), "<b>Points: </b>" + points, guiStyle1);
		}
	}
   
}
