using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeamMeRoundScotty{
	public class CutsceneManager : MonoBehaviour {

        
        LineSet currentLineSet;
		int currentLineIndex;
        //TempLineSet currentTempLineSet;

        //TempChoiceSet currentTempChoiceSet;
        ChoiceSet currentChoiceSet;


        //[Header("Canvas References")]
        //Canvas screenCanvas;

		public DialogPanel dialogPanelRef;
        public GameObject dialogPanelPrefab;
        
        public ActionBaseClass LineSetForQKey;
        public ActionBaseClass LineSetForWKey;
		/*
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
		*/

		// Use this for initialization
		void Start () {

			//Find the primary screen canvas
			Canvas screenCanvas = GameObject.FindGameObjectWithTag("ScreenCanvas").GetComponent<Canvas>();

			//Find the dialog panel
			dialogPanelRef = DialogPanel.instance;
			//If the dialog panel hasn't been made yet...
			if(dialogPanelRef == null){
				dialogPanelRef = Instantiate(dialogPanelPrefab, screenCanvas.transform).GetComponent<DialogPanel>();
				dialogPanelRef.transform.SetAsLastSibling();
			}

			dialogPanelRef.LineComplete += OnLineComplete;
			dialogPanelRef.ChoiceMade += OnChoiceMade;

            Events.instance.AddListener<CutsceneEvent>(OnCutsceneEvent);

		}
		
		// Update is called once per frame
		void Update () {

			#region Custom cutscenes for prototyping purposes
			if (Input.GetKeyDown(KeyCode.Q))
			{
				Events.instance.Raise(new CutsceneEvent(LineSetForQKey));
			}

			if (Input.GetKeyDown(KeyCode.W))
			{
				Events.instance.Raise(new CutsceneEvent(LineSetForWKey));
			}
			#endregion
		}

        

        void OnCutsceneEvent(CutsceneEvent e)
		{
            PlayActionSet(e.actionSet);
        }

        void PlayActionSet(ActionBaseClass actionSet)
        {
			if(actionSet == null){
				dialogPanelRef.HideAll();
			}

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
            //writingState = WritingState.WritingLine;

            currentChoiceSet = null;
            currentLineSet = lineSet;
            currentLineIndex = 0;

            Line currentLine = currentLineSet.lines[currentLineIndex];

            dialogPanelRef.DisplayLine(currentLine.line, currentLine.speakerName);

        }

        void PlayChoiceSet(ChoiceSet choiceSet)
        {
            //writingState = WritingState.WritingChoice;

            currentLineSet = null;
            currentChoiceSet = choiceSet;
            
			dialogPanelRef.DisplayChoice(choiceSet.lineToPlay.line, choiceSet.lineToPlay.speakerName, choiceSet.choices);
            //PopulateChoices(choiceSet.choices);

            
        }

		void OnLineComplete(){
			
			//Advance to next line
			if (currentLineIndex + 1 < currentLineSet.lines.Count)
			{
				currentLineIndex++;
				dialogPanelRef.DisplayLine(currentLineSet.lines[currentLineIndex].line, currentLineSet.lines[currentLineIndex].speakerName);

			} else {
				//There are no more lines in this set
				if(currentLineSet.doNext != null){
					PlayActionSet(currentLineSet.doNext);
				} else {
					//We're finished
					dialogPanelRef.HideAll();
				}
			}
		}

		void OnChoiceMade(int choiceIndex){
			
			PlayActionSet(currentChoiceSet.choices[choiceIndex].doNext);

		}
	}
}