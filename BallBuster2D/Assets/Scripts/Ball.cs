using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private int number = 0;
    [SerializeField] TextMeshProUGUI numberText;
    [SerializeField] GameManager gameManager;
    [SerializeField] ParticleSystem mergeEffect;
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] private bool isDefaultBall;

    bool primery;

    private void Start()
    {

        numberText.text = number.ToString();
        if (isDefaultBall)
            primery = true;
    }

    void SetBallState()
    {
        primery = true;
    }

    public void ChangePrimeryState()
    {
        Invoke("SetBallState", 2f);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckBallCollision2D(collision);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckBallCollision2D(collision);
    }

    private void CheckBallCollision2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(number.ToString()) && primery)
        {
            mergeEffect.Play();
            collision.gameObject.SetActive(false);
            number += number;
            gameObject.tag = number.ToString();
            numberText.text = number.ToString();

            //Switching ball sprite

            switch (number)
            {
                case 2:
                    spriteRenderer.sprite = gameManager.ballSpritesList[0];
                    break;
                case 4:
                    spriteRenderer.sprite = gameManager.ballSpritesList[1];
                    break;
                case 8:
                    spriteRenderer.sprite = gameManager.ballSpritesList[2];
                    break;
                case 16:
                    spriteRenderer.sprite = gameManager.ballSpritesList[3];
                    break;
                case 32:
                    spriteRenderer.sprite = gameManager.ballSpritesList[4];
                    break;
                case 64:
                    spriteRenderer.sprite = gameManager.ballSpritesList[5];
                    break;
                case 128:
                    spriteRenderer.sprite = gameManager.ballSpritesList[6];
                    break;
                case 256:
                    spriteRenderer.sprite = gameManager.ballSpritesList[7];
                    break;
                case 512:
                case 1024:
                case 2048:
                    spriteRenderer.sprite = gameManager.ballSpritesList[8];
                    break;
                default:
                    break;
            }


            if (gameManager.hasTargetBall)
            {
                gameManager.CheckBallTargetCount(number);
            }

            primery = false;

            ChangePrimeryState();
        }
    }
}
