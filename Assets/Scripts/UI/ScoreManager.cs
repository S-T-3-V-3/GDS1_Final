using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // TODO: Kill me, move to session data
    private int currentScore = 0;

    //Other Script References
    BasicPlayer playerRef;
    [SerializeField]
    private EXPBar experienceBar;

    //Set in inspector
    [SerializeField]
    private TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize() {
        while (playerRef == null) {
            if (GameManager.Instance.playerController != null)
            {
                playerRef = GameManager.Instance.playerController.GetComponent<BasicPlayer>();
                yield return new WaitForEndOfFrame();

                GameManager.Instance.OnAddScore.AddListener(OnAddScore);
            }

            yield return null;
        }
    }
       

    public void OnAddScore(int value, Vector3 pos)
    {
        currentScore += value;
        scoreText.text = $"Score: {currentScore}";

        experienceBar.AddExperience(value);
    }
}
