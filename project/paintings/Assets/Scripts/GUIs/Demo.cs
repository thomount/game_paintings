using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{
	private string _StrText = "";
	private string _StrPW = "";
	private int _IntSelectIndex = 0;
	private bool _BoolCheck1 = false;
	private bool _BoolCheck2 = false;

	private float value = 0;
	private int min = 0;
	private int max = 100;

	void OnGUI()
	{
		GUI.Label(new Rect(0, 0, 100, 30), "I am the Label");
		_StrText = GUI.TextField(new Rect(0, 50, 100, 30), _StrText);

		_StrPW = GUI.TextField(new Rect(0, 100, 100, 30), _StrPW);

		GUI.Button(new Rect(0, 150, 50, 30), "Sure");
_IntSelectIndex = GUI.Toolbar(new Rect(0, 200, 200, 30), _IntSelectIndex, new string[] { "Duty", "Equip", "Peopel" });

		_BoolCheck1 = GUI.Toggle(new Rect(0, 260, 100, 50), _BoolCheck1, "zhuangbei");
		_BoolCheck2 = GUI.Toggle(new Rect(0, 300, 100, 50), _BoolCheck2, "renyuan");

		value = GUI.HorizontalSlider(new Rect(0, 350, 200, 50), value, max, min);
	}

}