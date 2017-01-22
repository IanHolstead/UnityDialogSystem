using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    int count;
    bool test = false;
	// Use this for initialization
	void Start () {
        count = 0;
        KeyStore.UpdateKey("intTest", count);
        KeyStore.UpdateKey("boolTest", test);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.I))
        {
            count++;
            KeyStore.UpdateKey("intTest", count);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Value: " + KeyStore.GetIntKeyValue("intTest"));
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            KeyStore.UpdateKey("stringTest", "cat");
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            test ^= true;
            KeyStore.UpdateKey("boolTest", test);
        }
    }
}
