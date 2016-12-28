using System;
namespace WZYB.Model
{
	/// <summary>
	/// ʵ����OCSStatus
	/// </summary>
	[Serializable]
    public class OCSStatus : IComparable
	{
		#region Model
		private int _carid;
		private int _line;
		private int _direction;
		private int _sequence;
		private int _backline;
		private decimal _position;
		/// <summary>
		/// ����ID
		/// </summary>
		public int carId
		{
			set{ _carid=value;}
			get{return _carid;}
		}
		/// <summary>
		/// ����������
		/// </summary>
		public int line
		{
			set{ _line=value;}
			get{return _line;}
		}
		/// <summary>
		/// ����0ֹͣ��1��ǰ��2���
		/// </summary>
		public int direction
		{
			set{ _direction=value;}
			get{return _direction;}
		}
		/// <summary>
		/// ����������˳��
		/// </summary>
		public int sequence
		{
			set{ _sequence=value;}
			get{return _sequence;}
		}
		/// <summary>
		/// ��һ��������
		/// </summary>
		public int backLine
		{
			set{ _backline=value;}
			get{return _backline;}
		}
		/// <summary>
		/// ����λ��
		/// </summary>
		public decimal position
		{
			set{ _position=value;}
			get{return _position;}
		}
		#endregion Model

        #region ʵ�ֱȽϽӿڵ�CompareTo����
        public int CompareTo(object obj)
        {
            int res = 0;
            try
            {
                OCSStatus sObj = (OCSStatus)obj;
                if (this.sequence > sObj.sequence)
                {
                    res = 1;
                }
                else if (this.sequence < sObj.sequence)
                {
                    res = -1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("�Ƚ��쳣", ex.InnerException);
            }
            return res;
        }
        #endregion

	}
}

