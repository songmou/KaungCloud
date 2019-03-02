using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.UI
{
    public class Paginator
    {
        /// <summary>
        /// 页索引
        /// </summary>
        public int page = 1;

        /// <summary>
        /// 每页的数据量
        /// </summary>
        public int rows = 30;

        /// <summary>
        /// 总记录数
        /// </summary>
        public int records { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int total
        {
            get
            {
                if (records > 0)
                {
                    return records % this.rows == 0 ? records / this.rows : records / this.rows + 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 排序列
        /// </summary>
        public string sidx { get; set; }
        /// <summary>
        /// 排序类型
        /// </summary>
        public string sord { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public IEnumerable<object> data { get; set; }
    }

}
