using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour {

    // Can be automated
    [SerializeField] internal List<Spot> neighbours;

    internal Faction conqueror;
    private List<Spot> conquerableNeighbours;
    private Material material;

    // private Dictionary<Faction,Spot>

    IEnumerator Start () {
        conquerableNeighbours = new List<Spot>();
        material = GetComponent<Renderer>().material;

        // On the first frame the default faction is established in the singleton. On second frame we set it.
        yield return new WaitForEndOfFrame();
        conqueror = PlayerInput.DefaultFaction;
    }

    internal void Conquer(Faction conqueror) {
        this.conqueror.Disown(this);
        this.conqueror = conqueror;
        RecalculateConquerables();
        foreach (Spot spot in neighbours) {
            spot.RecalculateConquerables();
        }
        material.color = conqueror.color;
        print("I got conquered and now have " + conquerableNeighbours.Count + " conquerable neighbours");
    }

    internal void RecalculateConquerables() {
        conquerableNeighbours.Clear();
        foreach (Spot spot in neighbours) {
            if (spot.conqueror != this.conqueror) {
                conquerableNeighbours.Add(spot);
            }
        }
    }

    internal void AppendConquerablesOf(Faction faction, ref List<Spot> spots) {
        foreach (Spot spot in conquerableNeighbours) {
            print("Looking for conquerables, given " + faction + ", current " + spot.conqueror);
            if (spot.conqueror == faction) {
                spots.Add(spot);
            }
        }
    }

    void Update () {
      

    }

    // Called by custom editor button
    internal void RecalculateNeighbours () {
        neighbours.Clear();
        foreach (Collider c in Physics.OverlapSphere(transform.position, 1f)) {
            Spot s = c.GetComponent<Spot>();
            if (s != null && s != this) {
                neighbours.Add(s);
            }
        }
        print("Recalculated neighbours of " + gameObject.name + ", found " + neighbours.Count);
    }

}
