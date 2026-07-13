using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadTitle()
    {
        SceneManager.LoadScene(SceneNames.Title);
    }

    public static void LoadGame()
    {
        SceneManager.LoadScene(SceneNames.Game);
    }

    public static void LoadGameClear()
    {
        SceneManager.LoadScene(SceneNames.Clear);
    }

    public static void LoadGameOver()
    {
        SceneManager.LoadScene(SceneNames.Over);
    }
}

public static class SceneNames
{
    public const string Title = "TitleScene";
    public const string Game = "GameScene";
    public const string Clear = "GameClearScene";
    public const string Over = "GameOverScene";
}
