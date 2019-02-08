using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(AudioSource))]
public class CharacterCollsion : MonoBehaviour
{
    float hp = 5;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamageFrom(collision.gameObject);
            audioSource.PlayOneShot(AudioClipLibrary.GetInstance().GetAudioFromLibrary("Collsion"));
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
        if (hp <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        //TODO: death and Game Over UI
    }
}