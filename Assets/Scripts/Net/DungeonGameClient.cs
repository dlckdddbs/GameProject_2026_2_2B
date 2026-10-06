using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DungeonGameClient : MonoBehaviour
{
    [SerializeField] private string serverUrl = "http://127.0.0.1:3000";

    [SerializeField] private TMP_Text playerText;
    [SerializeField] private TMP_Text roomText;
    [SerializeField] private TMP_Text messageText;

    [SerializeField] private Button enterButton;
    [SerializeField] Button attackButton;
    [SerializeField] Button chestButton;
    [SerializeField] Button nextButton;
    [SerializeField] Button returnButton;
    [SerializeField] Button restButton;

    void Start()
    {
        enterButton.onClick.AddListener(() => StartCoroutine(Post("/api/dungeon/enter", "{}")));
        attackButton.onClick.AddListener(() => SendAction("ATTACK"));
        chestButton.onClick.AddListener(() => SendAction("OPEN_CHEST"));
        restButton.onClick.AddListener(() => SendAction("REST"));
        nextButton.onClick.AddListener(() => SendAction("NEXT_ROOM"));
        returnButton.onClick.AddListener(() => SendAction("RETURN"));
        StartCoroutine(GetState());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SendAction(string action)
    {
        string json = JsonUtility.ToJson(new DungeonActionRequest { action = action });
        StartCoroutine(Post("/api/dungeon/action", json));
    }

    private IEnumerator GetState()
    {
        using UnityWebRequest request = UnityWebRequest.Get(serverUrl + "/api/game/state");

        yield return request.SendWebRequest();
    }

    private IEnumerator Post(string path, string json)
    {
        using UnityWebRequest request = new UnityWebRequest(serverUrl + path, "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
    }

}
