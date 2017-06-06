using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.Remoting.Messaging;

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


        [SerializeField]
        public RectTransform _empty;

        public virtual void Setup(List<CellDataType> cellDataList){

            _cellViewTable = new Dictionary<int, CellViewType>();

            _cellViewPool = new List<CellViewType> ();

            _cellDataList = cellDataList;

            scrollRect.content.sizeDelta = GetContentSize();

            scrollRect.onValueChanged.AddListener (OnScroll);

            _empty.SetAsFirstSibling ();
        }
        //リストの数
        public int CellLength (){
            return _cellDataList.Count;
        }


        private void OnScroll(Vector2 v){
            var viewPos = CulcurateMinMaxPos ();
            var cellPos = CulcurateMinMaxCell(viewPos);
            int viewMinCell = (int)cellPos.x;
            int viewMaxCell = (int)cellPos.y;


            //before
            for (int i = 0; i < viewMinCell; i++) {
                RemoveCell (i);
            }
            //生成
            for (int i = (int)cellPos.x; i < viewMaxCell; i++) {
                if (_cellViewTable.ContainsKey (i) == false) 
                {
                    var c = GetCellForIndex (i);
                    c.gameObject.SetActive (true);
                    _cellViewTable [i] = c;
                    c.transform.SetParent (layoutgroup.transform);
                    c.OnAdded ();
                }
                _cellViewTable[i].transform.SetSiblingIndex (i + 1);
            }

            //after
            for (int i = viewMaxCell; i < _cellDataList.Count; i++) {
                RemoveCell (i);
            }

            _debug.text = "minPos:" + viewPos.x + ", maxPos: " + viewPos.y +"\n";
            _debug.text += "minCell:" + cellPos.x + ", maxCell: " + cellPos.y ;

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

        //ViewPort上で表示されるCellのうち何番目か
        public virtual Vector2 GetCellSize(int index){
            return Vector2.zero;
        }

        protected virtual Vector2 GetContentSize(){
            return Vector2.zero;
        }
        //ViewPort上で表示されるCellのうち何番目か
        public virtual CellViewType GetCellForIndex(int index){
            return null;
        }

        protected virtual Vector2 CulcurateMinMaxPos (){
            return Vector2.zero;
        }

        protected virtual Vector2 CulcurateMinMaxCell (Vector2 minMaxPos){
            return Vector2.zero;
        }
       

            
        [SerializeField]
        private Text _debug;


    }

}
