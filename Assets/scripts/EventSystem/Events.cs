using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BeamMeRoundScotty{

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