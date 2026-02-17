using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Farming/Crop Definition")]
public class CropDefinition : ScriptableObject
{
    public string cropId = "wheat";
    public TileBase[] growthTiles;        // 0..N (last is harvestable)
    public float secondsPerStage = 5f;    // time between stages
    public int harvestYield = 1;          // how many wheat items you get
}
