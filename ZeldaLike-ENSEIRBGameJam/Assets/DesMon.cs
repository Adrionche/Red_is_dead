﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesMon : MonoBehaviour {

    public List<DialogPage> Dial;
    public GameObject Player;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player.transform.position = new Vector2(2736, 731);
        Player.GetComponent<PlayerBehavior>().m_dialogDisplayer.SetDialog(Dial);
        
    }
}