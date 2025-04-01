using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityEaterHealth : EnemyHealth, IHealth
{
    private void Awake()
    {
        maxHealth = 40; // GravityEater için özel sağlık değeri
        currentHealth = maxHealth;
    }

    // bu olum durumlari her bir dusman icin farkli olabilir. O yuzden istedigimiz her bir kodu kendimizce override edeliriz.
    protected override void Die()
    {
        GetComponent<GravityEaterAI>()?.Die();
    }
}
