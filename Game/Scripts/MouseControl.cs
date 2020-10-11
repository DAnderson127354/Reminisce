using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseControl : MonoBehaviour
{
	public Sprite neutral;
	public Sprite ready;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Input.mousePosition;
    }

    void FixedUpdate()
    {
    	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Does the ray intersect with the ghost
        Debug.DrawRay(ray.origin, ray.direction, Color.green);
        if (Physics.Raycast(ray, out hit, 100) && hit.transform.gameObject.name == "Ghost" && hit.transform.gameObject.GetComponent<Renderer>().enabled)
        {
            //Debug.Log(hit.collider.gameObject.name);
            this.GetComponent<Image>().sprite = ready;
        }
        else
            this.GetComponent<Image>().sprite = neutral;
    }
}
