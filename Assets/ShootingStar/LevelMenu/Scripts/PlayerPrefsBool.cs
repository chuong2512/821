/*
PlayerPrefsBool
Version 1.0

contains methods for storing bools in the PlayerPrefs.
Note: stores 0 and 1 as int, but converts it to a bool on return

*/


using UnityEngine;
using System.Collections;

public class PlayerPrefsBool : MonoBehaviour {

    //set a Bool
    public static void SetBool(string Key,bool Value)
    {
        if (Value)
        {
            PlayerPrefs.SetInt(Key + "_bool",1);
        }
        else
        {
            PlayerPrefs.SetInt(Key + "_bool",0);
        }

    }
    
    //get a Bool
    public static bool GetBool(string Key)
    {
        if (!PlayerPrefs.HasKey(Key + "_bool"))
        {
            Debug.LogError("Value for " + Key + " Doesn't Exist");
        }

        if (PlayerPrefs.GetInt(Key + "_bool") == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //see if a Bool has been saved
    public static bool HasBoolKey(string Key)
    {
        if (PlayerPrefs.HasKey(Key + "_bool"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //deletes a Bool
    public static void DeleteBool(string Key)
    {
        PlayerPrefs.DeleteKey(Key + "_bool");
    }
}
