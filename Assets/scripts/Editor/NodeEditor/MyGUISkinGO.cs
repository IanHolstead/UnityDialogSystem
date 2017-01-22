using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MyGUISkinGO : MonoBehaviour {

	public GUISkin myGUISkin;

	public GUIStyle myCustomStyle;

	public Color myColor;

	public float sliderValue = 1f;

	void OnGUI () {
		//GUI.skin= myGUISkin;

		//myColor = RGBSlider (new Rect (10,10,200,10), myColor);

		//sliderValue = MyCustomSlider(new Rect(10, 10, 300, 100), sliderValue, GUIStyle.none);
		sliderValue = MyCustomSlider(sliderValue, EditorStyles.textArea);

		//bool flashResult = FlashingButton(new Rect(10, 10, 300, 100), new GUIContent("Press me!"), EditorStyles.textArea);

    }

	Color RGBSlider (Rect screenRect, Color rgb) {
		rgb.r = LabelSlider (screenRect, rgb.r, 1.0f, "Red: " + rgb.r);
    
        // <- Move the next control down a bit to avoid overlapping
        screenRect.y += 20; 
        rgb.g = LabelSlider (screenRect, rgb.g, 1.0f, "Green: " + rgb.g);
    
        // <- Move the next control down a bit to avoid overlapping
        screenRect.y += 20; 
    
        rgb.b = LabelSlider (screenRect, rgb.b, 1.0f, "Blue: " + rgb.b);
        
        return rgb;
    }   

	float LabelSlider (Rect screenRect, float sliderValue, float sliderMaxValue, string labelText) {
        GUI.Label (screenRect, labelText);
    
        // <- Push the Slider to the end of the Label
        screenRect.x += screenRect.width; 
    
        sliderValue = GUI.HorizontalSlider (screenRect, sliderValue, 0.0f, sliderMaxValue);
        return sliderValue;
    }

	public static float MyCustomSlider(float value, GUIStyle style){
		Rect position = GUILayoutUtility.GetRect(GUIContent.none, style);
		return MyCustomSlider(position, value, style);
	}

	public static float MyCustomSlider(Rect controlRect, float value, GUIStyle style){
		int controlID = GUIUtility.GetControlID(FocusType.Passive);

		switch(Event.current.GetTypeForControl(controlID)){
			case EventType.Repaint:
			{
				//Work out the width of the bar in pixels by lerping
				int pixelWidth = (int)Mathf.Lerp(1f, controlRect.width, value);

				//Build up the rectangle that the bar will cover
				//by copying the whole control rect and then setting the width
				Rect targetRect = new Rect(controlRect){width = pixelWidth};

				//Tint whatever we draw to be red/green depending on value
				GUI.color = Color.Lerp(Color.red, Color.green, value);

				GUI.DrawTexture(targetRect, style.normal.background);

				GUI.color = Color.white;
				break;
			}

			case EventType.MouseDown:
			{
				//If the click is actually on us...
				if(controlRect.Contains(Event.current.mousePosition) && Event.current.button == 0){
					GUIUtility.hotControl = controlID;
				}
				break;				
			}

			case EventType.MouseUp:
			{
				if(GUIUtility.hotControl == controlID){
					GUIUtility.hotControl = 0;
				}
				break;
			}
			
		}

		if(Event.current.isMouse && GUIUtility.hotControl == controlID){

			float relativeX = Event.current.mousePosition.x - controlRect.x;

			value = Mathf.Clamp01(relativeX / controlRect.width);

			GUI.changed = true;

			Event.current.Use();
		}

		return value;

	}

	public static bool FlashingButton(Rect rc, GUIContent content, GUIStyle style){
		int controlID = GUIUtility.GetControlID(FocusType.Passive);

		var state = (FlashingButtonInfo)GUIUtility.GetStateObject(typeof(FlashingButtonInfo), controlID);

		switch(Event.current.GetTypeForControl(controlID)){
			case EventType.Repaint:
			{
				GUI.color = state.IsFlashing(controlID) ? Color.red : Color.white;
				style.Draw(rc, content, controlID);
				break;
			}

			case EventType.MouseDown:
			{
				if(rc.Contains(Event.current.mousePosition) && Event.current.button == 0 && GUIUtility.hotControl == 0){
					GUIUtility.hotControl = controlID;
					state.MouseDownNow();
				}
				break;
			}

			case EventType.MouseUp:
			{
				if(GUIUtility.hotControl == controlID){
					GUIUtility.hotControl = 0;
				}
				break;
			}
		}

		return GUIUtility.hotControl == controlID;
	}

	public class FlashingButtonInfo{
		private double mouseDownAt;

		public void MouseDownNow(){
			mouseDownAt = EditorApplication.timeSinceStartup;
		}

		public bool IsFlashing(int controlID){
			if(GUIUtility.hotControl != controlID){
				return false;
			}

			double elapsedTime = EditorApplication.timeSinceStartup - mouseDownAt;
			if(elapsedTime < 2f){
				return false;
			}

			return (int)((elapsedTime - 2f) / 0.1f) % 2 == 0;
		}
	}

}
