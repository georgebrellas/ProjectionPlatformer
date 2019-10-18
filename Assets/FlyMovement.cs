using UnityEngine;

public class FlyMovement : MonoBehaviour
{
    private Rigidbody2D Rigid;
    private SpriteRenderer Rend;
    private float FlyVelocity;

    void Start()
    {
        this.GetComponentInParent<SpriteAnim>().PlayAnimation(0, 0.10f);
        Rigid = this.GetComponentInParent<Rigidbody2D>();
        Rend = this.GetComponentInParent<SpriteRenderer>();
        //Negative = left, Positive = Right (X axis)
        FlyVelocity = -2;
    }

    void Update()
    {
        Rigid.velocity = new Vector2(FlyVelocity, Rigid.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            FlyVelocity = -FlyVelocity;
            Rend.flipX = !Rend.flipX;
        }
    }
}