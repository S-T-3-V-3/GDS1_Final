using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

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

    //Score Tweener
    Tween scoreTween;
    int previousScoreTarget;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Initialize());

         //This is just for tween initialization since I couldn't figure it out
        scoreTween = DOTween.To(()=> currentScore, x=> currentScore = x, 0, 1);
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
        int targetScore = currentScore + value;

        if(scoreTween.IsActive())
        {
            scoreTween.Kill();
            targetScore = previousScoreTarget + value;
        }

        GameManager.Instance.playerScore = targetScore;

        experienceBar.AddExperience(value);

        scoreTween = DOTween.To(()=> currentScore, x=> currentScore = x, targetScore, 1);
        previousScoreTarget = targetScore;
    }

    private void Update()
    {
        //Runs every frame the tweener is alive
        if(scoreTween.IsActive())
        {
            scoreText.text = $"Score: {currentScore}";
        }
    }
}
