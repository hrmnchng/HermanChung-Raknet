using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class TimeLimitStuff : NetworkBehaviour {
	

	[SyncVar]
	[SerializeField] public float timeleft;
	[SerializeField] private string leveltoload;
	// Use this for initialization
	void Start () {
		timeleft = 120f;

	}
	
	// Update is called once per frame
	void Update () {


		timeleft = timeleft - Time.deltaTime;
		if (timeleft <= 0)
		{
			doloadscene();
		}

	}

	private GUIStyle guiStyle1 = new GUIStyle();
	void OnGUI(){
		
			guiStyle1.fontSize = 20;
			guiStyle1.normal.textColor = Color.black;

			GUI.Label (new Rect (100, 10, 500, 300), "<b>Time Left: </b>" + timeleft, guiStyle1);
		}

	public void doloadscene()
	{
		SceneManager.LoadScene(leveltoload);
	}

}
