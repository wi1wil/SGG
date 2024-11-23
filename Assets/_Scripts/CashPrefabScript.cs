using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class CashPrefabScript : MonoBehaviour
{
    [Header("Cash Prefab Values")]
    [SerializeField] private int[] cashValues;
    
    [Header("Cash Prefab Settings")]
    [SerializeField] private float timeLimit;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject popUpText;

    CurrencyManagerScript currencyManager;
    private bool isOnGround = false;
    private float timer = 0f;

    private void Start() 
    {
        // Set the Spawned Cash to have Cash Layer
        gameObject.layer = LayerMask.NameToLayer("CashLayer");

        // Find the Currency Manager
        GameObject currencyObject = GameObject.FindWithTag("Currency");
        currencyManager = currencyObject.GetComponent<CurrencyManagerScript>();
    }

    // Get a random Cash Value to give to the player on Collect
    private int GetRandomCurrency()
    {
        int randomIndex = Random.Range(0, cashValues.Length);
        return cashValues[randomIndex];
    }

    public void OnCashCollected()
    {
        if(isOnGround)
        {
            // Amount of Cash given to the Player
            int addedCash = GetRandomCurrency();
            Debug.Log("Collected Cash: " + addedCash);

            // Spawn Floating Text
            var go = Instantiate(popUpText, transform.position, Quaternion.identity, transform.parent);
            go.GetComponent<TextMeshProUGUI>().text = "+" + addedCash.ToString("N0");

            // Send the value to Currency Manager to Add to the Player's Cash
            currencyManager.addCash(addedCash);
            Destroy(gameObject);
        }
    }

    // Check if the Cash is on the Ground
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        isOnGround = true;
        anim.enabled = false;
        timer = 0f;
    }

    // Update the Timer for the Cash to be Destroyed
    private void Update() 
    {
        if(isOnGround)
        {
            timer += Time.deltaTime;
            if(timer >= timeLimit)
            {
                Debug.Log("Too late! The cash flew away!");
                Destroy(gameObject);
            }
        }
    }
}
