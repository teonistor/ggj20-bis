using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    private List<Faction> factions;


    IEnumerator Start () {
        factions = new List<Faction>();
        foreach(Faction f in FindObjectsOfType<Faction>()) {
            factions.Add(f);
        }
        print("PlayerInput: init: Found " + factions.Count + " factions");

        if (factions.Count < 2) {
            throw new System.Exception("Ce naiba");
        }

        while (true) {
            yield return new WaitForSeconds(1f);

            int i = Random.Range(0, factions.Count);
            int j = Random.Range(1, factions.Count);
            if (j <= i) j--;
            print(factions[i] + ", who owns " + factions[i].origin.Count + ", is about to conquer " + factions[j]);
            factions[i].Conquer(factions[j]);
        }
    }


    void Update () {
        
    }
}
