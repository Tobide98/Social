using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SEMISOFT.AudioSystem {

    [System.Serializable]
    public struct AudioPathPair
    {
        public AudioEventEnum audioId;
        public string path;
    }

    public class AudioDatabase : ScriptableObject
    {
        public List<AudioPathPair> audioPathList;
    }
}