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
    int[] empty;
    
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
        //Debug.Log("Inventory items: " + numItems);
        filled = new int[numItems];
        
        slots = GameObject.FindGameObjectsWithTag("Slot");
        hearts = GameObject.FindGameObjectsWithTag("Heart");
        float[] slots_x = new float[slots.Length];
        float[] hearts_x = new float[hearts.Length];

        empty = new int[hearts.Length];
        for(int i=0; i < slots_x.Length; i++)
        {
            slots_x[i] = slots[i].transform.position.x;
        }
        Array.Sort(slots_x, slots);
        /*
        for (int i = 0; i < slots.Length - 1; i++)
        {
            for (int j = 1; j < slots.Length; j++)
            {
                if (slots[i].transform.position.x > slots[j].transform.position.x)
                {
                    GameObject temp = slots[i];
                    slots[i] = slots[j];
                    slots[j] = temp;
                }
            }
        }
        */

        for (int i=0; i < filled.Length; i++)
        {
            filled[i] = 0;
        }
        for(int i=0; i < hearts.Length; i++)
        {
            empty[i] = 0;
            //Debug.Log("Hearts i: " + i);
            hearts[i].GetComponent<Image>().sprite = fullHeart;
        }
        //Array.Sort(hearts_x, hearts);
        for(int i=0; i < hearts.Length-1; i++)
        {
            for(int j=1; j < hearts.Length; j++)
            {
                if(hearts[i].transform.position.x > hearts[j].transform.position.x)
                {
                    GameObject temp = hearts[i];
                    hearts[i] = hearts[j];
                    hearts[j] = temp;
                }
            }
        }
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
        //Debug.Log("Manage: " + hcgm.lives);
        //hearts[hcgm.lives].GetComponent<Image>().sprite = emptyHeart;
        //Debug.Log("Length: " + hearts.Length);
        for(int i=hearts.Length-1; i >= 0; i--)
        {
            if (empty[i] == 0)
            {
                //Debug.Log("Heart " + i + " full");
                hearts[i].GetComponent<Image>().sprite = emptyHeart;
                empty[i] = 1;
                break;
            }
            else
            {
                //Debug.Log("Heart " + i + " empty");
            }
        }
    }
}
