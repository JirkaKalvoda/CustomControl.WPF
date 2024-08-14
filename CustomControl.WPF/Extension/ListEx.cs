using System.Collections.Generic;

namespace System.Collections
{
    public static class ListEx
    {
        /// <summary>
        /// 比较2个序列的内容，忽略排序
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static bool CompareIListOmitOrder(IList list1, IList list2)
        {
            if (list1 == null && list2 == null)
            {
                return true;
            }
            else if (list1 == null && list2 != null
                     || list1 != null && list2 == null
                     || list1.Count != list2.Count
                    )
            {
                return false;
            }
            Dictionary<object, int> dict = new Dictionary<object, int>();
            foreach (object item in list1)
            {
                if (dict.ContainsKey(item))
                {
                    dict[item] = dict[item] + 1;
                }
                else
                {
                    dict.Add(item, 1);
                }
            }
            foreach (object item in list2)
            {
                if (dict.ContainsKey(item))
                {
                    dict[item] = dict[item] - 1;
                }
                else
                {
                    return false;
                }
            }
            foreach (int i in dict.Values)
            {
                if (i != 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
