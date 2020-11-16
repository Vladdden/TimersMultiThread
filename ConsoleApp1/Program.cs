using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Threading.Timer;
using System.Windows;

namespace ConsoleApp1
{
    class Program
    { 
        static int timeInSec = 0;
        static System.Timers.Timer T1;
        static AutoResetEvent waitHandler = new AutoResetEvent(true);
        static ManualResetEvent resetEvent = new ManualResetEvent(false);
        static ManualResetEvent resetEvent2 = new ManualResetEvent(false);
        static System.Windows.Forms.Timer TimerWF = new System.Windows.Forms.Timer();
        //static string writePath = "/home/vladdden/RiderProjects/ConsoleApp1/ConsoleApp1/log.txt";
        private static string writePath = @"C:\Users\Владислав\Desktop\log\log2.txt";
        static void Main(string[] args)
        {
            Console.WriteLine("Указать путь к файлу лога?");
            string answer = Console.ReadLine();
            if (answer == "да")
            {
                Console.WriteLine("Введите путь ВМЕСТЕ С ИМЕНЕМ ФАЙЛА (log.txt или др.)");
                writePath = Console.ReadLine();
            }
            Console.WriteLine("Введите количество секунд!");
            timeInSec = Int32.Parse(Console.ReadLine()) * 1000;
            //Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);
            //System_Timers_Timer(timeInSec);
            //System_Threading_Timer(timeInSec);
            
            Thread t1 = new Thread(System_Timers_Timer);
            t1.Name = "System_Timers_Timer";
            t1.Start(timeInSec);
            
            
            Thread t2 = new Thread(System_Threading_Timer);
            t2.Name = "System_Threading_Timer";
            t2.Start(timeInSec);
            /*
            Thread t3 = new Thread(System_Windows_Forms_Timer);
            t3.Name = "System_Windows_Forms_Timer";
            t3.Start(timeInSec);
            /*
            Thread t4 = new Thread(System_Windows_Threading_DispatcherTimer);
            t4.Name = "System_Windows_Threading_DispatcherTimer";
            t4.Start(timeInSec);
            */

            Console.Read();
            using (StreamWriter endString = new StreamWriter(writePath, true, System.Text.Encoding.Default))
            {
                endString.WriteLineAsync("--------------------------------------------------------------------------------------------");
            }
        }

        static async void Logging(TimerInfo info)
        {
            waitHandler.WaitOne();
            //string writePath = "" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/log.txt";
            //@"C:\SomeDir\log.txt";
            
            string t1 = $"{info.start:HH:mm:ss.fff} Таймер {info.name} запустился ({info.startTicks})";
            string t2 = $"{info.stop:HH:mm:ss.fff} Таймер {info.name} завершился ({info.stopTicks})";
            string t3 = $"Время выполнения таймера {info.name} ({timeInSec/1000} сек.) - {info.stop - info.start}({info.difference})";
            string t4 = $"Во время выполнения таймера {info.name} была обнаружена ошибка - {info.timerException}";
            string t5 = "--------------------------------------------------------------------------------------------";
            try
            {
                using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {
                    await sw.WriteLineAsync(t1);
                }

                using (StreamWriter sw2 = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {
                    await sw2.WriteLineAsync(t2);
                }

                using (StreamWriter sw3 = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {
                    await sw3.WriteLineAsync(t3);
                }

                if (info.timerException != null)
                {
                    using (StreamWriter sw4 = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                    {
                        await sw4.WriteLineAsync(t4);
                    }
                }

                using (StreamWriter sw5 = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {
                    await sw5.WriteLineAsync(t5);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            waitHandler.Set();
        }
        
        static void System_Timers_Timer(object secVal)
        {
            Console.WriteLine("Запуск первого таймера - {0:HH:mm:ss.fff}", DateTime.Now);
            DateTime start = new DateTime();
            DateTime stop = new DateTime();
            long startTicks = 0;
            long stopTicks = 0;
            long difference = 0;
            Exception timerException = null;
            try
            {
                Stopwatch stopwatch = new Stopwatch();  // Запускаем внутренний таймер объекта Stopwatch
                stopwatch.Start();
                start = DateTime.Now;
                startTicks = DateTime.Now.Ticks;
                ////////////////////////////////////////////////////////////////////////////////////////////////////////
                //Thread.Sleep(100);
                // Create a timer with a two second interval.
                T1 = new System.Timers.Timer((int)secVal);  
                // Hook up the Elapsed event for the timer. 
                T1.Elapsed += SomeFuncForTimer1;
                T1.AutoReset = false;
                T1.Enabled = true;
                resetEvent.WaitOne(); // This blocks the thread until resetEvent is set
                resetEvent.Close();
                resetEvent.Dispose();
                ////////////////////////////////////////////////////////////////////////////////////////////////////////
                stopwatch.Stop(); // Останавливаем внутренний таймер объекта Stopwatch
                stop = DateTime.Now;
                stopTicks = DateTime.Now.Ticks;
                difference = stopwatch.ElapsedTicks;
            }
            catch (Exception e)
            {
                Console.WriteLine(Thread.CurrentThread.Name + ":" + e);
                timerException = e;
            }
            finally
            {
                Console.WriteLine("Первый таймер завершился.");
                TimerInfo timer = new TimerInfo(Thread.CurrentThread.Name, start, startTicks, stop, stopTicks, difference, timerException);
                Logging(timer);
            }
        }
        
        static void System_Threading_Timer(object secVal)
        {
            Console.WriteLine("Запуск второго таймера -  {0:HH:mm:ss.fff}", DateTime.Now);
            DateTime start = new DateTime();
            DateTime stop = new DateTime();
            long startTicks = 0;
            long stopTicks = 0;
            long difference = 0;
            Exception timerException = null;
            try
            {
                Stopwatch stopwatch = new Stopwatch();  // Запускаем внутренний таймер объекта Stopwatch
                stopwatch.Start();
                start = DateTime.Now;
                startTicks = DateTime.Now.Ticks;
                ////////////////////////////////////////////////////////////////////////////////////////////////////////
                //Thread.Sleep(100);
                // Create a timer with a two second interval.
                //TimerCallback tm = new TimerCallback(SomeFunc);
                Timer T2 = new Timer(SomeFuncForTimer2, null, (int)secVal, Timeout.Infinite);
                resetEvent2.WaitOne(); // This blocks the thread until resetEvent is set
                resetEvent2.Close();
                ////////////////////////////////////////////////////////////////////////////////////////////////////////
                stopwatch.Stop(); // Останавливаем внутренний таймер объекта Stopwatch
                stop = DateTime.Now;
                stopTicks = DateTime.Now.Ticks;
                difference = stopwatch.ElapsedTicks;
            }
            catch (Exception e)
            {
                Console.WriteLine(Thread.CurrentThread.Name + ":" + e);
                timerException = e;
            }
            finally
            {
                Console.WriteLine("Второй таймер завершился.");
                TimerInfo timer = new TimerInfo(Thread.CurrentThread.Name, start, startTicks, stop, stopTicks, difference, timerException);
                Logging(timer);
            }
        }
        
        static void System_Windows_Forms_Timer(object secVal)
        {
            Console.WriteLine("start T3");
            DateTime start = new DateTime();
            DateTime stop = new DateTime();
            long startTicks = 0;
            long stopTicks = 0;
            long difference = 0;
            Exception timerException = null;
            //int count_val = count;
            //!!!TimerEndInfo timerEnd = new TimerEndInfo();
            try
            {
                Stopwatch stopwatch = new Stopwatch();  // Запускаем внутренний таймер объекта Stopwatch
                stopwatch.Start();
                start = DateTime.Now;
                startTicks = DateTime.Now.Ticks;
                ////////////////////////////////////////////////////////////////////////////////////////////////////////
                TimerWF.Interval = (int) secVal; // 
                TimerWF.Tick += new EventHandler(SomeFuncForTimerWF);
                TimerWF.Enabled = true;
                TimerWF.Start();
                
                ////////////////////////////////////////////////////////////////////////////////////////////////////////
                stopwatch.Stop(); // Останавливаем внутренний таймер объекта Stopwatch
                stop = DateTime.Now;
                stopTicks = DateTime.Now.Ticks;
                difference = stopwatch.ElapsedTicks;
            }
            catch (Exception e)
            {
                Console.WriteLine(Thread.CurrentThread.Name + ":" + e);
                timerException = e;
            }
            finally
            {
                Console.WriteLine("EndT3"); // TODO изменить вывод
                TimerInfo timer = new TimerInfo(Thread.CurrentThread.Name, start, startTicks, stop, stopTicks, difference, timerException);
                Logging(timer);
            }
        }
        
        static void System_Windows_Threading_DispatcherTimer(object secVal)
        {
            
        }
        
        static void SomeFuncForTimer1(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Timer 1 is working!"); 
            resetEvent.Set();
        }
        
        static void SomeFuncForTimerWF(Object source, EventArgs e)
        {
            Console.WriteLine("Timer TimerWF is working!"); 
            TimerWF.Stop();
            TimerWF.Dispose();
        }
        
        static void SomeFuncForTimer2(object o)
        {
            Console.WriteLine("Timer 2 is working!"); 
            resetEvent2.Set();
        }
        
    }
    
    class TimerInfo
    {
        public string name;
        public DateTime start; // ("{0:O}", now)
        public long startTicks;
        public DateTime stop;
        public long stopTicks;
        public long difference;
        public Exception timerException;
        public TimerInfo(string Name, DateTime Start, long StartTicks, DateTime Stop, long StopTicks, long Difference, Exception TimerException)
        {
            name = Name;
            start = Start;
            startTicks = StartTicks;
            stop = Stop;
            stopTicks = StopTicks;
            difference = Difference;
            timerException = TimerException;
        }
    }
}