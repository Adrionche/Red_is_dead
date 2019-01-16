using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tpCavern : MonoBehaviour {

    public GameObject Player;
    public GameObject Syst;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player.transform.position = new Vector3(2760, 859, 0);
        Syst.GetComponent<World_change>().enabled = false;
    }
}
