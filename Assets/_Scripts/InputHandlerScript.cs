using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandlerScript : MonoBehaviour
{
    PauseMenuScript pauseMenuScript;
    private Camera _mainCamera;

    private void Awake()
    {
        pauseMenuScript = FindObjectOfType<PauseMenuScript>();
        _mainCamera = Camera.main;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        // Check if the game is paused
        if (pauseMenuScript.GameIsPaused) return;

        // Check if the input action has started
        if (!context.started) return;

        // Get the mouse position and create a ray from the camera
        var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        var rayHit = Physics2D.GetRayIntersection(ray);

        // Check if the ray hit a collider
        if (!rayHit.collider) return;

        // Check if the hit object has a CashScript component and call OnCashCollected
        CashPrefabScript cashScript = rayHit.collider.gameObject.GetComponent<CashPrefabScript>();
        if (cashScript != null)
        {
            cashScript.OnCashCollected();
        }
    }
}