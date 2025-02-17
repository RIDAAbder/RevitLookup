﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections
{
    public static class IEnumerableExtensions
    {
        public static List<T> ToList<T>(this System.Collections.IEnumerable set)
        {
            List<T> list = new List<T>();

            foreach (T element in set)
            {
                list.Add(element);
            }

            return list;
        }
    }
}