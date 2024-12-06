using UnityEngine;
using UnityEngine.InputSystem;

public class SkyboxTransition : MonoBehaviour
{
    public Material skyboxBlendMaterial;
    public float transitionSpeed = 0.5f;

    private float blendValue = 0.0f;
    private bool transitioningToNight = false;

    public void TransitionToNight()
    {
        transitioningToNight = true;
    }

    public void TransitionToDay()
    {
        transitioningToNight = false;
    }

    void Update()
    {
        if (transitioningToNight)
        {
            blendValue = Mathf.Min(1.0f, blendValue + transitionSpeed * Time.deltaTime);
        }
        else
        {
            blendValue = Mathf.Max(0.0f, blendValue - transitionSpeed * Time.deltaTime);
        }

        skyboxBlendMaterial.SetFloat("_Blend", blendValue);
    }
}
