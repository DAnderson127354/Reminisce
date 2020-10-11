using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject instructions;
    public GameObject[] texts;
    public int index;
    public Button nextButton;
    public Button prevButton;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            Application.Quit();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void Instructions()
    {
        titleScreen.SetActive(false);
        instructions.SetActive(true);
        index = 0;
        for (int i = 0; i < texts.Length; i++)
            texts[i].SetActive(false);

        texts[0].SetActive(true);
        prevButton.interactable = false;
        nextButton.interactable = true;
    }

    public void Back()
    {
        instructions.SetActive(false);
        titleScreen.SetActive(true);
    }

    public void Next()
    {
        texts[index].SetActive(false);
        index++;
        texts[index].SetActive(true);
        prevButton.interactable = true;
        if (index == 3)
            nextButton.interactable = false;
    }

    public void Prev()
    {
        texts[index].SetActive(false);
        index--;
        texts[index].SetActive(true);
        nextButton.interactable = true;
        if (index == 0)
            prevButton.interactable = false;
    }
}
