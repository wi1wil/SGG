using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyChairTrigger : MonoBehaviour
{
    BuyChairScript buyChairScript;

    private void Awake()
    {
        buyChairScript = FindObjectOfType<BuyChairScript>();
    }

    public void OnBuyTrigger()
    {
        buyChairScript.ShowConfirmationPanel();
    }
}