/*Character Control Script - Georgios Brellas 2019
@TODO: 
- Move physics related code into FixedUpdate

*/
using UnityEngine.UI;
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
    //Joystick
    private bool JoyLeftActive;
    private bool JoyRightActive; 

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
        if (Input.GetAxis("Horizontal") <= -0.9) {
            JoyLeftActive = true;
        } else
        {
            JoyLeftActive = false;
        }
        if (Input.GetAxis("Horizontal") >= 0.9)
        {
            JoyRightActive = true;
        } else
        {
            JoyRightActive = false;
        }

        if (Input.GetKeyDown(SprintButton))
        {
            MovementSpeed = SetSpeed + SprintBoost;
        }
        
        if (Input.GetKeyUp(SprintButton))
        {
            MovementSpeed = SetSpeed;
        }

        if (LeftKeyActive || RightKeyActive)
        {
            if (WalkAnimActive && !Anim.Active())
            {
                Debug.Log("AnimStart");
                Anim.PlayAnimation(0, AnimSpeed);
            }
        }

        PlayerRigid.velocity = new Vector2(Mathf.Clamp(PlayerRigid.velocity.x, -MovementSpeed, MovementSpeed), Mathf.Clamp(PlayerRigid.velocity.y, -9999, 6));
        
        if (Input.GetKey(LeftButton) || JoyLeftActive)
        {
            LeftKeyActive = true;
            if (MovementActive && LeftMovementActive)
            {
                PlayerRigid.velocity = new Vector2(-MovementSpeed, PlayerRigid.velocity.y);
            }

            //Flips the sprite so that it reflects the direction the player is facing.
            PlayerRenderer.flipX = true;
        } else
        {
            LeftKeyActive = false;
        }
        

        if (Input.GetKey(RightButton) || JoyRightActive)
        {
            RightKeyActive = true;
            if (MovementActive && RightMovementActive)
            {
                PlayerRigid.velocity = new Vector2(MovementSpeed, PlayerRigid.velocity.y);
            }

            //Flips the sprite so that it reflects the direction the player is facing.
            PlayerRenderer.flipX = false;
        } else
        {
            RightKeyActive = false;
        }

        if (!LeftKeyActive && !RightKeyActive)
        {
            //Stops the player from moving (sliding due to physics)
            Anim.Stop();
            PlayerRigid.velocity = new Vector2(0, PlayerRigid.velocity.y);

            if (CanJump)
            {
                PlayerRenderer.sprite = NormalSprite;
            }
        } 

        if ((Input.GetKeyDown(JumpButton) || Input.GetButton("Jump")) && CanJump)
        {
            Anim.Stop();
            WalkAnimActive = false;
            CanJump = false;
            PlayerRenderer.sprite = JumpSprite;
            PlayerRigid.AddForce(new Vector2(0, JumpForce));
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
        if (collision.collider.tag == "Ground")
        {
            Debug.Log("Collided");
            WalkAnimActive = true;
            MovementActive = true;
            PlayerRenderer.sprite = NormalSprite;
            CanJump = true;
        }

        if (collision.collider.tag == "Baddie")
        {
            PlayerRenderer.sprite = JumpSprite;
            PlayerRigid.AddForce(new Vector2(0, JumpForce));
        }
    }
}
