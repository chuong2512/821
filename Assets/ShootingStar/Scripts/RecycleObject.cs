/*
RecycleObject
Version 1.0

This script is used to recycle objects in the pool manager
*/

using UnityEngine;
using System.Collections;

public class RecycleObject : MonoBehaviour {

    public void Recycle()
    {
        PoolManager.Instance.Recycle(gameObject.name.Replace("(Clone)",""),gameObject);
    }
}
