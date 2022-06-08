using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasButton : MonoBehaviour
{
    public Sprite spriteClose, spriteOpen, musicOn, musicOff;
    Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
        if (gameObject.name == "SourceButton")
        {
            if (PlayerPrefs.GetString("music") == "No")
                transform.GetChild(0).GetComponent<Image>().sprite = musicOff;
        }
    }
    public void MusicButton()
    {
        if (PlayerPrefs.GetString("music") == "No")
        {
            PlayerPrefs.SetString("music", "Yes");
            transform.GetChild(0).GetComponent<Image>().sprite = musicOn;
        }
        else
        {
            PlayerPrefs.SetString("music", "No");
            transform.GetChild(0).GetComponent<Image>().sprite = musicOff;
        }
        PlayButtonSound();
    }

    public void ShopScene()
    {
        StartCoroutine(LoadScene("Shop"));
        PlayButtonSound();
    }
    public void ExitShopScene()
    {
        StartCoroutine(LoadScene("Main"));
        PlayButtonSound();
    }

    public void PlayGame()
    {
        if (PlayerPrefs.GetString("FirstGame") == "No")
            StartCoroutine(LoadScene("Game"));
        else
        {
            StartCoroutine(LoadScene("Study"));
        }
        PlayButtonSound();
    }

    public void CloseAllButtons()
    {
        _image.sprite = spriteClose;
        transform.GetChild(0).localPosition -= new Vector3(0f, 5f, 0f);

    }

    public void OpenAllButtons()
    {
        _image.sprite = spriteOpen;
        transform.GetChild(0).localPosition += new Vector3(0f, 5f, 0f);
        
    }

    IEnumerator LoadScene(string name)
    {
        float _fadeTime = Camera.main.GetComponent<Fading>().Fade(1f);
        yield return new WaitForSeconds(_fadeTime);
        SceneManager.LoadScene(name);
    }

    private void PlayButtonSound()
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();
    }
}
