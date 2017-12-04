
using UnityEditor;
using UnityEngine;


public abstract class Entity : MonoBehaviour{

    private bool toRight = true;
    
    private bool facedown = true;

    protected const float TILE_SIZE = 0.1600f;

    public LayerMask moveLayer;
    
    protected enum Direction{
        Up,
        Down,
        Left,
        Right
    }
    
    
    protected virtual void move(Direction direction){
        Vector3 pos = new Vector3(0, 0, 0);
        
        switch (direction){
            case Direction.Up:
                pos = new Vector3(0, 0.1600f, 0);
                break;
            case Direction.Down:
                pos = new Vector3(0, -0.1600f, 0);
                break;
            case Direction.Left:
                pos = new Vector3(-0.1600f, 0, 0);
                if (toRight)    flip();
                break;
            case Direction.Right:
                pos = new Vector3(0.1600f, 0, 0);
                if(!toRight)    flip();
                break;
        }
        
        transform.Translate(pos, Space.World);
        
    }

    protected virtual void attack(GameObject entity){
        if (entity.layer == LayerMask.GetMask("enemy")){
            entity.GetComponent<EnnemyController>().hit();
        }else{
            entity.GetComponent<CharacterController>().hit();
        }
    }

    public abstract void hit();


    protected void tryMove(Direction direction){
        switch (direction){
            case Direction.Up:
                defineAction(Direction.Up);
                break;
            case Direction.Down:
                defineAction(Direction.Down);
                break;
            case Direction.Left:
                defineAction(Direction.Left);
                break;
            case Direction.Right:
                defineAction(Direction.Right);
                break;
        }
    }

    protected virtual bool defineAction(Direction direction){
        LayerMask enemyLayer = 1 << LayerMask.NameToLayer("enemy");
        LayerMask playerLayer = LayerMask.NameToLayer("player");
        LayerMask wallLayer = LayerMask.NameToLayer("wall");
            
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

        RaycastHit2D hit;

        hit = Physics2D.Raycast(transform.position, vector, 0.16f, wallLayer);
        
        if (hit = Physics2D.Raycast(transform.position, vector, TILE_SIZE, 1 << enemyLayer.value)){
            if (gameObject.layer == playerLayer){
                attack(hit.collider.gameObject);
                return true;
            }
        }else if (hit = Physics2D.Raycast(transform.position, vector, TILE_SIZE, 1 << playerLayer.value)){
            if (gameObject.layer == enemyLayer){
                attack(hit.collider.gameObject);
                return true;
            }
        }else if (!Physics2D.Raycast(transform.position, vector, 0.16f, 1 << wallLayer.value)){
            move(direction);
            return true;
        }
        return false;
    }
    
    private void flip(){
        toRight = !toRight;
        Vector3 scale = this.transform.localScale;
        scale.x *= -1;
        this.transform.localScale = scale;
    }
}