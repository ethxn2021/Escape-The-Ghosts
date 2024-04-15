using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour
{
    public enum GhostTypes { Type1, Type2 };
    public GhostTypes GhostType;

    public float currentEnemyHealth;

    private float chaseDistance = 12.0f;
    private float attackDistance = 2.0f;
    private float attackDuration = 3.0f;
    private float runAwayTime = 10.0f;
    private float wanderingTime = 5.0f;
    private float wanderInterval = 10.0f;
    private float nextWanderTime = 0.0f;

    private Transform player;
    private NavMeshAgent nav;
    private bool isRunningAway = false;
    private bool isAttacking = false;

    public GameObject freezeTimePowerup;
    public GameObject speedBoostPowerup;
    public GameObject healthPowerup;
    public GameObject nukePowerup;

    public GameObject deathSound;
    public GameObject deathParticles;
    public GameObject footsteps;



    void Start()
    {
        // Set up the references.
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        GetGhostType();
        nav.ResetPath();
    }

    private void GetGhostType()
    {
        if (GhostType == GhostTypes.Type1)
        {
            GetGhostTypeHealth();
            GetGhostTypeSpeed();
        }
        else if (GhostType == GhostTypes.Type2)
        {
            GetGhostTypeHealth();
            GetGhostTypeSpeed();
        }
    }
    private void GetGhostTypeSpeed()
    {
        if (GhostType == GhostTypes.Type1)
        {
            nav.speed = 4;
        }
        else if (GhostType == GhostTypes.Type2)
        {
            nav.speed = 7;
        }
    }


    private void GetGhostTypeHealth()
    {
        if (GhostType == GhostTypes.Type1)
        {
            currentEnemyHealth = 3;
        }
        else if (GhostType == GhostTypes.Type2)
        {
            currentEnemyHealth = 2;
        }
    }


    void Update()
    {
        CheckPowerups();

    }

    private void FixedUpdate()
    {
        try
        {
            if (currentEnemyHealth > 0 && player)
            {

                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                if (currentEnemyHealth == 1 && !isRunningAway && !isAttacking)
                {
                    StartCoroutine(RunAway());

                }
                else if (!isRunningAway && !isAttacking && distanceToPlayer <= chaseDistance)
                {
                    nav.SetDestination(player.position);

                    if (distanceToPlayer <= attackDistance)
                    {
                        StartCoroutine(Attack());
                    }
                }
                else
                {
                    if (Time.time >= nextWanderTime)
                    {
                        StartCoroutine(Wander());
                        nextWanderTime = Time.time + wanderInterval;
                    }
                }
            }
            else
            {
                StartCoroutine(Death());

            }
        }
        catch
        {
            // Nav mesh errors
        }
    }

    IEnumerator Wander()
    {
        bool shouldMove = Random.Range(0f, 1f) > 0.5f;

        if (shouldMove)
        {
            Vector3 wanderPosition = GetRandomFloorPosition();
            nav.SetDestination(wanderPosition);
        }
        else
        {
            nav.ResetPath();
        }

        yield return new WaitForSeconds(wanderingTime);
    }

    IEnumerator RunAway()
    {
        isRunningAway = true;
        Vector3 runAwayPosition = GetRandomFloorPosition();

        transform.rotation = Quaternion.LookRotation(transform.position - player.position);
        Vector3 runTo = transform.position + transform.forward * 10;

        nav.SetDestination(runTo);
        nav.SetDestination(runAwayPosition);

        yield return new WaitForSeconds(runAwayTime);

        currentEnemyHealth += 1;
        isRunningAway = false;
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        nav.ResetPath();

        Debug.Log("Attacking the player!");
        player.GetComponent<PlayerMovement>().currentHealth -= 1;

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
    }

    IEnumerator Death()
    {
        nav.enabled = false;
        transform.rotation = Quaternion.Euler(-90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        deathSound.SetActive(true);
        deathParticles.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        // Gradually lower the ghost's position very slowly
        float duration = 5.0f; // Adjust the duration for a slower descent
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, -1.0f, startPosition.z); // Adjust the y value as needed

        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Check for a low chance to spawn a powerup
        float powerupChance = Random.Range(0f, 1f);
        if (powerupChance < 0.8f) // Adjust the chance as needed
        {
            SpawnRandomPowerup(startPosition);
        }

        Destroy(gameObject);
    }


    private void SpawnRandomPowerup(Vector3 position)
    {
        position.y = 1.0f;

        // Randomly choose between four powerups
        int randomPowerupIndex = Random.Range(0, 4);

        // Instantiate the selected powerup based on the random index
        GameObject powerupPrefabToSpawn;

        switch (randomPowerupIndex)
        {
            case 0:
                powerupPrefabToSpawn = freezeTimePowerup;
                break;
            case 1:
                powerupPrefabToSpawn = speedBoostPowerup;
                break;
            case 2:
                powerupPrefabToSpawn = healthPowerup;
                break;
            case 3:
                powerupPrefabToSpawn = nukePowerup;
                break;
            default:
                // This case should not happen, but provide a default value just in case
                powerupPrefabToSpawn = freezeTimePowerup;
                break;
        }

        Instantiate(powerupPrefabToSpawn, position, Quaternion.identity);
    }


    Vector3 GetRandomFloorPosition()
    {
        float floorWidth = 40f;
        float floorLength = 40f;
        float randomX = Random.Range(-floorWidth / 2f, floorWidth / 2f);
        float randomZ = Random.Range(-floorLength / 2f, floorLength / 2f);

        float y = 0.0f;

        return new Vector3(randomX, y, randomZ);
    }

    private void CheckPowerups()
    {
        if (PowerupController.freezeTime)
        {
            nav.speed = 0f;
            StartCoroutine(ResumeAfterDelay());
        } else if (PowerupController.nuke)
        {
            Destroy(gameObject);
            ResumeAfterDelay();
        }
    }

    IEnumerator ResumeAfterDelay()
    {
        if (PowerupController.freezeTime)
        {
            yield return new WaitForSeconds(10.0f);
            GetGhostTypeSpeed();
            PowerupController.freezeTime = false;
        } else if (PowerupController.nuke) {
            yield return new WaitForSeconds(8.0f);
            PowerupController.nuke = false;
        }

    }
}
