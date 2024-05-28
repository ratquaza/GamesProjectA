using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; protected set; }
    [SerializeField] protected LoadingUI loadingUI;

    private PlayerLiving player { get => PlayerLiving.Instance; }
    protected int dungeonLevel = 0;

    protected bool isLoading = false;
    protected PlayerActions actions;

    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        actions = PlayerLiving.Instance?.actions;
    }

    public void Defaults()
    {
        dungeonLevel = 0;
    }

    public bool ToDungeon(string String)
    {
        if (isLoading) return false;
        TransitionTo(String, (op) =>
        {
            if (player != null) player.transform.localScale = Vector3.one * 1.6f;
        });
        return true;
    }

    public bool ToOverworld()
    {
        if (isLoading) return false;
        TransitionTo("NewOverworld", (op) => 
        {
            if (player != null) player.transform.localScale = Vector3.one * 2f;
        });
        return true;
    }

    public bool ToTitleScreen()
    {
        if (isLoading) return false;
        TransitionTo("TitleScreen");
        return true;
    }

    private void TransitionTo(string scene, Action<AsyncOperation> onLoad = null)
    {
        isLoading = true;
        actions?.Disable();

        LoadingUI UI = Instantiate(loadingUI.gameObject).GetComponent<LoadingUI>();
        DontDestroyOnLoad(UI.gameObject);
        UI.SetVisible(true);

        SceneManager.LoadSceneAsync(scene).completed += (op) =>
        {
            UI.SetVisible(false);
            isLoading = false;
            actions?.Enable();
            Destroy(UI.gameObject);
            if (onLoad != null) onLoad(op);
        };
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}