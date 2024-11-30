using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceController : MonoBehaviour
{
    public GameObject humanPlayer;
    public GameObject beePlayer;
    public float ExperienceTime = 30f;
    public void RunBeeExperiment(GameObject bee)
    {
        bee.SetActive(false);
        beePlayer.transform.position = bee.transform.position;
        humanPlayer.SetActive(false);
        beePlayer.SetActive(true);

        StartCoroutine(FinishExperience(ExperienceTime, beePlayer, bee));

    }

    private IEnumerator FinishExperience(float delay, GameObject animalPlayer, GameObject animal)
    {
        yield return new WaitForSeconds(delay);

        animalPlayer.SetActive(false);
        humanPlayer.SetActive(true);
        animal.SetActive(true);


    }
}
