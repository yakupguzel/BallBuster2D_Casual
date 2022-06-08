using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Level Settings")]
    public Sprite[] ballSpritesList;
    [SerializeField] private GameObject[] balls;
    [SerializeField] private TextMeshProUGUI remainingBallText;

    int remainingBallCount;
    int ballPoolIndex;

    [Header("Bomb Object")]
    [SerializeField] private ParticleSystem bombExplosionEffect;

    [Header("Ball Throw Mechanism")]
    [SerializeField] private GameObject ballThrower;
    [SerializeField] private GameObject ballSpawnPosition;
    [SerializeField] private GameObject nextBallPosition;
    GameObject chosenBall;

    bool loadNextBall = false;
    bool onFirstAttempt = false;

    private void Start()
    {
        remainingBallCount = balls.Length;
        remainingBallText.text = remainingBallCount.ToString();
        onFirstAttempt = true;
        LoadNextBall();
    }

    private void Update()
    {
        MoveBallThrower();
    }

    void LoadNextBall()
    {

        if (onFirstAttempt)
        {
            balls[ballPoolIndex].transform.SetParent(ballThrower.transform);
            balls[ballPoolIndex].transform.position = ballSpawnPosition.transform.position;
            balls[ballPoolIndex].SetActive(true);

            chosenBall = balls[ballPoolIndex];

            ballPoolIndex++;

            balls[ballPoolIndex].transform.position = nextBallPosition.transform.position;
            balls[ballPoolIndex].SetActive(true);

            remainingBallText.text = remainingBallCount.ToString();
            onFirstAttempt = false;
        }
        else
        {
            if (balls.Length != 0)
            {
                balls[ballPoolIndex].transform.SetParent(ballThrower.transform);
                //balls[ballPoolIndex].transform.position = ballSpawnPosition.transform.position;
                balls[ballPoolIndex].transform.position = Vector3.Lerp(nextBallPosition.transform.position, ballSpawnPosition.transform.position, 450 * Time.deltaTime);
                balls[ballPoolIndex].SetActive(true);

                chosenBall = balls[ballPoolIndex];

                remainingBallCount--;
                remainingBallText.text = remainingBallCount.ToString();

                if (ballPoolIndex == balls.Length - 1)
                {
                    Debug.Log("Gameover");
                }
                else
                {
                    ballPoolIndex++;
                    balls[ballPoolIndex].transform.position = nextBallPosition.transform.position;
                    balls[ballPoolIndex].SetActive(true);
                }
            }
        }


    }

    private void MoveBallThrower()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    Vector2 myMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    ballThrower.transform.position = Vector2.MoveTowards(ballThrower.transform.position, new Vector2(myMousePosition.x, ballThrower.transform.position.y), 30 * Time.deltaTime);
                }
            }


        }

        if (Input.GetMouseButtonUp(0))
        {
            chosenBall.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            chosenBall.transform.SetParent(null);
            chosenBall.GetComponent<Ball>().ChangePrimeryState();
            loadNextBall = true;
        }

        if (loadNextBall)
        {
            loadNextBall = false;
            Invoke("LoadNextBall", .3f);
        }
    }

    public void BombExplosionEffect(Vector2 bombPosition)
    {
        bombExplosionEffect.gameObject.transform.position = bombPosition;
        bombExplosionEffect.gameObject.SetActive(true);
        bombExplosionEffect.Play();
    }
}
