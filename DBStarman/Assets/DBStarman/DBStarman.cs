using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace kenbu.DBStarman{
    public abstract class DBStarman<CellViewType, CellDataType> : MonoBehaviour 
        where CellViewType: DBStarmanCellView
    {
        
        [SerializeField]
        public ScrollRect scrollRect;

        [SerializeField]
        public LayoutGroup layoutgroup;


        public List<CellViewType> _cellViewPool;
        public List<CellDataType> _cellDataList;
        public Dictionary<int, CellViewType> _cellViewTable;


        private Vector2 contentSize;

        [SerializeField]
        private RectTransform _empty;

        public virtual void Setup(List<CellDataType> cellDataList){

            _cellViewTable = new Dictionary<int, CellViewType>();


            _cellViewPool = new List<CellViewType> ();

            _cellDataList = cellDataList;
            int l = _cellDataList.Count;
            contentSize = Vector2.zero;
            if (scrollRect.vertical) {
                for (var i = 0; i < l; i++) {
                    var v = GetCellSize (i);
                    contentSize.y += v.y;
                    contentSize.x = Math.Max (contentSize.x, v.x);
                }
            }
            else
            {
                for (var i = 0; i < l; i++) {
                    var v = GetCellSize (i);
                    contentSize.x += v.x;
                    contentSize.y = Math.Max (contentSize.y, v.y);

                }
            }

            scrollRect.content.sizeDelta = contentSize;

            scrollRect.onValueChanged.AddListener (OnScroll);

            _empty.SetAsFirstSibling ();
        }
        //リストの数
        public int CellLength (){
            return _cellDataList.Count;
        }
        //ViewPort上で表示されるCellのうち何番目か
        public abstract Vector2 GetCellSize(int index);
        //ViewPort上で表示されるCellのうち何番目か
        public abstract CellViewType GetCellForIndex(int index);


        private void OnScroll(Vector2 v){
            float viewMinPos = 0.0f;    //スクロール位置
            float viewMaxPos = 0.0f;    //表示される限界領域
            bool isVertical = scrollRect.vertical;
            if (isVertical) {
                viewMinPos = scrollRect.content.anchoredPosition.y;
                viewMaxPos = viewMinPos + scrollRect.viewport.rect.height; 
            }
            else
            {
                viewMinPos = scrollRect.content.anchoredPosition.x;
                viewMaxPos = viewMinPos + scrollRect.viewport.rect.width; 
            }



            var l = _cellDataList.Count;
            var cellPos = 0.0f;
            var viewMinCell = -1;
            var viewMaxCell = -1;

            for (var i = 0; i < l; i++) {
                
                var cellSizeV = GetCellSize (i); 
                var cellSize = 0.0f;
                if (isVertical) {
                    cellSize = cellSizeV.y;
                } else {
                    cellSize = cellSizeV.x;
                }

                //pos, sizeのセルは、viewMinpos, viewMaxPosの中に含まれるか？
                var pos = cellPos + cellSize;



                //画面内判定
                //上下のマージンとか考慮する・・・scrollRectから。な。
                if (viewMinCell == -1) {
                    if (viewMinPos <= pos) {
                        viewMinCell = i;
                        _empty.sizeDelta = new Vector2 (10, cellPos);
                    }
                } else if (cellPos <= viewMaxPos) {
                    viewMaxCell = i; 
                } else {
                    //viewMaxCell = i; 
                    break;
                }



                cellPos += cellSize;

            }
            viewMaxCell++;

            //before
            for (int i = 0; i < viewMinCell; i++) {
                RemoveCell (i);
            }
            //生成

            for (int i = viewMinCell; i < viewMaxCell; i++) {
                if (_cellViewTable.ContainsKey (i) == false) 
                {
                    var c = GetCellForIndex (i);
                    c.gameObject.SetActive (true);
                    _cellViewTable [i] = c;
                    c.transform.parent = layoutgroup.transform;
                    //c.transform.SetSiblingIndex (i + 1);
                    c.name = "list" + i;
                }
                _cellViewTable[i].transform.SetSiblingIndex (i + 1);
            }

            //after
            for (int i = viewMaxCell; i < _cellDataList.Count; i++) {
                RemoveCell (i);
            }

            //最後順番そーと
            _debug.text = "minPos:" + viewMinPos + ", maxPos: " + viewMaxPos +"\n";
            _debug.text += "minCell:" + viewMinCell + ", maxCell: " + viewMaxCell ;

        }

       
        private void RemoveCell(int i){
            if (_cellViewTable.ContainsKey (i)) 
            {
                var c = _cellViewTable [i];
                c.OnRemoved ();
                _cellViewPool.Add (c);
                _cellViewTable.Remove (i);
                c.gameObject.SetActive (false);
            }
        }

            
        [SerializeField]
        private Text _debug;

        private void Update(){
        }

    }

}
