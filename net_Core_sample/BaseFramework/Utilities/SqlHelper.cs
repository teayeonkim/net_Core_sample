using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace BaseFramework.Utilities
{
    public class SqlHelper : IDisposable
    {
        /*
         * NuGet 패키지에서 설치 진행
         * 
         * Microsoft.EntityFrameworkCore
         * System.Data.SqlClient
         */

        /// <summary>
        /// SQL 선언부 
        /// </summary>
        SqlConnection conn;
        SqlCommand cmd;
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        // ~SqlHelper()
        // {
        //     // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }



        public SqlHelper(string connectionName)
        {
            cmd = new SqlCommand();

            SetConnection(connectionName);
        }

        //DB 경로 추출 
        public void SetConnection(string connectionName)
        {
            conn = new SqlConnection(connectionName);
        }

        public void ExecuteQuery(string Query, int commandTimeout)
        {
            conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = Query;
            cmd.CommandTimeout = commandTimeout;

            cmd.ExecuteNonQuery();

            conn.Close();

        }

        #region 파라미터 설정
        public void ClearParameter() { cmd.Parameters.Clear(); }

        public void AddInParameter(string name, SqlDbType type, int size, object value)
        {
            SqlParameter par = new SqlParameter();
            par.ParameterName = name;
            par.SqlDbType = type;
            par.Size = size;
            par.Value = value == null ? DBNull.Value : value;
            par.Direction = ParameterDirection.Input;

            cmd.Parameters.Add(par);
        }

        public void AddInParameter(string name, SqlDbType type, object value)
        {
            SqlParameter par = new SqlParameter();
            par.ParameterName = name;
            par.SqlDbType = type;
            par.Value = value == null ? DBNull.Value : value;
            par.Direction = ParameterDirection.Input;

            cmd.Parameters.Add(par);
        }
        public void AddOutParameter(string name, SqlDbType type)
        {
            SqlParameter par = new SqlParameter();
            par.ParameterName = name;
            par.SqlDbType = type;
            par.Direction = ParameterDirection.Output;

            switch (type)
            {
                case SqlDbType.VarChar:
                case SqlDbType.NVarChar:
                    par.Size = -1;
                    break;
                default:
                    par.Size = 0;
                    break;
            }

            cmd.Parameters.Add(par);
        }
        public object GetParameterValue(string name)
        {
            return cmd.Parameters[name].Value;
        }
        public T GetParameterValue<T>(string name)
        {
            var result = cmd.Parameters[name].Value;

            if (result == DBNull.Value || result == null)
            {
                return default(T);
            }
            else
            {
                return (T)result;
            }
        }
        #endregion


    }
}
