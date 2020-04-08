using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction : MonoBehaviour {

   // public static Faction Human { get; private set; }

    [SerializeField] internal bool isDefault;
    [SerializeField] internal Color color;
    [SerializeField] internal Sprite image;
    [SerializeField] internal string gameName;

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

    internal void ConquerStartingSpot (Spot startingSpot) {
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

    //internal void Conquer(Faction who) {
    //    List<Spot> conquerables = new List<Spot>();
    //    origin.ForEach(spot => spot.AppendConquerablesOf(who, ref conquerables));
    //    if (conquerables.Count == 0) {
    //        Debug.LogWarning("No conquerables found");
    //        return;
    //    }
    //    Spot newSpot = conquerables.RandomElement();
    //    newSpot.Conquer(this);
    //    origin.Add(newSpot);
    //}

    //internal Spot FindPlausibleSpotToConquer() {
    //    IDictionary<Faction,Spot> conquerables = new Dictionary<Faction,Spot>();
    //    // Still not good enough when we meet multiple factions
    //    origin.ForEach(spot => spot.AppendNearestConquerablesOfAny(ref conquerables));
    //    if (conquerables.Count == 0) {
    //        // Debug.LogWarning("No conquerables found");
    //        return null;
    //    }
    //    return conquerables.RandomValue();
    //}

    //internal void ConquerAny () {
    //    Spot newSpot = FindPlausibleSpotToConquer();
    //    if (newSpot == null) {
    //        Debug.LogWarning("No conquerables found");
    //        return;
    //    }
    //    newSpot.Conquer(this);
    //    origin.Add(newSpot);
    //}

    internal void Disown(Spot spot) {
        origin.Remove(spot);
        // TODO possibly notify player
        // TODO if spot currently under attack is no longer reachable, reset energy and attack
    }

    void Update () {

    }

    public override string ToString () {
        return gameName;
    }
}
