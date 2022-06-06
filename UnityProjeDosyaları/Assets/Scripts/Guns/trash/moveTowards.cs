using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTowards : MonoBehaviour
{
    [SerializeField] GameObject target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.localPosition, target.transform.position, Time.deltaTime * 1f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this);
    }
}
