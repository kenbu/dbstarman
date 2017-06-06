using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class Dev : MonoBehaviour {

    [SerializeField]
    private ListView _list;
    [SerializeField]
    private ListView _horizontalList;

	// Use this for initialization
	void Start () {
        var dataList = new List<CellData> ();
        for(var i = 0; i < 50; i++)
        {
            var d = new CellData ();
            d.name = "name: " + i;
            dataList.Add (d);
        }
        _list.Setup (dataList);
        _horizontalList.Setup (dataList);
	}
	
}
