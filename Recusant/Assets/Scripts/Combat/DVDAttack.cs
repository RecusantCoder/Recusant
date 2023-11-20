using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DVDAttack : MonoBehaviour
{
    public float speed = 5f; // Adjust the speed as needed
    private Vector2 direction;
    private int attackDamage = 300;
    public Color[] colors;
    public SpriteRenderer visualRenderer;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        IgnoreBulletCollisions();

        // Set a random 45-degree direction
        direction = new Vector2(RandomSign(), RandomSign()).normalized;
        
        SetRandomColor();
    }

    private void Update()
    {
        // Move the object based on the chosen direction and speed
        transform.Translate(direction * speed * Time.deltaTime);

        // Check for collisions with screen edges
        CheckScreenEdges();
        
        // Check if the object is out of the camera's view
        if (!IsVisible())
        {
            // Destroy the object if it's no longer visible
            Destroy(gameObject);
        }
    }
    
    int RandomSign()
    {
        return (Random.Range(0, 2) * 2) - 1; // Returns either 1 or -1 randomly
    }
    
    void SetRandomColor()
    {
        // Set a random color from the array
        if (colors.Length > 0)
        {
            Color randomColor = colors[Random.Range(0, colors.Length)];
            visualRenderer.color = randomColor;
        }
    }
    
    bool IsVisible()
    {
        // Check if the object is visible in the camera's view
        Renderer renderer = visualRenderer.GetComponent<Renderer>();
        return renderer.isVisible;
    }

    // Method to perform the attack and update EnemyController for each enemy.
    void CheckScreenEdges()
    {
        // Get the object's position in world space
        Vector2 worldPosition = transform.position;

        // Convert the world position to screen space
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        // Check for collisions with screen edges
        if (screenPosition.x < 0 || screenPosition.x > Screen.width)
        {
            direction.x *= -1; // Reverse horizontal direction
            SetRandomColor();
        }

        if (screenPosition.y < 0 || screenPosition.y > Screen.height)
        {
            direction.y *= -1; // Reverse vertical direction
            SetRandomColor();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    { 
        //Debug.Log("Collided with " + other.name);
        if (other.transform.tag == "Enemy")
        {
            EnemyStats enemyStats = other.gameObject.GetComponent<EnemyStats>();
            enemyStats.wasProcessed = true;
            enemyStats.TakeDamage(attackDamage);
        }

    }
    
    void IgnoreBulletCollisions()
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("111");
        Collider2D projectileCollider = GetComponent<Collider2D>();
        foreach (GameObject obj in objectsWithTag)
        {
            Collider2D collider = obj.GetComponent<Collider2D>();
            if (collider != null)
            {
                Physics2D.IgnoreCollision(projectileCollider, collider);
            }
        }
    }
    
    private void OnDestroy()
    {
        PlayerManager.instance.player.GetComponent<Shooting>().directionalVectorDisruptorComponent.ProjectileDestroyed();
    }
}
