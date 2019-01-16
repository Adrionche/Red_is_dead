using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerchange : MonoBehaviour {

    public GameObject Syst;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        Syst.GetComponent<World_change>().enabled = true;
    }
}
