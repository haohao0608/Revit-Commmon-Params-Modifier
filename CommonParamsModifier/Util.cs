using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonParamsModifier
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public static class Util
    {
        public static List<T> RawConvertSetToList<T>(IEnumerable set)
        {
            List<T> list = (from T p in set select p).ToList<T>();
            return list;
        }
    }
}
