using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootingPoint;
    public float bulletSpeed = 20.0f;
    private float bulletCooldownCounter;
    private float bulletCooldownTarget; //Can be moved to the weapon or bullet obejct

    public void fireBullet()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // Instantiate the bullet
            GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
            // Access the Bullet script component of the new bullet instance and set its speed
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            if (bulletComponent != null) // Check if the Bullet component is attached to the prefab
            {
                bulletComponent.speed = 10; // Set the bullet's speed
            }
        }

    }

    void OnMouseDown() {
        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        // Access the Bullet script component of the new bullet instance and set its speed
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null) { // Check if the Bullet component is attached to the prefab
            bulletComponent.speed = 10; // Set the bullet's speed
        }

        bulletCooldownCounter = 0;
    }

    void OnMouseDrag() {
        bulletCooldownTarget = .5f;
        if (bulletCooldownCounter >= bulletCooldownTarget)
        {
            // Instantiate the bullet
            GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
            // Access the Bullet script component of the new bullet instance and set its speed
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            if (bulletComponent != null)
            { // Check if the Bullet component is attached to the prefab
                bulletComponent.speed = 10; // Set the bullet's speed
            }

            bulletCooldownCounter = 0;
        }

        bulletCooldownCounter += Time.deltaTime;
    }


}
