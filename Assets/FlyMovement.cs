using UnityEngine;

public class FlyMovement : MonoBehaviour
{
    public Sprite DeadSprite;
    private Rigidbody2D Rigid;
    private SpriteRenderer Rend;
    private float FlyVelocity;
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
        FlyVelocity = -1;
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
            Rigid.velocity = new Vector2(FlyVelocity, Rigid.velocity.y);
        }
    }

    private void TimeToDie()
    {
        Object.Destroy(transform.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Needed for walking baddies otherwise they'll instantly flip.
        if (collision.collider.tag == "Ground")
        {
            FlyVelocity = -FlyVelocity;
            Rend.flipX = !Rend.flipX;
        }

        if (collision.collider.tag == "Player")
        {
            if (Up.Collided())
            {
                if (!Dead)
                {
                    GetComponentInParent<SpriteAnim>().Stop();
                    GameObject.Find("Score").GetComponent<Score>().UpdateScore(100);
                    Dead = true;
                    DeathStart = Time.time;
                    Rigid.velocity = new Vector2(0, Rigid.velocity.y);
                    Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                    Rend.sprite = DeadSprite;
                }
            }
        }
    }
}