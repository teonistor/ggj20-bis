using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    private static readonly KeyCode[][] keys = {
            new KeyCode[] { KeyCode.D, KeyCode.W, KeyCode.A, KeyCode.S },
            new KeyCode[] { KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow },
            new KeyCode[] { KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L },
            new KeyCode[] { KeyCode.G, KeyCode.V, KeyCode.B, KeyCode.N } };

    private static int enableFactionMask = 3; // 1 + 2

    [SerializeField] private Faction humanFaction;
    [SerializeField] private List<Faction> playerFactions;
    [SerializeField] private List<Spot> startingPositions;
    [SerializeField] private GameObject startScreen;

    public void BeginGame() {
        StartCoroutine(DoGame());
    }

    public void Quit() {
        Application.Quit();
    }

    IEnumerator DoGame () {
        startScreen.SetActive(false);

        IEnumerator keysOne = keys.GetEnumerator();
        IEnumerator<Spot> startingPositionOne = startingPositions.GetEnumerator();

        // On first frame the default faction happens. On second frame player factions auto-conquer their first spot. This happens through Player.Init()
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < 4; i++) {
            if (IsFactionActive(i)) {
                keysOne.MoveNext();
                startingPositionOne.MoveNext();

                Player player = gameObject.AddComponent<Player>();
                player.Init((KeyCode[])keysOne.Current, startingPositionOne.Current, playerFactions[i]);
                GeneralUI.AddPlayerUI(player, playerFactions[i]);
            }
        }

        while (true) {
            yield return new WaitForSeconds(1f);
            // TODO Give Human some power in here...

            //int i = Random.Range(0, factions.Count);
            //int j = Random.Range(1, factions.Count);
            //if (j <= i) j--;
            //print(factions[i] + ", who owns " + factions[i].origin.Count + ", is about to conquer " + factions[j]);
            //factions[i].Conquer(factions[j]);
        }
    }

    internal static bool PressButtonForFaction(int faction) {
        int mask = 1 << faction;
        // Can't disable if it would result in less than 2 remaining enabled
        if ((enableFactionMask & mask)!= 0 && (enableFactionMask & 1) + ((enableFactionMask>>1) & 1) + ((enableFactionMask >> 2) & 1) + ((enableFactionMask >> 3) & 1) < 3 ) {
            return true;
        }
        enableFactionMask ^= mask;
        return (enableFactionMask & mask) != 0;
    }

    internal static bool IsFactionActive(int faction) {
        return (enableFactionMask & (1 << faction)) != 0;
    }


    void Update () {
        
    }
}
