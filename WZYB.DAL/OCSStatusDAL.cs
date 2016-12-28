using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using WZYB.DBUtility;

namespace WZYB.DAL
{
	/// <summary>
	/// ���ݷ�����OCSStatusDAL��
	/// </summary>
	public class OCSStatusDAL
	{
		#region  ��Ա����
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public static bool Exists(int carId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from T_OCSStatus");
			strSql.Append(" where carId=@carId ");
			SqlParameter[] parameters = {
					new SqlParameter("@carId", SqlDbType.Int,4)};
			parameters[0].Value = carId;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}
		public static bool ExistsBy(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from T_OCSStatus");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Exists(strSql.ToString());
		}


		/// <summary>
		/// ����һ������
		/// </summary>
		public static bool Add(WZYB.Model.OCSStatus model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into T_OCSStatus(");
			strSql.Append("carId,line,direction,sequence,backLine,position)");
			strSql.Append(" values (");
			strSql.Append("@carId,@line,@direction,@sequence,@backLine,@position)");
			SqlParameter[] parameters = {
					new SqlParameter("@carId", SqlDbType.Int,4),
					new SqlParameter("@line", SqlDbType.Int,4),
					new SqlParameter("@direction", SqlDbType.Int,4),
					new SqlParameter("@sequence", SqlDbType.Int,4),
					new SqlParameter("@backLine", SqlDbType.Int,4),
					new SqlParameter("@position", SqlDbType.Float,8)};
			parameters[0].Value = model.carId;
			parameters[1].Value = model.line;
			parameters[2].Value = model.direction;
			parameters[3].Value = model.sequence;
			parameters[4].Value = model.backLine;
			parameters[5].Value = model.position;

			int i = DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (i > 0)
				return true;
			else
				return false;
		}
		/// <summary>
		/// ����һ������
		/// </summary>
		public static bool Update(WZYB.Model.OCSStatus model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update T_OCSStatus set ");
			strSql.Append("line=@line,");
			strSql.Append("direction=@direction,");
			strSql.Append("sequence=@sequence,");
			strSql.Append("backLine=@backLine,");
			strSql.Append("position=@position");
			strSql.Append(" where carId=@carId ");
			SqlParameter[] parameters = {
					new SqlParameter("@carId", SqlDbType.Int,4),
					new SqlParameter("@line", SqlDbType.Int,4),
					new SqlParameter("@direction", SqlDbType.Int,4),
					new SqlParameter("@sequence", SqlDbType.Int,4),
					new SqlParameter("@backLine", SqlDbType.Int,4),
					new SqlParameter("@position", SqlDbType.Float,8)};
			parameters[0].Value = model.carId;
			parameters[1].Value = model.line;
			parameters[2].Value = model.direction;
			parameters[3].Value = model.sequence;
			parameters[4].Value = model.backLine;
			parameters[5].Value = model.position;

			int i = DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (i > 0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public static bool Delete(int carId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from T_OCSStatus ");
			strSql.Append(" where carId=@carId ");
			SqlParameter[] parameters = {
					new SqlParameter("@carId", SqlDbType.Int,4)};
			parameters[0].Value = carId;

			int i = DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (i > 0)
				return true;
			else
				return false;
		}
		/// <summary>
		/// ɾ������
		/// </summary>
		public static int DeleteBy(string strWhere)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from T_OCSStatus ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.ExecuteSql(strSql.ToString());
		}


		/// <summary>
		/// ���ǰ��������
		/// </summary>
		public static DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" carId,line,direction,sequence,backLine,position ");
			strSql.Append(" FROM T_OCSStatus ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			if(filedOrder.Trim()!="")
			{
				strSql.Append(" order by " + filedOrder);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		#endregion  ��Ա����
        
        #region �Զ��巽��

        public static int getCountByLine(int line)
        {
            string strSql = "select count(*) from T_OCSStatus where line = " + line;
            object obj = DbHelperSQL.GetSingle(strSql);
            return Convert.ToInt32(obj);
        }

        #endregion
    }
}

