using UnityEngine;
using UnityEngine.UI;

public class tabController : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] pages;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActivateTab(0);
    }

    public void ActivateTab(int index)
    {
        for (int i = 0; i < tabImages.Length; i++)
        {
            pages[i].SetActive(false);
            tabImages[i].color = Color.gray;
        }
        pages[index].SetActive(true);
        tabImages[index].color = Color.white;
    }
}
