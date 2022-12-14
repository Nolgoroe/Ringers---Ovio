using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
	float deltaTime = 0.0f;

	void Update()
	{
        if (ServerRelatedData.instance.isAdmin)
        {
			deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
		}
	}

	void OnGUI()
	{
		if (ServerRelatedData.instance.isAdmin)
        {
			int w = Screen.width, h = Screen.height;

			GUIStyle style = new GUIStyle();

			Rect rect = new Rect(75, 0, w, h);

			style.alignment = TextAnchor.LowerLeft;
			style.fontSize = h * 2 / 120;
			style.normal.textColor = Color.black;

			float msec = deltaTime * 1000.0f;
			float fps = 1.0f / deltaTime;
			string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
			GUI.Label(rect, text, style);
		}
	}
}