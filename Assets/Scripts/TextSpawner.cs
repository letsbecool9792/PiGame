using System.Collections;
using UnityEngine;
using TMPro;

public class TextSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject textPrefab;        // Prefab with TextMeshPro component
    public Transform circleObject;       // The circle object in the scene

    [Header("Spawn Settings")]
    public float spawnRate = 0.5f;       // How many seconds between spawns
    public float moveSpeed = 3.0f;       // How fast text moves
    public float destroyDistance = 10f;  // Distance from center to destroy (fallback)

    private Camera mainCamera;
    private Vector2 screenCenter;

    private void Start()
    {
        mainCamera = Camera.main;
        screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        StartCoroutine(SpawnText());
    }

    private IEnumerator SpawnText()
    {
        while (true)
        {
            // Spawn a new text object
            GameObject textObj = Instantiate(textPrefab);
            
            // Set position to screen center
            textObj.transform.position = mainCamera.ScreenToWorldPoint(new Vector3(screenCenter.x, screenCenter.y, 10));
            textObj.transform.position = new Vector3(textObj.transform.position.x, textObj.transform.position.y, 0);
            
            // Set random digit text
            TextMeshPro tmp = textObj.GetComponent<TextMeshPro>();
            if (tmp != null)
            {
                tmp.text = Random.Range(0, 10).ToString();
            }
            
            // Add movement component
            textObj.AddComponent<TextMover>().Initialize(Random.insideUnitCircle.normalized, moveSpeed, circleObject, destroyDistance);
            
            // Wait before spawning next
            yield return new WaitForSeconds(spawnRate);
        }
    }
}

public class TextMover : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private Transform circleObject;
    private float destroyDistance;
    private PiGameManager gameManager;

    public void Initialize(Vector2 dir, float spd, Transform circle, float maxDistance)
    {
        direction = dir;
        speed = spd;
        circleObject = circle;
        destroyDistance = maxDistance;

        gameManager = FindObjectOfType<PiGameManager>();
        
        // Add collider to text if not already present
        if (GetComponent<BoxCollider2D>() == null)
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform == circleObject)
        {
            // Get the digit from this TextMeshPro
            TextMeshPro tmp = GetComponent<TextMeshPro>();
            if (tmp != null && gameManager != null)
            {
                // Check if this is the correct digit
                gameManager.CheckDigit(tmp.text[0]);
            }
            
            // Destroy the text object
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Move in the random direction
        transform.Translate(direction * speed * Time.deltaTime);
        
        // Check if too far from center (fallback destroy mechanism)
        if (Vector3.Distance(Vector3.zero, transform.position) > destroyDistance)
        {
            Destroy(gameObject);
        }
        
        // Check if touching the circle
        // if (circleObject != null)
        // {
        //     Collider2D circleCollider = circleObject.GetComponent<Collider2D>();
        //     if (circleCollider != null)
        //     {
        //         Collider2D textCollider = GetComponent<Collider2D>();
        //         if (textCollider.IsTouching(circleCollider))
        //         {
        //             Destroy(gameObject);
        //         }
        //     }
        // }
    }
}