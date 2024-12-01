using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceController : MonoBehaviour
{
    public GameObject humanPlayer;
    public GameObject beePlayer;
    public GameObject birdPlayer;
    public float experienceTime = 30f;
    public float animationTime = 1f;

    public GameObject humanEffect;
    public GameObject beeEffect;
    public GameObject birdEffect;

    public Transform headCamera;
    public Transform leftHand;
    public Transform rightHand;
    public float callBirdTime = 5f;

    private bool _holdingLeftHand = false;
    private bool _holdingRightHand = false;
    private float _leftholdingTime = 0f;
    private float _rightholdingTime = 0f;

    private bool _inExperiment = false;

    public GameObject birdArrivingLeft;
    public GameObject birdArrivingRight;

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
                if (Time.time - _leftholdingTime > callBirdTime && !_inExperiment)
                {
                    birdArrivingLeft.SetActive(true);
                    RunBirdExperiment();
                    _inExperiment = true;
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
                if (Time.time - _rightholdingTime > callBirdTime && !_inExperiment)
                {
                    birdArrivingRight.SetActive(true);
                    RunBirdExperiment();
                    _inExperiment = true;
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

        humanEffect.SetActive(true);

        StartCoroutine(EffectTransformation(birdPlayer, birdEffect, null));
    }

    public void RunBeeExperiment(GameObject bee)
    {
        bee.SetActive(false);
        beePlayer.transform.position = bee.transform.position;

        humanEffect.SetActive(true);

        StartCoroutine(EffectTransformation(beePlayer, beeEffect, bee));
    }

    private IEnumerator FinishExperience(float delay, GameObject animalPlayer, GameObject animalEffect, GameObject animal)
    {
        yield return new WaitForSeconds(delay);
        animalEffect.SetActive(true);

        StartCoroutine(EffectTransformationBack(animalPlayer, animalEffect, animal));


    }

    private IEnumerator EffectTransformation(GameObject animalPlayer, GameObject animalEffect, GameObject animal = null)
    {
        yield return new WaitForSeconds(animationTime);

        humanEffect.SetActive(false);

        humanPlayer.SetActive(false);
        animalPlayer.SetActive(true);

        StartCoroutine(FinishExperience(experienceTime, animalPlayer, animalEffect, animal));
    }

    private IEnumerator EffectTransformationBack(GameObject animalPlayer, GameObject animalEffect, GameObject animal = null)
    {
        yield return new WaitForSeconds(animationTime);
        animalEffect.SetActive(false);
        birdArrivingLeft.SetActive(false);
        birdArrivingRight.SetActive(false);

        animalPlayer.SetActive(false);
        humanPlayer.SetActive(true);
        if (animal != null)
            animal.SetActive(true);
        _inExperiment = false;
        _holdingLeftHand = false;
        _holdingRightHand = false;
    }
}
