using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kenbu.DBStarman;
using UnityEngine.UI;

public class CellView : DBStarmanCellView {

    [SerializeField]
    private Text _text;

    public void UpdateData(int index, CellData data)
    {
        _text.text = index.ToString () + " / " + data.name;
    }
    public override void OnRemoved(){
        _text.text = "removed";
    }

}

public class CellData{
    public string name;
}
    