using System.IO;
using Unity.Cinemachine;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string savelocation;

    void Start()
    {
        savelocation = Path.Combine(Application.persistentDataPath, "save.json");
        string documentsPath = Path.Combine(System.Environment.GetFolderPath
                        (System.Environment.SpecialFolder.MyDocuments), "wissgame_save_path.txt");
        File.WriteAllText(documentsPath, savelocation);
        
        // Small delay to ensure scene is fully loaded
        Invoke(nameof(LoadGame), 0.1f);
    }

    public void SaveGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        CinemachineConfiner2D confiner = FindAnyObjectByType<CinemachineConfiner2D>();

        // Check if required objects exist
        if (player == null)
        {
            Debug.LogError("SaveGame: No GameObject with tag 'Player' found!");
            return;
        }

        if (confiner == null)
        {
            Debug.LogError("SaveGame: No CinemachineConfiner found in scene!");
            return;
        }

        if (confiner.BoundingShape2D == null)
        {
            Debug.LogError("SaveGame: CinemachineConfiner has no bounding shape assigned!");
            return;
        }

        SaveData data = new SaveData
        {
            playerPosition = player.transform.position,
            mapBoundary = confiner.BoundingShape2D.gameObject.name
        };

        File.WriteAllText(savelocation, JsonUtility.ToJson(data));
        Debug.Log($"Game saved to: {savelocation}");
    }

    public void LoadGame()
    {
        if (!File.Exists(savelocation))
        {
            Debug.Log("No save file found. Creating new save...");
            SaveGame();
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        CinemachineConfiner2D confiner = FindAnyObjectByType<CinemachineConfiner2D>();

        // Check if required objects exist
        if (player == null)
        {
            Debug.LogError("LoadGame: No GameObject with tag 'Player' found!");
            return;
        }

        if (confiner == null)
        {
            Debug.LogError("LoadGame: No CinemachineConfiner found in scene!");
            return;
        }

        // Load the save data
        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(savelocation));

        // Apply player position
        player.transform.position = data.playerPosition;

        // Find and apply map boundary
        GameObject boundaryObject = GameObject.Find(data.mapBoundary);
        if (boundaryObject == null)
        {
            Debug.LogError($"LoadGame: Boundary object '{data.mapBoundary}' not found in scene!");
            return;
        }

        PolygonCollider2D boundary = boundaryObject.GetComponent<PolygonCollider2D>();
        if (boundary == null)
        {
            Debug.LogError($"LoadGame: Object '{data.mapBoundary}' has no PolygonCollider2D component!");
            return;
        }

        confiner.BoundingShape2D = boundary;
        Debug.Log($"Game loaded from: {savelocation}");
    }
}