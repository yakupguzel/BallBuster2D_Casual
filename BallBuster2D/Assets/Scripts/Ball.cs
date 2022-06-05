using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ball : MonoBehaviour
{
    [SerializeField] private int number = 0;
    [SerializeField] TextMeshProUGUI numberText;
    [SerializeField] GameManager gameManager;
    [SerializeField] ParticleSystem mergeEffect;
    [SerializeField] SpriteRenderer spriteRenderer;

    private void Start()
    {

        numberText.text = number.ToString();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(number.ToString()))
        {
            mergeEffect.Play();
            collision.gameObject.SetActive(false);
            number += number;
            gameObject.tag = number.ToString();
            numberText.text = number.ToString();
        }
    }
}
