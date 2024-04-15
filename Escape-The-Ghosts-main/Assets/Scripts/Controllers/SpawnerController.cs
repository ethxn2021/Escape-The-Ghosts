using System.Collections;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public PointsCollectible pointsCollectible;
    public GameObject ghost;
    public GameObject freezeTimePowerup;
    public GameObject speedBoostPowerup;

    private int numberOfEnemies;
    private int numberOfPowerups;
    private int numberOfPointsCollectible;

    float minDistanceBetweenObjects = 10.0f; // Adjust this value based on your needs

    // Awake is called when the script instance is being loaded


    IEnumerator StartSpawning()
    {
        // Introduce a delay here (e.g., 5 seconds) before starting the spawning
        yield return new WaitForSeconds(2);

        // Start spawning ghosts and powerups repeatedly
        StartCoroutine(SpawnGhostsRepeatedly());
        //StartCoroutine(SpawnPowerupsRepeatedly());
        StartCoroutine(SpawnCollectiblesRepeatedly());
    }

    void Update()
    {
        // Update the number of enemies only when new ghosts are spawned or destroyed
        // This is more efficient than searching for all objects with the "Enemy" tag every frame
        numberOfEnemies = CountObjectsWithTag("Enemy");
        numberOfPowerups = CountObjectsWithTag("Powerup");
        numberOfPointsCollectible = CountObjectsWithTag("Collectible");

        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(StartSpawning());
        }
        

    }

    IEnumerator SpawnGhostsRepeatedly()
    {
        while (true)
        {
            if (numberOfEnemies <= 4)
            {
                SpawnGhosts();
            }
            yield return new WaitForSeconds(2);
        }
    }

    IEnumerator SpawnPowerupsRepeatedly()
    {
        while (true)
        {
            if (numberOfPowerups <= 2)
            {
                SpawnPowerups();
            }
            yield return new WaitForSeconds(2);
        }
    }

    IEnumerator SpawnCollectiblesRepeatedly()
    {
        while (true)
        {
            if (numberOfPointsCollectible <= 5)
            {
                SpawnCollectibles();
            }
            yield return new WaitForSeconds(2);
        }
    }

    void SpawnGhosts()
    {
        // Get a random position within the specified floor area
        Vector3 spawnPosition = GetRandomFloorPosition();

        if (IsPositionNotObstructed(spawnPosition, "Ghost"))
        {
            Instantiate(ghost, spawnPosition, Quaternion.identity);
        }
    }

    void SpawnPowerups()
    {
        // Get a random position within the specified floor area
        Vector3 spawnPosition = GetRandomFloorPosition();

        if (IsPositionNotObstructed(spawnPosition, "Powerup"))
        {
            // Generate a random number to decide which power-up to spawn
            int randomPowerup = Random.Range(0, 2);

            // Spawn freezeTimePowerup if randomPowerup is 0, otherwise spawn speedBoostPowerup
            GameObject powerupToSpawn = (randomPowerup == 0) ? freezeTimePowerup : speedBoostPowerup;

            Instantiate(powerupToSpawn, spawnPosition, Quaternion.identity);
        }
    }

    void SpawnCollectibles()
    {
        // Get a random position within the specified floor area
        Vector3 spawnPosition = GetRandomFloorPosition();

        if (IsPositionNotObstructed(spawnPosition, "Collectible"))
        {
            Instantiate(pointsCollectible, spawnPosition, Quaternion.identity);
        }
    }

    bool IsPositionNotObstructed(Vector3 position, string tag)
    {
        // Perform a check to see if there are any colliders within a certain radius of the spawn position
        Collider[] colliders = Physics.OverlapSphere(position, minDistanceBetweenObjects);

        foreach (var collider in colliders)
        {
            if (collider.tag == tag || collider.tag == "Player")
            {
                // If there's another object with the same tag nearby, consider the position obstructed
                return false;
            }
        }

        return true;
    }

    int CountObjectsWithTag(string tag)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);

        if (objectsWithTag != null)
        {
            return objectsWithTag.Length;
        }

        return 0;
    }

    Vector3 GetRandomFloorPosition()
    {
        // x - width
        // z - height
        // Specify the floor area
        float floorWidth = 40f;  // Adjust this based on your floor size.
        float floorLength = 40f; // Adjust this based on your floor size.

        // Get random positions within the floor area
        float randomX = Random.Range(23.1f - floorWidth / 2f, 23.1f + floorWidth / 2f);
        float randomZ = Random.Range(22.16f - floorLength / 2f, 22.16f + floorLength / 2f);

        // Ensure that the y coordinate is set to the floor's level.
        float y = 2.5f;

        return new Vector3(randomX, y, randomZ);
    }
}
