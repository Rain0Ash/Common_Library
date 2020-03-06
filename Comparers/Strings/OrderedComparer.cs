// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Library.Comparers
{
    public class OrderedComparer : OrderedComparer<Object>
    {
    }
    
    public class OrderedComparer<T> : IComparer<T>
    {
        private readonly List<T> _orderList;

        protected IList<T> Order
        {
            get
            {
                return _orderList;
            }
        }

        public OrderedComparer(IEnumerable<T> languageOrderList = null)
        {
            _orderList = (languageOrderList ?? new T[0]).ToList();
        }

        public Int32 Compare(T x, T y)
        {
            Int32 indexOfX = _orderList.IndexOf(x);
            Int32 indexOfY = _orderList.IndexOf(y);
                    
            Int32 ix = indexOfX != -1 ? indexOfX : _orderList.Count + 1;
            Int32 iy = indexOfY != -1 ? indexOfY : _orderList.Count + 1;
            return ix.CompareTo(iy);
        }
    }
}