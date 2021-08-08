using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class LevelText : MonoBehaviour, IPointerClickHandler
{
    string levelText;
    public TextMeshProUGUI _text;
    TextMeshProUGUI selectedText;
    public GameObject starHolder;
    public GameObject starObject;

    public Levels levels;

    string textToChange;

    RectTransform buttonHolder;

    void Start()
    {
        levels = FindObjectOfType<Levels>();
        string textToChange = _text.text;

        string[] infos = textToChange.Split(','); // we split the string "5,3". It means level 5 and 3 stars gained.

        textToChange = "LEVEL" + infos[0];

        if (starHolder != null)
        {
            for (int i = 0; i < int.Parse(infos[1]); i++)
            {
                Instantiate(starObject, starHolder.transform.position, Quaternion.identity, starHolder.transform);
            }
        }

        _text.text = textToChange;
        selectedText = GameObject.Find("selectedText").GetComponent<TextMeshProUGUI>();
        buttonHolder = GameObject.Find("ButtonHolder").GetComponent<RectTransform>();
        buttonHolder.anchoredPosition = new Vector2(2000, 0);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (name != "-1") //if level is not locked
        {
            levels.chooseLevel(name);
            selectedText.text = "LEVEL: " + name;
        }

    }
}
