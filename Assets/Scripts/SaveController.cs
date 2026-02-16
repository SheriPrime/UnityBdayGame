using System.IO;
using Unity.Cinemachine;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string savelocation;
    private InventoryController inventoryController;

    void Start()
    {
        savelocation = Path.Combine(Application.persistentDataPath, "savefile.json");
        inventoryController = FindFirstObjectByType<InventoryController>(); 

        LoadGame();

        // Small delay to ensure scene is fully loaded
        // Invoke(nameof(LoadGame), 0.1f);
    }

    public void SaveGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        CinemachineConfiner2D confiner = FindAnyObjectByType<CinemachineConfiner2D>();

        SaveData data = new SaveData
        {
            playerPosition = player.transform.position,
            mapBoundary = confiner.BoundingShape2D.gameObject.name,
            inventoryItems = inventoryController.GetInventoryItems()
        };

        File.WriteAllText(savelocation, JsonUtility.ToJson(data));
        Debug.Log($"Game saved to: {savelocation}");
    }

    public void LoadGame()
    {
        if (File.Exists(savelocation))
        {
            SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(savelocation));
            GameObject.FindGameObjectWithTag("Player").transform.position = data.playerPosition;
            FindAnyObjectByType<CinemachineConfiner2D>().BoundingShape2D = GameObject.Find(data.mapBoundary).GetComponent<PolygonCollider2D>();
            inventoryController.SetInventoryItems(data.inventoryItems);
        }
        else
        {
            SaveGame(); // Create a new save file if it doesn't exist
        }
    }
}