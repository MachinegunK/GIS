using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MapInfo;
using System.Runtime.InteropServices;

namespace CBSBitirme
{
    public partial class Form1 : Form
    {//global variables here
        public static MapInfo.MapInfoApplication mi;
        public static string win_id;
        public Form1()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);


        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            mi = new MapInfo.MapInfoApplication();
            mi = Activator.CreateInstance(Type.GetTypeFromProgID("Mapinfo.Application")) as MapInfoApplication;
            int p = panel2.Handle.ToInt32();
            

            mi.Do("set next document parent " + p.ToString() + "style 1");
            mi.Do("set application window " + p.ToString());
            mi.Do("run application \"" + "D:/GIS3Workspace/deneme.wor" + "\"");
            win_id = mi.Eval("frontwindow()");

        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            if (mi != null)
            {
                // The form has been resized. 
                if (mi.Eval("WindowID(0)") != "")
                {
                    // Update the map to match the current size of the panel. 
                    MoveWindow((System.IntPtr)long.Parse(mi.Eval("WindowInfo(FrontWindow(),12)")), 0, 0, this.panel2.Width, this.panel2.Height, false);
                    
                }

            }

        }
            private void button1_Click(object sender, EventArgs e)
        {
            removetematik();
            string p = panel2.Handle.ToString();
            string p4 = panel4.Handle.ToString();
            int n = Convert.ToInt16(textBox1.Text);
            string thematic_column = string.Empty;
            string arac_yili = Convert.ToString(comboBox1.SelectedItem);
            string arac_tipi = Convert.ToString(comboBox2.SelectedItem);
            
            string sorgu1 = arac_tipi + arac_yili;

           

            try
            {
                mi.Do("Add Column \"Iller\" (" + sorgu1 + " Integer)From data1 Set To " + sorgu1 + " Where COL2 = COL1  Dynamic");
            }
            catch
            {
            }
            thematic_column = sorgu1;
            mi.Do("select " + sorgu1 + " from Iller order by " + sorgu1 + " into sel");



            //tematik oluşturma
            int range = Convert.ToInt16(mi.Eval("int(tableinfo(sel,8)/" + Convert.ToString(n) + ")"));
            int c_range = Convert.ToInt16(255 / n);
            //----------part 2 -----
            mi.Do("fetch first from sel");
            string r1 = Convert.ToString(Form1.mi.Eval("sel.col1"));
            string r2 = string.Empty;
            string cmstr = string.Empty;

            for (int i = 1; i < n; i++)
            {
                mi.Do("fetch rec " + Convert.ToString(i * range) + " from sel");
                r2 = Convert.ToString(mi.Eval("sel.col2"));
                string rgb = Convert.ToString(mi.Eval("RGB(255," + Convert.ToString((n - i) * c_range) + "," + Convert.ToString((n - i) * c_range) + ")"));
                cmstr = cmstr + r1 + ":" + r2 + " brush(2," + rgb + ",16777215), ";
                r1 = r2;
            }
            mi.Do("fetch last from sel");
            r2 = Convert.ToString(mi.Eval("sel.col1"));
            cmstr = cmstr + r1 + ":" + r2 + " brush(2,16711680,16777215)";
            // ----------part 3 -----
            mi.Do("shade window " +Form1.win_id + " Iller with " + thematic_column + " ranges apply all use color Brush (2,16711680,16777215) " + cmstr);
            mi.Do("Set Next Document Parent " + p4 + " Style 1");
            mi.Do("Create Cartographic Legend From Window " + win_id + " Behind Frame From Layer 1");
            Form1.mi.Do("select * from iller where plaka_no =\"82\" into sel");
            Form1.mi.Do("fetch last from sel");
            r2 = Convert.ToString(Form1.mi.Eval("sel.col1"));
            cmstr = cmstr + r1 + ":" + r2 + " brush(2,255,16777215)";
            //kod sonu


            




        }


        //TEMATİK SİLME

        public void removetematik()
        {
            for (int k = Convert.ToInt16(Form1.mi.Eval("mapperinfo(" + Form1.win_id + ",9)")); k > 0; k = k - 1)
            {
                if (Convert.ToInt16(Form1.mi.Eval("layerinfo(" + Form1.win_id + "," + Convert.ToString(k) + ",24)")) == 3)
                {
                    Form1.mi.Do("remove map layer \"" + Form1.mi.Eval("layerinfo(" + Form1.win_id + "," + Convert.ToString(k) + ",1)") + "\"");
                }
            }

        }

   






       

        private void button3_Click(object sender, EventArgs e)
        {
            removetematik();
        }


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
