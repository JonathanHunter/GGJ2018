namespace GGJ2018.UI
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using Util;

	enum ActionType {Movement, Aim, Shooting, Screech, Meelee, Reload}

	public class InputImageHotSwap : MonoBehaviour
	{
		[SerializeField]
		Sprite[] images;
		[SerializeField]
		ActionType type;

		private SpriteRenderer image;
		private bool usingPad;

		void Start ()
		{
			image = GetComponentInChildren<SpriteRenderer>();
			usingPad = CustomInput.UsingPad;
			if (CustomInput.UsingPad) 
			{
				SetImage(1);
			}
		}
		
		void Update ()
		{
			if (CustomInput.UsingPad != usingPad)
			{
				usingPad = CustomInput.UsingPad;
				if (usingPad)
				{
					if (!(type == ActionType.Screech || type == ActionType.Reload))
						SetImage(1);	// General parts of a controller
					else if (CustomInput.GamePadType(0) == CustomInput.ControlType.Xbox)
						SetImage(1);	// Xbox buttons
					else if (CustomInput.GamePadType(0) == CustomInput.ControlType.PS3
						  || CustomInput.GamePadType(0) == CustomInput.ControlType.PS4)
						SetImage(2);	// PS buttons
				} 
				else
				{
					SetImage(0);		// Keyboard and Mouse
				}
			}
		}

		void SetImage(int i) 
		{
			image.sprite = null;
			image.sprite = images[i];
		}
	}
}
