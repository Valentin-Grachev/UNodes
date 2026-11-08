using System;
using System.Collections.Generic;

namespace UNodes.Extensions
{
    public static class Extensions
    {
        
        public static T FindOrExeption<T>(this List<T> list, Predicate<T> predicate)
        {
            int index = list.FindIndex(predicate);
            if (index >= 0) return list[index];

            throw new Exception("List does not contain the element!");
        }

    }
}
