using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadlevel : MonoBehaviour {

	[SerializeField] private string leveltoload;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void doloadscene()
		{
		SceneManager.LoadScene(leveltoload);
		}

	public void ClickExit()

	{
		Application.Quit();
	}
}
