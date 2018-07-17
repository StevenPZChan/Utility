using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace Utility.Form
{
    /// <summary>
    /// 控件属性设置类
    /// </summary>
    public static class UIControl
    {
        #region Methods
        /// <summary>
        /// 为控件的某个属性赋值，跨线程时自动使用委托
        /// </summary>
        /// <param name="control">控件对象</param>
        /// <param name="property">属性名</param>
        /// <param name="value">属性值</param>
        public static void Assign(this Control control, string property, object value)
        {
            if (!control.InvokeRequired)
                control.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)?.SetValue(control, value);
            else
                control.BeginInvoke(new Action<Control, string, object>(Assign), control, property, value);
        }

        /// <summary>
        /// 为多个控件的某个属性赋值，跨线程时自动使用委托
        /// </summary>
        /// <param name="controllist">控件对象List集</param>
        /// <param name="property">属性名</param>
        /// <param name="value">属性值</param>
        public static void Assign(this IEnumerable<Control> controllist, string property, object value)
        {
            foreach (Control control in controllist)
                Assign(control, property, value);
        }

        /// <summary>
        /// 为多个控件的某个属性赋值，赋的值为字典查询
        /// </summary>
        /// <param name="controllist">控件对象List集</param>
        /// <param name="property">属性名</param>
        /// <param name="valuelist">属性字典Dictionary集</param>
        /// <param name="index">赋值字典查询下标</param>
        public static void Assign(this IList<Control> controllist, string property, IDictionary<int, object> valuelist, IList<int> index)
        {
            try
            {
                for (var i = 0; i < controllist.Count; i++)
                    Assign(controllist[i], property, valuelist[index[i]]);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 为一组控件的某个属性赋值，使用字典查询的方法
        /// </summary>
        /// <param name="controllist"></param>
        /// <param name="property"></param>
        /// <param name="valuelist"></param>
        /// <param name="index"></param>
        public static void Assign(this IDictionary<string, Control> controllist, string property, IDictionary<string, object> valuelist,
            IDictionary<string, string> index)
        {
            try
            {
                ICollection<string> keys = index.Keys;
                foreach (string key in keys)
                    Assign(controllist[key], property, valuelist[index[key]]);
            }
            catch (Exception)
            {
                // ignored
            }
        }
        #endregion
    }
}
