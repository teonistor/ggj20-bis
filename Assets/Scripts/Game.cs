using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    private static readonly KeyCode[][] keys = {
            new KeyCode[] { KeyCode.A, KeyCode.W, KeyCode.D, KeyCode.S },
            new KeyCode[] { KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow },
            new KeyCode[] { KeyCode.J, KeyCode.I, KeyCode.L, KeyCode.K },
            new KeyCode[] { KeyCode.V, KeyCode.G, KeyCode.N, KeyCode.B } };
    private static readonly string[] playerUiKeyIndicatives = {
            "WASD", "Arrow Keys", "IJKL", "GVBN" };

    private static int enableFactionMask = 3; // 1 + 2
    private static List<Faction> factionsAlive;

    [SerializeField] private Faction humanFaction;
    [SerializeField] private List<Faction> playerFactions;
    [SerializeField] private List<Spot> startingPositions;
    [SerializeField] private CameraPan cameraPan;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject antibody;

    public void BeginGame() {
        StartCoroutine(BeginGame0());
    }

    public void Quit() {
        Application.Quit();
    }

    IEnumerator BeginGame0 () {
        factionsAlive = new List<Faction>();
        factionsAlive.Add(humanFaction);
        startScreen.SetActive(false);

        IEnumerator keysOne = keys.GetEnumerator();
        IEnumerator keyplayerUiKeyIndicativesOne = playerUiKeyIndicatives.GetEnumerator();
        IEnumerator<Spot> startingPositionOne = startingPositions.GetEnumerator();

        System.Action initPlayers = () => { };

        // On first frame the default faction happens. On second frame player factions auto-conquer their first spot. This happens through Player.Init()
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < 4; i++) {
            if (IsFactionActive(i)) {
                Faction factionCurrent = playerFactions[i];
                factionsAlive.Add(factionCurrent);
                keysOne.MoveNext();
                keyplayerUiKeyIndicativesOne.MoveNext();
                startingPositionOne.MoveNext();

                Player player = gameObject.AddComponent<Player>();
                GeneralUI.AddPlayerUI(player, factionCurrent, (string)keyplayerUiKeyIndicativesOne.Current);

                KeyCode[] keysOneCurrent = (KeyCode[])keysOne.Current;
                Spot startingPositionOneCurrent = startingPositionOne.Current;
                initPlayers += () => player.Init(keysOneCurrent, startingPositionOneCurrent, factionCurrent);
            }
        }
        
        cameraPan.MoveToPuzzle(() => StartCoroutine(DoGame(initPlayers)));
        SoundFX.Begin();
    }

    IEnumerator DoGame (System.Action initPlayers) {
        initPlayers();

        // Human loop..
        int spotCountMax = humanFaction.SpotCount;
        int spotCountMin = 3;
        while (true) {
            float wait = LerpIntervals(humanFaction.SpotCount, spotCountMin, spotCountMax, 3f, 10f);
            yield return new WaitForSeconds(wait);

            Spot moveFrom = humanFaction.RandomSpot;
            if (!moveFrom) {
                print("Human was killed. Exiting human loop.");
                break;
            }

            float chance = LerpIntervals(humanFaction.SpotCount, spotCountMin, spotCountMax, 1f, .25f);
            int howMany = Mathf.CeilToInt(LerpIntervals(humanFaction.SpotCount, spotCountMin, spotCountMax, 3f, 7f));
            int howManyPer = howMany / (factionsAlive.Count-1);
            foreach (Faction f in factionsAlive) {
                if (f != humanFaction) {
                    for (int i = 0; i < howManyPer && i < f.SpotCount; i++) {
                        Spot moveTo = f[i];
                        System.Action callback;
                        if (Random.Range(0f, 1f) <= chance) {
                            callback = () => moveTo.Conquer(humanFaction);
                        } else {
                            callback = () => { };
                        }
                        Antibody a = Instantiate(antibody, transform).GetComponent<Antibody>();
                        a.transform.position = moveFrom.transform.position;
                        a.Init(chance * 4, moveTo.transform.position, callback);
                    }
                }
            }
            SoundFX.Launch();
        }
    }

    float LerpIntervals(float input, float inputLo, float inputHi, float outputLo, float outputHi) {
        return Mathf.Lerp(outputLo, outputHi, (input - inputLo) / (inputHi - inputLo));
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

    internal static void NotifyFactionKilled(Faction faction) {
        factionsAlive.Remove(faction);
        if (factionsAlive.Count == 1) {
            SoundFX.Win();
            GeneralUI.EndGame(factionsAlive[0].gameName);
        }
    }


    void Update () {
        
    }
}
