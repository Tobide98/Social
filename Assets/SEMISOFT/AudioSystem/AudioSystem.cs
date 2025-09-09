using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SEMISOFT.AudioSystem
{
    public class AudioSystem : MonoBehaviour
    {
        private static Dictionary<AudioEventEnum, AudioClip> audioDictionary;

        /// <summary>
        /// contains all audio clip that will be used in the audioDictionary with their respective unique event with the same name
        /// </summary>
        private static AudioDatabase audioDB;
        
        public delegate void AudioLoadFinish();
        /// <summary>
        /// the event that will be invoked when the IEnumerator LoadAllAudio() has finished loading all audio asset
        /// </summary>
        public static event AudioLoadFinish onAudioLoadFinished;

        /// <summary>
        /// Load audio file from assets listed in audioDB paths into audioDictionary, will invoke the onAudioLoadFinished event on complete
        /// </summary>
        public static IEnumerator LoadAllAudio()
        {
            audioDictionary = new Dictionary<AudioEventEnum, AudioClip>();
            audioDB = Resources.Load<AudioDatabase>("AudioDB");
            
            foreach (var pathPair in audioDB.audioPathList)
            {
                ResourceRequest req = Resources.LoadAsync<AudioClip>(pathPair.path);
                yield return req;
                AudioClip loadedAudio = req.asset as AudioClip;
                if(loadedAudio != null)
                {
                    audioDictionary.Add(pathPair.audioId, loadedAudio);
                }
                else
                {
                    Debug.LogWarning("error loading audio: " + pathPair.path);
                }
            }
            onAudioLoadFinished?.Invoke();
        }

        /// <summary>
        /// play audio once, this will use default audiosource in main camera, will override previous sound playing in the default audiosource (if any)
        /// </summary>
        /// <param name="audioEvent"></param>
        public static void PlayAudioOneShot(AudioEventEnum audioEvent)
        {
            AudioSource defaultSource = Camera.main.GetComponent<AudioSource>();
            if(defaultSource == null)
            {
                defaultSource = Camera.main.gameObject.AddComponent<AudioSource>();
            }
            defaultSource.PlayOneShot(audioDictionary[audioEvent]);
        }

        /// <summary>
        /// play audio once by using the audioSource parameter as the audio source, will override previous sound playing in it (if any)
        /// </summary>
        /// <param name="audioSource"></param>
        /// <param name="audioEvent"></param>
        public static void PlayAudioOneShot(GameObject audioSource, AudioEventEnum audioEvent)
        {
            AudioSource source = audioSource.GetComponent<AudioSource>();
            if (source == null)
            {
                source = audioSource.AddComponent<AudioSource>();
            }
            source.PlayOneShot(audioDictionary[audioEvent]);

        }

        /// <summary>
        /// play audio and loop it, this will use default audiosource in main camera, will override previous sound playing in the default audiosource (if any)
        /// </summary>
        /// <param name="audioEvent"></param>
        public static void PlayAudioLoop(AudioEventEnum audioEvent)
        {
            AudioSource defaultSource = Camera.main.GetComponent<AudioSource>();
            if (defaultSource == null)
            {
                defaultSource = Camera.main.gameObject.AddComponent<AudioSource>();
            }
            defaultSource.loop = true;
            defaultSource.clip = audioDictionary[audioEvent];
            defaultSource.Play();
        }

        /// <summary>
        /// play audio and loop it by using the audioSource parameter as the audio source, will override previous sound playing in it (if any)
        /// </summary>
        /// <param name="audioEvent"></param>
        public static void PlayAudioLoop(GameObject audioSource, AudioEventEnum audioEvent)
        {
            AudioSource source = audioSource.GetComponent<AudioSource>();
            if (source == null)
            {
                source = audioSource.AddComponent<AudioSource>();
            }
            source.loop = true;
            source.clip = audioDictionary[audioEvent];
            source.Play();
        }
    }
}