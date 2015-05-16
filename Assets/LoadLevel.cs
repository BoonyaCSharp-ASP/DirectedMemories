using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {

    public string levelName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            Debug.Log("Loading next level: " + levelName);
        }
    }

    void OnTriggerExit(Collider other)
    {
    }
}
