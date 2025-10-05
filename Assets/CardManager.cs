using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject[] allCards;
    public float spacing = 200f;
    private float gameTimer;
    private int phaseCount = 0;
    public Vector2 uiPosition = new Vector2(-800, 0); 
    private List<GameObject> currentCards = new List<GameObject>();

    [Header("卡牌对应的UI预制体")]
    public List<CardUIInfo> cardUIList = new List<CardUIInfo>();

    // 存储原始卡牌索引，用于正确匹配
    private Dictionary<GameObject, int> cardIndexMap = new Dictionary<GameObject, int>();

    void Start()
    {
        SetupCanvas();
        SpawnCards();
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            gameTimer += Time.deltaTime;

            if (gameTimer >= 30 && phaseCount <= 4)
            {
                SpawnCards();
                phaseCount++;
            }
        }
    }

    void SetupCanvas()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("CardCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }

        transform.SetParent(canvas.transform, false);
        transform.localPosition = Vector3.zero;
    }

    void SpawnCards()
    {
        ClearCurrentCards();
        cardIndexMap.Clear();

        // 创建临时列表进行洗牌，保持原始索引
        List<GameObject> tempCards = new List<GameObject>(allCards);
        List<int> originalIndices = new List<int>();
        for (int i = 0; i < tempCards.Count; i++)
        {
            originalIndices.Add(i);
        }

        // 洗牌
        for (int i = 0; i < tempCards.Count; i++)
        {
            int rnd = Random.Range(i, tempCards.Count);

            // 交换卡片
            GameObject tempCard = tempCards[i];
            tempCards[i] = tempCards[rnd];
            tempCards[rnd] = tempCard;

            // 交换索引
            int tempIndex = originalIndices[i];
            originalIndices[i] = originalIndices[rnd];
            originalIndices[rnd] = tempIndex;
        }

        for (int i = 0; i < 2; i++)
        {
            if (tempCards[i] != null)
            {
                GameObject card = Instantiate(tempCards[i], transform);
                card.transform.localPosition = new Vector3(i * spacing - (spacing / 2), 0, 0);
                card.name = "Card_" + i;

                // 保存原始索引
                cardIndexMap[card] = originalIndices[i];

                AddClickComponent(card);
                currentCards.Add(card);
            }
        }

        Time.timeScale = 0;
    }

    void AddClickComponent(GameObject card)
    {
        if (card.GetComponent<RectTransform>() != null)
        {
            UnityEngine.UI.Button button = card.GetComponent<UnityEngine.UI.Button>();
            if (button == null) button = card.AddComponent<UnityEngine.UI.Button>();

            if (card.GetComponent<UnityEngine.UI.Image>() == null)
            {
                UnityEngine.UI.Image image = card.AddComponent<UnityEngine.UI.Image>();
                image.color = new Color(0, 0, 0, 0.01f);
            }

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnCardClicked(card));
        }
        else if (card.GetComponent<SpriteRenderer>() != null)
        {
            Collider2D collider = card.GetComponent<Collider2D>();
            if (collider == null)
            {
                collider = card.AddComponent<BoxCollider2D>();
            }

            CardClickHandler clickHandler = card.GetComponent<CardClickHandler>();
            if (clickHandler == null) clickHandler = card.AddComponent<CardClickHandler>();
            clickHandler.OnCardClicked = OnCardClicked;
        }
        else
        {
            if (card.GetComponent<Collider>() == null)
                card.AddComponent<BoxCollider>();

            CardClickHandler clickHandler = card.GetComponent<CardClickHandler>();
            if (clickHandler == null) clickHandler = card.AddComponent<CardClickHandler>();
            clickHandler.OnCardClicked = OnCardClicked;
        }
    }

    void OnCardClicked(GameObject card)
    {
        Time.timeScale = 1f;

        // 生成对应的UI
        SpawnCardUI(card);

        ClearCurrentCards();
    }

    void SpawnCardUI(GameObject card)
    {
        // 方法1：通过保存的索引匹配
        if (cardIndexMap.ContainsKey(card))
        {
            int originalIndex = cardIndexMap[card];
            if (originalIndex < allCards.Length)
            {
                GameObject originalCardPrefab = allCards[originalIndex];
                GameObject uiPrefab = FindUIByCardPrefab(originalCardPrefab);

                if (uiPrefab != null)
                {
                    CreateUI(uiPrefab);
                    return;
                }
            }
        }

        // 方法2：通过名称匹配（备用）
        GameObject uiPrefabByName = FindUIByCardName(card);
        if (uiPrefabByName != null)
        {
            CreateUI(uiPrefabByName);
            return;
        }

        Debug.LogWarning("没有找到卡牌对应的UI预制体: " + card.name);
    }

    GameObject FindUIByCardPrefab(GameObject cardPrefab)
    {
        foreach (CardUIInfo uiInfo in cardUIList)
        {
            if (uiInfo.cardPrefab == cardPrefab)
            {
                return uiInfo.uiPrefab;
            }
        }
        return null;
    }

    GameObject FindUIByCardName(GameObject cardInstance)
    {
        string originalName = cardInstance.name.Replace("(Clone)", "").Replace("Card_", "").Trim();

        foreach (CardUIInfo uiInfo in cardUIList)
        {
            if (uiInfo.cardPrefab != null)
            {
                string prefabName = uiInfo.cardPrefab.name;
                if (prefabName.Contains(originalName) || originalName.Contains(prefabName))
                {
                    return uiInfo.uiPrefab;
                }
            }
        }
        return null;
    }

    void CreateUI(GameObject uiPrefab)
    {
        GameObject cardUI = Instantiate(uiPrefab);
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            cardUI.transform.SetParent(canvas.transform, false);
            cardUI.transform.localPosition = Vector3.zero;
            RectTransform rect = cardUI.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition = uiPosition;
            }
            else
            {
                cardUI.transform.localPosition = new Vector3(uiPosition.x, uiPosition.y, 0);
            }
        }
        Debug.Log("成功生成卡牌UI: " + cardUI.name);
        
    }

    void ClearCurrentCards()
    {
        foreach (GameObject card in currentCards)
        {
            if (card != null)
            {
                Destroy(card);
            }
        }
        currentCards.Clear();
        cardIndexMap.Clear();
    }
    
}

// 卡牌和UI的对应关系
[System.Serializable]
public class CardUIInfo
{
    public GameObject cardPrefab;  // 卡牌预制体
    public GameObject uiPrefab;    // 对应的UI预制体
}

public class CardClickHandler : MonoBehaviour
{
    public System.Action<GameObject> OnCardClicked;
    
    void OnMouseDown()
    {
        OnCardClicked?.Invoke(gameObject);
    }
}