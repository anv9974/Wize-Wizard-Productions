using UnityEngine;
using UnityEngine.UI;
public class TroopSelect : MonoBehaviour
{
    public Image [] images;
    public Sprite [] sprites;
    public Sprite [] selectedSprites;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelected(int selectedIndex)
    {
        for(int i = 0; i < images.Length; i++)
        {
            if(i == selectedIndex)
            {
                images[i].sprite = selectedSprites[i];
            }
            else
            {
                images[i].sprite = sprites[i];
            }
        }
    }
}
