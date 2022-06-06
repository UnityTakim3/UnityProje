using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageActive : MonoBehaviour
{

    [SerializeField] GameObject firstCatwalk;
    [SerializeField] GameObject secondCatwalk;
    Animator cageAnim;
   

    // Start is called before the first frame update
    void Start()
    {
        cageAnim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        CatWalk catwalkFirst = firstCatwalk.gameObject.GetComponent<CatWalk>();
        CatWalk catwalkSecond = secondCatwalk.gameObject.GetComponent<CatWalk>();

        if(catwalkFirst.isPressed==true && catwalkSecond.isPressed==true)
        {
            cageAnim.SetBool("isPress", true);


        }
        else { cageAnim.SetBool("isPress", false); }

    }
}
