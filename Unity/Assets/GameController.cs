using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public static PlayerController Player;

	// Use this for initialization
	void Start () {
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
	}
}
