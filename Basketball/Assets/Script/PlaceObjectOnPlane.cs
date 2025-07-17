using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Events;

[RequireComponent(typeof(ARPlaneManager), typeof(ARRaycastManager))]
public class PlaceObjectOnPlane : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    ARRaycastManager raycastManager;
    ARPlaneManager planeManager;
    List<ARRaycastHit> hitList = new();
    GameObject obj;
    public UnityEvent objectPlaced;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
    }

    private void Start()
    {
        obj = Instantiate(prefab, transform.position, Quaternion.identity);
        obj.SetActive(false);
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void FingerDown(Finger finger)
    {
        if (finger.index != 0) return;

        if (raycastManager.Raycast(finger.currentTouch.screenPosition, hitList, TrackableType.PlaneWithinPolygon)){
            foreach(ARRaycastHit hit in hitList)
            {
                Debug.Log(hit.trackable.name);
                Pose pose = hit.pose;
                obj.transform.position = pose.position + new Vector3(-0.5f, 0, 0);
                obj.SetActive(true);
                objectPlaced?.Invoke();
            }
        }
    }
}
