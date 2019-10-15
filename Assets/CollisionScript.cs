using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    private bool HasCollided = false;

    public bool Collided()
    {
        return HasCollided;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HasCollided = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        HasCollided = false;
    }
}
