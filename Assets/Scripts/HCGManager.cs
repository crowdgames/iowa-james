using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class HCGManager : MonoBehaviour {

    GameObject hcgCanvas;
    InventoryManager inventory;
    string scenario;
    [HideInInspector]
    public GameObject[] items;
    string[] relevant_items;
    string[] irrelevant_items;

    [HideInInspector]
    public int relevant_count = 0;
    [HideInInspector]
    public int irrelevant_count = 0;
    [HideInInspector]
    public int lives = 3;
    
    GameObject chest;

    public Sprite closed;
    public Sprite open;
    Text scenarioText;
    GameObject irrelevantText;
    GameObject relevantText;
    PlayerController player;

    LevelManager lm;

    // Use this for initialization
    void Start () {

        player = GameObject.FindObjectOfType<PlayerController>();
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        inventory = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        chest = GameObject.Find("Chest");
        scenario = DataManager.scenarios[UnityEngine.Random.Range(0, DataManager.scenarios.Length)];
        //scenario = "Hardware Store";
        //Debug.Log("SCENARIO: " + scenarios[scenario]);
        hcgCanvas = GameObject.Find("HCGCanvas");
        if (DataManager.mode != 4)
        {
            hcgCanvas.SetActive(false);
        }
        else
        {
            relevant_items = DataManager.hcg_items[scenario];
            irrelevant_items = getIrrelevantItems().ToArray();
            string level_name = SceneManager.GetActiveScene().name;
            int num_items = int.Parse(level_name[level_name.LastIndexOf("_") + 1].ToString()) * 2;
            items = GameObject.FindGameObjectsWithTag("Item");
            inventory.InitInventory(items.Length / 2);
            scenarioText = GameObject.Find("ScenarioText").GetComponent<Text>();
            irrelevantText = GameObject.Find("Irrelevant");
            irrelevantText.SetActive(false);
            relevantText = GameObject.Find("Relevant");
            relevantText.SetActive(false);
            scenarioText.text = scenario;
            chest.GetComponent<SpriteRenderer>().sprite = closed;
            LoadItems();
        }

    }
	
    void LoadItems()
    {
        List<string> rel_temp = new List<string>(relevant_items);
        List<string> irrel_temp = new List<string>(irrelevant_items);

        //Debug.Log("Length: " + items.Length);
        /*
        foreach(GameObject i in items)
        {
            Debug.Log(i.transform.position.x);
        }
        items = ShuffleList(items);
        Debug.Log("SHUFFLED");
        foreach (GameObject i in items)
        {
            Debug.Log(i.transform.position.x);
        }
        */
        for (int i = 0; i < items.Length; i++)
        {
            string path = "";
            if (i % 2 == 0)
            {
                int idx = UnityEngine.Random.Range(0, rel_temp.Count);
                string item = rel_temp[idx];
                path = "Items/" + item;
                rel_temp.RemoveAt(idx);
            }
            else
            {
                int idx = UnityEngine.Random.Range(0, irrel_temp.Count);
                string item = irrel_temp[idx];
                path = "Items/" + item;
                irrel_temp.RemoveAt(idx);
            }
            GameObject obj = Resources.Load(path) as GameObject;
            //Debug.Log("Path: " + path);
            items[i] = Instantiate(obj, items[i].transform);
        }
    }

    IEnumerator ShowIrrelevant()
    {
        irrelevantText.SetActive(true);
        yield return new WaitForSeconds(2f);
        irrelevantText.SetActive(false);
    }

    public IEnumerator ShowRelevant()
    {
        relevantText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        relevantText.SetActive(false);
    }

    public List<string> getIrrelevantItems()
    {
        List<string> irrelevant = new List<string>();
        foreach (string scen in DataManager.hcg_items.Keys)
        {
            if (scen != scenario)
            {
                foreach (string item in DataManager.hcg_items[scen])
                    irrelevant.Add(item);
            }
        }
        return irrelevant;
    }

    public void CollectItem(string item, Sprite sprite)
    {
        //Debug.Log("Collected Item: " + item.name);
        //item.SetActive(false);
        //Destroy(item);
        //string item_name = item.name.Substring(0, item.name.IndexOf("("));
        string item_name = item.Substring(0, item.IndexOf("("));
        if (relevant_items.Contains(item_name))
        {
            relevant_count += 1;
            inventory.AddItem(sprite);
            if (relevant_count == items.Length / 2)
                chest.GetComponent<SpriteRenderer>().sprite = open;
        }
        else if (irrelevant_items.Contains(item_name))
        {
            irrelevant_count += 1;
            lives--;
            Debug.Log(("Lives: " + lives));
            inventory.ManageHearts(lives);
            StartCoroutine(ShowIrrelevant());
            if (lives < 0)
            {
                player.canMove = false;
                StartCoroutine(lm.FadeOut());
            }
        }
        else
            Debug.Log("C'est une blague?!");
    }

    GameObject[] ShuffleList(GameObject[] inputList)
    {
        System.Random random = new System.Random();
        GameObject myGameobject;

        int n = inputList.Length;
        for (int i = 0; i < n; i++)
        {
            int r = i + (int)(random.NextDouble() * (n - i));
            myGameobject = inputList[r];
            inputList[r] = inputList[i];
            inputList[i] = myGameobject;
        }

        return inputList;
    }
}
