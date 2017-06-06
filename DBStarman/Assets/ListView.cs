using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kenbu.DBStarman;
using UnityEngine.UI;

public class ListView : DBStarman<CellView, CellData> {


    [SerializeField]
    private CellView _cellPrefab;



    public override void Setup(List<CellData> dataList){
        base.Setup (dataList);
        _cellPrefab.gameObject.SetActive (false);
    }


    //ViewPort上で表示されるCellのうち何番目か
    public override Vector2 GetCellSize(int index){
        return new Vector2 (394, 100);
    }


    //ViewPort上で表示されるCellのうち何番目か
    public override CellView GetCellForIndex(int index){

        if (_cellDataList == null) {
            return null;
        }
        CellView c;
        if (_cellViewPool.Count == 0) {
            c = Instantiate (_cellPrefab) as CellView;
        } else {
            c = _cellViewPool [0];
            _cellViewPool.RemoveAt (0);
        }

        c.UpdateData (index, _cellDataList [index]);

        return c;
    }
}
