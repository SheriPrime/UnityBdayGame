using UnityEngine;

using TMPro;
public class CoinUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public static int coinCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coinCount = 0;
        coinText.text = coinCount.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        coinText.text = coinCount.ToString();
    }
}
