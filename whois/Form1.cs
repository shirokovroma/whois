using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using System.Threading;

namespace whois
{
    
    public partial class Whois : Form
    {
        bool IsInt(string val)
        {
            bool result = false;
            try
            {
                int.Parse(val);
                result = true;
            }
            catch { }
            return result;
        }
        public bool IsIP(string val)
        {
            int i=0;
            char[] chSplit = { '.' };
            string[] arra = val.Split(chSplit);
            foreach (string item in arra)
            {
                if (IsInt(item))
                {
                    if ((int.Parse(item) > 0) && ((int.Parse(item) < 256)))
                    {
                        i++;
                    }
                }
            }
            return i==4;
        }
        
    public  void doQuery(Object sender, EventArgs e)
    {
        String strResponse, strServer;
        String strDomain = txtDomain.Text;
        char[] chSplit = { '.' };
        if (IsIP(strDomain))
        {
            strServer = "whois.ripe.net";
           
            if (DoWhoisLookup(strDomain, strServer, out strResponse))
            {
                txtResult.Text = strResponse;
            }
            else
            {
                txtResult.Text = "Lookup failed";
            }
        
            return;
        }
        string[] arrDomain = strDomain.Split(chSplit);
        if (arrDomain.Length != 2)
        {
            txtResult.Text = "Lookup failed";
            return;
        }
        
        int nLength = arrDomain[1].Length;
        if (nLength != 2 && nLength != 3)
        {
            txtResult.Text = "Lookup failed";
            return; 
        }

        Hashtable table = new Hashtable();
        table.Add("at", "whois.nic.at");
        table.Add("de", "whois.denic.de");
        table.Add("be", "whois.dns.be");
        table.Add("gov", "whois.nic.gov");
        table.Add("mil", "whois.nic.mil");
        table.Add("com", "whois.verisign-grs.com");

        strServer = "whois.internic.net";
        if (table.ContainsKey(arrDomain[1]))
        {
            strServer = table[arrDomain[1]].ToString();
        }
        else if (nLength == 2)
        {
            strServer = "whois.ripe.net";
        }
  
        if (DoWhoisLookup(strDomain, strServer, out strResponse))
        {
            txtResult.Text = strResponse;
        }
        else
        {
            txtResult.Text = "Lookup failed";
        }
    }

    public bool DoWhoisLookup(String strDomain, String strServer, out String strResponse)
    {
        strResponse = "none";
        bool bSuccess = false;

        TcpClient tcpc = new TcpClient();
        try
        {
            tcpc.Connect(strServer, 43);
        }
        catch (SocketException ex)
        {
            strResponse = "Could not connect to Whois server";
            strResponse += ex;
            return false;
        }

        strDomain += "\r\n";
        Byte[] arrDomain = Encoding.ASCII.GetBytes(strDomain.ToCharArray());
        try
        {
            Stream s = tcpc.GetStream();
            s.Write(arrDomain, 0, strDomain.Length);

            StreamReader sr = new StreamReader(tcpc.GetStream(), Encoding.ASCII);
            StringBuilder strBuilder = new StringBuilder();
            string strLine = null;

            while (null != (strLine = sr.ReadLine()))
            {
                strBuilder.Append(strLine + "\n");
            }
            tcpc.Close();

            bSuccess = true;
            strResponse = strBuilder.ToString();
        }
        catch (Exception e)
        {
            strResponse = e.ToString();
        }

        return bSuccess;
        }

        public Whois()
        {
            InitializeComponent();
            tabPage1.Text = "Whois";
            tabPage2.Text = "Браузер";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            statusStrip1.Text = "Lookuping...";
            doQuery(sender,e);
            statusStrip1.Text = "Browsing...";
            webBrowser1.Navigate(txtDomain.Text.ToString());
            statusStrip1.Text = "Ready";  

        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        } 

        private void конфигурацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtResult.Font = fontDialog1.Font;
            }
            
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Whois.ActiveForm.Close();
        }

      
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.DefaultExt = "*.txt";
            sf.Filter = "Text files|*.txt";
            if(sf.ShowDialog() == System.Windows.Forms.DialogResult.OK && sf.FileName.Length > 0) 
            {
                txtResult.SaveFile(sf.FileName, RichTextBoxStreamType.PlainText);
            }
        }
      
    }
}
