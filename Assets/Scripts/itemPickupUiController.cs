using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class itemPickupUiController : MonoBehaviour
{
    public static itemPickupUiController instance { get; private set; }
    public GameObject popupPrefab;
    public int maxPopups = 5;
    public float popupDuration;

    private readonly Queue<GameObject> popupQueue = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowPopup(string itemName, Sprite itemIcon)
    {
        GameObject popup = Instantiate(popupPrefab, transform);
        popup.GetComponentInChildren<TMP_Text>().text = itemName;
        Image itemImage = popup.transform.Find("itemIcon")?.GetComponent<Image>();
        if (itemImage)
        {
            itemImage.sprite = itemIcon;
        }
        popupQueue.Enqueue(popup);
        if (popupQueue.Count > maxPopups)
        {
            GameObject oldestPopup = popupQueue.Dequeue();
            Destroy(oldestPopup);
        }

        StartCoroutine(DestroyPopupAfterDelay(popup));
    }

    private IEnumerator DestroyPopupAfterDelay(GameObject popup)
    {
        yield return new WaitForSeconds(popupDuration);
        if (popup == null) yield break; // Popup might have been destroyed already

        CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();
        for (float t = 0f; t < 1f; t += Time.deltaTime)
        {
            if (popup == null) yield break; // Popup might have been destroyed already
            canvasGroup.alpha = 1f - t;
            yield return null;
        }
        Destroy(popup);
    }
}
