/* -------------------------------------------------------------------------------------
 * 
 * Name: Util.cs
 * 
 * Author: Zhonghao Lu
 * 
 * Company: University of Alberta
 * 
 * Description: An utility class.
 * 
 * Copyright © University of Alberta 2016
 * 
 * -------------------------------------------------------------------------------------
 */



namespace CommonParamsModifier
{
    #region namespaces
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    #endregion

    public static class Util
    {
        //Try to convert a ParameterSet to a List of Parameter
        //Adapted from Spiderinnet, blog. 
        //https://spiderinnet.typepad.com/blog/2011/04/parameter-of-revit-api-17-generically-convert-set-to-list.html
        public static List<T> RawConvertSetToList<T>(IEnumerable set)
        {
            List<T> list = (from T p in set select p).ToList<T>();
            return list;
        }
    }
}
