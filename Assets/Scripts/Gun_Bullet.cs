using UnityEngine;

public class Gun_Bullet : MonoBehaviour
{
    public float Bullet_Speed;
    public float Bullet_KnockBack;
    public float Bullet_Damage;
    public float Bullet_Size;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
