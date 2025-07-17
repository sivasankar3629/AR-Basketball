using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.Events;

public class BasketballThrower : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnPoint; // Camera or fixed point
    public float throwForce = 10f;

    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private int ballCount = 10;
    [SerializeField] TMP_Text ballCountText;
    public UnityEvent gameOver;

    Queue<GameObject> spawnQueue;

    private void Start()
    {
        spawnQueue = new Queue<GameObject>();
        for (int i = 0; i < ballCount; i++)
        {
            GameObject ball = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            ball.SetActive(false);
            spawnQueue.Enqueue(ball);
        }
    }


    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerUp += OnFingerUp;
    }

    private void OnDisable()
    {
        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerUp -= OnFingerUp;
        EnhancedTouchSupport.Disable();
    }

    void OnFingerDown(Finger finger)
    {
        startTouchPos = finger.screenPosition;
    }

    void OnFingerUp(Finger finger)
    {
        endTouchPos = finger.screenPosition;
        Vector2 swipeVector = endTouchPos - startTouchPos;
        Vector3 swipeDirection = new Vector3(swipeVector.x, swipeVector.y, swipeVector.magnitude).normalized;
        Vector3 worldDir = Camera.main.transform.TransformDirection(swipeDirection);

        Vector3 swipeForce = worldDir * swipeVector.magnitude * throwForce;

        ThrowBall(swipeForce);
    }

    void ThrowBall(Vector3 force)
    {
        if (ballCount == 0)
        {
            gameOver?.Invoke();
            return;
        }
        
        ballCount--;
        ballCountText.text = $"x {ballCount}";

        GameObject ball = spawnQueue.Dequeue();
        ball.transform.position = spawnPoint.position;
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        ball.SetActive(true);
        rb.isKinematic = false;
        rb.AddForce(force);
        StartCoroutine(DeactivateBall(ball));
    }

    IEnumerator DeactivateBall(GameObject ball)
    {
        yield return new WaitForSeconds(5f);
        spawnQueue.Enqueue(ball);
        ball.GetComponent<Rigidbody>().isKinematic = true;
        ball.SetActive(false);
    }
}
