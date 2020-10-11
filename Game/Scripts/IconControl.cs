using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconControl : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
    	//Debug.Log(name + " y pos: " + this.gameObject.transform.position.y );
        if (this.gameObject.transform.position.y < 14f)
        	this.gameObject.transform.GetChild(0).gameObject.layer = 11;
        else
        	this.gameObject.transform.GetChild(0).gameObject.layer = 12;
    }
}
