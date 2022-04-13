using TMPro;
using UnityEngine;

public class SetDifficulty : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<TMP_Dropdown>().value = (int) DifficultyManager.Instance.GetComponent<DifficultyManager>().difficulty;
        GetComponent<TMP_Dropdown>().onValueChanged.AddListener(value => DifficultyManager.Instance.ChangeDifficulty(value));
    }
}
