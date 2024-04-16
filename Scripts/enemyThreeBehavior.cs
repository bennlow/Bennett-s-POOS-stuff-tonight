using UnityEngine;

public class Lunker : MonoBehaviour
{
    public float fallSpeed = 2.5f;
    public float cloneInterval = 3.0f;
    public int maxClones = 5;
    public int enemyHealth = 20;
    public int damageAmount = 5; // Damage taken from bullets
    private float cloneTimer;
    private static int currentClones = 0;
    public bool isOriginal = true;
    public float margin = .1f; //how far from the side of the screen an enemy should be
    public GameObject player;

    void Start()
    {
        cloneTimer = cloneInterval;
        if (isOriginal)
        {
            transform.position = new Vector3(1000, 1000, 0); // Move original off-screen
        }
    }

    void Update()
    {
        if (isOriginal)
        {
            if (currentClones < maxClones)
            {
                cloneTimer -= Time.deltaTime;
                if (cloneTimer <= 0)
                {
                    CloneEnemy();
                    cloneTimer = cloneInterval;
                }
            }
        }
        else
        {

            Vector2 finalValue = new Vector2(player.transform.position.x,transform.position.y);
            transform.position = finalValue;

            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

            if (Camera.main != null && transform.position.y < Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y)
            {
                currentClones--;
                Destroy(gameObject);
            }
        }
    }

    private void CloneEnemy()
    {
        GameObject clone = Instantiate(gameObject, new Vector3(0, 0, 0), Quaternion.identity);
        Lunker cloneScript = clone.GetComponent<Lunker>();

        if (cloneScript != null)
        {
            cloneScript.isOriginal = false;
            cloneScript.SetRandomStartPosition();
            currentClones++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOriginal) // Ensure this is a clone
        {
            if (collision.CompareTag("Player"))
            {
                PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(damageAmount);
                }
                Destroy(gameObject); // Destroy this enemy
            }
            else if (collision.CompareTag("Bullet")) // Check collision with bullet
            {

                Bullet bulletComponent = collision.gameObject.GetComponent<Bullet>();
                if (bulletComponent.fromPlayer)
                {
                    // Reduce health by the damage amount
                    enemyHealth -= damageAmount;

                    // Check if health is less or equal to zero
                    if (enemyHealth <= 0)
                    {
                        currentClones--;
                        Destroy(gameObject); // Destroy this enemy
                    }
                    // Destroy the bullet upon collision
                    Destroy(collision.gameObject);
                }
            }
        }
    }

    public void SetRandomStartPosition()
    {
        if (!isOriginal)
        {
            if (Camera.main != null)
            {
                float startX = player.transform.position.x;
                transform.position = new Vector3(startX, Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y, 0);
            }
        }
    }
}
