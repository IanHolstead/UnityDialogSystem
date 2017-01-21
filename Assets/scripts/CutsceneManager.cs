using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeamMeRoundScotty{
	public class CutsceneManager : MonoBehaviour {

		Canvas screenCanvas;

		public GameObject dialogPanelRef;
		public Text speakerNameTextRef;
		public Text spokenTextRef;

		public float charactersPerSecond = 3f;

		private enum WritingState{
			Hidden,
			WritingLine,
			WritingChoice,
			WaitingForChoice,
			FinishedLine,
		}
		private WritingState writingState = WritingState.Hidden;

		LineSet currentLineSet;

		int currentLineIndex;
		string currentLineText = "";
		int currentLineLength;
		float currentLinePosition;

		ChoiceSet currentChoiceSet;

		public GameObject choicePanelRef;

		public GameObject choicePanel0Ref;
		public GameObject choicePanel1Ref;
		public GameObject choicePanel2Ref;
		public GameObject choicePanel3Ref;

		public Text choice0TextRef;
		public Text choice1TextRef;
		public Text choice2TextRef;
		public Text choice3TextRef;

		public class ActionSet{
			
		}

		public class LineSet : ActionSet {
			public string[] lines;
			public ActionSet nextSet;

			public LineSet(string[] lines){
				this.lines = lines;
			}
		}

		public class ChoiceSet : ActionSet {
			public string mainText;
			public string[] choices;
			public ActionSet[] nextActions;

			public ChoiceSet(string mainText, string[] choices, ActionSet[] nextActions){
				this.mainText = mainText;
				this.choices = choices;
				this.nextActions = nextActions;
			}
		}

		// Use this for initialization
		void Start () {
			screenCanvas = GameObject.FindGameObjectWithTag("ScreenCanvas").GetComponent<Canvas>();	
			
			Events.instance.AddListener<CutsceneEvent>(OnCutsceneEvent);

			if(writingState == WritingState.Hidden){
				dialogPanelRef.SetActive(false);
			}
		}
		
		// Update is called once per frame
		void Update () {

			if(Input.GetKeyDown(KeyCode.L)){

				string[] testLines = {"Bacon ipsum dolor amet tenderloin short ribs ribeye id eiusmod. Ham hock shoulder flank laborum porchetta velit hamburger cupim pork loin rump ut ullamco cupidatat ut pork belly. Cupim ea dolore meatball ground round shank id turkey ut drumstick eiusmod. Consectetur irure deserunt swine ground round fatback.", "Hello?", "Is anybody there?"};
				LineSet exampleLineSet = new LineSet(testLines);

				Events.instance.Raise(new CutsceneEvent(exampleLineSet));
			}

			if(Input.GetKeyDown(KeyCode.C)){

				string[] convo0 = new string[]{"It was then that I saw it...", "A mutant cow!"};
				LineSet lineSet0 = new LineSet(convo0);

				string[] convo1 = new string[]{"Whatever you say.", "That coffee mug was getting pretty old anyway."};
				LineSet lineSet1 = new LineSet(convo1);

				string[] convo2 = new string[]{"You put your right hand in.", "You take your right hand out.", "You put your right hand in and you shake it all about."};
				LineSet lineSet2 = new LineSet(convo2);

				string[] convo3 = new string[]{"There are 10 kinds of people in this world.", "Those who understand binary and those who don.t"};
				LineSet lineSet3 = new LineSet(convo3);

				string[] choices = {"It was a dark and stormy night...", "I suppose you're right...", "Let's dance!", "Excelsior!"};
				ChoiceSet choiceSet = new ChoiceSet("As I see it, you've got a few choices...", choices, new ActionSet[]{lineSet0, lineSet1, lineSet2, lineSet3});

				Events.instance.Raise(new CutsceneEvent(choiceSet));
			}

			switch(writingState){
			case WritingState.WritingLine:
				{
					//Advance to the end of the line if the player presses space
					if(Input.GetKeyDown(KeyCode.Space)){
						FinishWriting();
					}

					//Advance the writing "cursor" over time
					if(currentLinePosition < currentLineLength){
						//Advance our cursor position
						currentLinePosition = Mathf.Clamp(currentLinePosition + Time.deltaTime * charactersPerSecond, 0f, currentLineLength);
					}

					//Update the displayed Text
					spokenTextRef.text = currentLineText.Substring(0, (int)currentLinePosition);

					if(currentLinePosition == currentLineText.Length){
						writingState = WritingState.FinishedLine;
					}	
					break;	
				}

			case WritingState.FinishedLine:
				{
					if(Input.GetKeyDown(KeyCode.Space)){
						AdvanceToNextLine();
					}
					break;
				}

			case WritingState.WritingChoice:
				{
					//Advance to the end of the line if the player presses space
					if(Input.GetKeyDown(KeyCode.Space)){
						FinishWriting();
					}

					//Advance the writing "cursor" over time
					if(currentLinePosition < currentLineLength){
						//Advance our cursor position
						currentLinePosition = Mathf.Clamp(currentLinePosition + Time.deltaTime * charactersPerSecond, 0f, currentLineLength);
					}

					//Update the displayed Text
					spokenTextRef.text = currentLineText.Substring(0, (int)currentLinePosition);

					if(currentLinePosition == currentLineText.Length){
						writingState = WritingState.WaitingForChoice;
					}	

					//Listen for choice
					ListenForChoice();

					break;
				}

			case WritingState.WaitingForChoice:
				{
					ListenForChoice();
					break;	
				}


			}
		}

		void OnCutsceneEvent(CutsceneEvent e){

			PlayActionSet(e.actionSet);

		}

		void PlayActionSet(ActionSet actionSet){
			if(actionSet is LineSet){
				PlayLineSet((LineSet)actionSet);
			} else if(actionSet is ChoiceSet){
				PlayChoiceSet((ChoiceSet)actionSet);
			}
		}

		void PlayLineSet(LineSet lineSet){

			writingState = WritingState.WritingLine;

			currentChoiceSet = null;
			currentLineSet = lineSet;
			currentLineIndex = 0;

			BeginWritingLine(currentLineSet.lines[currentLineIndex]);

			ShowPanel();

			HideChoicePanel();
		}

		void PlayChoiceSet(ChoiceSet choiceSet){

			writingState = WritingState.WritingChoice;

			currentLineSet = null;
			currentChoiceSet = choiceSet;

			BeginWritingLine(choiceSet.mainText);
			PopulateChoices(choiceSet.choices);

			ShowPanel();
			ShowChoicePanel();


		}

		void ShowPanel(){
			dialogPanelRef.SetActive(true);
		}

		void HidePanel(){
			dialogPanelRef.SetActive(false);
			writingState = WritingState.Hidden;
		}

		void ShowChoicePanel(){
			choicePanelRef.SetActive(true);
		}

		void HideChoicePanel(){
			choicePanelRef.SetActive(false);
		}

		void BeginWritingLine(string line){
			
			currentLineText = line;
			currentLineLength = line.Length;
			currentLinePosition = 0;

		}

		void FinishWriting(){
			currentLinePosition = currentLineLength;	
		}

		void AdvanceToNextLine(){
			
			//Are there more lines in this set?
			if(currentLineSet != null && currentLineIndex + 1 < currentLineSet.lines.Length){
				currentLineIndex++;

				BeginWritingLine(currentLineSet.lines[currentLineIndex]);
				writingState = WritingState.WritingLine;

			} else {
				//We're down with our current set of lines, close the dialog panel
				HidePanel();
			}
		}

		void PopulateChoices(string[] choicesText){
			choicePanel0Ref.SetActive(false);
			choicePanel1Ref.SetActive(false);
			choicePanel2Ref.SetActive(false);
			choicePanel3Ref.SetActive(false);

			if(choicesText.Length > 0){
				choicePanel0Ref.SetActive(true);
				choice0TextRef.text = choicesText[0];
			}

			if(choicesText.Length > 1){
				choicePanel1Ref.SetActive(true);
				choice1TextRef.text = choicesText[1];
			}

			if(choicesText.Length > 2){
				choicePanel2Ref.SetActive(true);
				choice2TextRef.text = choicesText[2];
			}

			if(choicesText.Length > 3){
				choicePanel3Ref.SetActive(true);
				choice3TextRef.text = choicesText[3];
			}
				
		}

		void ListenForChoice(){
			if(Input.GetKeyDown(KeyCode.A)){

				if(currentChoiceSet.choices.Length > 0){
					PlayActionSet(currentChoiceSet.nextActions[0]);
				}
				
			} else if(Input.GetKeyDown(KeyCode.D)){

				if(currentChoiceSet.choices.Length > 1){
					PlayActionSet(currentChoiceSet.nextActions[1]);
				}

			} else if(Input.GetKeyDown(KeyCode.S)){

				if(currentChoiceSet.choices.Length > 2){
					PlayActionSet(currentChoiceSet.nextActions[2]);
				}

			} else if(Input.GetKeyDown(KeyCode.W)){

				if(currentChoiceSet.choices.Length > 3){
					PlayActionSet(currentChoiceSet.nextActions[3]);
				}
			}


		}
	}
}