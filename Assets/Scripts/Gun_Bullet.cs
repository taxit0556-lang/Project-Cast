using UnityEngine;

public class Gun_Bullet : MonoBehaviour
{
    public float BulletSpeed;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
