using UnityEngine;

public class DestructionCollider : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject.Destroy(collision.collider.gameObject);
    }
}
