using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GeneralUI : MonoBehaviour {

    [SerializeField] private List<PlayerUI> playerUis;
    [SerializeField] private GameObject endScreen;
    private static IEnumerator<PlayerUI> nextPlayerUI;
    private static GeneralUI instance;

    void Awake () {
        nextPlayerUI = playerUis.GetEnumerator();
        instance = this; // Ughhhh
    }

    internal static void AddPlayerUI (Player player, Faction faction) {
        nextPlayerUI.MoveNext();
        PlayerUI p = nextPlayerUI.Current;
        p.gameObject.SetActive(true);
        p.Init(player, faction);
    }

    internal static void EndGame(string winner) {
        // TODO Diplay winner
        instance.StartCoroutine(WaitRestart(5f));
    }

    static IEnumerator WaitRestart(float wait) {
        yield return new WaitForSecondsRealtime(wait);
        SceneManager.LoadSceneAsync(0);
    }

    void Start () {

    }

    void Update () {

    }
}
