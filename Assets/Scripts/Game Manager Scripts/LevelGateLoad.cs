using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGateLoad : MonoBehaviour
{
    public string sceneName;
    public Vector3 newPlayerPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LoadNextScene(other));
        }
    }

    private IEnumerator LoadNextScene(Collider player)
    {
        yield return SceneLoader.instance.StartCoroutine(SceneLoader.LoadSceneCoroutine(sceneName));

        player.transform.position = newPlayerPosition;
    }
}
