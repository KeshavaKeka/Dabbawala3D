using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPUrban : MonoBehaviour
{
    public List<Transform> list1 = new List<Transform>();
    public List<Transform> list2 = new List<Transform>();
    public List<Transform> list3 = new List<Transform>();

    // The prefab to be placed at the positions
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        if (prefab != null && list1.Count > 0 && list2.Count > 0 && list3.Count > 0)
        {
            // Instantiate prefab at random positions from each list
            InstantiatePrefabAtRandomPosition(list1);
            InstantiatePrefabAtRandomPosition(list2);
            InstantiatePrefabAtRandomPosition(list3);
        }
        else
        {
            Debug.LogWarning("Prefab or lists are not assigned or are empty.");
        }
    }

    void InstantiatePrefabAtRandomPosition(List<Transform> positions)
    {
        // Get a random index from the list
        int randomIndex = Random.Range(0, positions.Count);

        // Instantiate the prefab at the chosen position and rotation
        Instantiate(prefab, positions[randomIndex].position, positions[randomIndex].rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
