using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class InventoryManager : MonoBehaviour
{
    public Sprite emptySection;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    int[] filled;
    
    Image[] heartsSection = new Image[3];
    GameObject[] slots;
    GameObject[] hearts;
    HCGManager hcgm;

    void Start()
    {
        hcgm = GameObject.FindObjectOfType<HCGManager>();
    }

    // Use this for initialization
    //public void AddItem(GameObject myObject)
    public void AddItem(Sprite sprite)
    {
        for (int i = 0; i < filled.Length; i++)
        {
            if (filled[i] == 0)
            {
                filled[i] = 1;
                //slots[i].GetComponent<Image>().sprite = myObject.GetComponent<SpriteRenderer>().sprite;
                slots[i].GetComponent<Image>().sprite = sprite;
                //myObject.SetActive(false);
                //Destroy(myObject);
                break;
            }
        }
    }

    public void InitInventory(int numItems)
    {
        Debug.Log("Inventory items: " + numItems);
        filled = new int[numItems];
        
        slots = GameObject.FindGameObjectsWithTag("Slot");
        hearts = GameObject.FindGameObjectsWithTag("Heart");
        float[] slots_x = new float[slots.Length];
        float[] hearts_x = new float[hearts.Length];
        for(int i=0; i < slots_x.Length; i++)
        {
            slots_x[i] = slots[i].transform.position.x;
        }
        Array.Sort(slots_x, slots);

        
        for(int i=0; i < filled.Length; i++)
        {
            filled[i] = 0;
        }
        for(int i=0; i < hearts.Length; i++)
        {
            hearts[i].GetComponent<Image>().sprite = fullHeart;
        }
        Array.Sort(hearts_x, hearts);
    }

    public void ClearInventory()
    {
        for(int i=0;i< filled.Length; i++)
        {
            filled[i] = 0;
            slots[i].GetComponent<Image>().sprite = emptySection;
        }
    }

    public void ManageHearts()
    {
        Debug.Log("Manage: " + hcgm.lives);
        if(hcgm.lives > 0)
          hearts[hcgm.lives-1].GetComponent<Image>().sprite = emptyHeart;
    }
}
