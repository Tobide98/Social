using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleSpawnObject : MonoBehaviour
{
    public List<GameObject> objects;
    // Start is called before the first frame update
    void Start()
    {
        RandomizeObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomizeObjects()
    {
        var random = Random.Range(0, objects.Count);
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].SetActive(false);
        }
        objects[random].SetActive(true);
    }
}
