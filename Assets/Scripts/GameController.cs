using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public bool IsMainScene;
    private bool _isLoseOnce;
    public GameObject[] cars;
    public GameObject[] maps;
    public GameObject canvasLosePanel;
    private float _spawnTimeFrom = 3;
    private float _spawnTimeTo = 6;
    private int _countCars;
    private Coroutine CarsDown, CarsLeft, CarsUp, CarsRight;
    public Text nowScore, topScore, coinsCount;
    public GameObject horn;
    public AudioSource turnSignal;

    private void Start()
    {
        if (PlayerPrefs.GetInt("NowOpen") == 2)
        {
            Destroy(maps[0]);
            maps[1].SetActive(true);
            Destroy(maps[2]);
        }
        else if (PlayerPrefs.GetInt("NowOpen") == 3)
        {
            Destroy(maps[0]);
            Destroy(maps[1]);
            maps[2].SetActive(true);
        }
        else
        {
            maps[0].SetActive(true);
            Destroy(maps[1]);
            Destroy(maps[2]);
        }
        CarController.isLose = false;
        CarController.countCars = 0;
        if (IsMainScene)
        {
            _spawnTimeFrom = 4;
            _spawnTimeTo = 8;
        }
        CarsDown = StartCoroutine(SpawnCarsDown());
        CarsLeft = StartCoroutine(SpawnCarsLeft());
        CarsUp = StartCoroutine(SpawnCarsUp());
        CarsRight = StartCoroutine(SpawnCarsRight());

        StartCoroutine(CreateHorn());
    }

    private void Update()
    {
        if (CarController.isLose && !_isLoseOnce)
        {
            StopCoroutine(CarsDown);
            StopCoroutine(CarsLeft);
            StopCoroutine(CarsUp);
            StopCoroutine(CarsRight);
            nowScore.text = "<color=#FF0000>SCORE:</color> " + CarController.countCars.ToString();
            if (PlayerPrefs.GetInt("Score") < CarController.countCars)
            {
                PlayerPrefs.SetInt("Score", CarController.countCars);
            }
            topScore.text = "<color=#FF0000>BEST SCORE:</color>" + PlayerPrefs.GetInt("Score").ToString();
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + CarController.countCars);
            coinsCount.text = PlayerPrefs.GetInt("Coins").ToString();

            canvasLosePanel.SetActive(true);
            _isLoseOnce = true;
        }
    }

    IEnumerator SpawnCarsDown()
    {
        while (true)
        {
            float _spawnTime = Random.Range(_spawnTimeFrom, _spawnTimeTo);
            SpawnCars(new Vector3(-1.39f, 0.4f, -28.3f), 180);
            yield return new WaitForSeconds(_spawnTime);
        }
    }
    IEnumerator SpawnCarsLeft()
    {
        while (true)
        {
            float _spawnTime = Random.Range(_spawnTimeFrom, _spawnTimeTo);
            SpawnCars(new Vector3(-79.6f, 0.4f, 3.4f), -90);
            yield return new WaitForSeconds(_spawnTime);
        }
    }
    IEnumerator SpawnCarsUp()
    {
        while (true)
        {
            float _spawnTime = Random.Range(_spawnTimeFrom, _spawnTimeTo);
            SpawnCars(new Vector3(-8.3f, 0.4f, 68.5f), 0, true);
            yield return new WaitForSeconds(_spawnTime);
        }
    }
    IEnumerator SpawnCarsRight()
    {
        while (true)
        {
            float _spawnTime = Random.Range(_spawnTimeFrom, _spawnTimeTo);
            SpawnCars(new Vector3(22.11f, 0.4f, 10.2f), 90);
            yield return new WaitForSeconds(_spawnTime);
        }
    }

    private void SpawnCars(Vector3 pos, float rotY, bool isMoveFromUp = false)
    {
        GameObject _newCar = Instantiate(cars[Random.Range(0, cars.Length)], pos, Quaternion.Euler(0, rotY, 0)) as GameObject;

        int numberRandomSpawn;
        _newCar.name = "Cars - " + ++_countCars;
        if (IsMainScene)
        {
            numberRandomSpawn = 1;
            _newCar.GetComponent<CarController>().speed = 10f;
        }
        else
        {
            numberRandomSpawn = Random.Range(1, 4);
        }

        switch (numberRandomSpawn)
        {
            case 1:
                _newCar.GetComponent<CarController>().rightTurn = true;
                if (PlayerPrefs.GetString("music") != "No" && !turnSignal.isPlaying)
                    turnSignal.Play();
                Invoke("StopSound", 4f);
                break;
            case 2:
                _newCar.GetComponent<CarController>().leftTurn = true;
                if (isMoveFromUp)
                {
                    _newCar.GetComponent<CarController>().moveFromUp = true;
                }
                if (PlayerPrefs.GetString("music") != "No" && !turnSignal.isPlaying)
                    turnSignal.Play();
                Invoke("StopSound", 4f);
                break;
            case 3:
                //Двигаем вперёд
                break;
        }
    }

    void StopSound()
    {
        turnSignal.Stop();
    }

    IEnumerator CreateHorn()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5, 9));
            if (PlayerPrefs.GetString("music") != "No")
                Instantiate(horn, Vector3.zero, Quaternion.identity);
        }
    }
}
