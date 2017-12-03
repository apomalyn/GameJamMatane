using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class CharacterController : MonoBehaviour{
    private bool toright = true;
    private bool facedown = true;
    private Animator character_animator;
    private Rigidbody2D character_body;
    public float tileSize = 0.1600f;
    public AudioSource attackSound;
    public AudioClip quack;

    // Use this for initialization
    void Start(){
        character_body = this.GetComponent<Rigidbody2D>();
        character_animator = this.GetComponent<Animator>();
        attackSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update(){
        character_animator.SetBool("Attack", false);

        if (Input.anyKeyDown){
            Vector3 playerPos = new Vector3();

            if (Input.GetKeyDown(KeyCode.UpArrow)){
                playerPos = new Vector3(0, 0.1600f, 0);
                //move vers le haut
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)){
                playerPos = new Vector3(0, -0.1600f, 0);
                //move vers le bas
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)){
                if (!toright){
                    flip();
                }
                playerPos = new Vector3(0.1600f, 0, 0);
                //move vers la droite
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)){
                if (toright){
                    flip();
                }

                playerPos = new Vector3(-0.1600f, 0, 0);
                //move vers la gauche
            }
            print(playerPos);
            if (playerPos != new Vector3())
                character_body.transform.Translate(playerPos, Space.World);


            if (Input.GetKeyDown(KeyCode.Space)){
                //attack
                //Attack sound
                attackSound.PlayOneShot(quack);
                //Attack animation
                character_animator.SetBool("Attack", true);
            }
        }
    }

    void flip(){
        toright = !toright;
        Vector3 scale = this.transform.localScale;
        scale.x *= -1;
        this.transform.localScale = scale;
    }
}
