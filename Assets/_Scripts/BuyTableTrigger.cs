using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyTableTrigger : MonoBehaviour
{
    BuyTableScript buyTableScript;

    private void Awake()
    {
        buyTableScript = FindObjectOfType<BuyTableScript>();
    }

    public void OnBuyTrigger()
    {
        buyTableScript.ShowConfirmationPanel();
    }
}
