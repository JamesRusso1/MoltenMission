using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rayscript : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        //Create Ray
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.forward, out hit))
        {
            Debug.DrawLine(transform.position, hit.point, Color.blue);

            Rockscript ro = hit.collider.GetComponent<Rockscript>();
            if (ro != null)
            {
                //Detect Stones
                Debug.DrawLine(transform.position, hit.point, Color.red);

                if (Input.GetMouseButtonDown(0))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + -transform.forward * 100f, Color.green);
        }
    }
}
