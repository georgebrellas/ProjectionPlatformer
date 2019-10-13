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

    //Our Keys as variables so they can be changed (Maybe in game menu?)
    public KeyCode LeftButton;
    public KeyCode RightButton;
    public KeyCode JumpButton;

    //Movement Speed and Jumping Force
    //different because jumping is done by applying force, movement is only a velocity change.
    public float MovementSpeed;
    public float JumpForce;

    //Fairly self explanatory, our 'default' (Idle) sprite and our jumping sprite.
    public Sprite JumpSprite;
    public Sprite NormalSprite;

    //Movement related booleans needed for more precise control of movement.
    private bool CanJump;
    private bool WalkAnimActive;
    private bool LeftKeyActive;
    private bool RightKeyActive;
    private bool MovementActive;

    // Start is called before the first frame update
    void Start()
    {
        CanJump = true;
        PlayerRigid = Player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        PlayerRigid.velocity = new Vector2(Mathf.Clamp(PlayerRigid.velocity.x, -MovementSpeed, MovementSpeed), Mathf.Clamp(PlayerRigid.velocity.y, -JumpForce, JumpForce));
 
        if (Input.GetKeyDown(LeftButton) || Input.GetKeyDown(RightButton))
        {
            if (WalkAnimActive)
            {
                Player.GetComponent<SpriteAnim>().PlayAnimation(0, 0.05f);
            }
        }

        if (Input.GetKey(LeftButton))
        {
            if (MovementActive)
            {
                LeftKeyActive = true;
                PlayerRigid.velocity = new Vector2(-MovementSpeed, PlayerRigid.velocity.y);
            }

            //Flips the sprite so that it reflects the direction the player is facing.
            Player.GetComponent<SpriteRenderer>().flipX = true;
        }
        
        if (Input.GetKey(RightButton))
        {
            if (MovementActive)
            {
                RightKeyActive = true;
                PlayerRigid.velocity = new Vector2(MovementSpeed, PlayerRigid.velocity.y);
            }

            //Flips the sprite so that it reflects the direction the player is facing.
            Player.GetComponent<SpriteRenderer>().flipX = false;
        }


        if (Input.GetKeyUp(LeftButton) || Input.GetKeyUp(RightButton))
        {
            //Stops the player from moving (sliding due to physics)
            PlayerRigid.velocity = new Vector2(0, PlayerRigid.velocity.y);

            if (CanJump)
            {
                Player.GetComponent<SpriteRenderer>().sprite = NormalSprite;
            }

        }
        
        if (Input.GetKeyUp(LeftButton))
        {
            LeftKeyActive = false;
        }

        if (Input.GetKeyUp(RightButton))
        {
            RightKeyActive = false;

            if (WalkAnimActive)
            {
                Player.GetComponent<SpriteAnim>().PlayAnimation(0, 0.05f);

            }
        }

        if (!LeftKeyActive && !RightKeyActive)
        {
            Player.GetComponent<SpriteAnim>().StopAnimation();
        }

        if (Input.GetKeyDown(JumpButton) && CanJump)
        {
            CanJump = false;
            Debug.Log("Jumping!");
            Player.GetComponent<SpriteAnim>().StopAnimation();
            Player.GetComponent<SpriteRenderer>().sprite = JumpSprite;
            PlayerRigid.AddRelativeForce(new Vector2(0, JumpForce));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //This is basically just a way to figure out the direction the collider is relative to us.
        var dif = collision.collider.transform.position - Player.transform.position;
        var distance = dif.magnitude;
        var direction = dif / distance;
        if (direction.y < 0)
        {
            if (collision.collider.tag == "Ground")
            {
                MovementActive = true;
                Debug.Log("Entered collision with ground. Y,X=" + direction.y + " - " + direction.x);
                Player.GetComponent<SpriteRenderer>().sprite = NormalSprite;
                Debug.Log("Changed sprite to: NormalSprite");
                CanJump = true;
                Debug.Log("CanJump set to true");

            }
        } 
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        WalkAnimActive = true;
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
