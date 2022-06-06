using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float health = 2f;
    [SerializeField] GameObject Camera;
    [SerializeField] float stunTime = .5f;
     [SerializeField] CanvasGroup canvasGroup;
    PhotonView photonView;
    LevelController levelController;
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        canvasGroup = GameObject.FindWithTag("Fade").GetComponent<CanvasGroup>();
        levelController = GameObject.FindWithTag("LevelController").GetComponent<LevelController>();
        if (photonView.IsMine)
        {
        WakeUp();
        }
    }
    private void WakeUp()
    {
        FadeOutImmadiate();
        StartCoroutine(FadeIn(5f));
    }

    [PunRPC]
    public void GetDamage()
    {
       
        health--;
        if (health <= 0)
        {
            photonView.RPC("Die", RpcTarget.All);
        }
        if (photonView.IsMine)
        {
        Damaged();
        }
    }
  
    private void Damaged()
    {
        Camera.GetComponent<CameraShake>().shakeSize = 0.1f;
        FadeOutImmadiate();
        StartCoroutine(FadeIn(stunTime));
        levelController.LoadLevel();
    }
    [PunRPC]
   public void Die()
    {
        FadeOutImmadiate();
        StartCoroutine(FadeIn(5f));
        levelController.LoadLevel();
    }



    public void FadeOutImmadiate()
    {
        canvasGroup.alpha = 1;

    }
    public IEnumerator FadeOut(float time)
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }
    public IEnumerator FadeIn(float time)
    {

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }
    }

   


}

