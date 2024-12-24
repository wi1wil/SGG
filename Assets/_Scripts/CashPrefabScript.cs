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
    AudioManagerScript audioManager;

    public bool isOnGround = false;
    private float timer = 0f;
    
    private void Awake() {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
    }

    private void Start() 
    {
        GameObject currencyObject = GameObject.FindWithTag("Currency");
        currencyManager = currencyObject.GetComponent<CurrencyManagerScript>();
    }

    private int GetRandomCurrency()
    {
        int randomIndex = Random.Range(0, cashValues.Length);
        return cashValues[randomIndex];
    }

    public void OnCashCollected()
    {
        if(isOnGround)
        {
            int addedCash = GetRandomCurrency();
            Debug.Log("Collected Cash: " + addedCash * CurrencyManagerScript.doubleCashValue);
            audioManager.PlaySfx(audioManager.moneyGrab);

            var go = Instantiate(popUpText, transform.position, Quaternion.identity, transform.parent);
            go.GetComponent<TextMeshProUGUI>().text = "+" + (addedCash * CurrencyManagerScript.doubleCashValue).ToString("N0");

            currencyManager.addCash(addedCash * CurrencyManagerScript.doubleCashValue);
            Destroy(gameObject);
        }
    }

    public bool IsOnGround()
    {
        return isOnGround;
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        isOnGround = true;
        anim.enabled = false;
        timer = 0f;
    }

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
