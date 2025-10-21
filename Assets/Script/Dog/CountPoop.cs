using UnityEngine;
using UnityEngine.UI;

public class CountPoop : MonoBehaviour
{
    public Image[] poopImages;
    public static CountPoop Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        foreach (Image poopImage in poopImages)
        {
            poopImage.enabled = false;
        }
    }

    public void CountIcon(int count)
    {
        foreach (Image poopImage in poopImages)
        {
            poopImage.enabled = false;
        }

        for (int i = 0; i < count && i < poopImages.Length; i++)
        {
            poopImages[i].enabled = true;
        }
    }
}
