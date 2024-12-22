/*
This script Recycles an Object after t seconds
*/

using UnityEngine;
using System.Collections;

public class RecycleAfter : MonoBehaviour {

    #region Variables

    //number of seconds until Recycle
    public float t = 1f;

    #endregion


    #region Methods


    //reset the time...if it's already been recycled once
    void OnEnable()
    {
        Invoke("Recycle",t);
    }


	public void Recycle()
    {
        PoolManager.Instance.Recycle(gameObject.name.Replace("(Clone)",""),gameObject);
    }

    #endregion
}
