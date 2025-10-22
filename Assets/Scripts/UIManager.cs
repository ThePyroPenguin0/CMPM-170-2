using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public Canvas canvas;
    public TMP_Text gameoverText;

    [Header("Text Options")]
    public string[] winMessages;
    public string[] lossMessages;

    void Start()
    {
        gameoverText.gameObject.SetActive(false);
    }
    
    public void ShowRandomText(bool won)
    {
        if (won)
        {
            int index = Random.Range(0, winMessages.Length);
            gameoverText.text = winMessages[index];
        }
        else
        {
            int index = Random.Range(0, lossMessages.Length);
            gameoverText.text = lossMessages[index];
        }
        gameoverText.gameObject.SetActive(true);
        StartCoroutine(HideTextAfterDelay(5f));
    }

    System.Collections.IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameoverText.gameObject.SetActive(false);
    }
}
