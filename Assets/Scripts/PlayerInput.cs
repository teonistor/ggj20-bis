using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    private static readonly KeyCode[][] keys = {
            new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.A, KeyCode.S },
            new KeyCode[] { KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow },
            new KeyCode[] { KeyCode.I, KeyCode.O, KeyCode.K, KeyCode.L },
            new KeyCode[] { KeyCode.G, KeyCode.H, KeyCode.B, KeyCode.N } };

    [SerializeField] private Faction humanFaction;
    [SerializeField] private List<Faction> playerFactions;

    // These should eventually come from UI
    [SerializeField] private bool enableFaction1, enableFaction2, enableFaction3, enableFaction4;

    IEnumerator Start () {
        IEnumerator keysOne = keys.GetEnumerator();

        // Array... ?
        if (enableFaction1) {
            keysOne.MoveNext();
            Player player = gameObject.AddComponent<Player>();
            player.Init((KeyCode[])keysOne.Current, playerFactions[0]);
            GeneralUI.AddPlayerUI(player, playerFactions[0]);
        }
        if (enableFaction2) {
            keysOne.MoveNext();
            Player player = gameObject.AddComponent<Player>();
            player.Init((KeyCode[])keysOne.Current, playerFactions[1]);
            GeneralUI.AddPlayerUI(player, playerFactions[1]);
        }
        if (enableFaction3) {
            keysOne.MoveNext();
            Player player = gameObject.AddComponent<Player>();
            player.Init((KeyCode[])keysOne.Current, playerFactions[2]);
            GeneralUI.AddPlayerUI(player, playerFactions[2]);
        }
        if (enableFaction4) {
            keysOne.MoveNext();
            Player player = gameObject.AddComponent<Player>();
            player.Init((KeyCode[])keysOne.Current, playerFactions[3]);
            GeneralUI.AddPlayerUI(player, playerFactions[3]);
        }
        // Input.ke


        //factions = new List<Faction>();
        //foreach(Faction f in FindObjectsOfType<Faction>()) {
        //    factions.Add(f);
        //}
        //print("PlayerInput: init: Found " + factions.Count + " factions");

        //if (factions.Count < 2) {
        //    throw new System.Exception("Ce naiba");
        //}

        while (true) {
            yield return new WaitForSeconds(1f);
            // Give Human some power in here...

            //int i = Random.Range(0, factions.Count);
            //int j = Random.Range(1, factions.Count);
            //if (j <= i) j--;
            //print(factions[i] + ", who owns " + factions[i].origin.Count + ", is about to conquer " + factions[j]);
            //factions[i].Conquer(factions[j]);
        }
    }


    void Update () {
        
    }
}
