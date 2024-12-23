using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideBarScript : MonoBehaviour
{
    AudioManagerScript audioManager;

    [Header("Side Bar Button")]
    [SerializeField] private Button OpenSideBarButton;
    [SerializeField] private Button CloseSideBarButton;

    [Header("Side Bar UI")]
    [SerializeField] private GameObject SideBarUI;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();

        if (OpenSideBarButton != null) OpenSideBarButton.onClick.AddListener(OpenSideBar);
        if (CloseSideBarButton != null) CloseSideBarButton.onClick.AddListener(CloseSideBar);
    }

    private void OpenSideBar()
    {
        audioManager.PlaySfx(audioManager.yesButton);
        SideBarUI.SetActive(true);
        
        OpenSideBarButton.gameObject.SetActive(false);
        CloseSideBarButton.gameObject.SetActive(true);
    }

    public void CloseSideBar()
    {
        audioManager.PlaySfx(audioManager.noButton);
        SideBarUI.SetActive(false);

        OpenSideBarButton.gameObject.SetActive(true);
        CloseSideBarButton.gameObject.SetActive(false);
    }
}
