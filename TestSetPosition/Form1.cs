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
using WZYB.Model;
using WZYB.BLL;
using System.Collections;


namespace TestSetPosition
{
    public partial class Form1 : Form
    {
        #region 数据定义

        #region 公用

        //iPhysics
        RemoteInterface remote;
        int handle = -1;
        ExternalAppDock iPhysicsDoc;
        bool loading_done;

        /// <summary>
        /// 是否启动
        /// </summary>
        private bool isStart = false;

        #endregion

        #region ocs
        /// <summary>
        /// 悬挂小车数量
        /// </summary>
        private int ocsCarCount = 46;
        /// <summary>
        /// 悬挂小车速度
        /// </summary>
        private float ocsSpeed = 0.05f;
        /// <summary>
        /// 悬挂小车起始位置
        /// </summary>
        private float ocsStartPos = 0.01f;
        /// <summary>
        /// 悬挂小车车身宽度
        /// </summary>
        private float ocsCarWidth = 1.1f;
        /// <summary>
        /// 悬挂小车线程
        /// </summary>
        private Thread[] ocsThread;
        /// <summary>
        /// 悬挂小车位置
        /// </summary>
        private float[] ocsCarPos;
        /// <summary>
        /// 悬挂小车缓存数据
        /// </summary>
        private List<OCSStatus> ocsModelList = new List<OCSStatus>();
        /// <summary>
        /// 悬挂小车线程刷新时间
        /// </summary>
        private int ocsThreadTime = 300;
        /// <summary>
        /// 悬挂线体长度
        /// </summary>
        private float[] ocsLineLength = { 0,   12.894f,    11.925f,    11.925f,    11.925f,    11.94f,     1.16f,      1.15f,      1.14f,      1.171f,     11.93f,
                                            1.142f,     11.93f,    1.15f,      11.93f,    1.154f,     11.93f,    1.151f,     11.93f,    1.144f,     11.93f,
                                            1.153f,     11.93f,    1.151f,     11.93f,    1.151f,     11.93f,    1.152f,     11.93f,    1.152f,     11.93f,
                                            1.143f,     11.93f,    1.152f,     11.93f,    1.578f,     11.744f,    1.145f,     1.151f,     1.145f,     1.151f,
                                            1.151f,     1.152f,     1.151f,     1.153f,     1.144f,     1.151f,     1.153f,     1.151f,     1.142f,     1.153f,
                                            1.145f,     1.148f,     1.154f,     0.785f};

        /// <summary>
        /// 悬挂小车路线
        /// </summary>
        private string[] ocsPath = { "",
                                "2,54,1",
                                "2,54,1",
                                "3,53,54,1,6",
                                "3,53,54,1,6",
                                "4,52,53,54,1,6",
                                "4,52,53,54,1,6",
                                "5,51,52,53,54,1,6,7",
                                "5,51,52,53,54,1,6,7",
                                "10,50,51,52,53,54,1,6,7,8,9",
                                "10,50,51,52,53,54,1,6,7,8,9",
                                "12,49,50,51,52,53,54,1,6,7,8,9,11",
                                "12,49,50,51,52,53,54,1,6,7,8,9,11",
                                "14,48,49,50,51,52,53,54,1,6,7,8,9,11,13",
                                "14,48,49,50,51,52,53,54,1,6,7,8,9,11,13",
                                "16,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15",
                                "16,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15",
                                "18,46,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15,17",
                                "18,46,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15,17",
                                "20,45,46,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15,17,19",
                                "20,45,46,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15,17,19",
                                "22,44,45,46,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15,17,19,21",
                                "22,44,45,46,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15,17,19,21",
                                "24,43,44,45,46,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15,17,19,21,23",
                                "24,43,44,45,46,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15,17,19,21,23",
                                "26,42,43,44,45,46,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15,17,19,21,23,25",
                                "26,42,43,44,45,46,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15,17,19,21,23,25",
                                "28,41,42,43,44,45,46,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15,17,19,21,23,25,27",
                                "28,41,42,43,44,45,46,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15,17,19,21,23,25,27",
                                "30,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15,17,19,21,23,25,27,29",
                                "30,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15,17,19,21,23,25,27,29",
                                "32,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15,17,19,21,23,25,27,29,31",
                                "32,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15,17,19,21,23,25,27,29,31",
                                "34,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15,17,19,21,23,25,27,29,31,33",
                                "34,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15,17,19,21,23,25,27,29,31,33",
                                "36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15,17,19,21,23,25,27,29,31,33,35",
                                "36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,1,6,7,8,9,11,13,15,17,19,21,23,25,27,29,31,33,35",
                                "1,6,7,8,9,11,13,15,17,19,21,23,25,27,29,31,33,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54", 
                                "1,6,7,8,9,11,13,15,17,19,21,23,25,27,29,31,33,34,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54",
                                "1,6,7,8,9,11,13,15,17,19,21,23,25,27,29,31,32,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54",
                                "1,6,7,8,9,11,13,15,17,19,21,23,25,27,29,30,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54",
                                "1,6,7,8,9,11,13,15,17,19,21,23,25,27,28,41,42,43,44,45,46,47,48,49,50,51,52,53,54",
                                "1,6,7,8,9,11,13,15,17,19,21,23,25,26,42,43,44,45,46,47,48,49,50,51,52,53,54",
                                "1,6,7,8,9,11,13,15,17,19,21,23,24,43,44,45,46,47,48,49,50,51,52,53,54",
                                "1,6,7,8,9,11,13,15,17,19,21,22,44,45,46,47,48,49,50,51,52,53,54",
                                "1,6,7,8,9,11,13,15,17,19,20,45,46,47,48,49,50,51,52,53,54",
                                "1,6,7,8,9,11,13,15,17,18,46,47,48,49,50,51,52,53,54",
                                };

        /// <summary>
        /// 悬挂小车临时数据
        /// </summary>
        private int[] ocsTempLine;

        #endregion

        #region agv
        /// <summary>
        /// agv小车数量
        /// </summary>
        private int agvCarCount = 3;
        /// <summary>
        /// agv小车速度
        /// </summary>
        private float agvSpeed = 0.05f;
        /// <summary>
        /// agv小车起始位置
        /// </summary>
        private float agvStartPos = 0.01f;
        /// <summary>
        /// agv小车起始位置
        /// </summary>
        private float agvEndPos = 0.001f;
        /// <summary>
        /// agv小车车身宽度
        /// </summary>
        private float agvCarWidth = 1.1f;
        /// <summary>
        /// agv小车线程
        /// </summary>
        private Thread[] agvThread;
        /// <summary>
        /// agv小车位置
        /// </summary>
        private float[] agvCarPos;
        /// <summary>
        /// agv小车缓存数据
        /// </summary>
        private List<AGVStatus> agvModelList = new List<AGVStatus>();
        /// <summary>
        /// agv小车线程刷新时间
        /// </summary>
        private int agvThreadTime = 300;
        /// <summary>
        /// 悬挂线体长度
        /// </summary>
        private float[] agvLineLength = { 0,2.1f,0.97f,2.95f,6.1f,1.85f,1.5f,6.91f,4.1f,1.3f,4.09f,4.89f,7.73f,1.4f,4.02f};

        /// <summary>
        /// 悬挂小车路线
        /// </summary>
        private string[] agvPath = { "",
                                "1,2,3,4,5,6,7,8",
                                "11,4,5,6,7,9,10",
                                "12,4,5,6,7,9,13,14"
                                };

        /// <summary>
        /// 悬挂小车临时数据
        /// </summary>
        private int[] agvTempLine;
        private int[] agvdirection;
        private int[] agvStopTime;

        #endregion

        #endregion

        #region 初始化

        public Form1()
        {
            InitializeComponent();
            Initialization();
            loadDemo();
        }

        /// <summary>
        /// 初始化三维模型
        /// </summary>
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
            
        }

        /// <summary>
        /// 加载三维模型
        /// </summary>
        public void loadDemo()
        {
            IPhysics_Command command = new LoadDocument(Environment.CurrentDirectory + System.Configuration.ConfigurationManager.AppSettings["modefile"].ToString());
            remote.execute(command);
            //remote.switchModePresentation(true);
            loading_done = true;
        }

        /// <summary>
        /// 与三维模型建立链接
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 关闭三维模型连接
        /// </summary>
        /// <returns></returns>
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

        #endregion
                
        #region 窗体逻辑

        private void Form1_Load(object sender, EventArgs e)
        {
            //ocs数据初始化
            ocsCarPos = new float[ocsCarCount + 1];
            ocsThread = new Thread[ocsCarCount];
            ocsTempLine = new int[ocsCarCount + 1];

            int[] ocsTmpSequence = new int[1000];
            for (int i = 1; i <= 5; i++)
            //for (int i = 1; i <= ocsCarCount; i++)
            {
                ocsTempLine[i] = 0;
                OCSStatus model = OCSStatusBLL.GetModel(i);

                if (model != null)
                {
                    model.line = int.Parse(ocsPath[i].Split(',')[0]);
                    model.sequence = ocsTmpSequence[model.line] + 1;
                    ocsTmpSequence[model.line]++;
                    OCSStatusBLL.Update(model);
                }
                else
                {
                    model = new OCSStatus();
                    model.carId = i;
                    model.line = int.Parse(ocsPath[i].Split(',')[0]);
                    model.direction = 1;
                    model.sequence = ocsTmpSequence[model.line] + 1;
                    model.backLine = 1;
                    model.position = -1;
                    ocsTmpSequence[model.line]++;
                    OCSStatusBLL.Add(model);
                }
            }

            //agv数据初始化
            agvCarPos = new float[agvCarCount + 1];
            agvThread = new Thread[agvCarCount];
            agvTempLine = new int[agvCarCount + 1];
            agvdirection = new int[agvCarCount + 1];
            agvStopTime = new int[agvCarCount + 1];

            int[] agvTmpSequence = new int[10000];

            for (int i = 1; i <= agvCarCount; i++)
            {
                agvTempLine[i] = 0;
                agvdirection[i] = 0;
                agvStopTime[i] = 0;
                AGVStatus model = AGVStatusBLL.GetModel(i);

                if (model != null)
                {
                    model.line = int.Parse(agvPath[i].Split(',')[0]);
                    model.direction = 1;
                    model.sequence = agvTmpSequence[model.line] + 1;
                    agvTmpSequence[model.line]++;
                    AGVStatusBLL.Update(model);
                }
                else
                {
                    model = new AGVStatus();
                    model.carId = i;
                    model.line = int.Parse(agvPath[i].Split(',')[0]);
                    model.direction = 1;
                    model.sequence = agvTmpSequence[model.line] + 1;
                    model.backLine = 1;
                    model.position = -1;
                    agvTmpSequence[model.line]++;
                    AGVStatusBLL.Add(model);
                }
            }
        }

        /// <summary>
        /// 模拟生成数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
           
            //ocs数据模拟
            for (int i = 1; i < ocsCarPos.Length; i++)
            {
                OCSStatus model = OCSStatusBLL.GetModel(i);

                if (ocsCarPos[i] > 0)
                {
                    if (ocsCarPos[i] >= ocsLineLength[int.Parse(ocsPath[i].Split(',')[ocsTempLine[i]])])
                    {
                        if (ocsTempLine[i] + 1 >= ocsPath[i].Split(',').Length)
                            ocsTempLine[i] = 0;
                        else
                            ocsTempLine[i]++;
                        model.line = int.Parse(ocsPath[i].Split(',')[ocsTempLine[i]]);
                        int count = OCSStatusBLL.getCountByLine(model.line);
                        model.sequence = count + 1;
                        OCSStatusBLL.Update(model);
                    }
                }
            }

            //agv数据模拟
            for (int i = 1; i < agvCarPos.Length; i++)
            {
                AGVStatus model = AGVStatusBLL.GetModel(i);

                if (agvCarPos[i] > 0)
                {
                    if (agvCarPos[i] >= agvLineLength[int.Parse(agvPath[i].Split(',')[agvTempLine[i]])] || agvCarPos[i] == agvEndPos)
                    {
                        if (agvdirection[i] == 0)
                        {
                            if (agvTempLine[i] + 1 < agvPath[i].Split(',').Length)
                            {
                                agvTempLine[i]++;
                            }
                            else
                            {
                                if (agvStopTime[i] > 3000)
                                {
                                    if (agvdirection[i] == 0)
                                        agvdirection[i] = 1;
                                    else
                                        agvdirection[i] = 0;

                                    agvStopTime[i] += 0;
                                }
                                else
                                {
                                    agvStopTime[i] += 500;
                                }
                            }
                        }
                        else
                        {
                            if (agvTempLine[i] - 1 > 0)
                            {
                                agvTempLine[i]--;
                            }
                            else
                            {
                                if (agvStopTime[i] > 3000)
                                {
                                    if (agvdirection[i] == 0)
                                        agvdirection[i] = 1;
                                    else
                                        agvdirection[i] = 0;

                                    agvStopTime[i] += 0;
                                }
                                else
                                {
                                    agvStopTime[i] += 500;
                                }
                            }
                        }

                        model.line = int.Parse(agvPath[i].Split(',')[agvTempLine[i]]);
                        int count = AGVStatusBLL.getCountByLine(model.line);

                        if (model.line == 6 || model.line == 8 || model.line == 10 || model.line == 14)
                        {
                            if (agvdirection[i] == 0)
                                model.direction = 2;
                            else
                                model.direction = 1;
                        }
                        else
                        {
                            if (agvdirection[i] == 0)
                                model.direction = 1;
                            else
                                model.direction = 2;
                        }

                        model.sequence = count + 1;
                        AGVStatusBLL.Update(model);
                    }
                }
            }
        }

        /// <summary>
        /// 定时刷新ip update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer3_Tick(object sender, EventArgs e)
        {
            double time, timeStep;
            ComTCPLib.UpdateData(handle, out time, out timeStep);
        }

        #endregion

        #region 界面按钮处理
        private void button1_Click(object sender, EventArgs e)
        {
            remote.sendReset();
            remote.sendPlay();
            Connect();

            Thread.Sleep(1000);
            isStart = true;

            timer2.Enabled = true;
            timer3.Enabled = true;

            //启动悬挂小车线程
            for (int i = 0; i < 5; i++)
            //for (int i = 0; i < ocsCarCount; i++)
            {
                ocsThread[i] = new Thread(new ParameterizedThreadStart(OCSThreadFunc));
                ocsThread[i].Start(i + 1);
            }

            //启动AGV小车线程
            for (int i = 0; i < agvCarCount; i++)
            {
                agvThread[i] = new Thread(new ParameterizedThreadStart(AGVThreadFunc));
                agvThread[i].Start(i + 1);
            }

            //PCC-----
            setBaseData();
            timer4.Enabled = true;
            timer5.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            remote.sendReset();
            remote.sendReset();
            Disconnect();

            isStart = false;
            for (int i = 0; i < 2; i++)
            {
                ocsThread[i].Abort();
            }

            timer2.Enabled = false;
            timer3.Enabled = false;
            timer4.Enabled = false;
            timer5.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            remote.sendPause();
            isStart = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            remote.sendPlay();
            isStart = true;
        }

        #endregion

        #region 悬挂小车处理逻辑

        /// <summary>
        /// 小车线程处理
        /// </summary>
        /// <param name="o"></param>
        private void OCSThreadFunc(object o)
        {
            int index = Convert.ToInt32(o);

            while (true)
            {
                if (isStart)
                {
                    //数据库最新数据
                    OCSStatus model = OCSStatusBLL.GetModel(index);

                    if (model.position == -1)
                    {
                        //内存数据
                        //OCSStatus oldModel = ocsModelList.Find(s => s.carId == index);
                        int i = ocsModelList.FindIndex(s => s.carId == index);
                        
                        //初始
                        if (i == -1)
                        {
                            int count = OCSStatusBLL.getCountByLine(model.line);

                            ocsCarPos[index] = (count - model.sequence) * ocsCarWidth + ocsStartPos;
                            ocsModelList.Add(model);
                        }
                        else
                        {
                            //驱动段改变
                            if (ocsModelList[i].line != model.line)
                            {
                                if (model.direction == 1)
                                    ocsCarPos[index] = ocsStartPos;
                                else
                                    ocsCarPos[index] = ocsLineLength[model.line] - ocsStartPos;
                            }
                            else
                            {
                                if (model.direction == 1)
                                    ocsCarPos[index] += ocsSpeed;
                                else if (model.direction == 2)
                                    ocsCarPos[index] -= ocsSpeed;
                            }

                            ocsModelList[i] = model;
                        }
                    }
                    else
                    {
                        ocsCarPos[index] = float.Parse(model.position.ToString());
                    }
                      
                    int index1 = GetIdex.getDicOutputIndex("ocsCar" + index + "01_input_pos");
                    ComTCPLib.SetOutputAsREAL32(handle, index1, float.Parse(ocsCarPos[index].ToString()));
                    index1 = GetIdex.getDicOutputIndex("ocsCar" + index + "01_input_Path");
                    ComTCPLib.SetOutputAsINT(handle, index1, int.Parse(model.line.ToString()));
                }

                Thread.Sleep(ocsThreadTime);
            }
        }

        #endregion

        #region AGV小车处理逻辑

        private void AGVThreadFunc(object obj)
        {
            int index = Convert.ToInt32(obj);
            bool tmp = false;

            while (true)
            {
                #region 临时使用，正式使用时去掉

                if (tmp == false)
                {
                    if (index == 2)
                    {
                        if (agvTempLine[3] > 0)
                            tmp = true;
                    }
                    else if (index == 1)
                    {
                        if (agvTempLine[2] > 0)
                            tmp = true;
                    }
                    else
                        tmp = true;
                }

                #endregion
                
                if (isStart && tmp)
                {
                    //数据库最新数据
                    AGVStatus model = AGVStatusBLL.GetModel(index);

                    if (model.position == -1)
                    {
                        //内存数据
                        //OCSStatus oldModel = ocsModelList.Find(s => s.carId == index);
                        int i = agvModelList.FindIndex(s => s.carId == index);

                        //初始
                        if (i == -1)
                        {
                            int count = AGVStatusBLL.getCountByLine(model.line);

                            agvCarPos[index] = (count - model.sequence) * agvCarWidth + agvStartPos;
                            agvModelList.Add(model);
                        }
                        else
                        {
                            //驱动段改变
                            if (agvModelList[i].line != model.line)
                            {
                                if(model.direction == 1)
                                    agvCarPos[index] = agvStartPos;
                                else
                                    agvCarPos[index] = agvLineLength[model.line] - agvStartPos;
                            }
                            else
                            {
                                if (model.direction == 1)
                                {
                                    if (agvCarPos[index] < agvLineLength[model.line])
                                        agvCarPos[index] += agvSpeed;
                                }
                                else if (model.direction == 2)
                                {
                                    if (agvCarPos[index] - agvSpeed < agvEndPos)
                                        agvCarPos[index] = agvEndPos;
                                    else
                                        agvCarPos[index] -= agvSpeed;
                                }
                            }

                            agvModelList[i] = model;
                        }
                    }
                    else
                    {
                        agvCarPos[index] = float.Parse(model.position.ToString());
                    }

                    int index1 = GetIdex.getDicOutputIndex("agvCar" + index + "01_input_pos");
                    ComTCPLib.SetOutputAsREAL32(handle, index1, float.Parse(agvCarPos[index].ToString()));
                    index1 = GetIdex.getDicOutputIndex("agvCar" + index + "01_input_Path");
                    ComTCPLib.SetOutputAsINT(handle, index1, int.Parse(model.line.ToString()));
                }

                Thread.Sleep(ocsThreadTime);
            }
        }

        #endregion


        #region 配餐车逻辑
        //-------------------------------------------------------------------------------
        //从新设置基础数据
        DBtest dd = new DBtest();
        public ControlPcc pcccontrol2 = new ControlPcc();
        public void setBaseData()
        {
            dd = new DBtest();
            this.timer5.Interval = int.Parse(System.Configuration.ConfigurationManager.AppSettings["car_interval"].ToString());

            pcccontrol2.setBaseData(this.handle);
        }

        //设置模型数据
        public void setModelData()
        {
            try
            {
                if (pcccontrol2.flag == false)
                {
                    return;
                }
                if (pcccontrol2.flag == true)
                {
                    pcccontrol2.flag = false;
                }
                pcccontrol2.setModelData(this.handle);
                pcccontrol2.flag = true;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            //PCC模拟数据
            dd.testdata2();
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            //PCC刷新数据
            setModelData();
        }


        #endregion

    }
}
