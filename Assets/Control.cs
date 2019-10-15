/*Character Control Script - Georgios Brellas 2019
@TODO: 
- Move physics related code into FixedUpdate
- Refractor Player rigidbody look ups (Should increase performance since it'll avoid constant look ups)
  Idea: Look it up once at the start then store it in a variable.
*/
using UnityEngine;

public class Control : MonoBehaviour
{
    //Our Player
    public GameObject Player;
    private Rigidbody2D PlayerRigid;
    private SpriteAnim Anim;
    private SpriteRenderer PlayerRenderer;
    private float AnimSpeed;

    //Our Direction Colliders
    private CollisionScript Left;
    private CollisionScript Right;
    private CollisionScript Up;

    //Our Keys as variables so they can be changed (Maybe an in-game menu?)
    public KeyCode LeftButton;
    public KeyCode RightButton;
    public KeyCode JumpButton;
    public KeyCode SprintButton;

    //Movement Speed and Jumping Force
    //different because jumping is done by applying force, movement is only a velocity change.
    public float SetSpeed;
    public float SprintBoost;
    public float JumpForce;
    private float MovementSpeed;

    //Fairly self explanatory, our 'default' (Idle) sprite and our jumping sprite.
    public Sprite JumpSprite;
    public Sprite NormalSprite;

    //Movement related booleans needed for more precise control of movement.
    private bool CanJump;
    private bool WalkAnimActive;
    private bool LeftKeyActive;
    private bool RightKeyActive;
    private bool MovementActive;
    private bool LeftMovementActive;
    private bool RightMovementActive;
    private bool Sprint;

    // Start is called before the first frame update
    void Start()
    {
        CanJump = true;
        MovementSpeed = SetSpeed;
        PlayerRigid = Player.GetComponent<Rigidbody2D>();
        Anim = Player.GetComponent<SpriteAnim>();
        PlayerRenderer = Player.GetComponent<SpriteRenderer>();
        AnimSpeed = 0.05f;

        // Direction colliders.
        Left = Player.transform.GetChild(0).GetComponent<CollisionScript>();
        Right = Player.transform.GetChild(1).GetComponent<CollisionScript>();
        Up = Player.transform.GetChild(2).GetComponent<CollisionScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(SprintButton))
        {
            MovementSpeed = SetSpeed + SprintBoost;
            if (WalkAnimActive)
            {
                Anim.StopAnimation();
                Anim.PlayAnimation(0, 0.03f);
            }
        }
        
        if (Input.GetKeyUp(SprintButton))
        {
            MovementSpeed = SetSpeed;

            if (WalkAnimActive)
            {
                Anim.StopAnimation();
                Anim.PlayAnimation(0, AnimSpeed);
            }
        }

        PlayerRigid.velocity = new Vector2(Mathf.Clamp(PlayerRigid.velocity.x, -MovementSpeed, MovementSpeed), Mathf.Clamp(PlayerRigid.velocity.y, -JumpForce, JumpForce));
        
        if (Input.GetKeyDown(LeftButton) || Input.GetKeyDown(RightButton))
        {
            if (WalkAnimActive)
            {
                Anim.PlayAnimation(0, AnimSpeed);
            }
        }
        if (Input.GetKey(LeftButton))
        {
            LeftKeyActive = true;
            if (MovementActive && LeftMovementActive)
            {
                PlayerRigid.velocity = new Vector2(-MovementSpeed, PlayerRigid.velocity.y);
            }

            //Flips the sprite so that it reflects the direction the player is facing.
            PlayerRenderer.flipX = true;
        }
        
        if (Input.GetKey(RightButton))
        {
            RightKeyActive = true;
            if (MovementActive && RightMovementActive)
            {
                PlayerRigid.velocity = new Vector2(MovementSpeed, PlayerRigid.velocity.y);
            }

            //Flips the sprite so that it reflects the direction the player is facing.
            PlayerRenderer.flipX = false;
        }


        if (Input.GetKeyUp(LeftButton) || Input.GetKeyUp(RightButton))
        {
            //Stops the player from moving (sliding due to physics)
            PlayerRigid.velocity = new Vector2(0, PlayerRigid.velocity.y);

            if (CanJump)
            {
                PlayerRenderer.sprite = NormalSprite;
            }

        }
        
        if (Input.GetKeyUp(LeftButton))
        {
            LeftKeyActive = false;
        }

        if (Input.GetKeyUp(RightButton))
        {
            RightKeyActive = false;
        }

        if (!LeftKeyActive && !RightKeyActive)
        {
            Anim.StopAnimation();
        }

        if (Input.GetKeyDown(JumpButton) && CanJump)
        {
            CanJump = false;
            Debug.Log("Jumping!");
            Anim.StopAnimation();
            PlayerRenderer.sprite = JumpSprite;
            PlayerRigid.AddRelativeForce(new Vector2(0, JumpForce));
        }

        if (Left.Collided())
        {
            LeftMovementActive = false;
        } else
        {
            LeftMovementActive = true;
        }

        if (Right.Collided())
        {
            RightMovementActive = false;
        } else
        {
            RightMovementActive = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //This is basically just a way to figure out the direction the collider is relative to us.
        var dif = collision.collider.transform.position - Player.transform.position;
        var distance = dif.magnitude;
        var direction = dif / distance;
        /*For my sanity:
         Left : X = -0.9~
         Right : X = 0.9~
         Up : Y = 0.9~
         Down: Y = Bellow 0
        */
        Debug.Log("Collision direction: y = " + direction.y + " | x = " + direction.x);
        if (direction.y < 0)
        {
            if (collision.collider.tag == "Ground")
            {
                if (LeftKeyActive || RightKeyActive)
                {
                    Anim.PlayAnimation(0, AnimSpeed);
                }
                MovementActive = true;
                WalkAnimActive = true;
                PlayerRenderer.sprite = NormalSprite;
                Debug.Log("Changed sprite to: NormalSprite");
                CanJump = true;
                Debug.Log("CanJump set to true");

            }
        } 
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        var dif = collision.collider.transform.position - Player.transform.position;
        var distance = dif.magnitude;
        var direction = dif / distance;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        WalkAnimActive = false;
        MovementActive = true;
    }
}
