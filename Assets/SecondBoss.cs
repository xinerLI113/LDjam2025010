using Unity.Mathematics;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    [Header("新Sprite")]
    public Sprite newSprite;
    
    private SpriteRenderer spriteRenderer;
    public GameObject firstBoss;
    public GameObject firstBossPre;
    private GameObject secondBoss;
    public Sprite secondBossSprite;
    public Vector3 secondpos = new Vector3(5, -1, 0);
    private bool canMake = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (firstBoss == null && canMake == true)
        {
            ChangeToNewSprite();
        }
    }

    void ChangeToNewSprite()
{
    canMake = false;
    if (spriteRenderer != null && newSprite != null)
    {
        spriteRenderer.sprite = newSprite;
    }
    
    secondBoss = Instantiate(firstBossPre, secondpos, quaternion.identity);
    
    
    SpriteRenderer[] allRenderers = secondBoss.GetComponentsInChildren<SpriteRenderer>(true);
    foreach (SpriteRenderer renderer in allRenderers)
    {
        if (secondBossSprite != null)
        {
            renderer.sprite = secondBossSprite;
            
        }
    }
    
    Transform spriteChild = secondBoss.transform.Find("Sprite"); // 替换为实际的子物体名字
    if (spriteChild != null)
    {
        SpriteRenderer specificRenderer = spriteChild.GetComponent<SpriteRenderer>();
        if (specificRenderer != null && secondBossSprite != null)
        {
            specificRenderer.sprite = secondBossSprite;
        }
    }
}
}