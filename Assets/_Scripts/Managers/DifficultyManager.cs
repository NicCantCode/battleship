using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public Difficulty difficulty;

    private static DifficultyManager _instance;
    public static DifficultyManager Instance => _instance;

    private void Awake()
    {
        # region Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        #endregion
        
        difficulty = Difficulty.SIMPLE;
    }

    public void ChangeDifficulty(int listItem)
    {
        difficulty = (Difficulty) listItem;
    }
}
