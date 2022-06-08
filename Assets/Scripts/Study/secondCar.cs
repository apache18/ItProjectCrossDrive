using UnityEngine.SceneManagement;
using UnityEngine;

public class secondCar : MonoBehaviour
{
    private void OnDestroy()
    {
        PlayerPrefs.SetString("FirstGame", "No");
        SceneManager.LoadScene("Game");
    }
}
