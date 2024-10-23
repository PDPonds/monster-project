using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 dir;
    float bulletSpeed;
    int damage;

    public void SetupBullet(Vector3 dir, float bulletSpeed, int damage, float duration)
    {
        this.dir = dir;
        this.bulletSpeed = bulletSpeed;
        this.damage = damage;
        Destroy(gameObject, duration);

    }

    private void Update()
    {
        transform.Translate(dir * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

    }

}
