using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuySignScript : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] CurrencyManagerScript currencyScript;

    [Header("GameObjects")]
    [SerializeField] GameObject parentInEnvironment;
    [SerializeField] GameObject universityBuilding;
    [SerializeField] GameObject confirmationDialog;

    [Header("Texts")]
    [SerializeField] TextMeshProUGUI doorConfirmationText;
    [SerializeField] TextMeshProUGUI signConfirmationText;
    [SerializeField] GameObject popUpText;

    [Header("Buttons")]
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;

    private Vector3 Offset = new Vector3(1, 0, 0);

    private void Start()
    {
        currencyScript = FindObjectOfType<CurrencyManagerScript>();

        // Add listeners to the buttons
        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
    }

    public void buySign()
    {
        // Show the confirmation panel and activate the sign confirmation text
        signConfirmationText.gameObject.SetActive(true);
        doorConfirmationText.gameObject.SetActive(false);
        confirmationDialog.SetActive(true);
    }

    private void OnYesButtonClicked()
    {
        // Check if the currency is enough
        if (currencyScript.currencyInGame >= currencyScript.buySignCost)
        {
            // Decrease the currency by the cost of the sign
            currencyScript.currencyInGame -= currencyScript.buySignCost;

            currencyScript.currencyPerSecText.gameObject.SetActive(true);
            currencyScript.moneyMultiplierText.gameObject.SetActive(true);

            // Set the university building to active
            universityBuilding.SetActive(true);

            // Destroy the buy sign
            Destroy(gameObject);
        }
        else
        {
            // Instantiate the pop-up text as a child of parentInEnvironment
            var go = Instantiate(popUpText, transform.position, Quaternion.identity);
            go.transform.SetParent(parentInEnvironment.transform, false);
            go.GetComponent<TextMeshProUGUI>().text = "Not enough money!";
        }

        // Close the confirmation panel
        confirmationDialog.SetActive(false);
    }

    private void OnNoButtonClicked()
    {
        // Close the confirmation panel
        confirmationDialog.SetActive(false);
    }
}