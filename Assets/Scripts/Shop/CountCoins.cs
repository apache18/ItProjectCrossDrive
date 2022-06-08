using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountCoins : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Text>().text = PlayerPrefs.GetInt("Coins").ToString();
    }
}
