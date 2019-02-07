using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollsion : MonoBehaviour
{
    float hp = 5;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("obstacle"))
        {
            TakeDamageFrom(collision.gameObject);
            //you can add some particle, or effect on here
            Destroy(collision.gameObject);
        }
    }

    private void TakeDamageFrom(GameObject damagableThing)
    {
        float damage = damagableThing.GetComponent<DamagableThing>().GetDamage();
        hp -= damage;
    }

    private void Update()
    {
        if (hp <= 0) Destroy(gameObject);
    }
}
