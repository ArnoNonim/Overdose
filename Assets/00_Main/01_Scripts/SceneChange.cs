using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private string nextScene;
    [SerializeField] private float changeTime;

    public void ChangeScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
