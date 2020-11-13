using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Threading.Timer;

namespace ConsoleApp1
{
    class Program
    { 
        static int count = 3;
        static System.Timers.Timer T1;
        static System.Threading.Timer T2;
        static AutoResetEvent waitHandler = new AutoResetEvent(true);
        static ManualResetEvent resetEvent = new ManualResetEvent(false);
        static ManualResetEvent resetEvent2 = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            int timeInSec = 0;
            Console.WriteLine("Введите количество секунд!");
            timeInSec = Int32.Parse(Console.ReadLine()) * 1000;
            //Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);
            //System_Timers_Timer(timeInSec);
            //System_Threading_Timer(timeInSec);
            Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);
            Thread t1 = new Thread(System_Timers_Timer);
            t1.Name = "System_Timers_Timer";
            t1.Start(timeInSec);
            
            Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);
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
            
        }

        static async void Logging(TimerInfo info)
        {
            waitHandler.WaitOne();
            //string writePath = "" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/log.txt";
            //@"C:\SomeDir\log.txt";
            string writePath = "/home/vladdden/RiderProjects/ConsoleApp1/ConsoleApp1/log.txt";
            string t1 = $"{info.start:HH:mm:ss.fff} Таймер {info.name} запустился ({info.startTicks})";
            string t2 = $"{info.stop:HH:mm:ss.fff} Таймер {info.name} завершился ({info.stopTicks})";
            string t3 = $"Время выполнения таймера {info.name} - {info.start - info.stop}({info.difference})";
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
            Console.WriteLine("start T1");
            DateTime start = new DateTime();
            DateTime stop = new DateTime();
            long startTicks = 0;
            long stopTicks = 0;
            long difference = 0;
            Exception timerException = null;
            //int count_val = count;
            TimerEndInfo timerEnd = new TimerEndInfo();
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
                T1.Elapsed += OnTimedEvent;
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
                Console.WriteLine("EndT1");
                TimerInfo timer = new TimerInfo(Thread.CurrentThread.Name, start, startTicks, stop, stopTicks, difference, timerException);
                Logging(timer);
            }
        }
        
        static void System_Threading_Timer(object secVal)
        {
            Console.WriteLine("start T2");
            DateTime start = new DateTime();
            DateTime stop = new DateTime();
            long startTicks = 0;
            long stopTicks = 0;
            long difference = 0;
            Exception timerException = null;
            //int count_val = count;
            TimerEndInfo timerEnd = new TimerEndInfo();
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
                Timer timer = new Timer(SomeFunc, null, (int)secVal, Timeout.Infinite);
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
                Console.WriteLine("EndT2");
                TimerInfo timer = new TimerInfo(Thread.CurrentThread.Name, start, startTicks, stop, stopTicks, difference, timerException);
                Logging(timer);
            }
        }
        
        static void System_Windows_Forms_Timer(object secVal)
        {
            
        }
        
        static void System_Windows_Threading_DispatcherTimer(object secVal)
        {
            
        }
        
        static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
            resetEvent.Set();
        }

        static void SomeFunc(object o)
        {
            Console.WriteLine("Timer working!");
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
    
    class TimerEndInfo
    {
        public DateTime stop { get; set; }
        public long stopTicks { get; set; }

    }
    
}