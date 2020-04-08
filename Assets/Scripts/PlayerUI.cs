using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField] private Text text;
    [SerializeField] private Image pic, energyBar, energyContour, boostBar, boostContour;
    [SerializeField] private GameObject keyText;

    private Player player;
    private TextMesh keyText1;
    private TextMesh keyText2;
    private Vector3 keyTextDisplacement;
    private RectTransform energyBarRectTransform, boostBarRectTransform;
    private float energyBarOffsetMaxX, energyBarOffsetMaxY, boostBarOffsetMaxX, boostBarOffsetMaxY;

    internal void Init(Player player, Faction faction) {
        this.player = player;
        text.text = faction.gameName;
        pic.sprite = faction.image;
        text.color = faction.color;
        energyBar.color = faction.color;
        boostBar.color = faction.color;

        energyBarRectTransform = energyBar.GetComponent<RectTransform>();
        boostBarRectTransform = boostBar.GetComponent<RectTransform>();
        energyBarOffsetMaxX = energyBarRectTransform.offsetMax.x;
        energyBarOffsetMaxY = energyBarRectTransform.offsetMax.y;
        boostBarOffsetMaxX = boostBarRectTransform.offsetMax.x;
        boostBarOffsetMaxY = boostBarRectTransform.offsetMax.y;

        boostBar.gameObject.SetActive(false);
        boostContour.gameObject.SetActive(false);

        keyText1 = Instantiate(keyText).GetComponent<TextMesh>();
        keyText1.color = faction.color;
        keyText2 = Instantiate(keyText).GetComponent<TextMesh>();
        keyText2.color = faction.color;
        keyTextDisplacement = Vector3.zero;// TODO fix to transform.position.normalized * -.5f;
        keyTextDisplacement.y = keyText1.transform.position.y;
    }

    void Start () {

    }

    void Update () {
        if (player) {
            //print("Min " + energyBarRectTransform.offsetMin + " max " + energyBarRectTransform.offsetMax);
            //print("AMin " + energyBarRectTransform.anchorMin + " amax " + energyBarRectTransform.anchorMax);
            energyBarRectTransform.offsetMax = new Vector2(Mathf.Lerp(energyBarRectTransform.offsetMin.x, energyBarOffsetMaxX, player.Energy), energyBarOffsetMaxY);

            if (player.CurrentKeyCode != 0) {
                keyText1.transform.position = player.CurrentlyAttacking.transform.position + keyTextDisplacement;
                keyText1.text = NiceKey(player.CurrentKeyDirection);

                if (player.AlternateKeyCode != 0) {
                    keyText2.transform.position = player.AlternateAttacking.transform.position + keyTextDisplacement;
                    keyText2.text = NiceKey(player.AlternateKeyDirection);
                } else {
                    keyText2.text = "";
                }

            } else {
                keyText1.text = "";
                keyText2.text = "";
            }
        }
    }

    string NiceKey(KeyDirection direction) {
        switch(direction) {
        // Arrows are such a bitch! It's hard to find a set that works in Unity. These triangles are nice, but a bit too easily confused...
            case KeyDirection.Left: return "◀";
            case KeyDirection.Up: return "▲";
            case KeyDirection.Right: return "▶"; // "➔";
            case KeyDirection.Down: return "▼";
            default: return "";
        }
    }
}
