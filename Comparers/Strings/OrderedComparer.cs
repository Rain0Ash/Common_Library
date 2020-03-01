// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Library.Comparers
{
    public class OrderedComparer : IComparer<Object>
    {
        private readonly List<Object> _orderList;

        public OrderedComparer(IEnumerable<Object> languageOrderList = null)
        {
            _orderList = (languageOrderList ?? new Object[0]).ToList();
        }

        public Int32 Compare(Object x, Object y)
        {
            Int32 indexOfX = _orderList.IndexOf(x);
            Int32 indexOfY = _orderList.IndexOf(y);
                    
            Int32 ix = indexOfX != -1 ? indexOfX : _orderList.Count + 1;
            Int32 iy = indexOfY != -1 ? indexOfY : _orderList.Count + 1;
            return ix.CompareTo(iy);
        }
    }
}