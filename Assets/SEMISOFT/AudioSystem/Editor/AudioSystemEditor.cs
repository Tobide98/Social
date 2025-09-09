#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

namespace SEMISOFT.AudioSystem
{
    public class AudioSystemEditor : MonoBehaviour
    {
        const string AUDIODBPATH = "Assets/Resources/AudioDB.asset";
        const string AUDIOPATH = "AudioAssets/";  //relative to the Resources folder, audio assets has to be in resources folder
        const string GENERATEDEVENTPATH = "Assets/SEMISOFT/AudioSystem/AudioEventEnum.gen.cs";
        const string EVENTENUMNAME = "AudioEventEnum";//don't change this

        [MenuItem("Tools/SEMISOFT/AudioSystem/Generate Audio Events")]
        public static void GenerateAudioEvents()
        {
            DirectoryInfo dir = new DirectoryInfo("Assets/Resources/"+AUDIOPATH);
            FileInfo[] info = dir.GetFiles("*.*");

            string generatedFilePath = GENERATEDEVENTPATH; //The folder Scripts/Enums/ is expected to exist

            AudioDatabase asset = ScriptableObject.CreateInstance<AudioDatabase>();
            AssetDatabase.CreateAsset(asset, AUDIODBPATH);
            asset.audioPathList = new List<AudioPathPair>();

            using (StreamWriter streamWriter = new StreamWriter(generatedFilePath))
            {
                streamWriter.WriteLine("namespace SEMISOFT.AudioSystem\n{");
                streamWriter.WriteLine("public enum AudioEventEnum");
                streamWriter.WriteLine("{");

                for (int i = 0; i < info.Length; i++)
                {
                    if (Regex.IsMatch(info[i].Name, @".*\.(wav|mp3|ogg|aif)$"))
                    {
                        string fileName = Path.GetFileNameWithoutExtension(info[i].Name);
                        Regex toUnderscore = new Regex("[- ]+");
                        fileName = toUnderscore.Replace(fileName, "_");
                        Regex toRemove = new Regex("[^A-Za-z0-9_]");
                        fileName = toRemove.Replace(fileName, "");

                        //create a key so that when the enum is regenerated, references don't get shifted
                        var mystring = fileName;
                        MD5 md5Hasher = MD5.Create();
                        var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(mystring));
                        var ivalue = BitConverter.ToInt32(hashed, 0);

                        streamWriter.WriteLine(fileName + " = " + ivalue + ",");
                        asset.audioPathList.Add(new AudioPathPair() { audioId = (AudioEventEnum)ivalue, path = AUDIOPATH + Path.GetFileNameWithoutExtension(info[i].Name) });
                    }
                }
                streamWriter.WriteLine("}\n}");
            }
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("audio event generated");
        }
    }
}
#endif