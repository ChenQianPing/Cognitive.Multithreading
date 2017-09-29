using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chapter01
{
    public class ThreadHelper
    {
        #region TestMethod01
        public static void TestMethod01()
        {
            var t = new Thread(Run01);
            t.Start();
            // Join相当于把Run方法内嵌如此
            t.Join();
            // 该死的t.Join(),害的我主线程必须在你执行完后才能执行。
            Console.WriteLine("我是主线程：" + Thread.CurrentThread.GetHashCode());
            Console.ReadLine();

            /*
             * 运行结果：
             * 我是线程：10
             * 我是主线程：9
             */
        }

        private static void Run01()
        {
            // 等待2s
            Thread.Sleep(2000);
            Console.WriteLine("我是线程：" + Thread.CurrentThread.GetHashCode());
        }
        #endregion

        public static void TestMethod02()
        {
            var t = new Thread(new ThreadStart(Run02));
            t.Start();

            /*
             * 阻止动作，如果终止工作线程，只能管到一次，
             * 工作线程的下一次sleep就管不到了，相当于一个contine操作。
             */
            t.Interrupt();

            Console.Read();

            /*
             * 运行结果：
             * 第1延迟执行：238ms,不过抛出异常
             * 第2延迟执行：999ms
             * 第3延迟执行：999ms
             */
        }


        private static void Run02()
        {
            for (var i = 1; i <= 3; i++)
            {
                var watch = new Stopwatch();

                try
                {
                    watch.Start();
                    Thread.Sleep(1000);
                    watch.Stop();

                    Console.WriteLine($"第{i}延迟执行：{watch.ElapsedMilliseconds}ms");
                }
                catch (ThreadInterruptedException e)
                {
                    Console.WriteLine($"第{i}延迟执行：{watch.ElapsedMilliseconds}ms,不过抛出异常");
                }
            }
        }

        public static void TestMethod03()
        {
            var t = new Thread(new ThreadStart(Run03));
            t.Start();
            Thread.Sleep(100);
            // 阻止动作，工作线程直接退出
            t.Abort();
            Console.Read();

            /*
             * 第1延迟执行：140ms,不过抛出异常
             */
        }

        private static void Run03()
        {
            for (var i = 1; i <= 3; i++)
            {
                var watch = new Stopwatch();

                try
                {
                    watch.Start();
                    Thread.Sleep(1000);
                    watch.Stop();

                    Console.WriteLine($"第{i}延迟执行：{watch.ElapsedMilliseconds}ms");
                }
                catch (ThreadAbortException e)
                {
                    Console.WriteLine($"第{i}延迟执行：{watch.ElapsedMilliseconds}ms,不过抛出异常");
                }
            }
        }


        public static void TestMethod04()
        {
            for (var i = 0; i < 10; i++)
            {
                var t = new Thread(Run04);
                t.Start();
            }
            Console.Read();

            /*
             * 当前数字：1
             * 当前数字：2
             * 当前数字：4
             * 当前数字：3
             * 当前数字：5
             * 当前数字：6
             * 当前数字：7
             * 当前数字：7
             * 当前数字：8
             * 当前数字：9
             */
        }

        private static readonly object Obj = new object();
        private static int _count = 0;

        private static void Run04()
        {
            // 不加锁的情况
            Thread.Sleep(10);
            Console.WriteLine($"当前数字：{++_count}");
        }

        public static void TestMethod05()
        {
            for (var i = 0; i < 10; i++)
            {
                var t = new Thread(Run05);
                t.Start();
            }
            Console.Read();

            /*
             * 当前数字：1
             * 当前数字：2
             * 当前数字：3
             * 当前数字：4
             * 当前数字：5
             * 当前数字：6
             * 当前数字：7
             * 当前数字：8
             * 当前数字：9
             * 当前数字：10
             */
        }

        private static void Run05()
        {
            // 加锁的情况
            Thread.Sleep(10);
            // 进入临界区
            Monitor.Enter(Obj);
            Console.WriteLine($"当前数字：{++_count}");
            // 退出临界区
            Monitor.Exit(Obj);
        }

    }
}
