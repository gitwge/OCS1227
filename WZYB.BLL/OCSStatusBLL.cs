using System;
using System.Data;
using System.Collections.Generic;
using WZYB.Model;
using WZYB.DAL;
namespace WZYB.BLL
{
	/// <summary>
	/// ҵ���߼���OCSStatusBLL ��ժҪ˵����
	/// </summary>
	public class OCSStatusBLL
	{
		#region  ��Ա����
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public static bool Exists(int carId)
		{
			return OCSStatusDAL.Exists(carId);
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public static bool ExistsBy(string strWhere)
		{
			return OCSStatusDAL.ExistsBy(strWhere);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public static bool Add(OCSStatus model)
		{
			return OCSStatusDAL.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public static bool Update(OCSStatus model)
		{
			return OCSStatusDAL.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public static bool Delete(int carId)
		{
			return OCSStatusDAL.Delete(carId);
		}

		public static int DeleteBy(string strWhere)
		{
			return OCSStatusDAL.DeleteBy(strWhere);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public static OCSStatus GetModel(int carId)
		{
			return GetModelBy("carId="+carId+" ");
		}
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public static OCSStatus GetModelBy(string strWhere)
		{
			DataSet ds = GetList(1,strWhere,"");
			if (ds.Tables[0].Rows.Count > 0)
				return DataTableToList(ds.Tables[0])[0];
			else
				return null;
		}
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public static OCSStatus GetModelBy(string strWhere,string filedOrder)
		{
			DataSet ds = GetList(1,strWhere,filedOrder);
			if (ds.Tables[0].Rows.Count > 0)
				return DataTableToList(ds.Tables[0])[0];
			else
				return null;
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public static DataSet GetList(string strWhere)
		{
			return GetList(0, strWhere, "");
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public static DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return OCSStatusDAL.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public static DataSet GetList(string strWhere,string filedOrder)
		{
			return GetList(0,strWhere,filedOrder);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public static List<OCSStatus> GetModelList(string strWhere)
		{
			DataSet ds = GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public static List<OCSStatus> GetModelList(int Top,string strWhere,string filedOrder)
		{
			DataSet ds = GetList(Top,strWhere,filedOrder);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public static List<OCSStatus> GetModelList(string strWhere,string filedOrder)
		{
			DataSet ds = GetList(strWhere,filedOrder);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public static List<OCSStatus> DataTableToList(DataTable dt)
		{
			List<OCSStatus> modelList = new List<OCSStatus>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				OCSStatus model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new OCSStatus();
					if(dt.Rows[n]["carId"].ToString()!="")
					{
						model.carId=int.Parse(dt.Rows[n]["carId"].ToString());
					}
					if(dt.Rows[n]["line"].ToString()!="")
					{
						model.line=int.Parse(dt.Rows[n]["line"].ToString());
					}
					if(dt.Rows[n]["direction"].ToString()!="")
					{
						model.direction=int.Parse(dt.Rows[n]["direction"].ToString());
					}
					if(dt.Rows[n]["sequence"].ToString()!="")
					{
						model.sequence=int.Parse(dt.Rows[n]["sequence"].ToString());
					}
					if(dt.Rows[n]["backLine"].ToString()!="")
					{
						model.backLine=int.Parse(dt.Rows[n]["backLine"].ToString());
					}
					if(dt.Rows[n]["position"].ToString()!="")
					{
						model.position=decimal.Parse(dt.Rows[n]["position"].ToString());
					}
					modelList.Add(model);
				}
			}
			return modelList;
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public static DataSet GetAllList()
		{
			return GetList("");
		}

		#endregion  ��Ա����

        #region �Զ��巽��
        
        public static int getCountByLine(int line)
        {
            return OCSStatusDAL.getCountByLine(line);
        }

        #endregion
    }
}

