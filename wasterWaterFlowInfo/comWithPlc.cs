using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharp7;

namespace comWithPlc
{
    class getPlcValues
    {
        S7Client Client = new S7Client();
        
        public int getPlcDbwValue(string plcIp,int Rack,int Slot,int DbNum,int DbwNum)
        {
            byte[] Buffer = new byte[65536];
            Client.ConnectTo(plcIp, Rack, Slot);        
            Client.DBRead(DbNum, DbwNum, 2, Buffer);//读取DbwNum所对应的字的值
            Client.Disconnect();
            return  S7.GetIntAt(Buffer, 0);           
        }

        public void resetPlcDbwValue(string plcIp, int Rack, int Slot, int DbNum, int DbwNum,int writeValue)
        {
            short a =(short) writeValue;//将整形的writeValue强制转换成short类型
            Client.ConnectTo(plcIp, Rack, Slot);
            byte[] buffer = new byte[65536];
            S7.SetIntAt(buffer, 0, a);
            Client.DBWrite(DbNum,DbwNum,2, buffer);//将DbwNum对应的字更新
            Client.Disconnect();
        }

        public float readPlcDbdValues(string plcIp, int Rack, int Slot, int DbNum, int DbdNum)
        {
            byte[] Buffer = new byte[65536];
            Client.ConnectTo(plcIp, Rack, Slot);
            Client.DBRead(DbNum, DbdNum, 4, Buffer);//读取DbdNum所对应的字的值
            Client.Disconnect();
            return S7.GetRealAt(Buffer, 0);
        }
        public void writePlcDbdValues(string plcIp, int Rack, int Slot, int DbNum, int DbdNum, int writeValue)
        {
            short a = (short)writeValue;//将整形的writeValue强制转换成short类型
            Client.ConnectTo(plcIp, Rack, Slot);
            byte[] buffer = new byte[65536];
            S7.SetDIntAt(buffer, 0, a);
            Client.DBWrite(DbNum, DbdNum, 4, buffer);//将DbwNum对应的字更新
            Client.Disconnect();

        }
        public bool getPlcDbxVaules(string plcIp, int Rack, int Slot, int DbNum, int dbx,int dbxx)
        {
            byte[] Buffer = new byte[2];
            Client.ConnectTo(plcIp, Rack, Slot);
            Client.DBRead(DbNum,dbx,2, Buffer);//读取DbwNum所对应的字的值        
            return S7.GetBitAt(Buffer, 0,dbxx);          
        }

    }
}
