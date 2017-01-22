using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeamMeRoundScotty{
	public class CutsceneManager : MonoBehaviour {

        private enum WritingState
        {
            Hidden,
            WritingLine,
            WritingChoice,
            WaitingForChoice,
            FinishedLine,
        }

        private WritingState writingState = WritingState.Hidden;

        int currentLineIndex;
		string currentLineText = "";
		int currentLineLength;
		float currentLinePosition;
        public float charactersPerSecond = 3f;

        LineSet currentLineSet;
        TempLineSet currentTempLineSet;

        TempChoiceSet currentTempChoiceSet;
        ChoiceSet currentChoiceSet;


        [Header("Canvas References")]
        Canvas screenCanvas;

        public GameObject dialogPanelRef;
        public Text speakerNameTextRef;
        public Text spokenTextRef;
        
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
        public ActionBaseClass LineSetForWKey;

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

			if(writingState == WritingState.Hidden){
                HideChoicePanel();
                dialogPanelRef.SetActive(false);
			}
		}
		
		// Update is called once per frame
		void Update () {

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Events.instance.Raise(new CutsceneEvent(LineSetForQKey));
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                Events.instance.Raise(new CutsceneEvent(LineSetForWKey));
            }
            PlayText();
		}

        void PlayText()
        {
            switch (writingState)
            {
                case WritingState.WritingLine:
                    {
                        //Advance to the end of the line if the player presses space
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            FinishWriting();
                        }

                        //Advance the writing "cursor" over time
                        if (currentLinePosition < currentLineLength)
                        {
                            //Advance our cursor position
                            currentLinePosition = Mathf.Clamp(currentLinePosition + Time.deltaTime * charactersPerSecond, 0f, currentLineLength);
                        }

                        //Update the displayed Text
                        spokenTextRef.text = currentLineText.Substring(0, (int)currentLinePosition);

                        if (currentLinePosition == currentLineText.Length)
                        {
                            writingState = WritingState.FinishedLine;
                        }
                        break;
                    }

                case WritingState.FinishedLine:
                    {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            AdvanceToNextLine();
                        }
                        break;
                    }

                case WritingState.WritingChoice:
                    {
                        //Advance to the end of the line if the player presses space
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            FinishWriting();
                        }

                        //Advance the writing "cursor" over time
                        if (currentLinePosition < currentLineLength)
                        {
                            //Advance our cursor position
                            currentLinePosition = Mathf.Clamp(currentLinePosition + Time.deltaTime * charactersPerSecond, 0f, currentLineLength);
                        }

                        //Update the displayed Text
                        spokenTextRef.text = currentLineText.Substring(0, (int)currentLinePosition);

                        if (currentLinePosition == currentLineText.Length)
                        {
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

        void PlayActionSet(ActionBaseClass actionSet)
        {
            if (actionSet is LineSet)
            {
                PlayLineSet((LineSet)actionSet);
            }
            else if (actionSet is ChoiceSet)
            {
                PlayChoiceSet((ChoiceSet)actionSet);    
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

		void FinishWriting(){
			currentLinePosition = currentLineLength;	
		}

        void AdvanceToNextLine()
        {
            if (currentLineSet != null && currentLineIndex + 1 < currentLineSet.lines.Count)
            {
                currentLineIndex++;

                BeginWritingLine(currentLineSet.lines[currentLineIndex].line, currentLineSet.lines[currentLineIndex].speakerName);
                writingState = WritingState.WritingLine;
            }
            else
            {
                if (currentLineSet.doNext != null)
                {
                    PlayActionSet(currentLineSet.doNext);
                }
                else
                {
                    HidePanel();
                }
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

        void ListenForChoice()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (currentChoiceSet.choices.Count > 0)
                {
                    PlayActionSet(currentChoiceSet.choices[0].doNext);
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (currentChoiceSet.choices.Count > 1)
                {
                    PlayActionSet(currentChoiceSet.choices[1].doNext);
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (currentChoiceSet.choices.Count > 2)
                {
                    PlayActionSet(currentChoiceSet.choices[2].doNext);
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                if (currentChoiceSet.choices.Count > 3)
                {
                    PlayActionSet(currentChoiceSet.choices[3].doNext);
                }
            }
        }
	}
}