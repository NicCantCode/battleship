using TMPro;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    [SerializeField] private GameObject logWindowUIElement;
    [SerializeField] private GameObject logMessagePrefab;
    [SerializeField] private float verticalLogWindowSpacing;
    
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }

    public void LogMessage(string message, Color textColor)
    {
        if (_gameManager.GetGameOverState()) return;
        
        ExpandViewportForNewMessage();
        var newMessage = Instantiate(logMessagePrefab, logWindowUIElement.transform).GetComponent<TextMeshProUGUI>();
        newMessage.text = message;
        newMessage.color = textColor;
        newMessage.gameObject.name = message;
    }

    private void ExpandViewportForNewMessage()
    {
        var rect = logWindowUIElement.GetComponent<RectTransform>();
        var messageRectHeight = logMessagePrefab.GetComponent<RectTransform>().rect.height + verticalLogWindowSpacing;
        var sizeDelta = rect.sizeDelta;
        var position = rect.position;
        
        sizeDelta = new Vector2(sizeDelta.x, sizeDelta.y + messageRectHeight);
        rect.sizeDelta = sizeDelta;
        
        position = new Vector3(position.x, position.y + messageRectHeight, position.z);
        rect.position = position;
    }
}
