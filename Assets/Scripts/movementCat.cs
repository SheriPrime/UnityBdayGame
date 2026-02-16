using UnityEngine;
using UnityEngine.Tilemaps; // REQUIRED for Tilemaps
using System.Collections.Generic;
using System.Collections;

public class movementCat : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private Animator animator;
    private bool isChopping = false;

    // Detection settings
    public float detectionRange = 1.5f; 
    public LayerMask treeLayer; 

    [Header("CRITICAL SETTINGS")]
    // 1. DRAG YOUR 'TREE' TILEMAP HERE IN THE INSPECTOR
    public Tilemap treeTilemap; 
    // 2. DRAG YOUR PLANK PREFAB HERE
    public GameObject plankPrefab; 
    
    void Start()
    {
        animator = GetComponent<Animator>();
        
        // DEBUG CHECK START
        if (treeTilemap == null) Debug.LogError("ERROR: You forgot to assign the Tree Tilemap in the Inspector!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isChopping) // Changed to GetKeyDown for cleaner one-tap chop
        {
            TryChop();
        }

        if (!isChopping)
        {
            HandleMovement();
        }
    }

    void HandleMovement()
    {
        horizontalInput = Input.GetAxis("Horizontal") * Time.deltaTime * 2f;
        verticalInput = Input.GetAxis("Vertical") * Time.deltaTime * 2f;

        animator.SetBool("right", horizontalInput > 0);
        animator.SetBool("left", horizontalInput < 0);
        animator.SetBool("up", verticalInput > 0);
        animator.SetBool("down", verticalInput < 0);

        transform.Translate(horizontalInput, verticalInput, 0);
    }
    
    void TryChop()
    {
        if (treeTilemap == null) return;

        Vector2[] directions = { Vector2.right, Vector2.left, Vector2.up, Vector2.down };
        string[] dirNames = { "right", "left", "up", "down" };
        
        for (int i = 0; i < directions.Length; i++)
        {
            // Visualize the ray in the scene view
            Debug.DrawRay(transform.position, directions[i] * detectionRange, Color.red, 1.0f);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], detectionRange, treeLayer);
            
            if (hit.collider != null)
            {
                Debug.Log($"Raycast Hit: {hit.collider.name} at distance {hit.distance}");

                // START CHOP ANIMATION
                StartChop(dirNames[i]);

                // CALCULATE TILE POSITION
                // We push the hit point slightly deeper into the tree (0.2f) to ensure we get inside the tile cell
                Vector3 hitPosition = hit.point + (directions[i] * 0.2f);
                Vector3Int cellPosition = treeTilemap.WorldToCell(hitPosition);

                TileBase tile = treeTilemap.GetTile(cellPosition);

                if (tile != null)
                {
                    Debug.Log($"Tile found at {cellPosition}. Destroying tree...");
                    StartCoroutine(WaitForAnimation(cellPosition, hit.point));
                }
                else
                {
                    Debug.LogWarning($"Hit collider, but found NO TILE at cell {cellPosition}. Try adjusting the '0.2f' nudge value.");
                }
                
                return; // Stop after finding the first tree
            }
        }
        Debug.Log("Swung axe, but didn't hit anything on the Tree Layer.");
    }

    void DestroyTree(Vector3Int startPos, Vector3 spawnPos)
    {
        // FLOOD FILL ALGORITHM
        // This finds all connected tree tiles and deletes them
        List<Vector3Int> tilesToDelete = new List<Vector3Int>();
        Queue<Vector3Int> queue = new Queue<Vector3Int>();

        queue.Enqueue(startPos);
        tilesToDelete.Add(startPos);

        int safetyLoop = 0;

        while (queue.Count > 0 && safetyLoop < 50)
        {
            Vector3Int current = queue.Dequeue();
            safetyLoop++;

            Vector3Int[] neighbors = {
                current + Vector3Int.up, current + Vector3Int.down,
                current + Vector3Int.left, current + Vector3Int.right
            };

            foreach (Vector3Int p in neighbors)
            {
                if (treeTilemap.HasTile(p) && !tilesToDelete.Contains(p))
                {
                    tilesToDelete.Add(p);
                    queue.Enqueue(p);
                }
            }
        }


        // Delete the tiles
        foreach (Vector3Int p in tilesToDelete)
        {
            treeTilemap.SetTile(p, null);
        }

        // Drop Plank
        if (plankPrefab != null)
        {
            Instantiate(plankPrefab, spawnPos, Quaternion.identity);
        }
    }

    public IEnumerator WaitForAnimation(Vector3Int startPos, Vector3 spawnPos)
    {
        Debug.Log("Waiting for chop animation to finish...");
        yield return new WaitForSeconds(1f);
        DestroyTree(startPos, spawnPos);
    }

    void StartChop(string direction)
    {
        isChopping = true;
        // Reset triggers to prevent stuck animations
        animator.ResetTrigger("chopRight");
        animator.ResetTrigger("chopLeft");
        animator.ResetTrigger("chopUp");
        animator.ResetTrigger("chopDown");

        if (direction == "right") animator.SetTrigger("chopRight");
        else if (direction == "left") animator.SetTrigger("chopLeft");
        else if (direction == "up") animator.SetTrigger("chopUp");
        else if (direction == "down") animator.SetTrigger("chopDown");
        
        Invoke("EndChop", 0.5f);
    }
    
    void EndChop()
    {
        isChopping = false;
    }
}