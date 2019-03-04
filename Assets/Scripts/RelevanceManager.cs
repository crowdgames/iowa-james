using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelevanceManager
{


    public static string[] DetermineLocation(string locationName)
    {
        switch (locationName)
        {
            case "Grocery Store":
                return HCGItems.groceryStore;

            case "Pastry Shop":

                return HCGItems.pastry;

            case "Clothing Store":
                return HCGItems.clothing;

            case "Sports Equipment":
                return HCGItems.sportsEquipment;

            case "Tools Shop":
                return HCGItems.tools;

            default:
                return null;
        }
    }

}
