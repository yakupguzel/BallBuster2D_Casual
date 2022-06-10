using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private int number = 0;
    [SerializeField] TextMeshProUGUI numberText;
    [SerializeField] GameManager gameManager;

    List<Collider2D> colliders = new List<Collider2D>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(number.ToString()))
        {
            Explode();
        }
    }

    private void Explode()
    {
        ContactFilter2D contactFilter2D = new ContactFilter2D()
        {
            useTriggers = true
        };

        Physics2D.OverlapBox(transform.position, transform.localScale * 2, 20f, contactFilter2D, colliders);

        gameManager.BombExplosionEffect(transform.position);
        gameObject.SetActive(false);

        foreach (var coll in colliders)
        {
            if (coll.gameObject.CompareTag("Box"))
            {
                coll.GetComponent<Box>().PlayExplosionEffect();
            }
            else
            {
                coll.gameObject.GetComponent<Rigidbody2D>().AddForce(
               90 * new Vector2(UnityEngine.Random.Range(0f, 3f), UnityEngine.Random.Range(6f, 14f)),
               ForceMode2D.Force
               );
            }


           
        }
    }
}
