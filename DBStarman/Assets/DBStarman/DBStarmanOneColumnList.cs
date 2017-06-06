using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kenbu.DBStarman;
using System;


namespace kenbu.DBStarman{

    public class DBStarmanOneColumList<CellViewType, CellDataType>: DBStarman<CellViewType, CellDataType>

        where CellViewType: DBStarmanCellView
    
    {
        protected override Vector2 GetContentSize(){
            int l = _cellDataList.Count;

            var contentSize = Vector2.zero;
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
            return contentSize;
        }

        protected override Vector2 CulcurateMinMaxPos(){
            bool isVertical = scrollRect.vertical;
            if (isVertical) {
                return new Vector2 (scrollRect.content.anchoredPosition.y, scrollRect.content.anchoredPosition.y + scrollRect.viewport.rect.height);
            } else {
                return new Vector2 (-scrollRect.content.anchoredPosition.x, -scrollRect.content.anchoredPosition.x + scrollRect.viewport.rect.width);
            }
        }

        protected override Vector2 CulcurateMinMaxCell(Vector2 minMaxPos){
            bool isVertical = scrollRect.vertical;
            var viewMinPos = minMaxPos.x;
            var viewMaxPos = minMaxPos.y;

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
                        if (isVertical) {
                            _empty.sizeDelta = new Vector2 (10, cellPos);
                        } else {
                            _empty.sizeDelta = new Vector2 (cellPos, 10);
                        }
                    }
                } else if (cellPos <= viewMaxPos) {
                    viewMaxCell = i; 
                } else {
                    break;
                }
                cellPos += cellSize;
            }
            viewMaxCell++;

            return new Vector2 (viewMinCell, viewMaxCell);
        }
    }
}