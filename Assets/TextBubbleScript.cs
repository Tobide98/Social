using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TextBubbleScript : MonoBehaviour
{
    public bool isBadText;
    public float destroyTime;
    private bool isSpawned = false;
    // Start is called before the first frame update
    void Start()
    {
        if (isBadText) destroyTime = Random.Range(3, 5);
        else destroyTime = Random.Range(0.5f, 1f);
        var rectTrans = this.GetComponent<RectTransform>();
        rectTrans.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.8f, 4, 1f);
        isSpawned = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawned)
        {
            destroyTime -= Time.deltaTime;
        }
        if(destroyTime < 0 && isSpawned)
        {
            Destroy(this.gameObject);
            GameManagerScript.instance.spawnedBubbleList.Remove(this.gameObject);
            if (isBadText)
            {
                OnFailedTask();
            }
        }
    }

    public void OnClick()
    {
        if (isBadText)
        {
            GameManagerScript.instance.CheckMultipler();
            GameManagerScript.instance.AddScore();
            GameManagerScript.instance.InstantiateShotFX(this.transform, true);
        }
        else
        {
            GameManagerScript.instance.InstantiateShotFX(this.transform, false);
            OnFailedTask();
        }
        GameManagerScript.instance.spawnedBubbleList.Remove(this.gameObject);
        GameManagerScript.instance.gunAnim.SetTrigger("Shoot");
        GameManagerScript.instance.totalShots++;
        AudioPlayer.instance.PlayAudio("Gunshot");
        Destroy(this.gameObject);
    }

    public void OnFailedTask()
    {
        GameManagerScript.instance.ReduceLife();
        GameManagerScript.instance.multiplierTimer = -1;
        GameManagerScript.instance.multiplier = 0;
        GameManagerScript.instance.comboBar.SetActive(false);
    }
}
