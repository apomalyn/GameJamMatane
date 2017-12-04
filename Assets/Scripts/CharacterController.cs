using System;
using UnityEngine;

public class CharacterController : Entity{

    private Animator character_animator;
    private Rigidbody2D character_body;
    public float tileSize = 0.1600f;
    public AudioSource attackSound;
    public AudioClip quack;
    public Canvas playerGUI;
    
    // Use this for initialization
    void Start(){
        character_body = GetComponent<Rigidbody2D>();
        character_animator = GetComponent<Animator>();
        attackSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update(){
        character_animator.SetBool("Attack", false);
        
        if (Input.anyKeyDown){

            if (Input.GetKeyDown(KeyCode.UpArrow)){
                tryMove(Direction.Up);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)){
                tryMove(Direction.Down);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)){
                tryMove(Direction.Right);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)){
                tryMove(Direction.Left);
            }
            
            if (Input.GetKeyDown(KeyCode.P)){
                hit();
            }
        }
    }

    protected override void tryMove(Direction direction){
        base.tryMove(direction);
        GameManager.instance.nextTurn();
    }

    protected override bool defineAction(Direction direction){
        LayerMask stairLayer = LayerMask.NameToLayer("stair");
        
        if (!base.defineAction(direction)){
            Vector2 vector = Vector2.up;
        
            switch (direction){
                case Direction.Up:
                    vector = Vector2.up;
                    break;
                case Direction.Down:
                    vector = Vector2.down;
                    break;
                case Direction.Right:
                    vector = Vector2.right;
                    break;
                case Direction.Left:
                    vector = Vector2.left;
                    break;
            }

            if (Physics2D.Raycast(transform.position, vector, TILE_SIZE, 1 << stairLayer.value)){
                changeLevel();
                return true;
            }
        }

        return false;
    }


    protected override void move(Direction direction){
        base.move(direction);
        GameManager.instance.updateCamera(transform.position);
    }

    protected override void attack(GameObject entity){
        attackSound.PlayOneShot(quack);
        character_animator.SetBool("Attack", true);

        if (entity.GetComponent<EnnemyController>().hit()){
            inscreaseScore();
        }
    }

    private void inscreaseScore(){
        GuiManager gui = playerGUI.GetComponent<GuiManager>();

        gui.updateScore(10);
    }

    public void changeLevel(){
        
    }
    
    public override bool hit(){
        playerGUI.GetComponent<GuiManager>().TakeDamage();
        return false;
    }
}
