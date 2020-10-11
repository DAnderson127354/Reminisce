using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapControl : MonoBehaviour
{
	public GameObject[] itemIcons;
	public Material normal;
	public Material warning;

	public void WarningColor(bool on, int index)
	{
		if (on)
			itemIcons[index].GetComponent<Renderer>().material = warning;
		else
			itemIcons[index].GetComponent<Renderer>().material = normal;
	}

   	public void RemoveIcon(int index)
   	{
   		itemIcons[index].SetActive(false);
   	}

   	public void AddIcon(int index)
   	{
   		itemIcons[index].SetActive(true);
   	}

   	public int FindIndex(string name)
   	{ 
   		int i = -1;
   		switch (name)
   		{
   			case "Ball_1":
   				i = 0;
   				break;
   			case "Footstool":
   				i = 1;
   				break;
   			case "Lamp":
   				i = 2;
   				break;
   			case "Plant_Potted_Monstera_Deliciosa":
   				i = 3;
   				break;
   			case "Vase_4":
   				i = 4;
   				break;
   			default:
   				Debug.Log("Oops! Something has gone wrong!");
   				break;
   		}
   		return i;
   	}
}
