using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapFarming : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap soilTilemap;   // dirt area tilemap
    public Tilemap cropTilemap;   // crops appear here

    [Header("Crop")]
    public CropDefinition wheatCrop;

    [Header("Player")]
    public Transform player;      // assign your player transform
    public KeyCode interactKey = KeyCode.E;

    [Header("Debug")]
    public bool verboseLogs = true;

    // Crop state per cell
    private class CropState
    {
        public CropDefinition def;
        public int stage;
        public float nextStageTime;
    }

    private Dictionary<Vector3Int, CropState> crops = new Dictionary<Vector3Int, CropState>();

    void Start()
    {
        if (verboseLogs)
        {
            Debug.Log("[Farming] TilemapFarming started.");
            Debug.Log($"[Farming] soilTilemap: {(soilTilemap ? soilTilemap.name : "NULL")}, cropTilemap: {(cropTilemap ? cropTilemap.name : "NULL")}");
            Debug.Log($"[Farming] wheatCrop: {(wheatCrop ? wheatCrop.cropId : "NULL")}, player: {(player ? player.name : "NULL")}");
        }

        if (!soilTilemap) Debug.LogError("[Farming] soilTilemap is NOT assigned!");
        if (!cropTilemap) Debug.LogError("[Farming] cropTilemap is NOT assigned!");
        if (!player) Debug.LogError("[Farming] player Transform is NOT assigned!");
        if (!wheatCrop) Debug.LogWarning("[Farming] wheatCrop is NOT assigned (planting will do nothing).");
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (!soilTilemap || !cropTilemap || !player)
            {
                Debug.LogError("[Farming] Missing references (soilTilemap/cropTilemap/player). Check Inspector.");
                return;
            }

            Vector3 playerPos = player.position;
            Vector3Int cell = soilTilemap.WorldToCell(playerPos);

            if (verboseLogs)
                Debug.Log($"[Farming] Interact pressed. PlayerPos={playerPos} -> Cell={cell}");

            if (!IsPlantable(cell))
            {
                if (verboseLogs)
                    Debug.Log($"[Farming] Cell {cell} is NOT plantable (no soil tile).");
                return;
            }

            if (!crops.ContainsKey(cell))
            {
                if (verboseLogs)
                    Debug.Log($"[Farming] No crop at {cell}. Attempting to PLANT { (wheatCrop ? wheatCrop.cropId : "NULL") }...");

                Plant(cell, wheatCrop);
            }
            else
            {
                if (verboseLogs)
                {
                    var s = crops[cell];
                    Debug.Log($"[Farming] Crop exists at {cell}. Stage={s.stage}/{s.def.growthTiles.Length - 1}. Attempting HARVEST...");
                }

                TryHarvest(cell);
            }
        }

        UpdateGrowth();
    }

    bool IsPlantable(Vector3Int cell)
    {
        bool hasSoil = soilTilemap.HasTile(cell);

        if (verboseLogs)
            Debug.Log($"[Farming] IsPlantable({cell}) -> {hasSoil}");

        return hasSoil;
    }

    void Plant(Vector3Int cell, CropDefinition def)
    {
        if (def == null)
        {
            Debug.LogWarning($"[Farming] Plant() aborted: CropDefinition is NULL at cell {cell}.");
            return;
        }

        if (def.growthTiles == null || def.growthTiles.Length == 0)
        {
            Debug.LogWarning($"[Farming] Plant() aborted: {def.cropId} has no growthTiles assigned.");
            return;
        }

        if (crops.ContainsKey(cell))
        {
            Debug.LogWarning($"[Farming] Plant() called but crop already exists at {cell}. Ignoring.");
            return;
        }

        var state = new CropState
        {
            def = def,
            stage = 0,
            nextStageTime = Time.time + def.secondsPerStage
        };

        crops[cell] = state;
        cropTilemap.SetTile(cell, def.growthTiles[0]);

        Debug.Log($"[Farming] PLANTED {def.cropId} at {cell}. Stage=0. NextStageTime={state.nextStageTime:0.00} (in {def.secondsPerStage:0.00}s)");
    }

    void UpdateGrowth()
    {
        if (crops.Count == 0) return;

        // Copy keys to avoid modifying dictionary while iterating
        var keys = new List<Vector3Int>(crops.Keys);

        foreach (var cell in keys)
        {
            var state = crops[cell];

            if (state.def == null || state.def.growthTiles == null || state.def.growthTiles.Length == 0)
            {
                Debug.LogWarning($"[Farming] Invalid crop state at {cell}. Removing.");
                cropTilemap.SetTile(cell, null);
                crops.Remove(cell);
                continue;
            }

            int lastStage = state.def.growthTiles.Length - 1;

            if (state.stage >= lastStage)
                continue; // fully grown, no log to avoid spam

            if (Time.time >= state.nextStageTime)
            {
                int oldStage = state.stage;
                state.stage++;
                state.nextStageTime = Time.time + state.def.secondsPerStage;

                cropTilemap.SetTile(cell, state.def.growthTiles[state.stage]);

                Debug.Log($"[Farming] GROWTH at {cell}: {state.def.cropId} Stage {oldStage} -> {state.stage}/{lastStage}. Next in {state.def.secondsPerStage:0.00}s");
            }
        }
    }

    void TryHarvest(Vector3Int cell)
    {
        if (!crops.ContainsKey(cell))
        {
            Debug.LogWarning($"[Farming] TryHarvest() called but no crop exists at {cell}.");
            return;
        }

        var state = crops[cell];
        int lastStage = state.def.growthTiles.Length - 1;
        bool ready = state.stage >= lastStage;

        if (!ready)
        {
            Debug.Log($"[Farming] NOT ready to harvest at {cell}. Stage={state.stage}/{lastStage}. TimeLeft={(state.nextStageTime - Time.time):0.00}s");
            return;
        }

        Debug.Log($"[Farming] HARVESTED {state.def.cropId} x{state.def.harvestYield} at {cell}!");

        // Remove crop tile + state
        cropTilemap.SetTile(cell, null);
        crops.Remove(cell);

        if (verboseLogs)
            Debug.Log($"[Farming] Crop removed at {cell}. Active crops now: {crops.Count}");
    }
}
