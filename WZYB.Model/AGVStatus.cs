using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WZYB.Model
{
    /// <summary>
	/// 实体类AGVStatus
	/// </summary>
    [Serializable]
    public class AGVStatus
    {
        #region Model
        private int _carid;
        private int _line;
        private int _direction;
        private int _sequence;
        private int _backline;
        private decimal? _position;
        /// <summary>
        /// 车辆ID
        /// </summary>
        public int carId
        {
            set { _carid = value; }
            get { return _carid; }
        }
        /// <summary>
        /// 所在驱动段
        /// </summary>
        public int line
        {
            set { _line = value; }
            get { return _line; }
        }
        /// <summary>
        /// 方向0停止　1向前　2向后
        /// </summary>
        public int direction
        {
            set { _direction = value; }
            get { return _direction; }
        }
        /// <summary>
        /// 所在驱动段顺序
        /// </summary>
        public int sequence
        {
            set { _sequence = value; }
            get { return _sequence; }
        }
        /// <summary>
        /// 上一条驱动段
        /// </summary>
        public int backLine
        {
            set { _backline = value; }
            get { return _backline; }
        }
        /// <summary>
        /// 具体位置
        /// </summary>
        public decimal? position
        {
            set { _position = value; }
            get { return _position; }
        }
        #endregion Model
    }
}
