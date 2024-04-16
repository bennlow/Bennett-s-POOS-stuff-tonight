using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    public float speed = 2.5f;
    public float cloneInterval = 3.0f;
    public int maxClones = 2;
    public int enemyHealth = 20;
    public int damageAmount = 5; // Damage taken from bullets
    private float cloneTimer;
    private static int currentClones = 0;
    public bool isOriginal = true;
    public float margin = .1f; //how far from the side of the screen an enemy should be
    private bool movingRight;


    private float bulletCooldownCounter;
    private float bulletCooldownTarget;
    public GameObject bulletPrefab;
    public Transform shootingPoint;
    public float bulletSpeed = 20.0f;


    void Start()
    {
        cloneTimer = cloneInterval;
        if (isOriginal)
        {
            transform.position = new Vector3(1000, 1000, 0); // Move original off-screen
        }

        float dummy = Random.Range(0,1);
        if (dummy > .5) { movingRight = true; }
        else { movingRight = false; }

        ShootStart();
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
            float screenWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - margin;
            float currentPosition = gameObject.transform.position.x;
            if (movingRight)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }

            if (currentPosition > screenWidth) { movingRight=false; }
            else if (currentPosition < -1*screenWidth) { movingRight=true; }

            if (Camera.main != null && transform.position.y < Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y)
            {
                currentClones--;
                Destroy(gameObject);
            }
        }

        ShootUpdate();
    }

    private void ShootStart()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        // Access the Bullet script component of the new bullet instance and set its speed
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        { // Check if the Bullet component is attached to the prefab
            bulletComponent.speed = -10f; // Set the bullet's speed
            bulletComponent.fromPlayer = false;
        }

        bulletCooldownCounter = 0;
    }

    private void ShootUpdate()
    {
        bulletCooldownTarget = 1.5f;
        if (bulletCooldownCounter >= bulletCooldownTarget)
        {
            // Instantiate the bullet
            GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
            // Access the Bullet script component of the new bullet instance and set its speed
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            if (bulletComponent != null)
            { // Check if the Bullet component is attached to the prefab
                bulletComponent.speed = -10f; // Set the bullet's speed
                bulletComponent.fromPlayer = false;
            }

            bulletCooldownCounter = 0;
        }

        bulletCooldownCounter += Time.deltaTime;
    }



    private void CloneEnemy()
    {
        GameObject clone = Instantiate(gameObject, new Vector3(0, 0, 0), Quaternion.identity);
        ShootingEnemy cloneScript = clone.GetComponent<ShootingEnemy>();
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
                float screenWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - margin;
                float randomX = Random.Range(-screenWidth, screenWidth);
                transform.position = new Vector3(randomX, Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y - margin, 0);
            }
        }
    }
}
