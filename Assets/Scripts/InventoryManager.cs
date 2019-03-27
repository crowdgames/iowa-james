using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Sprite emptySection;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public GameObject[] myInventory = new GameObject[3];
    public Image[] inventorySections = new Image[3];

    public int currentHealth;
    public Image[] heartsSection = new Image[3];
    

    // Use this for initialization
    public void AddItem(GameObject myObject)
    {
        for (int i = 0; i < myInventory.Length; i++)
        {

            if (myInventory[i] == null)
            {
                myInventory[i] = myObject;
                inventorySections[i].overrideSprite = myObject.GetComponent<SpriteRenderer>().sprite;
                myObject.SetActive(false);
                //Destroy(myObject);
                break;
            }

        }
    }

    public void ClearInventory()
    {
        for(int i=0;i< myInventory.Length; i++)
        {
            myInventory[i] = null;
            inventorySections[i].overrideSprite = emptySection;
        }
    }

    public void DisplayHeart(int value)
    {
        //Debug.Log(value);
        switch (value)
        {
            case 0:
                heartsSection[0].overrideSprite = emptyHeart;
                heartsSection[1].overrideSprite = emptyHeart;
                heartsSection[2].overrideSprite = emptyHeart;
                break;
            case 1:
                heartsSection[0].overrideSprite = fullHeart;
                heartsSection[1].overrideSprite = emptyHeart;
                heartsSection[2].overrideSprite = emptyHeart;
                break;

            case 2:
                heartsSection[0].overrideSprite = fullHeart;
                heartsSection[1].overrideSprite = fullHeart;
                heartsSection[2].overrideSprite = emptyHeart;
                break;

            default:
                heartsSection[0].overrideSprite = fullHeart;
                heartsSection[1].overrideSprite = fullHeart;
                heartsSection[2].overrideSprite = fullHeart;
                break;
        }

    }


}
