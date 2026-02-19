using TMPro;
using UnityEngine;

public class shopSlot : MonoBehaviour
{
    public GameObject currentItem;
    public int itemPrice;
    public TMP_Text priceText;
    public bool isShopSlot = true;

    void Awake()
    {
        if (!priceText)
        {
            priceText = transform.Find("priceText").GetComponent<TMP_Text>();
        }
    }
    
    public void updatePriceDisplay()
    {
        
    }
}
