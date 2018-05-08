using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public Game gameManager;
	void Start ()
    {
        gameManager = GameObject.Find("Game").GetComponent<Game>();  // Cannot set in UI as object is initialsed in code
	}
	
	void Update () {
		if(transform.position.y < -5)
        {
            gameManager.Miss();
            Destroy(this.gameObject);
        }
	}
}
