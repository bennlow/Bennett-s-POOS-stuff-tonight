using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private float horizontalMovement = 0f;
    public int health = 20;
    private float verticalPosition;
    public HealthBar healthBar;

    private Vector3 screenPoint;
    private Vector3 offset;

    void start()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;

        healthBar.SetHealth(health);
        verticalPosition = transform.position.y;
    }
    void Update()
    {
        // This ensures the player moves based on the current horizontalMovement value
        Vector3 movement = new Vector3(horizontalMovement, 0, 0);
        transform.Translate(movement * speed * Time.deltaTime);
    }



    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, verticalPosition, screenPoint.z));

    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, verticalPosition, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }



    // Call this method when the "Move Left" button is pressed down
    public void MoveLeft()
    {
        horizontalMovement = -1f;
    }

    // Call this method when the "Move Right" button is pressed down
    public void MoveRight()
    {
        horizontalMovement = 1f;
    }

    // Call this method when either the "Move Left" or "Move Right" button is released
    public void StopMoving()
    {
        horizontalMovement = 0f;
    }

    // Handle the player taking damage
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        // Update the health bar when taking damage
        if (healthBar != null)
        {
            healthBar.SetHealth(health);
        }

        // Check if health fell below zero and handle accordingly
        if (health <= 0)
        {
            Debug.Log("Player defeated");
            Destroy(gameObject);
            // Additional actions in the future maybe??
        }
    }
}
