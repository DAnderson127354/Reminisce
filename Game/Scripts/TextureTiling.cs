using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureTiling : MonoBehaviour
{
	public float x;
	public float y;
	public float offx;
	public float offy;
	Material mat;

    // Start is called before the first frame update
    void Start()
    {
       GetComponent<Renderer>().material.mainTextureScale = new Vector2 (x, y);
       GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offx, offy);
    }

}
