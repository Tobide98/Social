using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SEMISOFT.AudioSystem;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer instance;
    public AudioSource audioSource;
    public AudioSource BGM;

    public bool SFXon;
    public string sfxState;
    public string bgmState;

    public GameObject SFXOnBtn;
    public GameObject SFXOffBtn;
    public GameObject BGMOnBtn;
    public GameObject BGMOffBtn;

    public void Awake()
    {
        instance = this;
        AudioSystem.onAudioLoadFinished += PlayBGM;
        StartCoroutine(AudioSystem.LoadAllAudio());
    }

    private void OnDestroy()
    {
        AudioSystem.onAudioLoadFinished -= PlayBGM;
    }

    private void Start()
    {
        CheckSettings();
    }
    void PlayBGM()
    {
        //AudioEventEnum myEnum = (AudioEventEnum)Enum.Parse(typeof(AudioEventEnum), "bgm_id");
        AudioEventEnum myEnum = (AudioEventEnum)Enum.Parse(typeof(AudioEventEnum), "BGM");
        AudioSystem.PlayAudioLoop(BGM.gameObject, myEnum);
    }

    public void PlayAudio(string name)
    {
        AudioEventEnum myEnum = (AudioEventEnum)Enum.Parse(typeof(AudioEventEnum), name);
        AudioSystem.PlayAudioOneShot(audioSource.gameObject, myEnum);
    }

    public void PlayAudio(AudioEventEnum audioEvent)
    {
        AudioSystem.PlayAudioOneShot(audioSource.gameObject, audioEvent);
    }
    public void StopAudioOneShot()
    {
        audioSource.Stop();
    }

    public void ButtonClickSFX()
    {
        PlayAudio("ButtonClick");
    }

    public void TurnOffSFX()
    {
        sfxState = "off";
        PlayerPrefs.SetString("sfxState", sfxState);
        audioSource.mute = true;
        SFXOffBtn.SetActive(true);
        SFXOnBtn.SetActive(false);
    }

    public void TurnOnSFX()
    {
        sfxState = "on";
        PlayerPrefs.SetString("sfxState", sfxState);
        audioSource.mute = false;
        SFXOnBtn.SetActive(true);
        SFXOffBtn.SetActive(false);
    }

    public void TurnOffBGM()
    {
        bgmState = "off";
        PlayerPrefs.SetString("bgmState", bgmState);
        BGM.mute = true;
        BGMOffBtn.SetActive(true);
        BGMOnBtn.SetActive(false);
    }

    public void TurnOnBGM()
    {
        bgmState = "on";
        PlayerPrefs.SetString("bgmState", bgmState);
        BGM.mute = false;
        BGMOnBtn.SetActive(true);
        BGMOffBtn.SetActive(false);
    }


    public void CheckSettings()
    {
        if (PlayerPrefs.HasKey("sfxState"))
        {
            sfxState = PlayerPrefs.GetString("sfxState");
            if (sfxState.Equals("on")) TurnOnSFX();
            else if (sfxState.Equals("off")) TurnOffSFX();
        }
        else TurnOnSFX();

        if (PlayerPrefs.HasKey("bgmState"))
        {
            bgmState = PlayerPrefs.GetString("bgmState");
            if (bgmState.Equals("on")) TurnOnBGM();
            else if (bgmState.Equals("off")) TurnOffBGM();
        }
        else TurnOnBGM();
    }

}
