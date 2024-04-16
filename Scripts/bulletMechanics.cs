using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10.0f;
    public bool fromPlayer = true;

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    // Destroy bullet when it goes off-screen
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
