using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    public Sprite DeadSprite;
    private Rigidbody2D Rigid;
    private SpriteRenderer Rend;
    private float SlimeVelocity;
    private bool FirstCollisionOver;
    private CollisionScript Up;
    private bool Dead;
    private float DeathStart;
    private float DeathEnd;

    void Start()
    {
        //Seconds until proper death...
        DeathEnd = 1;
        GetComponentInParent<SpriteAnim>().PlayAnimation(0, 0.20f);
        Rigid = GetComponentInParent<Rigidbody2D>();
        Rend = GetComponentInParent<SpriteRenderer>();
        Up = transform.GetChild(0).GetComponent<CollisionScript>();

        //Negative = left, Positive = Right (X axis)
        SlimeVelocity = -1;
    }

    void Update()
    {
        if (Dead)
        {
            if (Time.time - DeathStart > DeathEnd)
            {
                TimeToDie();
            }
        }
        else
        {
            Rigid.velocity = new Vector2(SlimeVelocity, Rigid.velocity.y);
        }
    }

    private void TimeToDie()
    {
        Object.Destroy(transform.gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Needed for walking baddies otherwise they'll instantly flip.
        if (FirstCollisionOver)
        {
            if (collision.collider.tag == "Ground")
            {
                SlimeVelocity = -SlimeVelocity;
                Rend.flipX = !Rend.flipX;
            }
        } else {
            FirstCollisionOver = true;
        }

        if (collision.collider.tag == "Player")
        {
            if (Up.Collided())
            {
                if (!Dead)
                {
                    GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<BoxCollider2D>().size.x, 0.1850187f);
                    GetComponentInParent<SpriteAnim>().StopAnimation();
                    GameObject.Find("Score").GetComponent<Score>().UpdateScore(50);
                    Dead = true;
                    DeathStart = Time.time;
                    Rigid.velocity = new Vector2(0, Rigid.velocity.y);
                    Rend.sprite = DeadSprite;
                }
            }
        }
    }
}

