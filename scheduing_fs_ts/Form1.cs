﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace scheduing_fs_ts
{//trzeba zapisać instancje do pliku , potem nawet wczytać!
//w zapisie bug , poprawiony dla taskow poprawic dla pauz
    public partial class Form1 : Form
    {
        //int instance_nO = 0;
        int N = 30;
        Random rnd = new Random();
        int time_mach1 = 0;
        int time_mach2 = 0;
        public struct task
        {
                public int maszyna_op1;
                public int duration_op1;
                public int duration_op2;
                public int maszyna_op2;
                public int start;
                //public int pause;
        };
        //private List<task> tasks = new List<task>();
        private List<List<task>> task_instances = new List<List<task>>();

        struct pause
        {
            public int p_id;
            public int p_start;
            public int p_end;
            public int p_duration;

        };
         //private List<pause> pauses = new List<pause>();
         private List<List<pause>> pause_instances = new List<List<pause>>();

        public void task_generator()
        {
            List<task> tasks = new List<task>();
            for (int i = 0; i < N; i++)//N liczba  zadan
            {
                task new_task = new task();
                new_task.maszyna_op1 = 1;
                new_task.maszyna_op2 = 2;
                if(i % 2 == 0)
                {
                    new_task.duration_op1 = rnd.Next(1, 200);
                    new_task.duration_op2 = rnd.Next(1, 200);
                }
                else
                {
                    new_task.duration_op1 = rnd.Next(1, 50);
                    new_task.duration_op2 = rnd.Next(1, 50);
                }
                
                time_mach1 += new_task.duration_op1;
                time_mach2 += new_task.duration_op2;
                if (i <= N/2)
                {
                    new_task.start = 0;
                }
                
                tasks.Add(new_task);



            }

            for(int j = N/2 + 1; j < N; j++)
            {
                task temp_task = new task();
                temp_task.maszyna_op1 = 1;
                temp_task.maszyna_op2 = 2;
                temp_task.duration_op1 = tasks[j].duration_op1;
                temp_task.duration_op2 = tasks[j].duration_op2;
                temp_task.start = rnd.Next(1, (time_mach1 + time_mach2) * 1 / 4);
                tasks[j] = temp_task;
            }
            //buffer_task = tasks;
            task_instances.Add(tasks);
            System.Console.WriteLine("{0}", task_instances[task_instances.Count - 1].Count);
            //tasks.Clear();
            System.Console.WriteLine("{0}", task_instances[task_instances.Count - 1].Count);
        }
        public void pause_generator()
        {
            List<pause> pauses = new List<pause>();
            int pause_number = rnd.Next(N/5,N);
            for (int i = 0; i < pause_number; i++)
            {
                pause temp_pause = new pause();
                temp_pause.p_id = i;
                temp_pause.p_duration = rnd.Next(1, 20);//górna granica czasu?
                temp_pause.p_start = rnd.Next(1, time_mach1 - temp_pause.p_duration - 1);
                temp_pause.p_end = temp_pause.p_start + temp_pause.p_duration;
                pauses.Add(temp_pause);
                
            }
            pause_instances.Add(pauses);
            //pauses.Clear();
        }
        private void save(string path)
        {
            StreamWriter sr = new StreamWriter(path);
            //int i = 0;
            for(var i = 0; i < task_instances.Count; i++)
            {
                sr.WriteLine("***{0}***", i);
                //System.Console.WriteLine("{0}", task_list.Count);
                foreach (task taskk in task_instances[i])
                {
                    Console.WriteLine("{0};{1};{2};{3};{4};", taskk.duration_op1, taskk.duration_op2, taskk.maszyna_op1, taskk.maszyna_op2, taskk.start);//czas_operacji1_1; czas_operacji2_1; nr_maszyny_dla_op1_1; nr_maszyny_dla_op1_2; 
                    sr.WriteLine("{0};{1};{2};{3};{4};", taskk.duration_op1 , taskk.duration_op2,taskk.maszyna_op1,taskk.maszyna_op2,taskk.start);//czas_operacji1_1; czas_operacji2_1; nr_maszyny_dla_op1_1; nr_maszyny_dla_op1_2; 
                }
                
                int x = 0;
                foreach (pause pausee in pause_instances[i])
                {
                    sr.WriteLine("{0};{1};{2};", x, pausee.p_duration, pausee.p_start);
                    x++;
                }
                sr.WriteLine("***EOF***");
            }
            sr.Close();
        }



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Int32.TryParse(textBox1.Text, out N);
            task_generator();
            pause_generator();
            System.Console.WriteLine("{0} {1}", pause_instances.Count, task_instances.Count);
            textBox1.Text = "";
            time_mach1 = 0;
            time_mach2 = 0;
            save_button.Enabled = true;

        }

        private void save_button_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string save_file = saveFileDialog1.FileName;
            save(save_file);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
            {
                button1.PerformClick();
                e.Handled = true;
            }
        }
    }
}
