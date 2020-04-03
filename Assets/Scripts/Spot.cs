using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour {

    // Automated by custom editor button
    [SerializeField] internal List<Spot> neighbours;

    internal Faction conqueror;
    private List<Spot> conquerableNeighbours;
    private SpriteRenderer spriteRenderer;

    // private Dictionary<Faction,Spot>

    void Start () {
        conquerableNeighbours = new List<Spot>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    internal void Conquer(Faction conqueror) {
        this.conqueror.Disown(this);
        this.conqueror = conqueror;
        RecalculateConquerables();
        foreach (Spot spot in neighbours) {
            spot.RecalculateConquerables();
        }
        spriteRenderer.color = conqueror.color;
        print("I got conquered and now have " + conquerableNeighbours.Count + " conquerable neighbours out of " + neighbours.Count);
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

    // To be called by custom editor button
    internal void RecalculateNeighbours () {
        neighbours = new List<Spot>();
        foreach (Collider c in Physics.OverlapSphere(transform.position, 1f)) {
            Spot s = c.GetComponent<Spot>();
            if (s != null && s != this) {
                neighbours.Add(s);
            }
        }

        if (neighbours.Count > 0 && neighbours.Count < 5) {
            Debug.Log("Recalculated neighbours of " + gameObject.name + ", found " + neighbours.Count);
        } else {
            Debug.LogWarning("Recalculated neighbours of " + gameObject.name + ", found " + neighbours.Count);
        }
    }

}
