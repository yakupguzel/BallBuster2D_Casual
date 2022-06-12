using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("---- Level Settings")]
    public Sprite[] ballSpritesList;
    [SerializeField] private GameObject[] balls;
    [SerializeField] private TextMeshProUGUI remainingBallText;

    int remainingBallCount;
    int ballPoolIndex;

    [Header("---- Bomb Explosion Effect")]
    [SerializeField] private ParticleSystem bombExplosionEffect;
    [Header("---- Box Explosion Effect")]
    [SerializeField] private ParticleSystem[] boxExplosionEffects;
    int boxExplosionEffectIndex = 0;

    [Header("---- Ball Throw Mechanism")]
    [SerializeField] private GameObject ballThrower;
    [SerializeField] private GameObject ballSpawnPosition;
    [SerializeField] private GameObject nextBallPosition;

    [Header("---- Mission Settings")]
    [SerializeField] private RectTransform missionArea;
    [SerializeField] private List<TargetUI> targetsUI;
    [SerializeField] private List<Target> targets;

    int targetBallValue, targetBoxValue;
    internal bool hasTargetBall;
    bool hasTargetBox;
    Image ballTargetComplated, boxTargetComplated;


    GameObject chosenBall;

    AudioManager audioManager;

    bool loadNextBall = false;
    bool onFirstAttempt = false;

    
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void Start()
    {
        remainingBallCount = balls.Length;
        remainingBallText.text = remainingBallCount.ToString();
        onFirstAttempt = true;
        LoadNextBall();
        InitializeMissions();
    }

    private void InitializeMissions()
    {
        if (targets.Count > 0)
        {
            missionArea.sizeDelta = new Vector2(missionArea.sizeDelta.x * targets.Count, missionArea.sizeDelta.y);

            for (int i = 0; i < targets.Count; i++)
            {
                targetsUI[i].targetImage.sprite = targets[i].targetSprite;
                targetsUI[i].targetValueText.text = targets[i].targetValue.ToString();
                targetsUI[i].missionComplateImage.sprite = targets[i].missionComplateImage;

                if (targets[i].targetType == TargetTypes.Ball)
                {
                    hasTargetBall = true;
                    targetBallValue = targets[i].targetValue;
                    ballTargetComplated = targetsUI[i].missionComplateImage;
                }
                else if (targets[i].targetType == TargetTypes.Box)
                {
                    hasTargetBox = true;
                    targetBoxValue = targets[i].targetValue;
                    boxTargetComplated = targetsUI[i].missionComplateImage;
                }

            }
        }
        else
        {
            missionArea.gameObject.SetActive(false);
        }


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
            audioManager.PlayBallThrowSound();
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
        audioManager.PlayBombExplosionSound();
        bombExplosionEffect.Play();


    }

    public void BoxExplosionEffect(Vector2 boxPosition)
    {
        boxExplosionEffects[boxExplosionEffectIndex].gameObject.transform.position = boxPosition;
        boxExplosionEffects[boxExplosionEffectIndex].gameObject.SetActive(true);
        audioManager.PlayBoxExplosionSound();
        boxExplosionEffects[boxExplosionEffectIndex].Play();

        if (hasTargetBox)
        {
            targetBoxValue--;
            if (targetBoxValue == 0)
            {
                boxTargetComplated.gameObject.SetActive(true);
                // Play complated audio
            }
        }


        if (boxExplosionEffectIndex == boxExplosionEffects.Length - 1)

            boxExplosionEffectIndex = 0;
        else
            boxExplosionEffectIndex++;
    }

    internal void CheckBallTargetCount(int ballValue)
    {
        if (targetBallValue==ballValue)
        {
            ballTargetComplated.gameObject.SetActive(true);
        }
    }
}
