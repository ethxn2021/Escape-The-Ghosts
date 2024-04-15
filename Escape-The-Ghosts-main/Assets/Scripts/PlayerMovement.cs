using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 movementDirection;
    Rigidbody playerRigidbody;
    private float speed = 10.0f;
    private float sprintSpeed = 15.0f; // Added sprint speed
    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    float horizontalInput;
    float verticalInput;
    bool isSprinting = false;

    public float maxStamina = 100.0f;
    float currentStamina;

    public Transform orientation;

    // for bomb
    public GameObject bombPrefab;
    public GameController gameController;

    private int noBomb = int.MaxValue;
    public GameObject nuke;
    public int currentHealth = 4;
    public int playerScore = 0;

    public GameObject spawnerController;

    // Use this for initialization
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.freezeRotation = true;
        currentStamina = maxStamina;
    }

    private void Update()
    {
        
        CheckPowerups();
        UnityEngine.Debug.Log(currentHealth);
        if (currentHealth == 0)
        {
            gameController.EndGame();
            gameObject.SetActive(false);
        } else if (playerScore == 30)
        {
            gameController.VictoryScreen();
        }
        else
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

            MyInput();
            Sprint();
            SpeedControl();
            dropBomb();

            if (grounded)
            {
                playerRigidbody.drag = groundDrag;
            }
            else
            {
                playerRigidbody.drag = 0;
            }
  
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Sprint input
        isSprinting = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0;
    }

    private void MovePlayer()
    {
        movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        playerRigidbody.AddForce(movementDirection.normalized * speed, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);

        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            playerRigidbody.velocity = new Vector3(limitedVel.x, playerRigidbody.velocity.y, limitedVel.z);
        }
    }

    void dropBomb()
    {
        if (noBomb > 0 && Input.GetKeyUp(KeyCode.Space))
        { // Drop bomb
            Instantiate(bombPrefab, new Vector3(transform.position.x, bombPrefab.transform.position.y, transform.position.z), bombPrefab.transform.rotation);
            noBomb--; // Decrease bomb count
        }
    }

    private void CheckPowerups()
    {
        if (PowerupController.speedBoost)
        {
            speed = 21.0f; // Set the boosted speed
            StartCoroutine(ResumeAfterDelay());
        }
        else if (PowerupController.healthPack)
        {
            currentHealth += 1;
            PowerupController.healthPack = false;
        }
        else if (PowerupController.nuke)
        {
            StartCoroutine(ShakeCamera());
            spawnerController.SetActive(false);
            nuke.SetActive(true);
            StartCoroutine(ResumeAfterDelay());
            
        }
    }

    IEnumerator ShakeCamera()
    {
        float duration = 3f; // Adjust the duration of the shake
        float magnitude = 0.5f; // Adjust the magnitude of the shake

        Vector3 originalPos = Camera.main.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;
    }

    IEnumerator ResumeAfterDelay()
    {
        if (PowerupController.speedBoost)
        {
            // Wait for a specified time (you can adjust this as needed)
            yield return new WaitForSeconds(7.0f);
            // Resume normal speed after the delay
            speed = 10.0f;
            PowerupController.speedBoost = false;
        } else if (PowerupController.nuke)
        {
            yield return new WaitForSeconds(8.0f);
            spawnerController.SetActive(true);
            nuke.SetActive(false);
            PowerupController.nuke = false;
        }

    }

    private void Sprint()
    {
        if (isSprinting)
        {
            // Sprinting consumes stamina
            currentStamina -= Time.deltaTime * 25.0f; // Adjust the sprint cost as needed

            // Apply sprint speed
            speed = sprintSpeed;
        }
        else
        {
            // Regenerate stamina
            currentStamina += Time.deltaTime * 25.0f; // Adjust the regeneration rate as needed

            // Clamp stamina within the range [0, maxStamina]
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

            // Reset speed to normal speed
            speed = 10.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            playerScore++;
            // Destroy the collectible
            Destroy(other.gameObject);

        }
    }
}
