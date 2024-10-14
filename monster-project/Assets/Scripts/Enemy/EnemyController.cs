using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyBehavior
{
    Idle
}

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]

public class EnemyController : MonoBehaviour, ICombatable
{
    Rigidbody rb;
    Collider col;

    public int HP { get; set; }

    public EnemyBehavior enemyBehavior;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        SwitchBehavior(EnemyBehavior.Idle);
    }

    private void Update()
    {
        UpdateBehavior();
    }

    public void SwitchBehavior(EnemyBehavior behavior)
    {
        enemyBehavior = behavior;
        switch (enemyBehavior)
        {
            case EnemyBehavior.Idle:
                break;
        }
    }

    void UpdateBehavior()
    {
        switch (enemyBehavior)
        {
            case EnemyBehavior.Idle:
                break;
        }
    }

    public void Heal(int amount)
    {

    }

    public void TakeDamageFormMelee(int damage, float knockbackForce)
    {
        HP -= damage;
        Knockback(knockbackForce);
        if (HP <= 0)
        {
            Death();
        }
    }

    public void TakeDamageFormGun(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Death();
        }
    }

    public void Death()
    {

    }

    void Knockback(float knockbackForce)
    {
        Vector3 dir = transform.position - PlayerManager.Instance.transform.position;
        dir = dir.normalized;
        rb.AddForce(dir * knockbackForce, ForceMode.Impulse);
    }

}
