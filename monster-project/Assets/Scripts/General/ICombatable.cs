using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatable
{
    public int HP { get; set; }

    public void Heal(int amount);

    public void TakeDamageFormMelee(int damage , float knockbackForce);

    public void TakeDamageFormGun(int damage);

    public void Death();

}
