using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoungeTeamManager : MonoBehaviour {
	public static List<string> teams = new List<string>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void AddTeam (string teamToAdd) {
		if (teams.Contains (teamToAdd))
			RemoveTeam (teamToAdd);
		teams.Add (teamToAdd);
	}
	public static void RemoveTeam (string teamToRemove) {
		teams.Remove (teamToRemove);
	}
}
