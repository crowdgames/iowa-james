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
    public static string host = "viridian.ccs.neu.edu:3004";
    public static bool hcg = true;
}