using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using comWithPlc;

namespace wasterWaterFlowInfo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Basic_DataBase_Operate operateDatabase = new Basic_DataBase_Operate();
        getPlcValues operatePlc = new getPlcValues();

        float a1, b1, c1, d1, e1, f1, g1;

        public void reset()
        {
            a1 = 0; b1 = 0; c1 = 0; d1 = 0; e1 = 0; f1 = 0; g1 = 0;
        }

        //窗体关闭时执行，窗体后台运行
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            this.Hide();
        }

       
      

     

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Start();
            textBox1.Text = "0";
            textBox2.Text = "0";
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要退出程序吗？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                notifyIcon1.Visible = false;
                this.Close();
                this.Dispose();
                Application.Exit();
            }

        }

        private void hideMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void showMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();

        }

        //桌面右小角图标
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//判断鼠标的按键
            {
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.WindowState = FormWindowState.Minimized;

                    this.Hide();
                }
                else if (this.WindowState == FormWindowState.Minimized)
                {
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    this.Activate();
                }
            }
        }

    

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 60000;//执行间隔时间,单位为毫秒;此时时间间隔为60秒   

            float a2 = operatePlc.readPlcDbdValues("10.228.142.106", 0, 3, 1, 18)/60, //磷化稀水流量
                b2 = operatePlc.readPlcDbdValues("10.228.142.106", 0, 3, 1, 14) /60,//磷化废液流量
                c2 = operatePlc.readPlcDbdValues("10.228.142.106", 0, 3, 1, 66) /60,//脱脂稀水流量
                d2 = operatePlc.readPlcDbdValues("10.228.142.106", 0, 3, 1, 46) /60,//脱脂浓水流量
                e2 = operatePlc.readPlcDbdValues("10.228.142.106", 0, 3, 1, 38) /60,//电泳稀水流量
                f2 = operatePlc.readPlcDbdValues("10.228.142.106", 0, 3, 1, 42) /60,//电泳浓水流量
                g2 = operatePlc.readPlcDbdValues("10.228.142.106", 0, 3, 1, 62) /60;//喷漆废水流量

            a1 = a1 + a2;
            b1 = b1 + b2;
            c1 = c1 + c2;
            d1 = d1 + d2;
            e1 = e1 + e2;
            f1 = f1 + f2;
            g1 = g1 + g2;
         
           

            label15.Text = Convert.ToString(a1);
            label16.Text = Convert.ToString(b1);
            label17.Text = Convert.ToString(c1);
            label18.Text = Convert.ToString(d1);
            label19.Text = Convert.ToString(e1);
            label20.Text = Convert.ToString(f1);
            label21.Text = Convert.ToString(g1);
        }

        //获取累积流量
        public void waterFlowInfoToDatabasee()
        {
            string riqi = DateTime.Now.ToString("yyyy-MM-dd");
            string shijian = DateTime.Now.ToLongTimeString().ToString();
            string str_sqlstr = "insert into WASTEWATERFLOWINFO values('" + riqi + "','" + shijian + "','" + a1 + "','" + b1 + "','" + c1 + "','" + d1 + "','" + e1 + "','" + f1 + "','" + g1 + "') ";
            operateDatabase.OrcGetCom(str_sqlstr);

        }

        //累积流量数据插入数据库 每天的额23：58：00
        public void insert_data_to_database()
        {
            if (DateTime.Now.Hour == Convert.ToInt32(23) && DateTime.Now.Minute == Convert.ToInt32(58) && DateTime.Now.Second == Convert.ToInt32(00))
            {
              waterFlowInfoToDatabasee();
                reset();
            }
        }
        //每1s执行一次数据插入程序
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Interval = 1000;
            insert_data_to_database();

           if(operatePlc.getPlcDbxVaules("10.228.142.172", 0, 0, 32, 0, 3))
            {
                textBox1.Text = "1";
            }
            else
            {
                textBox1.Text = "0";
            }          
            if (operatePlc.getPlcDbxVaules("10.228.142.173", 0, 0, 32, 0, 3))
            {
                textBox2.Text = "1";
            }
            else
            {
                textBox2.Text = "0";
            }
            if (operatePlc.getPlcDbxVaules("10.228.142.172", 0, 0, 32, 0, 7))
            {
                textBox3.Text = "1";
            }
            else
            {
                textBox3.Text = "0";
            }
            if (operatePlc.getPlcDbxVaules("10.228.142.173", 0, 0, 32, 0, 7))
            {
                textBox4.Text = "1";
            }
            else
            {
                textBox4.Text = "0";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string riqi = DateTime.Now.ToString("yyyy-MM-dd");
            string shijian = DateTime.Now.ToLongTimeString().ToString();
            string status = "磷化压滤机可以开板了";
            string str_sqlstr = "insert into WASTEWATERFILTERINFO values('" + riqi + "','" + shijian + "','" +textBox1.Text+ "','"+status+"')";
            operateDatabase.OrcGetCom(str_sqlstr);
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string riqi = DateTime.Now.ToString("yyyy-MM-dd");
            string shijian = DateTime.Now.ToLongTimeString().ToString();
            string status = "磷化压滤机可以开始工作了";
            string str_sqlstr = "insert into WASTEWATERFILTERINFO values('" + riqi + "','" + shijian + "','" + textBox3.Text + "','" + status + "')";
            operateDatabase.OrcGetCom(str_sqlstr);
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string riqi = DateTime.Now.ToString("yyyy-MM-dd");
            string shijian = DateTime.Now.ToLongTimeString().ToString();
            string status = "综合压滤机可以开板了";
            string str_sqlstr = "insert into WASTEWATERFILTERINFO values('" + riqi + "','" + shijian + "','" +textBox2.Text + "','" + status + "') ";
            operateDatabase.OrcGetCom(str_sqlstr);
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string riqi = DateTime.Now.ToString("yyyy-MM-dd");
            string shijian = DateTime.Now.ToLongTimeString().ToString();
            string status = "综合压滤机可以开始工作了";
            string str_sqlstr = "insert into WASTEWATERFILTERINFO values('" + riqi + "','" + shijian + "','" + textBox4.Text + "','" + status + "') ";
            operateDatabase.OrcGetCom(str_sqlstr);
        }
    }
}
