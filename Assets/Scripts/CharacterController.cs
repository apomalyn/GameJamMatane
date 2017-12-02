using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

	private bool toright = true;
	private bool facedown = true;
	private Animator character_animator;
	private Rigidbody2D character_body;	
	
	// Use this for initialization
	void Start ()
	{
		this.character_body = this.GetComponent<Rigidbody2D>();
		this.character_animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
