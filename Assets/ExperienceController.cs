using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceController : MonoBehaviour
{
    public GameObject humanPlayer;
    public GameObject beePlayer;
    public GameObject birdPlayer;
    public float ExperienceTime = 30f;

    public Transform headCamera;
    public Transform leftHand;
    public Transform rightHand;
    public float callBirdTime = 5f;

    private bool _holdingLeftHand = false;
    private bool _holdingRightHand = false;
    private float _leftholdingTime = 0f;
    private float _rightholdingTime = 0f;

    private void Update()
    {
        if (leftHand.position.y > headCamera.position.y)
        {
            if (!_holdingLeftHand)
            {
                _holdingLeftHand = true;
                _leftholdingTime = Time.time;
            }
            else
            {
                if (Time.time - _leftholdingTime > callBirdTime)
                {
                    RunBirdExperiment();
                }
            }
        }
        else
        {
            _holdingLeftHand = false;
            _leftholdingTime = 0f;
        }

        if (rightHand.position.y > headCamera.position.y && !_holdingLeftHand)
        {
            if (!_holdingRightHand)
            {
                _holdingRightHand = true;
                _rightholdingTime = Time.time;
            }
            else
            {
                if (Time.time - _rightholdingTime > callBirdTime)
                {
                    RunBirdExperiment();
                }
            }
        }
        else
        {
            _holdingRightHand = false;
            _rightholdingTime = 0f;
        }
    }

    public void RunBirdExperiment()
    {
        //Need bird animation of arrival
        humanPlayer.SetActive(false);
        birdPlayer.SetActive(true);

        StartCoroutine(FinishExperience(ExperienceTime, birdPlayer, null));
    }

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
        if (animal != null)
            animal.SetActive(true);


    }
}
