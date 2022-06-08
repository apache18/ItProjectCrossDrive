using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoseMap : MonoBehaviour
{
    public AudioClip btnClick;

    public void ChooseNewMap(int numberMap)
    {
        if (PlayerPrefs.GetString("music") != "No")
        {
            GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().clip = btnClick;
        }

        PlayerPrefs.SetInt("NowOpen", numberMap);
        GetComponent<CheckMaps>().CheckButtonsActive();
    }
}
