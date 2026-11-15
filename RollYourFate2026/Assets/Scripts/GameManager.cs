using UnityEngine;
using UnityEngine.InputSystem;
public class GameManager : MonoBehaviour
{
    //UI Manager needs to read troop type
    public int troopType {get; private set;}

    public TroopSelect iconSelector;
    public Troop [] troopPrefabs;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        select(1);
    }

    // Update is called once per frame
    void Update()
    {

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            pos.z = 0f;
            Instantiate(troopPrefabs[troopType - 1], pos, Quaternion.identity);
            //Instantiate(troopPrefabs[troopType - 1], Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Quaternion.identity);
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
}
