using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using IPhysics;
using Microsoft.Win32;
using System.Threading;
using System.Xml;

namespace TestSource
{
    public partial class Form1 : Form
    {
        RemoteInterface remote;
        int handle = -1;
        ExternalAppDock iPhysicsDoc;
        bool loading_done;

        //初始化
        public void Initialization()
        {
            remote = new RemoteInterface(true, true);

            iPhysicsDoc = new ExternalAppDock();
            this.panel1.Controls.Add(iPhysicsDoc);
            iPhysicsDoc.Dock = DockStyle.Fill;
            if (!remote.IsStarted)
                remote.StartIPhysics(6000);
            int timeout = 30000;
            int sleepTime = 500;
            while (!remote.IsConnected && timeout > 0)
            {
                System.Threading.Thread.Sleep(sleepTime);
                timeout -= sleepTime;

                if (!remote.IsConnected)
                    remote.Connect("localhost", 6000);
            }

            iPhysicsDoc.DockExternalApp(remote.ExeProcess, iPhysicsDoc.Parent);
            if (!(timeout > 0))
                throw new Exception("Error, cannot connect to industrialPhysics");
            //Default File Load
            RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default);
            String Path = key.GetValue(
                "Software\\machineering\\industrialPhysics\\InstallDir(x64)"
                , "c:\\Program Files\\machineering\\industrialPhysics(x64)").ToString();
            //IPhysics_Command command = new LoadDocument(Environment.CurrentDirectory + "/light.iphz");
            //remote.execute(command);
            //remote.lookFromIsometry();
            //remote.switchModePresentation(true);
            //remote.zoomOut(0);
            //remote.sendPlay();
            //loading_done = true;
            //remote.sendReset();
            //remote.sendPlay();
        }

        public void loadDemo()
        {
            IPhysics_Command command = new LoadDocument(Environment.CurrentDirectory + System.Configuration.ConfigurationManager.AppSettings["modefile"].ToString());
            remote.execute(command);
           
            loading_done = true;
        }

        //建立链接
        private int Connect()
        {
            if (handle >= 0)
                return -10;

            ComTCPLib.Init();

            // Create a new comtcp node
            int localhandle = ComTCPLib.CreateNode("Hello_industrialPhysics");
            if (localhandle < 0)
                return -1;

            // open the config file
            string filepath = Environment.CurrentDirectory + "/CodeGen.xml";
            //string filepath = Environment.CurrentDirectory + @"\CodeGen.xml";
            if (ComTCPLib.Result.Failed == ComTCPLib.LoadConfig(localhandle, filepath))
                return -2;

            // Start the internal thread system
            if (ComTCPLib.Result.Failed == ComTCPLib.Start(localhandle))
                return -3;

            // Connect to running industrialPhysics as specified in the file
            if (ComTCPLib.Result.Failed == ComTCPLib.Connect(localhandle))
                return -4;

            int numOutputs = ComTCPLib.GetNumOutputs(localhandle);
            //float[] outputValues = new float[numOutputs];

            //if (6 != numOutputs)
            //{
            //    return Disconnect();
            //}

            handle = localhandle;

            return 0;
        }

        //关闭连接
        private int Disconnect()
        {
            if (handle < 0)
                return -1;

            // Disconnect
            if (ComTCPLib.Result.Failed == ComTCPLib.Disconnect(handle))
                return -1000;

            // Stop the threading
            if (ComTCPLib.Result.Failed == ComTCPLib.Stop(handle))
                return -1001;

            // delete the node
            if (ComTCPLib.Result.Failed == ComTCPLib.DeleteNode(handle))
                return -1002;

            return 0;
        }
        public Form1()
        {
            InitializeComponent();
            Initialization();
            loadDemo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            remote.sendReset();
            remote.sendPlay();
            Connect();
           // setSource();
        }

        //20170104 wge -------------------------------------------------
        public void setSource()
        {
            
            string str = "";
            updateValue("Source01_IN_ISSHOW", "1", 1, 1);
            for (int i = 0; i < 2000;i++ )
            {
                if (!updateValue("Source01_IN_LOCATION", (i).ToString(), 1, 1))
                {
                   str += ",";
                   str += i.ToString();
                   MessageBox.Show(str);
                }
                
                Thread.Sleep(400);
            }

             
            
            
        }

      

        int i = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            i++;
            updateValue("Source01_IN_ISSHOW", "1", 1, 1);
            updateValue("Source01_IN_LOCATION", (i + 1).ToString(), 1, 1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            setSource();
        }
        

        public bool updateValue(string name, string value, int type, int handle)
        {
            double time, timeStep;
            ComTCPLib.UpdateData(handle, out time, out timeStep);
            int index = GetIdex.getDicOutputIndex(name);

            ComTCPLib.Result r = ComTCPLib.Result.Failed;
            if (type == 1)
            {
                r = ComTCPLib.SetOutputAsUINT(handle, index, uint.Parse(value));
            }
            else if (type == 2)
            {
                r = ComTCPLib.SetOutputAsREAL32(handle, index, float.Parse(value));
            }
            else if (type == 3)
            {
                r = ComTCPLib.SetOutputAsBOOL(handle, index, bool.Parse(value));
            }
            if(r == ComTCPLib.Result.Success)
            {
                return true;
            }
            return false;
        }

    }
}
