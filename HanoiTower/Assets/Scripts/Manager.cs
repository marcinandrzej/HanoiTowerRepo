using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public Sprite sprite;

    private const int OFFSET_X = 250;
    private const int OFFSET_Y = -40;
    private const int STICK_X = 30;
    private const int STICK_Y = 300;
    private const int BASE_BLOCK_X = 200;
    private const int MODIFIER_BLOCK_X = 30;
    private const int BLOCK_Y = 40;
    private const int MODIFIER_BLOCK_Y = 35;
    private const int GRABBED_BLOCK_X = 0;
    private const int GRABBED_BLOCK_Y = 200;

    private bool end;

    private DrawingScript drawingScript;
    private ClockScript clock;

    private GameObject[] sticks;
    private GameObject[] buttons;
    private GameObject clockText;
    private GameObject menuPanel;

    private Stack<GameObject> stack1;
    private Stack<GameObject> stack2;
    private Stack<GameObject> stack3;

    private Color32[] colors;

    private GameStates gameState;
    private GameObject blockInHand;

    void Awake()
    {
        colors = new Color32[5];
        colors[0] = new Color32(255, 0, 0, 255);
        colors[1] = new Color32(255, 0, 255, 255);
        colors[2] = new Color32(0, 255, 0, 255);
        colors[3] = new Color32(255, 255, 0, 255);
        colors[4] = new Color32(0, 255, 255, 255);

        end = false;
    }

    // Use this for initialization
    void Start()
    {
        ChangeState(new FirstClickState());
        drawingScript = new DrawingScript();
        clock = new ClockScript();

        DrawSticks(new Color32(150, 60, 25, 255));
        DrawBlocks();
        DrawButtons();
        AddButtonAction();
        DrawClockText();
        clock.StartClock();
    }

    // Update is called once per frame
    void Update()
    {
        if (!end)
        {
            clock.UpdateTime();
            drawingScript.UpdateGui(clockText, clock.RefactorTime(clock.GetTime()));
        }
    }

    public void ChangeState(GameStates newState)
    {
        gameState = newState;
        gameState.OnStateEnter(this);
    }

    public void ExecuteState(int index)
    {
        gameState.ExecuteState(index);
    }

    public bool isStackEmpty(int index)
    {
        bool isEmpty = false;
        switch (index)
        {
            case 0:
                if (stack1.Count <= 0)
                {
                    isEmpty = true;
                }
                break;
            case 1:
                if (stack2.Count <= 0)
                {
                    isEmpty = true;
                }
                break;
            case 2:
                if (stack3.Count <= 0)
                {
                    isEmpty = true;
                }
                break;
            default:
                break;
        }
        return isEmpty;
    }

    public void GrabBlock(int index)
    {
        switch (index)
        {
            case 0:
                    blockInHand = stack1.Pop();
                break;
            case 1:
                    blockInHand = stack2.Pop();
                break;
            case 2:
                    blockInHand = stack3.Pop();
                break;
            default:
                break;
        }
        blockInHand.GetComponent<RectTransform>().anchoredPosition = new Vector2(GRABBED_BLOCK_X, GRABBED_BLOCK_Y);
    }

    public bool CanPlace(int index)
    {
        bool can = false;
        switch (index)
        {
            case 0:
                if (stack1.Count <= 0)
                {
                    can = true;
                }
                else if (stack1.Peek().GetComponent<RectTransform>().sizeDelta.x >
                    blockInHand.GetComponent<RectTransform>().sizeDelta.x)
                {
                    can = true;
                }
                break;
            case 1:
                if (stack2.Count <= 0)
                {
                    can = true;
                }
                else if (stack2.Peek().GetComponent<RectTransform>().sizeDelta.x >
                    blockInHand.GetComponent<RectTransform>().sizeDelta.x)
                {
                    can = true;
                }
                break;
            case 2:
                if (stack3.Count <= 0)
                {
                    can = true;
                }
                else if (stack3.Peek().GetComponent<RectTransform>().sizeDelta.x >
                    blockInHand.GetComponent<RectTransform>().sizeDelta.x)
                {
                    can = true;
                }
                break;
            default:
                break;
        }
        return can;
    }

    public void PlaceBlock(int index)
    {
        int count = 0;
        switch (index)
        {
            case 0:
                count = stack1.Count;
                stack1.Push(blockInHand);
                break;
            case 1:
                count = stack2.Count;
                stack2.Push(blockInHand);
                break;
            case 2:
                count = stack3.Count;
                stack3.Push(blockInHand);
                break;
            default:
                break;
        }
        blockInHand.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(OFFSET_X * (index - 1), (-(STICK_Y / 2) + OFFSET_Y) + (BLOCK_Y / 2) + (count * MODIFIER_BLOCK_Y));
        blockInHand = null;
    }

    public bool CheckWin()
    {
        return (stack3.Count >= 5) ;
    }

    public void Win()
    {
        end = true;
        DrawMenu(clock.GetTime());
    }

    private void DrawSticks(Color32 col)
    {
        sticks = new GameObject[3];

        sticks[0] = drawingScript.DrawBlock("Stick1", transform, new Vector2(STICK_X, STICK_Y),
            new Vector2(-OFFSET_X, OFFSET_Y), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector3(1, 1, 1), col, sprite);
        sticks[1] = drawingScript.DrawBlock("Stick2", transform, new Vector2(STICK_X, STICK_Y),
            new Vector2(0, OFFSET_Y), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector3(1, 1, 1), col, sprite);
        sticks[2] = drawingScript.DrawBlock("Stick3", transform, new Vector2(STICK_X, STICK_Y),
            new Vector2(OFFSET_X, OFFSET_Y), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector3(1, 1, 1), col, sprite);
    }

    private void DrawBlocks()
    {
        stack1 = new Stack<GameObject>();
        stack2 = new Stack<GameObject>();
        stack3 = new Stack<GameObject>();

        for (int i = 0; i < colors.Length; i++)
        {
            stack1.Push(drawingScript.DrawBlock(("Block" + (i+1).ToString()), transform,
                new Vector2(BASE_BLOCK_X - i * MODIFIER_BLOCK_X, BLOCK_Y),
                new Vector2(-OFFSET_X, (-(STICK_Y/2) + OFFSET_Y) + (BLOCK_Y/2) + (i* MODIFIER_BLOCK_Y)),
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector3(1, 1, 1), colors[i], sprite));
        }
    }

    private void DrawButtons()
    {
        buttons = new GameObject[3];

        for (int i = 0; i < 3; i++)
        {
            buttons[i] = drawingScript.DrawButton(("Button" + (i + 1).ToString()), transform,
                new Vector2(BASE_BLOCK_X, STICK_Y + BLOCK_Y), new Vector2(OFFSET_X * (i-1), OFFSET_Y),
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector3(1, 1, 1),
            sprite, new Color32(0,0,0,0));
        }
    }

    private void AddButtonAction()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            drawingScript.SetAction(buttons[i], new action(ExecuteState), index);
        }
    }

    private void DrawClockText()
    {
        clockText = drawingScript.DrawText("Clock", transform, new Vector2(200, 50), new Vector2(10, -10),
            new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1), new Vector3(1, 1, 1),
            new Color32(255, 255, 255, 255), "00:00:00");
    }

    private void DrawMenu(int time)
    {
        int t = int.MaxValue;
        string text;

        if (PlayerPrefs.HasKey("HanoiTime"))
            t = PlayerPrefs.GetInt("HanoiTime");

        if (t > time)
        {
            PlayerPrefs.SetInt("HanoiTime", time);
            text = "NEW RECORD" + "\n" + clock.RefactorTime(time);
        }
        else
        {
            text = "HIGHSCORE" + "\n" + clock.RefactorTime(t);
        }

        menuPanel = drawingScript.DrawPanel(transform, "Menu", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector3(1, 1, 1), new Vector2(400, 300), new Vector2(0, 0), sprite, new Color32(100,100,100,255));
        drawingScript.DrawText("Text", menuPanel.transform, new Vector2(400, 100), new Vector2(0, 0),
            new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector3(1, 1, 1),
            new Color32(255, 255, 255, 255), text);
        GameObject exBut = drawingScript.DrawButton("Exit", menuPanel.transform, new Vector2(180,120), new Vector2(20,10),
            new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector3(1, 1, 1),
            sprite, new Color32(255, 255, 255, 255));
        drawingScript.DrawText("Text", exBut.transform, new Vector2(160, 100), new Vector2(0, 0),
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector3(1, 1, 1),
            new Color32(100, 100, 100, 255), "EXIT");
        drawingScript.SetAction(exBut, new action2(Application.Quit));
        GameObject resBut = drawingScript.DrawButton("Retry", menuPanel.transform, new Vector2(180, 120), new Vector2(-20, 10),
            new Vector2(1, 0), new Vector2(1, 0), new Vector2(1, 0), new Vector3(1, 1, 1),
            sprite, new Color32(255, 255, 255, 255));
        drawingScript.DrawText("Text", resBut.transform, new Vector2(160, 100), new Vector2(0, 0),
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector3(1, 1, 1),
            new Color32(100, 100, 100, 255), "RETRY");
        drawingScript.SetAction(resBut, new action2(Reset));
    }

    private void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
