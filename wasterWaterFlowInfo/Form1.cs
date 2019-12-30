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


        //窗体加载时，执行程序
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Start();
            textBox1.Text = "0";
            textBox2.Text = "0";
        }


        //窗体关闭时执行，窗体后台运行
        //需要在form1属性中修改showintaskbar为false
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            this.Hide();
        }

        //托盘程序，窗体退出
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

        //托盘程序，窗体隐藏
        private void hideMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        //托盘程序，窗体弹出
        private void showMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();

        }

        //托盘程序，图标功能
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


        //每1s读取流量信息
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 1000;//执行间隔时间,单位为毫秒;此时时间间隔为60秒   
                
       
            label15.Text = Convert.ToString(operatePlc.readPlcDbdValues("10.228.142.106", 0, 3, 200, 78));//磷化稀水流量
            label16.Text = Convert.ToString(operatePlc.readPlcDbdValues("10.228.142.106", 0, 3, 200, 60));//磷化废液流量
            label17.Text = Convert.ToString(operatePlc.readPlcDbdValues("10.228.142.106", 0, 3, 200, 294));//脱脂稀水流量
            label18.Text = Convert.ToString(operatePlc.readPlcDbdValues("10.228.142.106", 0, 3, 200, 204));//脱脂浓水流量
            label19.Text = Convert.ToString(operatePlc.readPlcDbdValues("10.228.142.106", 0, 3, 200, 168));//电泳稀水流量
            label20.Text = Convert.ToString(operatePlc.readPlcDbdValues("10.228.142.106", 0, 3, 200, 186));//电泳浓水流量
            label21.Text = Convert.ToString(operatePlc.readPlcDbdValues("10.228.142.106", 0, 3, 200, 276));//喷漆废水流量

           
        }
       
   

        //累积流量数据插入数据库 每天的额23：58：00 同时将每天的累积流量清0
        public void insert_data_to_database()
        {
            if (DateTime.Now.Hour == Convert.ToInt32(23) && DateTime.Now.Minute == Convert.ToInt32(58) && DateTime.Now.Second == Convert.ToInt32(00))
            {
                string riqi = DateTime.Now.ToString("yyyy-MM-dd");
                string shijian = DateTime.Now.ToLongTimeString().ToString();
                string str_sqlstr = "insert into WASTEWATERFLOWINFO values('" + riqi + "','" + shijian + "','" + label15.Text + "','" + label16.Text + "','" + label17.Text + "','" + label18.Text + "','" + label19.Text + "','" + label20.Text + "','" + label21.Text + "') ";
                operateDatabase.OrcGetCom(str_sqlstr);

                operatePlc.writePlcDbdValues("10.228.142.106", 0, 3, 200, 78, 0);
                operatePlc.writePlcDbdValues("10.228.142.106", 0, 3, 200, 60, 0);
                operatePlc.writePlcDbdValues("10.228.142.106", 0, 3, 200, 294, 0);
                operatePlc.writePlcDbdValues("10.228.142.106", 0, 3, 200, 204, 0);
                operatePlc.writePlcDbdValues("10.228.142.106", 0, 3, 200, 168, 0);
                operatePlc.writePlcDbdValues("10.228.142.106", 0, 3, 200, 186, 0);
                operatePlc.writePlcDbdValues("10.228.142.106", 0, 3, 200, 276, 0);

            }
        }


        //记录压滤机工作状态
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
