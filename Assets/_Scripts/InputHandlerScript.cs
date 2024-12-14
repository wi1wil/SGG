using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandlerScript : MonoBehaviour
{
    private PauseMenuScript pauseMenuScript;
    private Camera _mainCamera;

    BuyChairScript buyChairScript;
    BuyTableScript buyTableScript;  
    
    UpgradeChairScript upgradeChairScript;  
    UpgradeTableScript upgradeTableScript;

    public GameObject confirmationPanel;
    public GameObject focusUpUI;

    private void Awake()
    {
        _mainCamera = Camera.main;

        upgradeChairScript = FindObjectOfType<UpgradeChairScript>();
        upgradeTableScript = FindObjectOfType<UpgradeTableScript>();

        pauseMenuScript = FindObjectOfType<PauseMenuScript>();

        buyChairScript = FindObjectOfType<BuyChairScript>();
        buyTableScript = FindObjectOfType<BuyTableScript>();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        // Check if the game is paused
        if (pauseMenuScript != null && pauseMenuScript.GameIsPaused) return;
        if (confirmationPanel != null && confirmationPanel.activeSelf) return;
        if (focusUpUI != null && focusUpUI.activeSelf) return;

        // Check if the input action has started
        if (!context.started) return;

        // Detects if raycast hits a collider
        var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        var rayHit = Physics2D.GetRayIntersection(ray);
    
        if (!rayHit.collider)
        {
            return;
        }

        Debug.Log("Clicked on " + rayHit.collider.gameObject.name);

        // Check if the hit object has a CashScript component and call OnCashCollected
        CashPrefabScript cashScript = rayHit.collider.gameObject.GetComponent<CashPrefabScript>();
        if (cashScript != null)
        {
            cashScript.OnCashCollected();
            return;
        }

        // Check if the hit object has a BuySignScript component and call buySign
        BuySignScript buySignScript = rayHit.collider.gameObject.GetComponent<BuySignScript>();
        if (buySignScript != null)
        {
            buySignScript.buySign();
            return;
        }

        // Check if the hit object has a UniversityDoorScript component and call LoadUniversity
        UniversityDoorScript universityDoorScript = rayHit.collider.gameObject.GetComponent<UniversityDoorScript>();
        if (universityDoorScript != null)
        {
            universityDoorScript.LoadUniversity();
            return;
        }


        if (rayHit.collider.gameObject.name == "BuyTable")
        {
            buyTableScript.ShowConfirmationPanel();
            return;
        } 
        else if (rayHit.collider.gameObject.name == "BuyChair")
        {
            buyChairScript.ShowConfirmationPanel();
            return;
        }



        if(rayHit.collider.gameObject.tag == "UTable" && CurrencyManagerScript.TablePrefabIndex == 14)
        {
            upgradeTableScript.ShowConfirmationPanel();
            return;
        }
        else if(rayHit.collider.gameObject.tag == "UChair" && CurrencyManagerScript.ChairPrefabIndex == 14)
        {
            upgradeChairScript.ShowConfirmationPanel();
            return;
        }
    }
}