using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World_change : MonoBehaviour {

    public Transform Player;
    public AudioSource GoodMusic;
    public AudioSource BadMusic;

    private float _startTime;
    private float _time;
    private int _swap;
    private bool _inRed = false;
    private Vector3 _offset;


    // Use this for initialization
    void Start () {
        GoodMusic.Play();
        _startTime = Time.time;
        _offset = new Vector3(0, 1552, 0);
	}
	
	// Update is called once per frame
	void Update () {
        _time = Time.time - _startTime;
        _swap = Random.Range(5, 10);
        if (_time > _swap)
        {
            _inRed = !_inRed;
            _startTime = Time.time;
            if (_inRed)
            {
                GoodMusic.Stop();
                BadMusic.Play();
                Player.position -= _offset;
            } else
            {
                BadMusic.Stop();
                GoodMusic.Play();
                Player.position += _offset;
            }
        }

	}
}
