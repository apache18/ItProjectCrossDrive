using UnityEngine;

public class MovementFirstCar : MonoBehaviour
{
    public GameObject firstCanvas, secondCar, secondCanvas;
    private bool _isFirst;
    private CarController _carController;

    private void Start()
    {
        _carController = GetComponent<CarController>();
    }

    private void Update()
    {
        if (transform.position.x < 8 && !_isFirst)
        {
            _isFirst = true;
            _carController.speed = 0;
            firstCanvas.SetActive(true);
        }
    }

    private void OnMouseDown()
    {
        if (_isFirst || transform.position.x > 9)  
        _carController.speed = 15f;
        firstCanvas.SetActive(false);
        secondCar.GetComponent<CarController>().speed = 12f;
        secondCanvas.SetActive(true);
    }
}
