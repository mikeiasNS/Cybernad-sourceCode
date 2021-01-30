using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(GraphicRaycaster))]
public class TouchHandler : MonoBehaviour
{
    [HideInInspector]
    public bool TouchStarted;
    [HideInInspector]
    public bool Touching;
    [HideInInspector]
    public bool TouchEnded;
    [HideInInspector]
    public Vector2 TouchPosition;

    private GraphicRaycaster raycaster;

    private void Awake()
    {
        raycaster = GetComponent<GraphicRaycaster>();
    }

    void Update()
    {
        //if(Application.isEditor)
            HandleClicks();
        //else 
        //    HandleTouches();
    }

    private void HandleClicks()
    {
        bool uiTouched = UIElementsTouched(Input.mousePosition);
        TouchStarted = Input.GetKeyDown(KeyCode.Mouse0) && !uiTouched;
        Touching = Input.GetKey(KeyCode.Mouse0) && !uiTouched;
        TouchEnded = Input.GetKeyUp(KeyCode.Mouse0) && !uiTouched;

        TouchPosition = Input.mousePosition;
    }

    // Adaptation for mobile control
    private void HandleTouches()
    {
        var touches = Input.touches;
        for (int i = 0; i < touches.Length; i++)
        {
            if (i > 1) return;

            Touch touch = touches[i];
            bool uiTouched = UIElementsTouched(touch.position);

            TouchStarted = touch.phase == TouchPhase.Began && !uiTouched;
            Touching = (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) && !uiTouched;
            TouchEnded = touch.phase == TouchPhase.Ended;

            TouchPosition = touch.position;
            if (!uiTouched) return;
        }
    }

    private bool UIElementsTouched(Vector2 position)
    {
        var pointerEventData = new PointerEventData(null)
        {
            position = position
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        return results.Count > 0;
    }

    public void GameOver()
    {
        TouchStarted = false;
        Touching = false;
        TouchEnded = false;
    }
}
