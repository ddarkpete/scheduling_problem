using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace scheduing_fs_ts
{
    class Backend : Form1
    {
        private static Backend instance;

        private Backend() { }

        public static Backend Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Backend();
                }
                return instance;
            }
        }
        public string loaded_instance_id;
        public string loaded_instance_path;
        public int instance_nO = 0;
        public int N = 30;
        Random rnd = new Random();
        public int time_mach1 = 0;
        public int time_mach2 = 0;
        public int p_time;
        public int t_time1;
        public int t_time2;


        public List<Task> Tasks = new List<Task>();
        public List<Task> ScheduledTasks = new List<Task>();

        public List<Pause> Pauses = new List<Pause>();
        public List<Pause> SortedPauses = new List<Pause>();
        public void task_generator()
        {

            for (int i = 0; i < N; i++)//N liczba  zadan
            {
                Task New_task = new Task();
                New_task.id = i; 
                New_task.maszyna_op1 = 1;
                New_task.maszyna_op2 = 2;
                if (i % 2 == 0)
                {
                    New_task.duration_op1 = rnd.Next(1, 200);
                    New_task.duration_op2 = rnd.Next(1, 200);
                    
                }
                else
                {
                    New_task.duration_op1 = rnd.Next(1, 200);
                    New_task.duration_op2 = rnd.Next(1, 200);
                    
                }

                time_mach1 += New_task.duration_op1;
                time_mach2 += New_task.duration_op2;
                if (i <= N / 2)
                {
                    New_task.start = 0;
                }

                Tasks.Add(New_task);
            }
            for (int j = N / 2 + 1; j < N; j++)
            {
                Tasks[j].start = rnd.Next(1, (time_mach1 + time_mach2) * 1 / 4);

            }
            //buffer_task = tasks;
            /*     task_instances.Add(tasks);
                 System.Console.WriteLine("{0}", task_instances[task_instances.Count - 1].Count);
                 //tasks.Clear();
                 System.Console.WriteLine("{0}", task_instances[task_instances.Count - 1].Count);*/
        }
        public void pause_generator()
        {

            int pause_number = rnd.Next(N / 5, N);
            for (int i = 0; i < pause_number; i++)
            {
                Pause Temp_pause = new Pause();
                Temp_pause.p_id = i;
                Temp_pause.p_duration = rnd.Next(1, 20);//górna granica czasu?
                
                Temp_pause.p_start = rnd.Next(1, time_mach1 - Temp_pause.p_duration - 1);
                Temp_pause.p_end = Temp_pause.p_start + Temp_pause.p_duration;
                Pauses.Add(Temp_pause);

            }

        }
       

        public int count_time(List<Task> SortedTasks, Form1 formobject)
        {

            int m1_time = 0;
            int m2_time = 0;
            List<Task> copy = SortedTasks.ToList();
            List<int> end_op1 = new List<int>();
            List<int> end_op2 = new List<int>();
            //List<bool> scheduled_op1
            for (int i = 0; i < SortedTasks.Count; i++)
            {
                end_op1.Add(0);
                end_op2.Add(0);
            }
            bool scheduled = false;
            while (scheduled == false)
            {
                
                if (end_op1.Contains(0))
                {              



                    for (int i = 0; i < copy.Count; i++)
                    {
                        while (end_op1[i] == 0)
                        {
                           
                            if (copy[i].start <= m1_time && end_op1[i] == 0)
                            {
                                //int p_counter = 0;
                                
                                for(int p =0; p < SortedPauses.Count; p++)
                                {
                                    if (m1_time <= SortedPauses[p].p_start && SortedPauses[p].p_start < (m1_time + copy[i].duration_op1))
                                    {

                                        int before_pause = SortedPauses[p].p_start - m1_time;
                                        m1_time += before_pause + SortedPauses[p].p_duration;//+ copy[i].duration_op1;
                                        //p_counter++;

                                    }

                                }
                                m1_time += copy[i].duration_op1;
                                end_op1[i] = m1_time;
                                //break;
                            }
                            else
                            {
                                 m1_time++;

                            }
                        }
                    }

                 
                   
                    for (int j = 0; j < copy.Count; j++)
                    {
                       

                        if (j > 0)
                        {
                            if (end_op1[j] != 0 && end_op2[j - 1] != 0)
                            {

                                
                                if (end_op2[j - 1] >= end_op1[j])
                                {
                                    m2_time = end_op2[j - 1];
                                    m2_time += copy[j].duration_op2;
                                    end_op2[j] = m2_time;
                                }

                                else if (end_op1[j] > end_op2[j - 1])
                                {
                                    m2_time = end_op1[j];
                                    m2_time += copy[j].duration_op2;
                                    end_op2[j] = m2_time;
                                }
                            }
                        }
                        else
                        {
                            if (end_op1[0] != 0)
                            {
                                m2_time = end_op1[0];
                                m2_time += copy[0].duration_op2;
                                end_op2[0] = m2_time;
                            }

                        }
                    }
               
                }
                else { scheduled = true;
                  
                }
            }
            int total_end_time = end_op1.Sum() + end_op2.Sum();
           
            formobject.taskBox.Text += "Total time: " + m2_time + System.Environment.NewLine;
            return total_end_time;
        }

        public void tabu(Form1 form)
        {
            List<Task> ActualSchedule = new List<Task> (ScheduledTasks);
            int ActualScheduleTime = count_time(ActualSchedule, form);
            int start_time = ActualScheduleTime;

            List<Task> BestSchedule =new List<Task> (ScheduledTasks);
            int BestScheduleTime = ActualScheduleTime;

            List<Task> PreviousSchedule = new List<Task>();
            int PreviousScheduleTime = BestScheduleTime * 2;

            Console.WriteLine("{0} {1}", ActualScheduleTime, BestScheduleTime);

            List<TabuChange> TabuChanges = new List<TabuChange>();

            Console.WriteLine("Wyszukiwanie...");
            for (int i = 0; i < 650; i++)// TU ZMIENIAJ OD 100-650
            {
                bool LocalMin = false;
                int BadChanges = 0;
                

                while(!(LocalMin))
                {
                    if (BadChanges == 2)//zbyt wiele zlych zmian , trzeba pomieszac kolejnosc
                    {
                        //Console.WriteLine("Minimum lokalne");
                        List<Task> Randomize = new List<Task>();
                        Randomize = ActualSchedule.OrderBy(item => rnd.Next()).ToList();
                        ActualSchedule = Randomize;
                        PreviousSchedule = ActualSchedule;
                        ActualScheduleTime = count_time(ActualSchedule, form);//obliczamy nowy aktualny czas dla pomieszanego
                        PreviousScheduleTime = ActualScheduleTime;
                        TabuChanges.Clear();
                        //Console.WriteLine("BEst na koniec {0}", count_time(BestSchedule, form));
                        LocalMin = true;//konczymy lokalne poszukiwanie
                        break;
                    }
                   /* if (ActualScheduleTime > PreviousScheduleTime)//Czas sie pogorszył musimy wrócić do poprzedniego rozwiązania
                    {
                        Console.WriteLine("Gorszy niz poprzedni"); 

                        
                    }*/
                   
                    else if(ActualScheduleTime >= BestScheduleTime)//jesli czas jest gorszy lub rowny najlepszemu , to sprobujmy go poprawic
                    {
                        //Console.WriteLine("Próba poprawy");
                        PreviousScheduleTime = ActualScheduleTime;
                        PreviousSchedule = ActualSchedule;

                        int index_1st = rnd.Next(ActualSchedule.Count);//losujemy id własne
                        int index_2st = rnd.Next(ActualSchedule.Count);//losujemy id własne
                        while (index_1st == index_2st) { index_2st = rnd.Next(ActualSchedule.Count); }//żeby miec pewność że są różne
                        var index_2 = ActualSchedule.FindIndex(x => x.id == index_2st);// szukam gdzie w tablicy jest zadanie o takim id własnym
                       
                        
                        

                        TabuChange TempChange = new TabuChange();
                        TempChange.tabu_el_1 = index_1st;//zapisuje to id własne
                        TempChange.tabu_el_2 = index_2st;// zapisuje to id własne

                        if (!(TabuChanges.Contains(TempChange)))//jak N I E ma takiej zmiany na liście zakazanych to zamieniamy
                        {
                            /* Task TempTask = ActualSchedule[index_1st];
                             ActualSchedule[index_1st] = ActualSchedule[index_2st];
                             ActualSchedule[index_2st] = TempTask;*/

                            
                        
                            Task TempTask = new Task();
                            TempTask = ActualSchedule[index_2st];
                            ActualSchedule.RemoveAt(index_2st);//usuwam zadanie które id własne jest takie jakie wylosowane (patrz 264)
                            var index_1 = ActualSchedule.FindIndex(x => x.id == index_1st);//szukam gdzie w liscie jest zadanie o wylosowanym id
                            ActualSchedule.Insert(index_1st, TempTask); // wstawiam przed to zadanie zadanie o id 2


                            ActualScheduleTime = count_time(ActualSchedule, form);//liczymy czas po zamianie
                            if(ActualScheduleTime >= PreviousScheduleTime)// pogorszenie lub brak polepszenia
                            {
                                //Console.WriteLine("        Poprawa dała to samo lub nawet gorzej");
                                ActualSchedule = PreviousSchedule;
                                ActualScheduleTime = PreviousScheduleTime;

                           

                                if (TabuChanges.Count < 7)// TU MONIKA ZMIENIAJ NA LICXZBY Z EXCELA
                                {
                                    TabuChanges.Add(TempChange);
                                }
                                else
                                {
                                    TabuChanges.RemoveAt(0);// wyrzuc pierwszy
                                    TabuChanges.Add(TempChange);
                                }
                                BadChanges++;
                            }
                        }

                    }
                    else if(ActualScheduleTime < BestScheduleTime)//actual jest lepszy <wow> od najlepszego :OOOO
                    {
                        BestSchedule = ActualSchedule.ToList();
                        BestScheduleTime = ActualScheduleTime;
                        continue;
                    }
                    
                }
            }
            //Console.WriteLine("Najlepszy: {0}", BestScheduleTime);
            //Console.WriteLine("Najlepszy obliczony tutaj: {0}", count_time(BestSchedule, form));
            form.textBox3.Text =BestScheduleTime.ToString();
            save_schedule(BestSchedule, BestScheduleTime, start_time);
        }


        ////////**************************************ZAPIS/ODCZYT*********************************************/
        public void load(string path)
        {
            int tasks_count;
            int pauses_count;

            loaded_instance_path = path ;

            List<Task> loaded_tasks = new List<Task>();
            List<Pause> loaded_pauses = new List<Pause>();
            StreamReader sr = new StreamReader(path);
            loaded_instance_id = sr.ReadLine();
            int tasks1 = 0;
            Int32.TryParse(sr.ReadLine(), out tasks_count);
            for (int i = 0; i < tasks_count; i++)
            {
                string loaded_task = sr.ReadLine();
                string[] split = loaded_task.Split(';');
              //  Console.WriteLine("{0}", split.Length);
                //Console.WriteLine("{0};{1};{2};{3};{4};", split[0], split[1], split[2], split[3], split[4]);
                Task TempLoad_task = new Task();//czas_operacji1_1; czas_operacji2_1; nr_maszyny_dla_op1_1; nr_maszyny_dla_op1_2; 
                TempLoad_task.id = i;
                Int32.TryParse(split[0], out TempLoad_task.duration_op1);
                tasks1 += TempLoad_task.duration_op1;
                Int32.TryParse(split[1], out TempLoad_task.duration_op2);
                Int32.TryParse(split[2], out TempLoad_task.maszyna_op1);
                Int32.TryParse(split[3], out TempLoad_task.maszyna_op2);
                Int32.TryParse(split[4], out TempLoad_task.start);
                t_time1 += TempLoad_task.duration_op1;
                t_time2 += TempLoad_task.duration_op2;
                loaded_tasks.Add(TempLoad_task);
            }
            Int32.TryParse(sr.ReadLine(), out pauses_count);
            int pauses1 = 0;
            for (int j = 0; j < pauses_count; j++)
            {
                Pause Temp_pause = new Pause();
                string loaded_pause = sr.ReadLine();
                string[] split = loaded_pause.Split(';');
                Int32.TryParse(split[0], out Temp_pause.p_id);
                Int32.TryParse(split[1], out Temp_pause.p_duration);
                Int32.TryParse(split[2], out Temp_pause.p_start);
                pauses1 += Temp_pause.p_duration + Temp_pause.p_start;
                Temp_pause.p_end = Temp_pause.p_start + Temp_pause.p_duration;
                p_time += Temp_pause.p_duration;
                loaded_pauses.Add(Temp_pause);

            }
            ScheduledTasks = loaded_tasks;
            SortedPauses = loaded_pauses.OrderBy(o => o.p_start).ToList();
            //System.Console.WriteLine("tasks op1 {0}", tasks1);


        }

        public void save(string path)
        {
            path += ".txt";
            StreamWriter sr = new StreamWriter(path);
            sr.WriteLine("***{0}***", instance_nO); 
            instance_nO++;
            sr.WriteLine("{0}", Tasks.Count);
            foreach (Task taskk in Tasks)
            {
                //Console.WriteLine("{0};{1};{2};{3};{4};", taskk.duration_op1, taskk.duration_op2, taskk.maszyna_op1, taskk.maszyna_op2, taskk.start);//czas_operacji1_1; czas_operacji2_1; nr_maszyny_dla_op1_1; nr_maszyny_dla_op1_2; 
                sr.WriteLine("{0};{1};{2};{3};{4};", taskk.duration_op1, taskk.duration_op2, taskk.maszyna_op1, taskk.maszyna_op2, taskk.start);//czas_operacji1_1; czas_operacji2_1; nr_maszyny_dla_op1_1; nr_maszyny_dla_op1_2; 
                                                                                                                                                // 
            }

            int x = 0;
            sr.WriteLine("{0}", Pauses.Count);
            foreach (Pause pause in Pauses)
            {
                sr.WriteLine("{0};{1};{2};", x, pause.p_duration, pause.p_start);
                x++;
            }

            sr.WriteLine("***EOF***");
            sr.Close();
        }

        public void save_schedule(List<Task> Result, int best_time, int start_schedule)
        {
            string path = loaded_instance_path.Remove(loaded_instance_path.Length - 4);
            path += "_result.txt";

            Console.WriteLine("Zapis w {0}", path);

            StreamWriter Sr = new StreamWriter(path);
            Sr.WriteLine(loaded_instance_id);
            Sr.WriteLine("{0},{1}", best_time, start_schedule);
            Sr.Write("M1:");
            int all_idles_m1 = 0;// idle_count = 0 ;
            int all_maints=0;
            int maints_counter=0;
            bool idle_m1 = false;
            int idle_time_m1=0, idle_start_m1=0;
            int idle_id_m1 = 0;

            int all_idles_m2 = 0, idle_count_m2 = 0;
            //int idle_start_m2, idle_time_m2;
            int idle_id_m2 = 0;

            int m1_time = 0;
            int m2_time = 0;
            List<Task> copy = Result.ToList();
            List<int> end_op1 = new List<int>();
            List<int> end_op2 = new List<int>();
            //List<bool> scheduled_op1
            for (int i = 0; i < Result.Count; i++)
            {
                end_op1.Add(0);
                end_op2.Add(0);
            }
            bool scheduled = false;
            while (scheduled == false)
            {

                if (end_op1.Contains(0))
                {



                    for (int i = 0; i < copy.Count; i++)
                    {
                        while (end_op1[i] == 0)
                        {

                            if (copy[i].start <= m1_time && end_op1[i] == 0)
                            {
                                //int p_counter = 0;
                                if(idle_m1)
                                {
                                    idle_id_m1++;
                                    Sr.Write("idle{0}_M1,{1},{2};", idle_id_m1, idle_start_m1, idle_time_m1);
                                    idle_id_m1++;
                                    all_idles_m1 += idle_time_m1;
                                    idle_time_m1 = 0;
                                    idle_m1 = false;
                                }
                                int pcounter = 0;
                                for (int p = 0; p < SortedPauses.Count; p++)
                                {
                                    bool multipause = false;
                                    if (m1_time <= SortedPauses[p].p_start && SortedPauses[p].p_start < (m1_time + copy[i].duration_op1))
                                    {
                                        pcounter++;
                                        int before_pause = SortedPauses[p].p_start - m1_time;
                                        if (multipause)
                                        {
                                            idle_start_m1 = m1_time - before_pause;
                                            idle_time_m1 = before_pause;
                                             Sr.Write("idle{0}_M1,{1},{2};", idle_id_m1, idle_start_m1, idle_time_m1);
                                            idle_id_m1++;
                                            all_idles_m1 += idle_time_m1;
                                            idle_time_m1 = 0;
                                        }
                                        if(before_pause > 0)
                                        {
                                            idle_start_m1 = m1_time - before_pause;
                                            idle_time_m1 = before_pause;
                                            idle_id_m1++;
                                            Sr.Write("idle{0}_M1,{1},{2};", idle_id_m1, m1_time, before_pause);
                                            idle_id_m1++;
                                            all_idles_m1 += before_pause;
                                            
                                        }

                                        m1_time += before_pause + SortedPauses[p].p_duration;//+ copy[i].duration_op1;

                                        if (idle_m1)
                                        {
                                            Sr.Write("idle{0}_M1,{1},{2};", idle_id_m1, idle_start_m1, idle_time_m1);
                                            idle_id_m1++;
                                            all_idles_m1 += idle_time_m1;
                                            idle_time_m1 = 0;
                                            idle_m1 = false;
                                        }
                                        multipause = true;
                                        //Console.Write("maint{0}_M1,{1},{2}", SortedPauses[p].p_id , SortedPauses[p].p_start, SortedPauses[p].p_duration);
                                        Sr.Write("maint{0}_M1,{1},{2};", SortedPauses[p].p_id, SortedPauses[p].p_start, SortedPauses[p].p_duration);
                                        all_maints += SortedPauses[p].p_duration;
                                        maints_counter++;

                                    }

                                }

                                if (idle_m1)
                                {
                                    Sr.Write("idle{0}_M1,{1},{2};", idle_id_m1, idle_start_m1, idle_time_m1);
                                    idle_id_m1++;
                                    all_idles_m1 += idle_time_m1;
                                    idle_time_m1 = 0;
                                    idle_m1 = false;
                                }
                                if(i > 0 && pcounter == 0 && m1_time > end_op1[i-1])
                                {
                                    Sr.Write("idle{0}_M1,{1},{2};", idle_id_m1,end_op1[i-1], m1_time - end_op1[i - 1]);
                                    idle_id_m1++;
                                    all_idles_m1 += idle_time_m1;
                                }
                                Sr.Write("op1_{0}_M1,{1},{2};",copy[i].id, m1_time, copy[i].duration_op1);
                                m1_time += copy[i].duration_op1;
                                end_op1[i] = m1_time;
                                //break;
                            }
                            else
                            {
                                if (idle_m1)
                                {
                                    idle_time_m1++;
                                }
                                else
                                {
                                    idle_m1 = true;
                                    idle_start_m1 = m1_time;
                                    idle_time_m1++;
                                }
                                m1_time++;

                            }
                        }
                    }
                    Sr.Write(Environment.NewLine);
                    Sr.Write("M2:");
                    

                    for (int j = 0; j < copy.Count; j++)
                    {


                        if (j > 0)
                        {
                            if (end_op1[j] != 0 && end_op2[j - 1] != 0)
                            {


                                if (end_op2[j - 1] >= end_op1[j])
                                {
                                    m2_time = end_op2[j - 1];
                                    Sr.Write("op2_{0}_M2,{1},{2};", copy[j].id, m2_time, copy[j].duration_op2);
                                    m2_time += copy[j].duration_op2;
                                    end_op2[j] = m2_time;
                                }

                                else if (end_op1[j] > end_op2[j - 1])
                                {
                                    idle_id_m2++;
                                    Sr.Write("idle{0}_M2,{1},{2};", idle_id_m2, end_op2[j - 1], end_op1[j] - end_op2[j - 1]);
                                    all_idles_m2 += end_op1[j] - end_op2[j - 1];

                                    m2_time = end_op1[j];
                                    Sr.Write("op2_{0}_M2,{1},{2};", copy[j].id, m2_time, copy[j].duration_op2);
                                    m2_time += copy[j].duration_op2;
                                    end_op2[j] = m2_time;
                                }
                            }
                        }
                        else
                        {
                            if (end_op1[0] != 0)
                            {
                                m2_time = end_op1[0];
                                if(m2_time >0)
                                {
                                    idle_id_m2++;
                                    Sr.Write("idle{0}_M2,{1},{2};", idle_id_m2, 0, idle_time_m1, m2_time);
                                    all_idles_m2 += m2_time;
                                }
                                Sr.Write("op2_{0}_M2,{1},{2};", copy[j].id, m2_time, copy[j].duration_op2);
                                m2_time += copy[0].duration_op2;
                                
                                end_op2[0] = m2_time;
                            }

                        }
                    }

                }
                else
                {
                    scheduled = true;

                }
            }
            Sr.Write(Environment.NewLine);
            Sr.WriteLine("{0},{1} mints", maints_counter, all_maints);
            Sr.WriteLine("0,0");
            Sr.WriteLine("{0},{1} idles1", idle_id_m1 ,all_idles_m1);
            Sr.WriteLine("{0},{1} idles2", idle_id_m2, all_idles_m2);
            Sr.WriteLine("***EOF***");
            Sr.Close();

        }

       
    }
}
