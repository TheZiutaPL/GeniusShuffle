using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PagesHandler : MonoBehaviour
{
    [SerializeField] List<GameObject> pages;
    [SerializeField] TMP_Text pagesCountDisplay;
    [SerializeField] Button previousPageButton;
    [SerializeField] Button nextPageButton;
    int currentPage = 1;

    private void Start()
    {
        previousPageButton.onClick.AddListener(PrevPage);
        nextPageButton.onClick.AddListener(NextPage);
    }

    private void SetPage(int page = 1)
    {
        pages[currentPage - 1].SetActive(false);

        currentPage = Mathf.Clamp(page, 1, pages.Count);

        pages[currentPage - 1].SetActive(true);

        if (pagesCountDisplay != null)
            pagesCountDisplay.SetText($"({currentPage}/{pages.Count})");

        previousPageButton.interactable = currentPage > 1;
        nextPageButton.interactable = currentPage < pages.Count;
    }

    private void PrevPage() => SetPage(currentPage - 1);
    private void NextPage() => SetPage(currentPage + 1);
}
