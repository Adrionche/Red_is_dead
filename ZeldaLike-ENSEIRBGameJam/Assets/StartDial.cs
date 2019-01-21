using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDial : MonoBehaviour {

    public List<DialogPage> Dial;
    public GameObject Player;
    // Use this for initialization
    void Start () {
        Player.GetComponent<PlayerBehavior>().m_dialogDisplayer.SetDialog(Dial);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
