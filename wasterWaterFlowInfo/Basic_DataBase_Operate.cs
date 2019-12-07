using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Data;


namespace wasterWaterFlowInfo
{
    class Basic_DataBase_Operate
    {

        //连接Oracle数据库的方法by ethan  20180916

        //连接数据库
        public OracleConnection OrcGetCon()
        {
            string M_str_sqlcon = "Data Source=10.228.141.253/ORCL;User Id=WEBKF;Password=WEBKF";//定义数据库连接字符串   
            OracleConnection myCon = new OracleConnection(M_str_sqlcon);
            return myCon;
        }


        //连接OracleConnection,执行SQL
        public void OrcGetCom(string M_str_sqlstr)
        {
            OracleConnection orccon = this.OrcGetCon();
            orccon.Open();
            OracleCommand orccom = new OracleCommand(M_str_sqlstr, orccon);
            orccom.ExecuteNonQuery();
            orccon.Close();

        }


        //创建DataSet对象
        public DataSet OrcGetDs(string M_str_sqlstr, string M_str_table)
        {
            OracleConnection orccon = this.OrcGetCon();
            OracleDataAdapter orcda = new OracleDataAdapter(M_str_sqlstr, orccon);
            DataSet myds = new DataSet();
            orcda.Fill(myds, M_str_table);
            return myds;
        }


        //创建OracleDataReader对象
        public OracleDataReader OrcGetRead(string M_str_sqlstr)
        {
            OracleConnection orccon = this.OrcGetCon();
            orccon.Open();
            OracleCommand orccom = new OracleCommand(M_str_sqlstr, orccon);
            OracleDataReader orcread = orccom.ExecuteReader();
            return orcread;

        }
    }
}
