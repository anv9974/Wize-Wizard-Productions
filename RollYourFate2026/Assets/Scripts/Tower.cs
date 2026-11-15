using UnityEngine;

public class Tower : MonoBehaviour
{
    public int HP;
    private int maxHP;
    public int DamageDealt;

    public SpriteRenderer spriteRenderer;
    public Sprite[] towerSprites;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHP = HP;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Destroy(gameObject);
        }

        if (HP < maxHP / 3)
        {
            spriteRenderer.sprite = towerSprites[2];
        }
        else if (HP < maxHP / 3 * 2)
        {
            spriteRenderer.sprite = towerSprites[1];
        }
    }
}
