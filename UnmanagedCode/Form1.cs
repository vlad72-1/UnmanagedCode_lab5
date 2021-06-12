using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace UnmanagedCode
{

    public partial class Form1 : Form
    {

        // Прототип функции "FuncSin".
        [DllImport ("Project1.dll", CharSet = CharSet.Ansi, EntryPoint = "calcSin", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr FuncSin ();

        [StructLayout (LayoutKind.Sequential)]
        public struct TSin
        {
            public DateTime dateTime { get; set; }
            public double value { get; set; }
        }

        public class XANDY
        {
            public int i { get; set; }
            public int x { get; set; }
            public double y { get; set; }
            public XANDY (int xx, double yy, int ii)
            {
                i = ii;
                x = xx;
                y = yy;
            }
        }

        // Конструктор по умолчанию.
        public Form1 ()
        {
            InitializeComponent ();
            chart1.Series [0].Name = "Sin (fromDLL)";
            chart1.Series [0].ChartType = SeriesChartType.Spline;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowHeadersVisible = false;
        }

        private void button1_Click (object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            var pointList = new List<XANDY>(); 

            for (int i = 0; i < 11; i++)
            {
                TSin tsin1 = (TSin) Marshal.PtrToStructure (FuncSin (), typeof (TSin));
                pointList.Add (new XANDY (tsin1.dateTime.Second, tsin1.value, i + 1));
                chart1.Series[0].Points.AddXY (tsin1.dateTime.Second, tsin1.value);
                Thread.Sleep (1100);
            }

            dataGridView1.DataSource = pointList; 
        }
    }
}
