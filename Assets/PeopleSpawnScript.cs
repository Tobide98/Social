using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleSpawnScript : MonoBehaviour
{
    public List<GameObject> peopleObjects;
    public List<GameObject> SpawnedObjects;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void OnEnable()
    {
        ClearPeople();
        SpawnAllPeople();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnAllPeople()
    {
        for (int i = 0; i < 5; i++)
        {
            InstantiateObj();
        }
    }

    public void ClearPeople()
    {
        if(SpawnedObjects.Count > 0)
        {
            for (int i = 0; i < SpawnedObjects.Count; i++)
            {
                Destroy(SpawnedObjects[i]);
            }
            SpawnedObjects.Clear();
        }
    }

    public void InstantiateObj()
    {
        var random = Random.Range(0, peopleObjects.Count);
        var obj = Instantiate(peopleObjects[random], this.transform);
        SpawnedObjects.Add(obj);
    }
}
