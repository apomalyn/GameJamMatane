using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class EnnemyController : MonoBehaviour
{

	[SerializeField]
	private int life;
	private Animator ennemy_animator;
	private Rigidbody2D ennemy_body;
	
	// Use this for initialization
	void Start () {
		this.ennemy_body = this.GetComponent<Rigidbody2D>();
		this.ennemy_animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player") 
		{	
			life -= 1;
			if (life <= 0) 
			{
				destroyEnnemy();
			}
		}
	}

	void destroyEnnemy()
	{
		Destroy(gameObject);
	}
}
