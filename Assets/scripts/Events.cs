using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BeamMeRoundScotty{

	public class CutsceneEvent : GameEvent {
		//public string[] lines;//TODO: Replace this with whatever "screenplay" scriptableObject we end up making
		public CutsceneManager.ActionSet actionSet;

		public CutsceneEvent(CutsceneManager.ActionSet actionSetToPlay){
			this.actionSet = actionSetToPlay;
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