using System.Collections;
using Unity.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject instructionsPanel;
    public GameObject loading;
    public GameObject toDisappearDuringInstructions;
    public GameObject[] slides;
    private int selectedSlide = 0;

    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
        toDisappearDuringInstructions.SetActive(false);
    }

    public void HideInstructions()
    {
        Debug.Log("Hide!");
        instructionsPanel.SetActive(false);
        toDisappearDuringInstructions.SetActive(true);
    }

    public void StartGame()
    {
        loading.SetActive(true);
        StartCoroutine(LoadSceneAsync("SampleScene"));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the scene is fully loaded
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    public void SlideLeft()
    {
        slides[selectedSlide].SetActive(false);
        if (selectedSlide == 0)
        {
            selectedSlide = slides.Length-1;
        } else 
        {
            selectedSlide--;
        }
        slides[selectedSlide].SetActive(true);
    }

    public void SlideRight()
    {
        slides[selectedSlide].SetActive(false);
        if (selectedSlide == slides.Length - 1)
        {
            selectedSlide = 0;
        }
        else
        {
            selectedSlide++;
        }
        slides[selectedSlide].SetActive(true);

    }
}
