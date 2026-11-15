using UnityEngine;
using UnityEngine.InputSystem;
public class GameManager : MonoBehaviour
{
    //UI Manager needs to read troop type
    public int troopType {get; private set;}

    public TroopSelect iconSelector;
    public UIManager uiManager;
    public Troop [] troopPrefabs;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        select(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame & CanSpawn())
        {
            SpendMoney();
        }
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            select(1);
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            select(2);
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            select(3);
        }
        else if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            select(4);
        }
    }

    void select(float inputValue)
    {
        if (inputValue != 1 && inputValue != 2 && inputValue != 3 && inputValue != 4)
        {
            return;
        }
        else
        {
            troopType = (int)inputValue;
            iconSelector.SetSelected(troopType - 1);
        }
    }

    void SpendMoney()
    {
        int amt = 0;
        switch (troopType)
        {
            case 1:
                amt = 1;
                break;
            case 2:
                amt = 5;
                break;
            case 3:
                amt = 10;
                break;
            case 4:
                amt = 20;
                break;
        }

        if (uiManager.spendMoney(amt))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            pos.z = 0;
            Debug.Log("Money spent: " + amt);
            Instantiate(troopPrefabs[troopType - 1], pos, Quaternion.identity);
            //uiManager.increaseMultiplier(0.5f);
            //Instantiate(troopPrefabs[troopType - 1], Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Quaternion.identity);
        }
    }

    bool CanSpawn()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Collider2D hit = Physics2D.OverlapPoint(pos);
        pos.z = 0f;
        bool isOnScreen = pos.x > -8.85 & pos.x < 17.7 & pos.y < 13.2 & pos.y > -13.2;
        bool notInRadius = hit == null;
        bool canSpawn = isOnScreen && notInRadius;
        return canSpawn;
    }
}
