using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    { 
        
        static void Main(string[] args)
        {
            int timeInSec = 0;
            Console.WriteLine("Введите количество секунд!");
            timeInSec = Int32.Parse(Console.ReadLine());
            
            Thread t1 = new Thread(System_Timers_Timer);
            t1.Name = "System_Timers_Timer";
            t1.Start(timeInSec);
            Thread t2 = new Thread(System_Threading_Timer);
            t2.Name = "System_Threading_Timer";
            t2.Start(timeInSec);
            Thread t3 = new Thread(System_Windows_Forms_Timer);
            t3.Name = "System_Windows_Forms_Timer";
            t3.Start(timeInSec);
            Thread t4 = new Thread(System_Windows_Threading_DispatcherTimer);
            t4.Name = "System_Windows_Threading_DispatcherTimer";
            t4.Start(timeInSec);
            Console.Read();
            
        }

        static async void Logging(TimerInfo info)
        {
            string writePath = @"C:\SomeDir\hta2.txt";

            string text = "Привет мир!\nПока мир...";
            try
            {
                using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                {
                    await sw.WriteLineAsync(text);
                }

                using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {
                    await sw.WriteLineAsync(text);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        static void System_Timers_Timer(object secVal)
        {
            Stopwatch stopwatch = new Stopwatch();  // Запускаем внутренний таймер объекта Stopwatch
            stopwatch.Start();
            Console.WriteLine(DateTime.Now.Ticks + ": Таймер " + Thread.CurrentThread.Name + " - запустился ");
            
            Thread.Sleep(100);
            
            stopwatch.Stop(); // Останавливаем внутренний таймер объекта Stopwatch
            Console.WriteLine(DateTime.Now.Ticks + ": Таймер " + Thread.CurrentThread.Name + " - завершился ");
            Console.WriteLine("Потрачено тактов на выполнение: " + stopwatch.ElapsedTicks);
        }
        
        static void System_Threading_Timer(object secVal)
        {
            
        }
        
        static void System_Windows_Forms_Timer(object secVal)
        {
            
        }
        
        static void System_Windows_Threading_DispatcherTimer(object secVal)
        {
            
        }
        

    }

    class TimerInfo
    {
        public string name;
        public DateTime start;
        public DateTime end;
        public long difference;
        public Exception timerException;
        public TimerInfo(string Name, DateTime Start, DateTime End, long Difference, Exception TimerException)
        {
            name = Name;
            start = Start;
            end = End;
            difference = Difference;
            timerException = TimerException;
        }
    }
}