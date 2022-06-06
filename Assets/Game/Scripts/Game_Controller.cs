using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_Controller : MonoBehaviour
{
    [Header("Player props")]
    public Player _player;
    public float _score;
    public float _playerHp;
    public float _playerLifes;
    float _playerMhp;
    
    [Header("UI control")]
    public Text _scoreText;
    public Text _lifeText;
    public RectTransform _hpBar;
    public Text _hpText;
    float _hpBarWidth;
    float _hpBarHeight;
    [SerializeField] Scene_Loader SceneLoader;

    // Start is called before the first frame update
    private void Awake()
    {
        SceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<Scene_Loader>();
    }
    void Start()
    {
        _playerMhp = _player._maxHp;
        _hpBarWidth = _hpBar.rect.width;
        _hpBarHeight = _hpBar.rect.height;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerProps();
        UpdateText();
        UpdateLifeBar();
        UpdateStageUp();
    }
    void UpdatePlayerProps()
    {
        _score = _player._score;
        _playerHp = _player._hp;
        _playerLifes = _player._lifes;
        
    }

    void UpdateText()
    {
        _scoreText.text = _score.ToString();
        _lifeText.text = _playerLifes.ToString();
    }

    void UpdateLifeBar()
    {
        var percent = _playerHp / _playerMhp;
        var newX = _hpBarWidth * percent;
        _hpText.text = _playerHp.ToString();
        _hpBar.sizeDelta = new Vector2(newX, _hpBarHeight);
        if(_playerHp <= 0)
        {
            PlayerDeathProcess();
        }
    }
    void ResetStage()
    {
        PlayerPrefs.SetFloat("_playerLifes", _player._lifes);
        var sceneId = SceneManager.GetActiveScene().buildIndex;
        SceneLoader.Transition(sceneId, true);
    }
    void PlayerDeathProcess()
    {
        if (_player._lifes <= 0)
        {
            GameOver();
        }
        else
        {
            ResetStage();
        }
    }

    void UpdateStageUp()
    {
        if (_player._isStageUp)
        {
            _player._isStageUp = false;
            StageUp();
        }
    }

    void StageUp()
    {
        var nextStage = SceneManager.GetActiveScene().buildIndex + 1;
        PlayerPrefs.SetFloat("_currentStage", nextStage);
        PlayerPrefs.SetFloat("_playerScore", _score);
        var maxScore = PlayerPrefs.GetFloat("_maxScore", 0);
        if (_score > maxScore)
        {
            PlayerPrefs.SetFloat("_maxScore", _score);

        }
        PlayerPrefs.SetFloat("_playerLifes", _playerLifes);
        SceneLoader.Transition(nextStage, true);
    }
    void GameOver()
    {
        PlayerPrefs.SetFloat("_currentStage", 1);
        PlayerPrefs.SetFloat("_playerLifes", 3);
        PlayerPrefs.SetFloat("_playerScore", 0);
        SceneLoader.Transition(0, false);
    }
}
