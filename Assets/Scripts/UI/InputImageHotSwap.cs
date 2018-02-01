namespace GGJ2018.UI
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using Util;

	enum ActionType {Movement, Aim, Shooting, Screech, Meelee}

	public class InputImageHotSwap : MonoBehaviour
	{
		[SerializeField]
		Sprite[] images;
		[SerializeField]
		ActionType type;

		private Image image;

		void Start ()
		{
			image = GetComponentInChildren<Image>();
		}
		
		void Update ()
		{
			// if ((CustomInput. == CustomInput.ControlType.Xbox)
		}
	}
}
