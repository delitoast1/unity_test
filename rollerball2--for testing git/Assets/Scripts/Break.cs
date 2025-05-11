using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Break : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject fractured;
    public float breakForce;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            BreakTheThing();
        }
    }

    public void BreakTheThing()
    {
        GameObject frac= Instantiate(fractured, transform.position, transform.rotation);

        foreach (Rigidbody rb in frac.GetComponentsInChildren<Rigidbody>()) 
        {
            Vector3 force = (rb.transform.position - transform.position).normalized * breakForce;
            rb.AddForce(force);
        }
        Destroy(gameObject);
    }
}
