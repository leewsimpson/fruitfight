using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Game gameManager;
    public float Speed = 20f;
    private Rigidbody rb;
    public string Name = "";

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetAxis(Name) != 0)
        {
            rb.velocity = Input.GetAxis(Name) * Speed * Vector3.right;
        }

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            gameManager.Drop(name);
        }
    }
}
