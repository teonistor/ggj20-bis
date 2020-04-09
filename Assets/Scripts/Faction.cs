using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction : MonoBehaviour {

   // public static Faction Human { get; private set; }

    [SerializeField] internal bool isDefault;
    [SerializeField] internal Color color;
    [SerializeField] internal Sprite image;
    [SerializeField] internal string gameName;

    private Player player;
    internal PlayerUI playerUI;   // The spagetti nature of this code has been set in stone once I added this :(
    private List<Spot> origin;

    void Start () {
        origin = new List<Spot>();
        if (isDefault) {
            foreach(Spot s in FindObjectsOfType<Spot>()) {
                origin.Add(s);
                s.InitialAssignHuman(this);
            }
            return;
        }
        //if (origin.Count != 1) {
        //    throw new System.Exception("Ce naiba");
        //}

        // On first frame the default faction happens. On second frame player factions auto-conquer their first spot
        // yield return new WaitForEndOfFrame();
        // origin[0].Conquer(this);
    }

    // Note this isn't called for Human as it has no player
    internal void Init (Player player, Spot startingSpot) {
        this.player = player;
        origin.Add(startingSpot);
        startingSpot.Conquer(this);
    }

    List<Object> sth;
    internal Object this[int i] { get => sth[i]; set => sth[i] = value; }

    internal HashSet<Faction> FindNeighbouringFactionsExceptSelf() {
        HashSet<Faction> factions = new HashSet<Faction>();
        origin.ForEach(spot => spot.ForEachNeighbouringFactionExcept(this, factions.Add));
        return factions;
    }

    internal List<Spot> FindNeighbouringSpotsOf(Faction faction) {
        List<Spot> spots = new List<Spot>();
        origin.ForEach(spot => spot.ForEachNeighbouringSpotOf(faction, spots.Add));
        return spots;
    }

    internal void Conquer(Spot what) {
        what.Conquer(this);
        origin.Add(what);
    }
    
    internal void Disown (Spot spot) {
        origin.Remove(spot);
        if (player) {
            player.NotifyConquer();
        }
        if (origin.Count == 0) {
            if (playerUI) {
                playerUI.NotifyKilled();
            }
            Game.NotifyFactionKilled(this);
        }
    }
    
    public override string ToString () {
        return gameName;
    }
}
