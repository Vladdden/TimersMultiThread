using System;
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

        static void Logging()
        {
            
        }
        
        static void System_Timers_Timer(int secVal)
        {
            
        }
        
        static void System_Threading_Timer(int secVal)
        {
            
        }
        
        static void System_Windows_Forms_Timer(int secVal)
        {
            
        }
        
        static void System_Windows_Threading_DispatcherTimer(int secVal)
        {
            
        }
        
        
        class TimerThread 
        {
            Thread thread;

            public TimerThread(string name, int timeInSec) //Конструктор получает имя функции и номер до кторого ведется счет
            {

                thread = new Thread(this.func);
                thread.Name = name;
                thread.Start(timeInSec);//передача параметра в поток
            }
            
            void func(object num)//Функция потока, передаем параметр
            {
                for (int i = 0;i < (int)num;i++ ) 
                {
                    Console.WriteLine(Thread.CurrentThread.Name + " выводит " + i);
                    Thread.Sleep(0);
                }
                Console.WriteLine(Thread.CurrentThread.Name + " завершился");
            }
        
        
        }     
        
    }
}