using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalFunctions {
    public static T FindComponent<T>(GameObject obj) {
        T returnVal = obj.GetComponent<T>();
        if (!returnVal.Equals(null)) {
            return returnVal;
        }
        returnVal = obj.GetComponentInChildren<T>();
        if (!returnVal.Equals(null)) {
            return returnVal;
        }
        returnVal = obj.GetComponentInParent<T>();
        if (returnVal.Equals(null)) {
            Debug.Log("ERROR: Could not Find Component");
        }
        return returnVal;
    }
}