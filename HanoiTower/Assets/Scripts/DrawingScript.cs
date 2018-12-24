using UnityEngine;
using UnityEngine.UI;

public delegate void action(int index);
public delegate void action2();

public class DrawingScript
{

    public GameObject DrawBlock(string name, Transform parent, Vector2 deltaSize, Vector2 anchoredPosition,
        Vector2 anchorMin, Vector2 anchorMax, Vector3 localScale, Color32 color, Sprite sprite)
    {
        GameObject block = new GameObject(name);

        block.AddComponent<RectTransform>();
        block.AddComponent<Image>();

        block.transform.SetParent(parent);

        block.GetComponent<Image>().sprite = sprite;
        block.GetComponent<Image>().type = Image.Type.Sliced;
        block.GetComponent<Image>().color = color;
        block.GetComponent<Image>().raycastTarget = false;

        block.GetComponent<RectTransform>().anchorMin = anchorMin;
        block.GetComponent<RectTransform>().anchorMax = anchorMax;
        block.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        block.GetComponent<RectTransform>().sizeDelta = deltaSize;
        block.GetComponent<RectTransform>().localScale = localScale;

        return block;
    }

    public GameObject DrawButton(string name, Transform parent, Vector2 deltaSize, Vector2 anchoredPosition,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector3 localScale, Sprite spr, Color32 col)
    {
        GameObject button = new GameObject(name);

        button.AddComponent<RectTransform>();
        button.AddComponent<Image>();
        button.AddComponent<Button>();

        button.transform.SetParent(parent);

        button.GetComponent<Image>().sprite = spr;
        button.GetComponent<Image>().type = Image.Type.Sliced;
        button.GetComponent<Image>().color = col;

        button.GetComponent<RectTransform>().anchorMin = anchorMin;
        button.GetComponent<RectTransform>().anchorMax = anchorMax;
        button.GetComponent<RectTransform>().pivot = pivot;
        button.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        button.GetComponent<RectTransform>().sizeDelta = deltaSize;
        button.GetComponent<RectTransform>().localScale = localScale;

        return button;
    }

    public void SetAction(GameObject button, action act, int index)
    {
        button.GetComponent<Button>().onClick.AddListener(delegate { act.Invoke(index); });
    }

    public void SetAction(GameObject button, action2 act)
    {
        button.GetComponent<Button>().onClick.AddListener(delegate { act.Invoke(); });
    }

    public GameObject DrawText(string name, Transform parent, Vector2 deltaSize, Vector2 anchoredPosition,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector3 localScale, Color32 col, string text)
    {
        GameObject textt = new GameObject(name);

        textt.AddComponent<RectTransform>();
        textt.AddComponent<Text>();

        textt.transform.SetParent(parent);

        textt.GetComponent<RectTransform>().anchorMin = anchorMin;
        textt.GetComponent<RectTransform>().anchorMax = anchorMax;
        textt.GetComponent<RectTransform>().pivot = pivot;
        textt.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        textt.GetComponent<RectTransform>().sizeDelta = deltaSize;
        textt.GetComponent<RectTransform>().localScale = localScale;

        textt.GetComponent<Text>().text = text;
        textt.GetComponent<Text>().resizeTextForBestFit = true;
        textt.GetComponent<Text>().resizeTextMaxSize = 40;
        textt.GetComponent<Text>().resizeTextMinSize = 10;
        textt.GetComponent<Text>().fontStyle = FontStyle.BoldAndItalic;
        textt.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        textt.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        textt.GetComponent<Text>().color = col;

        return textt;
    }

    public void UpdateGui(GameObject textOb, string text)
    {
        textOb.GetComponent<Text>().text = text;
    }

    public GameObject DrawPanel(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector3 localScale,
        Vector2 sizeDelta, Vector2 anchoredPosition, Sprite image, Color32 color)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent);
        panel.AddComponent<RectTransform>();
        panel.AddComponent<Image>();

        panel.GetComponent<Image>().sprite = image;
        panel.GetComponent<Image>().type = Image.Type.Sliced;
        panel.GetComponent<Image>().color = color;

        panel.GetComponent<RectTransform>().anchorMin = anchorMin;
        panel.GetComponent<RectTransform>().anchorMax = anchorMax;
        panel.GetComponent<RectTransform>().localScale = localScale;
        panel.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        panel.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        return panel;
    }
}
