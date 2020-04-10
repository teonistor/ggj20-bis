using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GeneralUI : MonoBehaviour {

    [SerializeField] private List<PlayerUI> playerUis;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject endScreenButtons;
    [SerializeField] private Text endScreenText;
    private static IEnumerator<PlayerUI> nextPlayerUI;
    private static GeneralUI instance;

    void Awake () {
        nextPlayerUI = playerUis.GetEnumerator();
        instance = this;   // Don't tell the teacher I did this
        Time.timeScale = 1f;
    }

    internal static void AddPlayerUI (Player player, Faction faction, string keysIndicative) {
        nextPlayerUI.MoveNext();
        PlayerUI pui = nextPlayerUI.Current;
        pui.gameObject.SetActive(true);
        pui.Init(player, faction, keysIndicative);
    }

    internal static void EndGame(string winner) {
        instance.StartCoroutine(instance.EndWaitRestart(winner, 5f));
    }

    IEnumerator EndWaitRestart(string winner, float wait) {
        // Gotta give the Player UI one frame's time to update the killed status of the last killed player
        // (or maybe it's another concurrency bug and this fixes it)
        yield return new WaitForEndOfFrame();

        endScreen.SetActive(true);
        endScreenText.text = winner + " wins!";
        Time.timeScale = 0f;


        float incr = 1f / 30f;
        //float oneWait = incr / wait;
        float h, s, v;
        Color.RGBToHSV(endScreenText.color, out h, out s, out v);
        for (float t = 0f; t < 2f; t += incr) {
            endScreenText.color = Color.HSVToRGB(Mathf.Repeat((h + t), 1f), s, v);
            yield return new WaitForSecondsRealtime(incr);
        }
        endScreenButtons.SetActive(true);
        for (float t = 0f;; t += incr) {
            endScreenText.color = Color.HSVToRGB(Mathf.Repeat((h + t), 1f), s, v);
            yield return new WaitForSecondsRealtime(incr);
        }
    }

    public void Reload () {
        SceneManager.LoadSceneAsync(0);
    }
}
