using UnityEngine;
using UnityEngine.UI;

public class CheckMaps : MonoBehaviour
{
    private ByMappCoins _byMappCoins;
    public Image[] maps;
    public Sprite selected, notSelected;

    private void Start()
    {
        CheckButtonsActive();
        _byMappCoins = GetComponent<ByMappCoins>();
        if (PlayerPrefs.GetString("City") == "Open")
        {
            _byMappCoins.coins1000.SetActive(false);
            _byMappCoins.money0_99.SetActive(false);
            _byMappCoins.city_btn.SetActive(true);
        }
        if (PlayerPrefs.GetString("Megapolis") == "Open")
        {
            _byMappCoins.coins5000.SetActive(false);
            _byMappCoins.money1_99.SetActive(false);
            _byMappCoins.megapolis_btn.SetActive(true);
        }
    }

    public void CheckButtonsActive()
    {
        switch (PlayerPrefs.GetInt("NowOpen"))
        {
            case 2:
                maps[0].sprite = notSelected;
                maps[1].sprite = selected;
                maps[2].sprite = notSelected;
                break;
            case 3:
                maps[0].sprite = notSelected;
                maps[1].sprite = notSelected;
                maps[2].sprite = selected;
                break;
            default:
                maps[0].sprite = selected;
                maps[1].sprite = notSelected;
                maps[2].sprite = notSelected;
                break;
        }
    }
}
