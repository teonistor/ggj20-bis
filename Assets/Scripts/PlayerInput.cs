using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public static Faction DefaultFaction { get; private set; }
    private List<Faction> factions;


    IEnumerator Start () {
        factions = new List<Faction>();
        foreach(Faction f in FindObjectsOfType<Faction>()) {
            factions.Add(f);
            if (f.isDefault) {
                if (DefaultFaction == null) {
                    DefaultFaction = f;
                    continue;
                }
                throw new System.Exception("Ce naiba");
            }
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
            print(factions[i] + " is about to conquer " + factions[j]);
            factions[i].Conquer(factions[j]);
        }
    }


    void Update () {
        
    }
}
