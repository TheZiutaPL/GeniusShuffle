using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextPagesHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI contentText;

    [SerializeField] private GameObject pagesTooltip;
    [SerializeField] private TextMeshProUGUI pagesCountDisplay;
    [SerializeField] private Button previousPageButton;
    [SerializeField] private Button nextPageButton;
    private int currentPage = 1;

    private void Awake()
    {
        previousPageButton.onClick.AddListener(PrevPage);
        nextPageButton.onClick.AddListener(NextPage);
    }

    public void SetText(string text)
    {
        contentText.SetText(text);

        StartCoroutine(InvokeNextFrame(() => SetTextPage()));
    }

    private void SetTextPage(int page = 1)
    {
        int pageCount = contentText.textInfo.pageCount;
        currentPage = Mathf.Clamp(page, 1, pageCount);

        bool multiplePages = pageCount > 1;

        pagesTooltip.SetActive(multiplePages);

        if (!multiplePages)
            return;

        pagesCountDisplay.SetText($"({currentPage}/{pageCount})");

        contentText.pageToDisplay = currentPage;

        previousPageButton.interactable = currentPage > 1;
        nextPageButton.interactable = currentPage < pageCount;
    }

    private void PrevPage() => SetTextPage(currentPage - 1);

    private void NextPage() => SetTextPage(currentPage + 1);

    private static IEnumerator InvokeNextFrame(System.Action action)
    {
        yield return new WaitForEndOfFrame();

        action?.Invoke();
    }
}
