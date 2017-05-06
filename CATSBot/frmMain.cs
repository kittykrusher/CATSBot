﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace C.A.T.S.Auto_Fighter
{
    public partial class frmMain : Form
    {
        Thread thread;

        public frmMain()
        {
            InitializeComponent();
            BotHelper.main = this;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(btnStart.Text == "Start")
            {
                // Check if the MEmu process is running
                Process[] pname = Process.GetProcessesByName("MEmu");
                if (pname.Length == 0)
                {
                    MessageBox.Show("MEmu is not running!");
                    return;
                }

                BotHelper.memu = Process.GetProcessesByName("MEmu").First().MainWindowHandle;

                btnStart.Text = "Stop";

                // Start the Bot thread
                Thread.Sleep(100);
                thread = new Thread(doLoop);
                thread.IsBackground = true;
                thread.Start();
            }
            else
            {
                if (thread.IsAlive)
                    thread.Suspend(); // TODO: Proper Multithreading

                btnStart.Text = "Start";
            }
        }

        public void doLoop()
        {
            BotHelper.Log("(Re-)Starting main loop.");
            BotLogics.AttackLogic.doLogic();

            doLoop();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(thread != null && thread.IsAlive)
                thread.Suspend(); // TODO: Proper Multithreading

            Application.Exit();
        }
    }
}
