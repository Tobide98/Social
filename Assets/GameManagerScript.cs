using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameManagerScript : MonoBehaviour
{
    [Header("Gameobject")]
    public List<GameObject> rightBubbleList;
    public List<GameObject> leftBubbleList;
    public List<GameObject> rightBaitBubbleList;
    public List<GameObject> leftBaitBubbleList;

    public List<GameObject> spawnedBubbleList;
    public List<Animator> lifeList;
    public GameObject gameoverOverlay;
    public GameObject gameplayOverlay;
    public GameObject comboBar;
    public PlayerScript playerScript;
    public GameObject correctShotFX;
    public GameObject wrongShotFX;
    public ParticleSystem muzzleFx;
    public AdsManager adsManager;

    [Header("Game Related")]
    public Transform rightTransformArea;
    public Transform leftTransformArea;
    public Vector3 spawnPos;

    public int score;
    public int totalShots;
    public int life;

    public float spawnTime;
    public float spawnTimeMax = 3f;

    public float spawnTimeBait;
    public float spawnTimeBaitMax = 3f;
    public float shuffleTimer;

    public int multiplier;
    public float multiplierTime;
    public float multiplierTimer;
    public int highestMultiplier;

    public bool gameStarted;
    public Animator gunAnim;
    public int unityAdsCounter;

    [Header("UI Related")]
    public Text scoreText;
    public Text scoreTextGameover;
    public Text currMultiplerText;
    public Text highestScoreText;
    public Text highestComboText;
    public Text totalShotsText;
    public TextMeshProUGUI gameoverContext;
    public Image multiplierFill;
    public Animator gameoverAnim;
    public GameObject gameoverNoticeText;



    public static GameManagerScript instance;
    // Start is called before the first frame update
    void Start()
    {
        life = -1;
        unityAdsCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckSpawnTime();
        ComboUpdate();
        if (gameStarted)
        {
            shuffleTimer += Time.deltaTime;
        }
        if(shuffleTimer >= 10 && spawnedBubbleList.Count == 3)
        {
            ShuffleBubble();
            shuffleTimer = 0;
        }
        if (life == 0) Gameover();
    }

    public void ComboUpdate()
    {
        if (multiplier >= 1)
        {
            multiplierTimer -= Time.deltaTime;
            multiplierFill.fillAmount = multiplierTimer / multiplierTime;
        }
        
        if (multiplierTimer < 0) multiplier = 0;
        
        if (multiplier >= 1)
        {
            currMultiplerText.text =  multiplier.ToString()+"x" ;
            comboBar.SetActive(true);
        }
        else comboBar.SetActive(false);
    }

    public void CheckSpawnTime()
    {
        if (spawnTime > 0)
        {
            spawnTime -= Time.deltaTime;
        }
        else if (spawnTime < 0 && gameStarted)
        {
            SpawnTextBubble();
        }

        if (spawnTimeBait > 0)
        {
            spawnTimeBait -= Time.deltaTime;
        }
        else if (spawnTimeBait < 0 && gameStarted)
        {
            SpawnBaitTextBubble();
        }
    }

    public void StartGame()
    {
        score = 0;
        spawnTime = 3;
        spawnTimeMax = 3f;
        life = 3;
        spawnTimeBait = 3;
        spawnTimeBaitMax = 3f;
        multiplierTimer = 3f;
        multiplier = 0;
        highestMultiplier = 0;
        totalShots = 0;
        shuffleTimer = 0f;
        unityAdsCounter++;
        ResetUIState();
        ClearSpawnedBubble();
        gameStarted = true;
        gameplayOverlay.SetActive(true);
    }
    public void ResetUIState()
    {
        scoreText.text = score.ToString();
        for (int i = 0; i < lifeList.Count; i++)
        {
            lifeList[i].SetTrigger("Default");
        }
        comboBar.SetActive(false);
        gunAnim.SetTrigger("Normal");
        gameoverAnim.SetTrigger("Default");
        gameoverNoticeText.SetActive(false);
    }

    public void SpawnTextBubble()
    {
        var randomSpawn = Random.Range(0, 2);
        Debug.Log("SpawnIdx: " + randomSpawn);
        SetSpawnTime();
        float randomX = Random.Range(-50, 50);
        float randomY = Random.Range(-300, 400);
        if (randomSpawn.Equals(1))
        {
            var random = Random.Range(0, rightBubbleList.Count);
            spawnPos = new Vector3(rightTransformArea.position.x + randomX, rightTransformArea.position.y + randomY, 0);
            var bubbleObj = Instantiate(rightBubbleList[random], rightTransformArea);
            spawnedBubbleList.Add(bubbleObj);
            bubbleObj.transform.position = spawnPos;
        }
        else if(randomSpawn.Equals(0))
        {
            var random = Random.Range(0, leftBubbleList.Count);
            spawnPos = new Vector3(leftTransformArea.position.x + randomX, leftTransformArea.position.y + randomY, 0);
            var bubbleObj = Instantiate(leftBubbleList[random], leftTransformArea);
            spawnedBubbleList.Add(bubbleObj);
            bubbleObj.transform.position = spawnPos;
        }
    }

    public void SpawnBaitTextBubble()
    {
        var randomSpawn = Random.Range(0, 2);
        Debug.Log("SpawnIdx: " + randomSpawn);
        SetBaitSpawnTime();
        float randomX = Random.Range(-50, 50);
        float randomY = Random.Range(-300, 400);
        if (randomSpawn.Equals(1))
        {
            var random = Random.Range(0, rightBaitBubbleList.Count);
            spawnPos = new Vector3(rightTransformArea.position.x + randomX, rightTransformArea.position.y + randomY, 0);
            var bubbleObj = Instantiate(rightBaitBubbleList[random], rightTransformArea);
            spawnedBubbleList.Add(bubbleObj);
            bubbleObj.transform.position = spawnPos;
        }
        else if (randomSpawn.Equals(0))
        {
            var random = Random.Range(0, leftBaitBubbleList.Count);
            spawnPos = new Vector3(leftTransformArea.position.x + randomX, leftTransformArea.position.y + randomY, 0);
            var bubbleObj = Instantiate(leftBaitBubbleList[random], leftTransformArea);
            spawnedBubbleList.Add(bubbleObj);
            bubbleObj.transform.position = spawnPos;
        }
    }

    public void SetSpawnTime()
    {
        if (spawnTimeMax > 0.3f) spawnTimeMax *= 0.9f;
        else spawnTimeMax = Random.Range(1f, 2.5f);
        spawnTime = Random.Range(0.5f, spawnTimeMax);
    }

    public void SetBaitSpawnTime()
    {
        if (spawnTimeBaitMax > 0.5f) spawnTimeBaitMax *= 0.95f;
        else spawnTimeBaitMax = Random.Range(3, 5);
        spawnTimeBait = Random.Range(2, spawnTimeBaitMax);
    }

    public void AddScore()
    {
        score += multiplier*1;
        scoreText.text = score.ToString();
    }

    public void ReduceLife()
    {
        life--;
        lifeList[life].SetTrigger("Died");
    }

    public void InstantiateShotFX(Transform pos, bool isCorrect)
    {
        if (isCorrect)
        {
            var particleObj = Instantiate(correctShotFX, pos.parent);
            particleObj.transform.position = pos.position;
        }
        else
        {
            var particleObj = Instantiate(wrongShotFX, pos.parent);
            particleObj.transform.position = pos.position;
        }
        muzzleFx.Play();
    }


    public void Gameover()
    {
        gameStarted = false;
        gunAnim.SetTrigger("Lose");
        gameoverNoticeText.SetActive(true);
        RandomizeGameoverContext();
        life = -1;
        if (score >= playerScript.highestScore)
        {
            playerScript.highestScore = score;
            playerScript.SavePlayerProfile();
        }
        scoreTextGameover.text = score.ToString();
        highestScoreText.text = playerScript.highestScore.ToString();
        totalShotsText.text = totalShots.ToString();
        highestComboText.text = highestMultiplier.ToString()+"x";
        ClearSpawnedBubble();
        StartCoroutine(GameoverHelper());
    }

    IEnumerator GameoverHelper()
    {
        yield return new WaitForSeconds(1.3f);
        gameoverOverlay.SetActive(true);
    }

    public void RandomizeGameoverContext()
    {
        var random = Random.Range(0,5);
        switch (random)
        {
            case 0: gameoverContext.text = '\u0022' + "Too… Much… Stuff…" + '\u0022'; break; 
            case 1: gameoverContext.text = "(starts updating her RinkuIn)"; break; 
            case 2: gameoverContext.text = "(inhuman groaning noises)"; break; 
            case 3: gameoverContext.text = '\u0022' + "I need to get a back surgery for how hard I carried…" + '\u0022'; break; 
            case 4: gameoverContext.text = '\u0022' + "Time to put on my clown makeup yet again..." + '\u0022'; break; 
            case 5: gameoverContext.text = '\u0022' + "OK Googaru: how much is the average intern salary?" + '\u0022'; break; 
        }
    }

    public void ShuffleBubble()
    {
        Debug.Log("SHUFFLE");
        for (int i = 0; i < spawnedBubbleList.Count; i++)
        {
            float randomX = Random.Range(-200, 200);
            float randomY = Random.Range(-400, 400);
            var parentTrans = spawnedBubbleList[i].transform.parent;
            spawnPos = new Vector3(parentTrans.position.x + randomX, parentTrans.position.y + randomY, 0);
            spawnedBubbleList[i].transform.position = spawnPos;
            spawnedBubbleList[i].transform.DOShakePosition(1f, 20, 10, 90, false, true);
        }
        //AudioPlayer.instance.PlayAudio("Glitch");
        //shuffleVfx.Play();
    }

    public void RestartGame()
    {
        gameoverAnim.SetTrigger("Close");
        gameoverNoticeText.SetActive(false);
        StartCoroutine(RestartGameHelper());
    }

    IEnumerator RestartGameHelper()
    {
        yield return new WaitForSeconds(0.2f);
        if (unityAdsCounter >= 3)
        {
            adsManager.WatchInterstitial(StartGame);
            unityAdsCounter = 0;
        }
        else StartGame();
        gameoverOverlay.SetActive(false);
    }

    public void ClearSpawnedBubble()
    {
        if (spawnedBubbleList.Count > 0)
        {
            for (int i = 0; i < spawnedBubbleList.Count; i++)
            {
                Destroy(spawnedBubbleList[i].gameObject);
            }
            spawnedBubbleList.Clear();
        }
    }

    public void CloseGame()
    {
        ClearSpawnedBubble();
        life = -1;
        gameplayOverlay.SetActive(false);
        gameoverOverlay.SetActive(false);
        if (unityAdsCounter >= 3)
        {
            adsManager.WatchVideoAds();
            unityAdsCounter = 0;
        }
    }

    public void CheckMultipler()
    {
        multiplier++;
        if (multiplier == 1) multiplierTimer = 4f;
        if (multiplier < 10)
        {
            multiplierTimer = 6f;
            multiplierTimer -= (multiplier * 0.1f);
        }
        else multiplierTimer = 4f;
        if (highestMultiplier < multiplier) highestMultiplier = multiplier;
        multiplierTime = multiplierTimer;
    }

    private void OnEnable()
    {
        instance = this;

    }

    private void OnDisable()
    {
        instance = null;
    }
}
