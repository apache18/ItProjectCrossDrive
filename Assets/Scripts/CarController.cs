using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public AudioClip crash;
    public AudioClip[] accelerates;
    public bool rightTurn, leftTurn, moveFromUp;
    public float speed = 15f, force = 250f;
    Rigidbody _rigidbody;
    private float _originRotationY, _rotateMaMultRight = 6f, _rotateMaMultLeft = 4.5f;
    Camera _camera;
    public LayerMask carsLayer;
    private bool __isMovingFast, _carCrashed;
    [NonSerialized] public bool carPassed;
    public static bool isLose;
    public GameObject TurnSignalRight, TurnSignalLeft, explosion, Exhaust;
    [NonSerialized] public static int countCars;

    private void Start()
    {
        _camera = Camera.main;
        _originRotationY = transform.eulerAngles.y;
        _rigidbody = GetComponent<Rigidbody>();
        if (leftTurn)
            StartCoroutine(TurnSignal(TurnSignalLeft));
        else if (rightTurn) 
        StartCoroutine(TurnSignal(TurnSignalRight));
    }

    IEnumerator TurnSignal(GameObject signal)
    {
        while (!carPassed)
        {
            signal.SetActive(!signal.activeSelf);
            yield return new WaitForSeconds(0.5f);
        }
        signal.SetActive(false);
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(transform.position - transform.forward * speed * Time.fixedDeltaTime);
    }

    private void Update()
    {
#if UNITY_EDITOR
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
#else
            if (Input.touchCount == 0)         
            return;

            Ray ray = _camera.ScreenPointToRay(Input.GetTouch(0).position);
        
#endif
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, 100f, carsLayer)) 
        {
            string carName = raycastHit.transform.gameObject.name;
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0) && !__isMovingFast && gameObject.name == carName) { 
#else 
            if (Input.GetTouch(0).phase == TouchPhase.Began && !_isMouseClick && gameObject.name == carName) {
#endif       
                speed *= 2;
                __isMovingFast = true;
                if (_carCrashed == false)
                {
                    GameObject particle = Instantiate(Exhaust, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.Euler(90, 0, 0)) as GameObject;
                    Destroy(particle, 3f);
                }

                if (PlayerPrefs.GetString("music") != "No")
                {
                    GetComponent<AudioSource>().clip = accelerates[Random.Range(0, accelerates.Length)];
                    GetComponent<AudioSource>().Play();
                }
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Car") && !_carCrashed) 
        {
            _carCrashed = true;
            isLose = true;
            speed = 0f;
            other.gameObject.GetComponent<CarController>().speed = 0f;
            GameObject vfx = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
            Destroy(vfx, 5);
            
            if(__isMovingFast)
            {
                force *= 1.3f;
            }
            _rigidbody.AddRelativeForce(Vector3.back * force);
            if (PlayerPrefs.GetString("music") != "No")
            {
                GetComponent<AudioSource>().clip = crash;
                GetComponent<AudioSource>().Play();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_carCrashed)
            return;
        if (other.transform.CompareTag("TurnBlockRight") && rightTurn)
        {
            RotateCar(_rotateMaMultRight);
        }
        else if (other.transform.CompareTag("TurnBlockLeft") && leftTurn)
        {
            RotateCar(_rotateMaMultLeft, -1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Car") && other.GetComponent<CarController>().carPassed)  
        {
            other.GetComponent<CarController>().speed = speed + 5f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_carCrashed)
            return;
        if (other.transform.CompareTag("TriggerPass")) 
        {
            if (carPassed)
            {
                return;
            }
            carPassed = true;
            Collider[] colliders = GetComponents<BoxCollider>();
            foreach (Collider collider in colliders)
                collider.enabled = true;
            countCars++;
        }
        if (other.transform.CompareTag("TurnBlockRight") && rightTurn)
        {
            _rigidbody.rotation = Quaternion.Euler(0, _originRotationY + 90f, 0);
        }
        else if (other.transform.CompareTag("TurnBlockLeft") && leftTurn)
        {
            _rigidbody.rotation = Quaternion.Euler(0, _originRotationY - 90f, 0);
        }
        else if (other.transform.CompareTag("DeleteCar"))
        {
            Destroy(gameObject);
        }
    }

    private void RotateCar(float speedRotate, int dir = 1)
    {
        if (_carCrashed)
            return;

        if (dir == -1 && transform.localRotation.eulerAngles.y < _originRotationY - 90f)
        {
            return;
        }
        if (dir == -1 && moveFromUp && transform.localRotation.eulerAngles.y > 250f && transform.localRotation.eulerAngles.y < 270f)
        {
            return;
        }
        float rotateSpeed = speed * speedRotate * dir;
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, rotateSpeed, 0) * Time.fixedDeltaTime);
        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
    }
}
