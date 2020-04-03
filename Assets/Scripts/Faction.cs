using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction : MonoBehaviour {

   // public static Faction Human { get; private set; }

    [SerializeField] internal bool isDefault;
    [SerializeField] internal Color color;
    [SerializeField] internal List<Spot> origin;
    [SerializeField] private string gameName;
    // Symbol.. 

    IEnumerator Start () {
        if (isDefault) {
            foreach(Spot s in FindObjectsOfType<Spot>()) {
                origin.Add(s);
                s.conqueror = this;
            }
            yield break;
        }
        if (origin.Count != 1) {
            throw new System.Exception("Ce naiba");
        }

        // On first frame the default faction happens. On second frame player factions auto-conquer their first spot
        yield return new WaitForEndOfFrame();
        origin[0].Conquer(this);
    }

    internal void Conquer(Faction who) {
        List<Spot> conquerables = new List<Spot>();
        origin.ForEach(spot => spot.AppendConquerablesOf(who, ref conquerables));
        if (conquerables.Count == 0) {
            Debug.LogWarning("No conquerables found");
            return;
        }
        Spot newSpot = conquerables[Random.Range(0, conquerables.Count)];
        newSpot.Conquer(this);
        origin.Add(newSpot);
    }

    internal void ConquerAny () {
        IDictionary<Faction, List<Spot>> conquerables = new Dictionary<Faction, List<Spot>>();
        origin.ForEach(spot => spot.AppendConquerablesOfAny(ref conquerables));
        if (conquerables.Count == 0) {
            Debug.LogWarning("No conquerables found");
            return;
        }
        List<Faction> possible = new List<Faction>(conquerables.Keys);
        Spot newSpot = conquerables[possible[Random.Range(0, possible.Count)]][Random.Range(0, conquerables.Count)];
        newSpot.Conquer(this);
        origin.Add(newSpot);
    }

    internal void Disown(Spot spot) {
        origin.Remove(spot);
    }

    void Update () {

    }

    public override string ToString () {
        return gameName;
    }
}
