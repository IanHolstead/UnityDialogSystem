using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BeamMeRoundScotty{

	public class TempCutsceneEvent : GameEvent {
		//public string[] lines;//TODO: Replace this with whatever "screenplay" scriptableObject we end up making
		public CutsceneManager.ActionSet actionSet;

		public TempCutsceneEvent(CutsceneManager.ActionSet actionSetToPlay){
			this.actionSet = actionSetToPlay;
		}
	}

    public class CutsceneEvent : GameEvent
    {
        public ActionBaseClass actionSet;

        public CutsceneEvent(ActionBaseClass actionSetToPlayFirst)
        {
            actionSet = actionSetToPlayFirst;
        }
    }
/*
    public class SceneChangeRequestEvent : GameEvent
    {
        
        //public Scene fromScene;
        public string toSceneName;

        public SceneChangeRequestEvent(string toSceneName)
        {
          //  this.fromScene = fromScene;
            this.toSceneName = toSceneName;
        }
    }
*/
  
}