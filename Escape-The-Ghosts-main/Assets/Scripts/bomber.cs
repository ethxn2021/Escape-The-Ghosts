using System.Collections;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    public float growthRate = 0.5f; // Rate at which the bomb grows per second
    public float explosionTime = 3f; // Time before the bomb explodes
    public GameObject explosionPrefab; // Prefab for explosion effect

    private Vector3 initialScale; // Initial scale of the bomb
    private float currentSize = 0f; // Current size of the bomb

    void Start()
    {
        // Store the initial scale of the bomb
        initialScale = transform.localScale;

        StartCoroutine(ExplodeAfterDelay());
    }

    void Update()
    {
        GrowBomb();
    }

    void GrowBomb()
    {
        // Gradually increase the size of the bomb
        currentSize += growthRate * Time.deltaTime;
        transform.localScale = initialScale * currentSize;
    }

    IEnumerator ExplodeAfterDelay()
    {
        // Wait for the specified delay before exploding
        yield return new WaitForSeconds(explosionTime);

        Explode();
    }

    void Explode()
    {
        // Find all colliders within a radius of the bomb's position
        Collider[] colliders = Physics.OverlapSphere(transform.position, currentSize);

        // Check each collider and deduct health from the player or ghost
        foreach (Collider collider in colliders)
        {
            GameObject obj = collider.gameObject;

            // Assuming Player and Ghost have scripts with health variables
            PlayerMovement playerHealth = obj.GetComponent<PlayerMovement>();
            enemyAI ghostHealth = obj.GetComponent<enemyAI>();

            if (playerHealth != null)
            {
                // Deduct health from the player
                playerHealth.currentHealth -= 1;
                Debug.Log("Player Health Deducted");
            }

            if (ghostHealth != null)
            {
                // Deduct health from the ghost
                ghostHealth.currentEnemyHealth -= 1;
                Debug.Log("Ghost Health Deducted: " + ghostHealth.currentEnemyHealth);

                // Check if ghost's health is zero and destroy the ghost
                if (ghostHealth.currentEnemyHealth <= 0)
                {
                    Destroy(obj);
                    Debug.Log("Ghost Destroyed");
                }
            }
        }

        // Instantiate explosion effect
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Destroy the bomb
        Destroy(gameObject);
    }
}
