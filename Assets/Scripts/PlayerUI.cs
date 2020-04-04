using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField] private Text text;

    private Player player;
    private string textFormat;

    internal void Init(Player player, Faction faction) {
        this.player = player;
        this.textFormat = faction.gameName + "\n{0}";
    }

    void Start () {

    }

    void Update () {
        if (player) {
            text.text = string.Format(textFormat, "".PadRight(Mathf.CeilToInt(player.Energy * 7), '#'));
        }
    }
}
