using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int itemID;
    public string itemName;
    
    public virtual void pickup()
    {
        Sprite itemIcon = GetComponent<Image>().sprite;
        if(itemPickupUiController.instance != null)
        {
            itemPickupUiController.instance.ShowPopup(itemName, itemIcon);
        }
    }
}
