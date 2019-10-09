/*Character Control Script - Georgios Brellas 2019
@TODO: 
- Move physics related code into FixedUpdate
- Refractor Player rigidbody look ups (Should increase performance since it'll avoid constant look ups)
  Maybe we can just look it up once at the start and store it in a variable???


*/
using UnityEngine;

public class Control : MonoBehaviour
{
    //Our Player's gameobject
    public GameObject Player;

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

    // Start is called before the first frame update
    void Start()
    {
        CanJump = true;
    }

    // Update is called once per frame
    void Update()
    {

        Player.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Clamp(Player.GetComponent<Rigidbody2D>().velocity.x, -MovementSpeed, MovementSpeed), Mathf.Clamp(Player.GetComponent<Rigidbody2D>().velocity.y, -JumpForce, JumpForce));
        if (Input.GetKeyDown(LeftButton))
        {
            LeftKeyActive = true;
            if (WalkAnimActive)
            {
                Player.GetComponent<SpriteAnim>().PlayAnimation(0, 0.05f);
            }
        }

        if (Input.GetKeyDown(LeftButton))
        {
            RightKeyActive = false;
            if (WalkAnimActive)
            {
                Player.GetComponent<SpriteAnim>().PlayAnimation(0, 0.05f);
            }
        }
        if (Input.GetKey(LeftButton))
        {
            Player.GetComponent<Rigidbody2D>().velocity = new Vector2(-MovementSpeed, Player.GetComponent<Rigidbody2D>().velocity.y);
            //Flips the sprite so that it reflects the direction the player is facing.
            Player.GetComponent<SpriteRenderer>().flipX = true;
        }
        
        if (Input.GetKey(RightButton))
        {
            Player.GetComponent<Rigidbody2D>().velocity = new Vector2(MovementSpeed, Player.GetComponent<Rigidbody2D>().velocity.y);
            //Flips the sprite so that it reflects the direction the player is facing.
            Player.GetComponent<SpriteRenderer>().flipX = false;
        }

        if (Input.GetKeyUp(LeftButton) || Input.GetKeyUp(RightButton))
        {
            Player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, Player.GetComponent<Rigidbody2D>().velocity.y);

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
            WalkAnimActive = false;
        } else
        {
            WalkAnimActive = true;
        }

        if (Input.GetKeyDown(JumpButton) && CanJump)
        {
            Debug.Log("CanJump set to false");
            CanJump = false;
            Debug.Log("Jumping!");
            Player.GetComponent<SpriteAnim>().StopAnimation();
            Player.GetComponent<SpriteRenderer>().sprite = JumpSprite;
            Debug.Log("Changed sprite to: JumpSprite");
            Player.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0, JumpForce));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var dif = collision.collider.transform.position - Player.transform.position;
        var distance = dif.magnitude;
        var direction = dif / distance;
        if (direction.y < 0)
        {
            if (collision.collider.tag == "Ground")
            {
                Debug.Log("Entered collision with ground.");
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
    }
}
