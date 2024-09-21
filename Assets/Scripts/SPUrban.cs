using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPUrban : MonoBehaviour
{
    // Three lists to hold the empty game objects from the hierarchy
    public List<Transform> list1 = new List<Transform>();
    public List<Transform> list2 = new List<Transform>();
    public List<Transform> list3 = new List<Transform>();

    // The first and second prefabs to be placed
    public GameObject firstPrefab;
    public GameObject secondPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Check if the prefabs and lists are assigned
        if (firstPrefab != null && secondPrefab != null && list1.Count > 0 && list2.Count > 0 && list3.Count > 0)
        {
            // List to keep track of the positions already occupied by the first prefab
            List<Transform> occupiedPositions = new List<Transform>();

            // Instantiate the first prefab at random positions from each list
            occupiedPositions.Add(InstantiatePrefabAtRandomPosition(list1));
            occupiedPositions.Add(InstantiatePrefabAtRandomPosition(list2));
            occupiedPositions.Add(InstantiatePrefabAtRandomPosition(list3));

            // Select a random position from list2 for the second prefab, excluding occupied positions
            Transform availablePosition = GetAvailablePosition(list2, occupiedPositions);
            if (availablePosition != null)
            {
                Instantiate(secondPrefab, availablePosition.position, availablePosition.rotation);
            }
            else
            {
                Debug.LogWarning("No available positions left in list2 for the second prefab.");
            }
        }
        else
        {
            Debug.LogWarning("Prefabs or lists are not assigned or are empty.");
        }
    }

    Transform InstantiatePrefabAtRandomPosition(List<Transform> positions)
    {
        // Get a random index from the list
        int randomIndex = Random.Range(0, positions.Count);

        // Instantiate the prefab at the chosen position and rotation
        Instantiate(firstPrefab, positions[randomIndex].position, positions[randomIndex].rotation);

        // Return the position occupied by this prefab
        return positions[randomIndex];
    }

    Transform GetAvailablePosition(List<Transform> positions, List<Transform> occupiedPositions)
    {
        // Create a list of available positions by excluding occupied positions
        List<Transform> availablePositions = new List<Transform>();

        foreach (Transform position in positions)
        {
            if (!occupiedPositions.Contains(position))
            {
                availablePositions.Add(position);
            }
        }

        // If there are available positions, return a random one
        if (availablePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, availablePositions.Count);
            return availablePositions[randomIndex];
        }

        // If no positions are available, return null
        return null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}