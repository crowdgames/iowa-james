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
                return HCGItems.grocery_store;

            case "Pastry Shop":
                return HCGItems.pastry_shop;

            case "Clothing Store":
                return HCGItems.clothing_store;

            case "Sports Equipment":
                return HCGItems.sports_equipment;

            case "Tools Shop":
                return HCGItems.hardware_store;

            default:
                return null;
        }
    }

}


