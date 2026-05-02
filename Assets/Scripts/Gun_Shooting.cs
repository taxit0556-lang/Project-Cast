using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Gun_Shooting : MonoBehaviour
{
   [Header("Bullet Vars")]
    [SerializeField] private GameObject CurrentBullet;
    [SerializeField] private Transform Gun;
    [SerializeField] private Transform ShootPoint;

    public List<GameObject> Bullets = new List<GameObject>();

    Vector2 direction;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GunShooting();
        }
    }

    public void GunShooting()
    {
        CurrentBullet = Bullets[0];

        GameObject BulletIns = Instantiate(CurrentBullet, ShootPoint.position, ShootPoint.rotation);
        BulletIns.GetComponent<Rigidbody2D>().AddForce(BulletIns.transform.right * BulletIns.GetComponent<Gun_Bullet>().Bullet_Speed);

        Bullets.Insert(5,  Bullets[0]);
        Bullets.Remove(Bullets[0]);
    }

}