using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tpCavern : MonoBehaviour {

    public GameObject Player;
    public GameObject Syst;
    public GameObject Game;
    public AudioSource BadMusic;
    public List<DialogPage> Dial;

	// Use this for initialization
	void Start () {
        GameObject.Find("GUI/Canvas/BlackScreen").GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {

	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player.transform.position = new Vector3(2760, 859, 0);
        Syst.GetComponent<World_change>().enabled = false;
        Game.SetActive(false);
        BadMusic.Play();
        Player.GetComponent<PlayerBehavior>().m_dialogDisplayer.SetDialog(Dial);
        //GameObject.Find("GUI/Canvas/BlackScreen").GetComponent<Image>().color = new Color(0, 0, 0, 255);
    }
}
