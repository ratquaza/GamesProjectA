using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; protected set; }
    [SerializeField] protected LoadingUI loadingUI;

    protected bool isLoading = false;
    protected PlayerActions actions;

    void Start()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        actions = PlayerLiving.Instance.actions;
    }

    public bool TransitionTo(string scene)
    {
        if (isLoading) return false;
        isLoading = true;
        actions.Disable();

        LoadingUI UI = Instantiate(loadingUI.gameObject).GetComponent<LoadingUI>();
        DontDestroyOnLoad(UI.gameObject);
        UI.SetVisible(true);

        string currScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadSceneAsync(scene).completed += (op) =>
        {
            UI.SetVisible(false);
            isLoading = false;
            actions.Enable();
            Destroy(UI.gameObject);
        };
        return true;
    }
}