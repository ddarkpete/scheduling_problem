using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace scheduing_fs_ts
{
    public partial class Form1 : Form
    {
        static int N = 30;
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
        public List<task> tasks;

        struct pause
        {
            public int p_start;
            public int p_end;
            public int p_duration;

        };
        List<pause> pauses;

        public void task_generator()
        {
            for (int i = 0; i < N; i++)//N liczba  zadan
            {
                task new_task = new task();
                //new_task.start = rnd.Next(0 ,30); //rozpoczecie na tamtych zasadach najpierw trzeba zssumować wszystko
                new_task.maszyna_op1 = 1;
                new_task.maszyna_op2 = 2;
                new_task.duration_op1 = rnd.Next(1, 50);//tu do zmiany w zależności od 'klasy instancji'
                new_task.duration_op2 = rnd.Next(1, 50);
                time_mach1 += new_task.duration_op1;
                time_mach2 += new_task.duration_op2;
                if (i <= N/2)
                {
                    new_task.start = 0;
                    //ile powinien wynosić stop?
                    //new_task.pause = new_task.duration_op1 + new_task.duration_op1 + 10;//ile czasu zeby skonczyc???
                }
               /* else { 
                        }*/
                tasks.Add(new_task);


                /* if (tasks[i].maszyna_op1 == 1)
                {
                    tasks[i].maszyna_op2 = 2;
                    time1 += tasks[i].duration_op1;
                    time2 += tasks[i].duration_op2;
                }
                else
                {
                    tasks[i].maszyna_op2 = 1;
                    time2 += tasks[i].duration_op1;
                    time1 += tasks[i].duration_op2;
                }*/

            }

            for(int j = N/2 + 1; j < N; j++)
            {
                task temp_task = new task();
                temp_task.maszyna_op1 = 1;
                temp_task.maszyna_op2 = 2;
                temp_task.duration_op1 = tasks[j].duration_op1;
                temp_task.duration_op2 = tasks[j].duration_op2;
                temp_task.start = rnd.Next(1, (time_mach1 + time_mach2) * 1 / 4);
                //temp_task.pause = rnd.Next(temp_task.start, temp_task.duration_op1 + temp_task.duration_op2 + 10);//to jest do wymyslenia
                tasks[j] = temp_task;
            }
        }
        public void pause_generator()
        {
            int pause_number = rnd.Next(N/5,N);//jaka górna granica liczby przestojów?
            //a = pause_number;
            for (int i = 0; i < pause_number; i++)
            {
                pause temp_pause = new pause();
                temp_pause.p_duration = rnd.Next(1, 20);//górna granica czasu?
                temp_pause.p_start = rnd.Next(1, time_mach1 - temp_pause.p_duration - 1);
                temp_pause.p_end = temp_pause.p_start + temp_pause.p_duration;
                pauses.Add(temp_pause);
                /*pauses[i].p_end = 0;
                pauses[i].p_start = pauses[i].p_end + rand() % (time1 - 100) + 10;
                pauses[i].p_duration = rand() % 20 + 1;
                pauses[i].p_end = pauses[i].p_start + pauses[i].p_duration;*/
            }
        }



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
