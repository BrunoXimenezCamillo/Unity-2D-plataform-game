using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] Scene_Loader SceneLoader;

    [Header("Texts")]

    [SerializeField] Text _currentScore;
    [SerializeField] Text _maxScore;
    [SerializeField] Text _currentStage;
    [SerializeField] Text _maxStage;
    [SerializeField] Text _currentLifes;

    private void Awake()
    {
        SceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<Scene_Loader>();
    }
    private void Update()
    {
        _currentScore.text = PlayerPrefs.GetFloat("_playerScore", 0).ToString();
        _maxScore.text = PlayerPrefs.GetFloat("_maxScore", 0).ToString();
        _currentStage.text = PlayerPrefs.GetFloat("_currentStage", 0).ToString();
        //TODO MAX STAGE
        _currentLifes.text = PlayerPrefs.GetFloat("_playerLifes", 3).ToString();
    }
    public void onButtonPlay()
    {
        playeButtonSe();
        var _stage = PlayerPrefs.GetFloat("_currentStage", 1);
        SceneLoader.Transition(Mathf.RoundToInt(_stage), true);
    }

    public void onButtonExit()
    {
        playeButtonSe();

    }

    public void onButtonReset()
    {
        playeButtonSe();
        PlayerPrefs.SetFloat ("_currentStage", 1);
        PlayerPrefs.SetFloat("_playerLifes", 3);
        PlayerPrefs.SetFloat("_playerScore", 0);
    }

    void playeButtonSe()
    {
        var se = GetComponent<AudioSource>();
        se.Play();
    }
}
