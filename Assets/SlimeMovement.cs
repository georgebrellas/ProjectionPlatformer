using UnityEngine.UI;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    private Rigidbody2D Rigid;
    private SpriteRenderer Rend;
    private float SlimeVelocity;
    private bool FirstCollisionOver;
    private CollisionScript Up;
    private Text ScoreText;
    private int ScoreNum;
    void Start()
    {
        ScoreText = 
        ScoreNum = int.Parse(this.GetComponentInParent<Text>().text);

        this.GetComponentInParent<SpriteAnim>().PlayAnimation(0, 0.20f);
        Rigid = this.GetComponentInParent<Rigidbody2D>();
        Rend = this.GetComponentInParent<SpriteRenderer>();
        Up = transform.GetChild(0).GetComponent<CollisionScript>();
        //Negative = left, Positive = Right (X axis)
        SlimeVelocity = -1;
    }

    void Update()
    {
        Rigid.velocity = new Vector2(SlimeVelocity, Rigid.velocity.y);

        if (Up.Collided())
        {

        }
    }

    private void UpdateScore(int AddedScore)
    {
        Debug.Log("Adding " + AddedScore);
        ScoreNum += AddedScore;
        ScoreText.text = ScoreNum.ToString();
        Debug.Log("Current score:" + ScoreNum);
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
    }
}

