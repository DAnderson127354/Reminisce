using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public float mouseSensitivity;
	public GameObject player;
	private float xRotation;
	private Animator anim;
	public Transform face;
	public Transform cam;

	void Start()
	{
		mouseSensitivity = 100f;
		xRotation = 0f;
		Cursor.lockState = CursorLockMode.Locked; // keep mouse at center of the screen
		anim = player.GetComponent<Animator>();
	}

	void Update()
	{
		//camera moves with mouse
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f); //keep from player looking behind them by looking up/down

		transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
		player.transform.GetChild(0).transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

		if (anim.GetBool("sitDown") == false)
		{
			player.transform.Rotate(Vector3.up * mouseX);
			transform.position = cam.position;
			transform.parent = cam.transform;
		}
        else
        {
			//transform.position = face.position;
			transform.parent = face;
        }
		
	}
	
}
