using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWalk : MonoBehaviour
{
    Animator catWalkanim;
    [HideInInspector] public bool isPressed = false;

    AudioSource pressSound;
    // Start is called before the first frame update
    void Start()
    {
        pressSound = GetComponent<AudioSource>();
        catWalkanim = GetComponent<Animator>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("asd");

        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            catWalkanim.SetBool("isPress", true);
            isPressed = true;
            pressSound.Play();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            catWalkanim.SetBool("isPress", false);
            isPressed = false;
            pressSound.Stop();

        }

    }


}




