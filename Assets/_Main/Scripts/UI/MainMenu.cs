using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private const string NEXT = "next";
    private const string PLAY = "start";

    [SerializeField] private GameObject background;
    [SerializeField] private Button nextPlayButton;
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject endGameMenu;

    [SerializeField, TextArea(1, 10)] private string[] pageArray;

    private int currentPage;

    private void Awake()
    {
        nextPlayButton.onClick.AddListener(() =>
        {
            if (currentPage < pageArray.Length - 1)
            {
                currentPage++;
            }
            else
            {
                background.SetActive(false);
                nextPlayButton.gameObject.SetActive(false);
                descriptionPanel.SetActive(false);

                GameManager.Instance.StartGame();
            }

            UpdateText();
        });
    }

    private void Start()
    {
        UpdateText();

        GameManager.Instance.OnGameEnded += GameManager_OnGameEnded;
    }

    private void GameManager_OnGameEnded(object sender, EventArgs e)
    {
        background.SetActive(true);
        endGameMenu.SetActive(true);
    }

    private void UpdateText()
    {
        if (currentPage < pageArray.Length - 1)
        {
            nextPlayButton.GetComponentInChildren<TextMeshProUGUI>().text = NEXT;
        }
        else
        {
            nextPlayButton.GetComponentInChildren<TextMeshProUGUI>().text = PLAY;
        }

        descriptionText.text = pageArray[currentPage];
    }
}
