using UnityEngine;

public class Fading : MonoBehaviour
{
    public Texture2D texture;//будет ссылаться на спрайт fading
    private float _fadeSpeed = 0.8f; //скорость, с которой будем затемнять или осветлять экран
    private int _drawDepth = -1000; //для рисования по верх других объектов
    public float _alpha = 1f;//отвечает за прозрачность
    private float _fadeDir = -1;//затемнение или осветление экрана

    void OnGUI()
    {
        _alpha += _fadeDir * _fadeSpeed * Time.deltaTime;
        _alpha = Mathf.Clamp01(_alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, _alpha);
        GUI.depth = _drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
    }

    public float Fade(float dir)
    {
        _fadeDir = dir;
        return _fadeSpeed;
    }
}
