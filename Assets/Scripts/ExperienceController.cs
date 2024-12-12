using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ExperienceController : MonoBehaviour
{
    public GameObject humanPlayer;
    public GameObject beePlayer;
    public GameObject birdPlayer;
    public GameObject fishPlayer;
    public float experienceTime = 30f;
    public float animationTime = 1f;

    public GameObject humanEffect;
    public GameObject beeEffect;
    public GameObject birdEffect;
    public GameObject fishEffect;

    public GameObject fishSpawn;

    public Transform headCamera;
    public Transform leftHand;
    public Transform rightHand;
    public float callBirdTime = 5f;

    private bool _holdingLeftHand = false;
    private bool _holdingRightHand = false;
    private float _leftholdingTime = 0f;
    private float _rightholdingTime = 0f;

    private bool _inExperiment = false;

    private bool _inAnyExperiment = false;
    private GameObject _animalPlayer = null;
    private GameObject _animalEffect = null;
    private GameObject _animal = null;
    private bool _finishExperiment = false;

    public GameObject birdArrivingLeft;
    public GameObject birdArrivingRight;

    public XRSimpleInteractable waterInteractable;

    public Volume globalVolume;

    public GameObject portal;

    public InputActionReference finishExperience;


    private void Start()
    {
        fishPlayer.transform.position = fishSpawn.transform.position;
        finishExperience.action.started += ButtonWasPressed;
        finishExperience.action.canceled += ButtonWasReleased;
    }

    private void ButtonWasPressed(InputAction.CallbackContext context)
    {
        _finishExperiment = true;
    }

    private void ButtonWasReleased(InputAction.CallbackContext context)
    {
        _finishExperiment = false;
    }

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

        if (_inAnyExperiment && _finishExperiment)
        {
            _inAnyExperiment = false;
            _animalEffect.SetActive(true);
            StartCoroutine(EffectTransformationBack(_animalPlayer, _animalEffect, _animal));
        }
    }

    public void RunBirdExperiment()
    {
        //Need bird animation of arrival
        birdPlayer.transform.position = humanPlayer.transform.position;
        humanEffect.SetActive(true);

        StartCoroutine(EffectTransformation(birdPlayer, birdEffect, null));
    }

    public void RunFishExperiment()
    {
        humanEffect.SetActive(true);
        globalVolume.profile.TryGet<ColorAdjustments>(out var colorAdjustments);
        colorAdjustments.active = true;
        StartCoroutine(EffectTransformation(fishPlayer, fishEffect, null));
    }

    public void RunBeeExperimentMenu()
    {
        beePlayer.transform.position = humanPlayer.transform.position;
        humanEffect.SetActive(true);

        StartCoroutine(EffectTransformation(beePlayer, beeEffect, null));
    }


    public void RunBeeExperiment(GameObject bee)
    {

        bee.SetActive(false);
        beePlayer.transform.position = bee.transform.position;

        humanEffect.SetActive(true);

        StartCoroutine(EffectTransformation(beePlayer, beeEffect, bee));
    }


    private IEnumerator EffectTransformation(GameObject animalPlayer, GameObject animalEffect, GameObject animal = null)
    {
        yield return new WaitForSeconds(animationTime);

        humanEffect.SetActive(false);
        waterInteractable.enabled = false;
        humanPlayer.SetActive(false);
        animalPlayer.SetActive(true);
        _inAnyExperiment = true;
        _animalPlayer = animalPlayer;
        _animalEffect = animalEffect;
        _animal = animal;
    }

    private IEnumerator EffectTransformationBack(GameObject animalPlayer, GameObject animalEffect, GameObject animal = null)
    {
        yield return new WaitForSeconds(animationTime);
        waterInteractable.enabled = true;
        globalVolume.profile.TryGet<ColorAdjustments>(out var colorAdjustments);
        colorAdjustments.active = false;
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
        _animalPlayer = null;
        _animalEffect = null;
        _animal = null;
    }

    public void ExitButton()
    {
        portal.SetActive(true);
    }
}
