using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    [SerializeField] private UIController _controllerUI;
    [SerializeField] private Animator _animatorUI;

    public static GameController instance;
    public static bool IsPlaying { get; set; }
    public static bool IsFunMode { get; set; } = false;

    public int Score {
        get {
            return _score;
        }
        set {
            if (!IsPlaying) return;
            _score = value;
            _controllerUI.UpdateScoreText(_score);

            if (_score > _bestScore && !IsFunMode) {
                _bestScore = _score;
                _controllerUI.UpdateBestScoreText(_bestScore);
            }
        }
    }

    public int cleanCount = 3;
    public int dequeueCount = 5;

    private int _score = 0;
    private int _bestScore;

    private void Awake() {
        instance = this;
        Application.targetFrameRate = -1;
    }

    private void Start() {
        IsPlaying = true;
        IsFunMode = false;

        _bestScore = PlayerPrefs.GetInt("Best Score", 0);
    }

    public void Lose() {
        _animatorUI.SetTrigger("Lose");
        IsPlaying = false;
        _controllerUI.UpdateLoseMenuScoreText(_score);
    }

    public void BackToMenu() {
        _animatorUI.SetTrigger("Back Menu");
        IsPlaying = false;
    }

    public void Resume(string triggerName) {
        _animatorUI.SetTrigger(triggerName);
        StartCoroutine(ChangeBoolAfterDelay());
    }

    public void EnableFunMode() {
        IsFunMode = true;
    }

    private IEnumerator ChangeBoolAfterDelay() {
        yield return new WaitForSeconds(0.5f);
        IsPlaying = true;
    }

    private void OnDestroy() {
        PlayerPrefs.SetInt("Best Score", _bestScore);
        PlayerPrefs.Save();
    }
}
