using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : MonoBehaviour {

	public Text speakerNameTextRef;
	public Text spokenTextRef;
	public Text spaceToSkipTextRef;

	public GameObject choicePanelRef;

	public GameObject choicePanel0Ref;
	public GameObject choicePanel1Ref;
	public GameObject choicePanel2Ref;
	public GameObject choicePanel3Ref;

	public Text choice0TextRef;
	public Text choice1TextRef;
	public Text choice2TextRef;
	public Text choice3TextRef;

	private enum WritingState
	{
		Hidden,
		WritingLine,
		WritingChoice,
		WaitingForChoice,
		FinishedLine,
	}

	private WritingState writingState = WritingState.Hidden;

	string currentLineText = "";
	int currentLineLength;
	float currentLinePosition;
	public float charactersPerSecond = 20f;

	public Action LineComplete;
	public Action<int> ChoiceMade;

	private static DialogPanel _instance;
	public static DialogPanel instance {
		get{return _instance;}
	}

	void Awake(){
		if(_instance == null){
			_instance = this;
		} else {
			Debug.LogError("There should be only one DialogPanel!");
			Destroy(this.gameObject);
		}
	}

	// Use this for initialization
	void OnEnable () {
		if(writingState == WritingState.Hidden){
			HideChoicePanel();
			HideSpeechPanel();
		}
	}
	
	// Update is called once per frame
	void Update () {

		switch(writingState){

		case WritingState.WritingLine:
			AnimateWriting();
			ListenForSkip();
			break;

		case WritingState.WritingChoice:
			AnimateWriting();
			ListenForSkip();
			ListenForChoice();
			break;

		case WritingState.FinishedLine:
			
			if (Input.GetKeyDown(KeyCode.Space))
			{
				if(LineComplete != null)
					LineComplete();
			}
			break;


		case WritingState.WaitingForChoice:			
			ListenForChoice();
			break;

		}
	}

	void AnimateWriting()
	{
		//Advance the writing "cursor" over time
		if (currentLinePosition < currentLineLength)
		{
			//Advance our cursor position
			currentLinePosition = Mathf.Clamp(currentLinePosition + Time.deltaTime * charactersPerSecond, 0f, currentLineLength);
		}

		if (currentLinePosition >= currentLineText.Length)
		{
			FinishWriting();
		}

		RefreshDialogText();
	}

	void ListenForSkip(){
		//Advance to the end of the line if the player presses space
		if (Input.GetKeyDown(KeyCode.Space))
		{
			FinishWriting();
		}
	}

	void ListenForChoice()
	{
		int chosenIndex = -1;

		if (Input.GetKeyDown(KeyCode.A))
		{
			chosenIndex = 0;
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			chosenIndex = 1;
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			chosenIndex = 2;
		}
		else if (Input.GetKeyDown(KeyCode.W))
		{
			chosenIndex = 3;
		}

		if(chosenIndex >= 0 && ChoiceMade != null){
			ChoiceMade(chosenIndex); //Report the choice
		}
	}

	void ShowSpeechPanel(){
		this.gameObject.SetActive(true);
	}

	void HideSpeechPanel(){
		this.gameObject.SetActive(false);
		writingState = WritingState.Hidden;
	}

	void ShowChoicePanel(){
		choicePanelRef.SetActive(true);
	}

	void HideChoicePanel(){
		choicePanelRef.SetActive(false);
	}

	public void HideAll(){
		HideChoicePanel();
		HideSpeechPanel();
		this.gameObject.SetActive(false);
	}

	public void DisplayLine(string line, string speakerName)
	{
		writingState = WritingState.WritingLine;

		speakerNameTextRef.text = speakerName;
		currentLineText = line;
		currentLineLength = line.Length;
		currentLinePosition = 0;


		ShowSpeechPanel();
		HideChoicePanel();
	}

	public void DisplayChoice(string spokenText, string speakerName, List<Choice> choiceList){

		writingState = WritingState.WritingChoice;

		speakerNameTextRef.text = speakerName;
		currentLineText = spokenText;
		currentLineLength = spokenText.Length;
		currentLinePosition = 0;

		ShowSpeechPanel();
		ShowChoicePanel();

		PopulateChoices(choiceList);
	}

	void FinishWriting(){
		currentLinePosition = currentLineLength;
		RefreshDialogText();

		switch(writingState){

		case WritingState.WritingLine:
			writingState = WritingState.FinishedLine;
			break;

		case WritingState.WritingChoice:
			writingState = WritingState.WaitingForChoice;
			break;
		}
		/*
		if(LineComplete != null){
			LineComplete();
		}*/

	}

	void RefreshDialogText(){
		//Update the displayed Text
		spokenTextRef.text = currentLineText.Substring(0, (int)currentLinePosition);
	}

	void PopulateChoices(List<Choice> choices)
	{
		choicePanel0Ref.SetActive(false);
		choicePanel1Ref.SetActive(false);
		choicePanel2Ref.SetActive(false);
		choicePanel3Ref.SetActive(false);

		if (choices.Count > 0)
		{
			choicePanel0Ref.SetActive(true);
			choice0TextRef.text = choices[0].shortText;
		}

		if (choices.Count > 1)
		{
			choicePanel1Ref.SetActive(true);
			choice1TextRef.text = choices[1].shortText;
		}

		if (choices.Count > 2)
		{
			choicePanel2Ref.SetActive(true);
			choice2TextRef.text = choices[2].shortText;
		}

		if (choices.Count > 3)
		{
			choicePanel3Ref.SetActive(true);
			choice3TextRef.text = choices[3].shortText;
		}
	}
}
