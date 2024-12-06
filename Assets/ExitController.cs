using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour
{
    public GameObject playerEffect;
    public GameObject moveObj;
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            moveObj.SetActive(false);
            playerEffect.SetActive(true);
            StartCoroutine(FinishGame());
        }
    }
    private IEnumerator FinishGame()
    {
        yield return new WaitForSeconds(2f);

        Application.Quit();


    }
}
