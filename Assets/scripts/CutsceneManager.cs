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
        TempLineSet currentTempLineSet;

		int currentLineIndex;
		string currentLineText = "";
		int currentLineLength;
		float currentLinePosition;

		TempChoiceSet currentTempChoiceSet;

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

        public ActionBaseClass LineSetForQKey;

		public class ActionSet{
			
		}

		public class TempLineSet : ActionSet {
			public string[] lines;
			public ActionSet nextSet;

			public TempLineSet(string[] lines){
				this.lines = lines;
			}
		}

		public class TempChoiceSet : ActionSet {
			public string mainText;
			public string[] choices;
			public ActionSet[] nextActions;

			public TempChoiceSet(string mainText, string[] choices, ActionSet[] nextActions){
				this.mainText = mainText;
				this.choices = choices;
				this.nextActions = nextActions;
			}
		}

		// Use this for initialization
		void Start () {
			screenCanvas = GameObject.FindGameObjectWithTag("ScreenCanvas").GetComponent<Canvas>();

            Events.instance.AddListener<CutsceneEvent>(OnCutsceneEvent);
			Events.instance.AddListener<TempCutsceneEvent>(OnTempCutsceneEvent);

			if(writingState == WritingState.Hidden){
				dialogPanelRef.SetActive(false);
			}
		}
		
		// Update is called once per frame
		void Update () {

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Events.instance.Raise(new CutsceneEvent(LineSetForQKey));
            }

			if(Input.GetKeyDown(KeyCode.L)){

				string[] testLines = {"Bacon ipsum dolor amet tenderloin short ribs ribeye id eiusmod. Ham hock shoulder flank laborum porchetta velit hamburger cupim pork loin rump ut ullamco cupidatat ut pork belly. Cupim ea dolore meatball ground round shank id turkey ut drumstick eiusmod. Consectetur irure deserunt swine ground round fatback.", "Hello?", "Is anybody there?"};
				TempLineSet exampleLineSet = new TempLineSet(testLines);

				Events.instance.Raise(new TempCutsceneEvent(exampleLineSet));
			}

			if(Input.GetKeyDown(KeyCode.C)){

				string[] convo0 = new string[]{"It was then that I saw it...", "A mutant cow!"};
				TempLineSet lineSet0 = new TempLineSet(convo0);

				string[] convo1 = new string[]{"Whatever you say.", "That coffee mug was getting pretty old anyway."};
				TempLineSet lineSet1 = new TempLineSet(convo1);

				string[] convo2 = new string[]{"You put your right hand in.", "You take your right hand out.", "You put your right hand in and you shake it all about."};
				TempLineSet lineSet2 = new TempLineSet(convo2);

				string[] convo3 = new string[]{"There are 10 kinds of people in this world.", "Those who understand binary and those who don.t"};
				TempLineSet lineSet3 = new TempLineSet(convo3);

				string[] choices = {"It was a dark and stormy night...", "I suppose you're right...", "Let's dance!", "Excelsior!"};
				TempChoiceSet choiceSet = new TempChoiceSet("As I see it, you've got a few choices...", choices, new ActionSet[]{lineSet0, lineSet1, lineSet2, lineSet3});

				Events.instance.Raise(new TempCutsceneEvent(choiceSet));
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

        void OnCutsceneEvent(CutsceneEvent e)
        {
            PlayActionSet(e.actionSet);
        }

        void OnTempCutsceneEvent(TempCutsceneEvent e){

			PlayTempActionSet(e.actionSet);
		}

        void PlayActionSet(ActionBaseClass actionSet)
        {
            Debug.Log(actionSet.GetType().ToString());
            if (actionSet is LineSet)
            {
                print("Did I make it here?");
                PlayLineSet((LineSet)actionSet);
            }
            else if (actionSet is ChoiceSet)
            {
                PlayChoiceSet((ChoiceSet)actionSet);
            }
        }

        void PlayTempActionSet(ActionSet actionSet){
			if(actionSet is TempLineSet){
				PlayTempLineSet((TempLineSet)actionSet);
			} else if(actionSet is TempChoiceSet){
				PlayTempChoiceSet((TempChoiceSet)actionSet);
			}
		}

        void PlayLineSet(LineSet lineSet)
        {

            writingState = WritingState.WritingLine;

            currentChoiceSet = null;
            currentLineSet = lineSet;
            currentLineIndex = 0;

            Line currentLine = currentLineSet.lines[currentLineIndex];

            BeginWritingLine(currentLine.line, currentLine.speakerName);

            ShowPanel();

            HideChoicePanel();
        }

        void PlayTempLineSet(TempLineSet lineSet){

			writingState = WritingState.WritingLine;

			currentTempChoiceSet = null;
			currentTempLineSet = lineSet;
			currentLineIndex = 0;

			BeginWritingLine(currentTempLineSet.lines[currentLineIndex]);

			ShowPanel();

			HideChoicePanel();
		}

		void PlayTempChoiceSet(TempChoiceSet choiceSet){

			writingState = WritingState.WritingChoice;

			currentTempLineSet = null;
			currentTempChoiceSet = choiceSet;

			BeginWritingLine(choiceSet.mainText);
			PopulateTempChoices(choiceSet.choices);

			ShowPanel();
			ShowChoicePanel();
		}


        void PlayChoiceSet(ChoiceSet choiceSet)
        {

            writingState = WritingState.WritingChoice;

            currentLineSet = null;
            currentChoiceSet = choiceSet;

            BeginWritingLine(choiceSet.lineToPlay.line, choiceSet.lineToPlay.speakerName);
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

        void BeginWritingLine(string line, string speakerName)
        {
            speakerNameTextRef.text = speakerName;
            currentLineText = line;
            currentLineLength = line.Length;
            currentLinePosition = 0;
        }

        [System.Obsolete]
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
			if(currentTempLineSet != null && currentLineIndex + 1 < currentTempLineSet.lines.Length){
				currentLineIndex++;

				BeginWritingLine(currentTempLineSet.lines[currentLineIndex]);
				writingState = WritingState.WritingLine;

            }
            else if (currentLineSet != null && currentLineIndex + 1 < currentLineSet.lines.Count)
            {
                currentLineIndex++;

                BeginWritingLine(currentLineSet.lines[currentLineIndex].line, currentLineSet.lines[currentLineIndex].speakerName);
                writingState = WritingState.WritingLine;
            }
            else {
				//We're down with our current set of lines, close the dialog panel
                //TODO: we need to check whats next!
				HidePanel();
			}
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

        void PopulateTempChoices(string[] choicesText){
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

				if(currentTempChoiceSet.choices.Length > 0){
					PlayTempActionSet(currentTempChoiceSet.nextActions[0]);
				}
				
			} else if(Input.GetKeyDown(KeyCode.D)){

				if(currentTempChoiceSet.choices.Length > 1){
					PlayTempActionSet(currentTempChoiceSet.nextActions[1]);
				}

			} else if(Input.GetKeyDown(KeyCode.S)){

				if(currentTempChoiceSet.choices.Length > 2){
					PlayTempActionSet(currentTempChoiceSet.nextActions[2]);
				}

			} else if(Input.GetKeyDown(KeyCode.W)){

				if(currentTempChoiceSet.choices.Length > 3){
					PlayTempActionSet(currentTempChoiceSet.nextActions[3]);
				}
			}


		}
	}
}