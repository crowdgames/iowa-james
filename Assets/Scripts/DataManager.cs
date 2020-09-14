using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager {

    public static int index = 0;
    public static int points = 0;
    public static int mode = 4; //0 - Designer, 1 - Path, 2 - Random, 3 - No coins, 4 - HCG
    public static int NCOINS = 10;
    public static float play_time = 0.0f;
    public static string player_id = "";
    public static int INIT_LIVES = 3;
    //public static string host = "viridian.ccs.neu.edu";
    public static string server = "localhost";
    public static int port_number = 3004;
    public static int matchmaking = 0;   //0 - only levels have ratings, 1 - levels and tasks both have ratings
    public static int decoupled = 0;   //0 - coupled, 1 - decoupled 
    public static string host = "";
    public static bool separate_dbs = true;
    public static int num_dbs = 2;
    
    public static string[] scenarios = new string[] {"Grocery Store","Pastry Shop","Clothing Store","Sports Store","Hardware Store"};

    public static Dictionary<string, string[]> hcg_items = new Dictionary<string, string[]>
    {
        { "Grocery Store",new string[]{ "bread", "candy", "carrot", "milk", "popcorn", "bananas", "bread" } },
        { "Pastry Shop",new string[]{ "pie", "bun", "cake", "croissant", "donut", "cupcake", "doughnut" } },
        { "Clothing Store",new string[]{ "shirts","hat","sweater","coat","tshirt", "shirts", "hat"} },
        { "Sports Store",new string[]{ "baseball", "basketball", "gloves", "soccerball", "cleats", "football", "basketball"} },
        { "Hardware Store",new string[]{ "drill", "saw", "hammer", "nails", "pliers", "wrench", "axe" } }
    };

    public static Dictionary<string, string> coupled_mapping = new Dictionary<string, string>
    {
        { "Level_00", "grocery_3"},
        { "Level_02", "pastry_3"},
        { "Level_15", "clothing_3"},
        { "Level_01", "sports_3"},
        { "Level_14", "hardware_3"},
        { "Level_16", "grocery_3"},
        { "Level_10", "pastry_3"},
        { "Level_19", "sports_3"},
        { "Level_03", "hardware_3"},
        { "Level_20", "grocery_5"},
        { "Level_18", "pastry_5"},
        { "Level_17", "clothing_5"},
        { "Level_21", "sports_5"},
        { "Level_11", "hardware_5"},
        { "Level_04", "grocery_5"},
        { "Level_22", "pastry_5"},
        { "Level_24", "clothing_5"},
        { "Level_09", "grocery_7"},
        { "Level_08", "pastry_7"},
        { "Level_06", "clothing_7"},
        { "Level_05", "sports_7"},
        { "Level_23", "hardware_7"},
        { "Level_12", "clothing_7"},
        { "Level_07", "sports_7"},
        { "Level_13", "hardware_7"},
    };
}