using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Loader : MonoBehaviour
{
    [SerializeField] Animator _transitionAnim;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    public void Transition(int sceneId, bool async)
    {
        StartCoroutine(LoadScene(sceneId, async));
    }

    public IEnumerator LoadScene(int sceneId, bool async)
    {
        _transitionAnim.SetTrigger("Fade");

        yield return new WaitForSeconds(1f);

        if (async)
        {
            SceneManager.LoadSceneAsync(sceneId);
        }
        else
        {
            SceneManager.LoadScene(sceneId);
        }

    }

}
