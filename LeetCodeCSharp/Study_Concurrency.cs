namespace LeetCodeCSharp;
///////////////////////////////////////////////////// 多线程 ////////////////////////////////////////////////////////////
/*
 * 单线程是一个人做一件事,多线程是多个人做不同的事
 * IDE的调试中,可以看出,代码的执行顺序是乱的,我们需要线程间的通信,来控制代码的执行顺序,这就是多线程问题的核心
 * 还有一个问题就是如何避免死锁,死锁是指两个或两个以上的进程在执行过程中,因争夺资源而造成的一种互相等待的现象 见 1226. 哲学家进餐
 *
 * 人生也是如此,我们往往不能一心多用,但是却又很多事情同事进行,如何让它们有条理,这是我们需要解决的问题
 *
 *
 */

using System.Threading;


/// Study Concurrency
public class Study_Concurrency
{
    // 什么是线程 ?
    // 线程是操作系统中能够独立运行的最小单位，也是程序中能够并发执行的一段指令序列
    // 线程是进程的一部分，一个进程可以包含多个线程，这些线程共享进程的资源
    // 进程有入口线程，也可以创建更多的线程

    // 为什么要多线程 ?
    // 批量重复任务希望同时进行（比如对于数组中的 每个元素都进行相同且耗时的操作）
    // 多个不同任务希望同时进行，互不干扰（比如有 多个后台线程需要做轮询等操作）

    // 什么是线程池 ?
    // 一组预先创建的线程，可以被重复使用来执行多个任务避免频繁地创建和销毁线程，从而减少了线程创建和销毁的开销，提高了系统的性能和效率
    // 异步编程默认使用线程池

    // 什么是线程安全 ?
    // 线程安全 多个线程访问共享资源时，对共享资源的访问不会导致数据不一致或不可预期的结果
    // 同步机制用于协调和控制多个线程之间执行顺序和互斥访问共享资源确保线程按照特定的顺序执行，避免竞态条件和数据不一致的问题
    // 原子操作:[在执行过程中不会被中断的操作]。不可分割，[要么完全执行，要么完全不执行]，
    // 没有中间状态在多线程环境下，原子操作能够保证数据的一致性和可靠性，避免出现 [竞态条件] 和 [数据竞争] 的问题

    // 线程不安全的两种操作

    [Test]
    public void NoSafe01()
    {
        const int total = 100_000;

        var count = 0;

        // 加索可以避免线程不安全
        // var lockObj = new object();

        var thread1 = new Thread(Increment);
        var thread2 = new Thread(Increment);

        thread1.Start(); // 使操作系统将当前实例的状态更改为“正在运行”。
        thread2.Start();

        thread1.Join(); //阻止调用线程，直到该实例表示的线程终止，同时继续执行标准的COM和 SendMessage 抽取
        thread2.Join();

        Console.WriteLine($"Count: {count}");
        // 为什么打印的值会小于 200000 ? 
        // 因为线程1和线程2同时对 count 进行操作，线程1和线程2同时读取 count 的值，然后同时对 count 进行加 1 操作
        // 即: 线程1: count = count + 1;  先获取 count 的值，然后加 1 ，再赋值给 count
        //    线程2: count = count + 1; ↑ 结果是同时获取count 的值 (相当于脏了一次)    ↑正常在这里插入,就可以正常执行了


        return;

        void Increment()
        {
            for (var i = 0 ; i < total ; i++)
            {
                // lock (lockObj)

                Interlocked.Increment(ref count); // 原子操作 atomic operations

                //count++;
            }
        }
    }

    [Test]
    public void NoSafe02()
    {
        var queue = new Queue<int>();
        // 加索可以避免线程不安全
        var lockObj = new object();

        var producer  = new Thread(Producer);
        var consumer1 = new Thread(Consumer);
        var consumer2 = new Thread(Consumer);

        producer.Start();
        consumer1.Start();
        consumer2.Start();

        producer.Join();
        Thread.Sleep(100); // Wait for consumers to finish

        consumer1.Interrupt(); //中断 WaitSleepJoin 线程状态中的线程
        consumer2.Interrupt();
        consumer1.Join();
        consumer2.Join();
        return;

        void Producer()
        {
            for (var i = 0 ; i < 20 ; i++)
            {
                Thread.Sleep(20);
                queue.Enqueue(i);
            }
        }

        void Consumer()
        {
            try
            {
                while (true)
                {
                    lock (lockObj)
                    {
                        if (queue.TryDequeue(out var res)) // 冲突在这里内部发生
                        {
                            Console.WriteLine(res);
                        }
                    }

                    Thread.Sleep(1);
                }
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine("Thread interrupted.");
            }
        }
    }

    [Test]
    public void ParallelMethod()
    {
        var inputs = Enumerable.Range(1, 20).ToArray();

        {
            // Sequential
            var sw_Sequential = Stopwatch.StartNew();
            var forOutputs    = new int[inputs.Length];
            for (var i = 0 ; i < inputs.Length ; i++)
            {
                forOutputs[i] = HeavyJob(inputs[i]);
            }

            sw_Sequential.Stop();
            Console.WriteLine($"Sequential time: {sw_Sequential.ElapsedMilliseconds}ms");
        }

        {
            // Parallel
            var sw_Parallel     = Stopwatch.StartNew();
            var parallelOutputs = new int[inputs.Length];
            Parallel.For(0, inputs.Length, i =>
            {
                parallelOutputs[i] = HeavyJob(inputs[i]);
            });

            sw_Parallel.Stop();
            Console.WriteLine($"Sequential time: {sw_Parallel.ElapsedMilliseconds}ms");
        }

        {
            // PLINQ
            var sw_PLINQ     = Stopwatch.StartNew();
            var plinqOutputs = inputs.AsParallel().Select(HeavyJob).AsOrdered().ToArray();

            sw_PLINQ.Stop();
            Console.WriteLine($"Sequential time: {sw_PLINQ.ElapsedMilliseconds}ms");
        }


        return;

        int HeavyJob(int input)
        {
            Thread.Sleep(300);
            return input;
        }
    }

    [Test]
    public void PLINQ()
    {
        var inputs    = Enumerable.Range(1, 20).ToArray();
        var semaphore = new Semaphore(3, 3);

        var sw           = Stopwatch.StartNew();
        var plinqOutputs = inputs.AsParallel().Select(HeavyJob).ToArray();
        sw.Stop();
        Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds}ms");

        return;

        int HeavyJob(int input)
        {
            semaphore.WaitOne();
            Thread.Sleep(300);
            semaphore.Release();
            return input;
        }
    }


    [Test]
    public void KeyStudy()
    {
        // 1. 创建一个线程 ,并传入一个 ThreadStart 委托(可以含参)
        var thread = new Thread(startObj => Console.WriteLine("Hello World" + startObj))
        {
            // 1.1 同时可以对线程进行一些配置
            IsBackground = true                  // 是否是后台线程
          , Name         = "HelloWorld"          // 线程名称
          , Priority     = ThreadPriority.Normal // 线程优先级
        };


        // 2. 启动线程
        thread.Start();

        // 3. 等待线程结束
        thread.Join();

        // 4. 中断线程的执行
        thread.Interrupt();

        // 会在相应线程中抛出 ThreadInterruptedException
        // 如果线程中包含一个 while (true) 循环，那么需 要保证包含等待方法，如IO操作，Thread.Sleep 等

        void Consumer()
        {
            try
            {
                while (true)
                {
                    //doSomething
                }
            }
            catch (ThreadInterruptedException) // 调用Interrupt时,中断线程的执行
            {
                Console.WriteLine("Thread interrupted.");
            }
        }

        {
            // 4.1. 强制终端线程(已经过时)只能老平台使用了
            // 使用 Abort 方法来强制终止线程可能导致一些严重的问题，包括资源泄漏和不可预测的行为
            // 较新版本的 .NET 中如果使用这个方法，会报PlatformNotSupportedException
            // 推荐使用 Thread.Interrupt 或 CancellationToken
            if (false)
            {
                thread.Abort();
            }
        }
    }


    public async Task Test01()
    {
        for (var i = 0 ; i < 100 ; i++)
        {
            Console.WriteLine(i);

            await Task.Yield();
        }

        Console.WriteLine("Hello World  Thread Final");
    }

    [Test]
    public async Task Test02()
    {
        var task = Test01();

        Console.WriteLine("Hello World Main1");
        Console.WriteLine("Hello World Main2");
        Console.WriteLine("Hello World Main2");

        await task;

        Console.WriteLine("Hello World Main");
    }
}


/// 1114. 按序打印
public class Solution_1114
{
    public class Foo
    {
        //第二种方式，使用自旋锁\

        //增强了对旋转式等待的支持
        private System.Threading.SpinWait _spinWait          = new SpinWait();
        private int                       _continueCondition = 1;


        public void First(Action printFirst)
        {
            printFirst();
            //向字段写入一个值。在需要它的系统中，插入一个内存屏障，防止处理器重新排序内存操作，如下所示：如果代码中的读或写出现在此方法之前，则处理器不能在此方法之后移动它。
            Thread.VolatileWrite(ref _continueCondition, 2); //写栅栏
        }

        public void Second(Action printSecond)
        {
            while (Thread.VolatileRead(ref _continueCondition) != 2)
            {
                _spinWait.SpinOnce();
            }

            printSecond();
            Thread.VolatileWrite(ref _continueCondition, 3); //写栅栏
        }

        public void Third(Action printThird)
        {
            while (Thread.VolatileRead(ref _continueCondition) != 3)
            {
                _spinWait.SpinOnce();
            }

            printThird();
            Thread.VolatileWrite(ref _continueCondition, 1); //写栅栏
        }
    }


    public class Foo2
    {
        // 第一种方式
        //表示一个线程同步事件，当发出信号时，该事件会在释放一个等待线程后自动重置。此类不能继承。
        private readonly AutoResetEvent _second = new(false);
        private readonly AutoResetEvent _three  = new(false);

        public void First(Action printFirst)
        {
            printFirst();
            _second.Set(); //通知第二个可以执行了
        }

        public void Second(Action printSecond)
        {
            _second.WaitOne(); //等待通知
            printSecond();
            _three.Set(); //通知第三个可以执行了
        }

        public void Third(Action printThird)
        {
            _three.WaitOne(); //等待通知
            printThird();
        }
    }
}


/// 1115. 交替打印 FooBar
public class Solution_1115
{
    /// 单线程做法
    public class FooBar01(int Count)
    {
        public int Counter1 = 0;
        public int Counter2 = 0;

        public void Foo(Action printFoo)
        {
            for (; Counter1 < Count ;)
            {
                if (Counter1 == Counter2)
                {
                    printFoo(); // printFoo() outputs "foo". Do not change or remove this line.
                    Counter1++;
                }

                Thread.Sleep(1);
            }
        }

        public void Bar(Action printBar)
        {
            for (; Counter2 < Count ;)
            {
                if (Counter1 > Counter2)
                {
                    printBar(); // printBar() outputs "bar". Do not change or remove this line.
                    Counter2++;
                }

                Thread.Sleep(1);
            }
        }
    }


    /// 线程同步事件 的 做法
    public class FooBar02(int Count)
    {
        private readonly AutoResetEvent Event01 = new(false);
        private readonly AutoResetEvent Event02 = new(true);

        public void Foo(Action printFoo)
        {
            for (var i = 0 ; i < Count ; i++)
            {
                Event02.WaitOne();
                printFoo(); // printFoo() outputs "foo". Do not change or remove this line.
                Event01.Set();
            }
        }

        public void Bar(Action printBar)
        {
            for (var i = 0 ; i < Count ; i++)
            {
                Event01.WaitOne();
                printBar(); // printBar() outputs "bar". Do not change or remove this line.
                Event02.Set();
            }
        }
    }



    //两个不同的线程将会共用一个 FooBar 实例:
    //线程 A 将会调用 foo() 方法
    //线程 B 将会调用 bar() 方法
    //请设计修改程序，以确保 "foobar" 被输出 n 次。


    [Test]
    public void METHOD()
    {
        var fooBar = new FooBar01(1000);

        var thread1 = new Thread(() =>
        {
            fooBar.Foo(() => Console.Write("Foo"));
        });

        var thread2 = new Thread(() =>
        {
            fooBar.Bar(() => Console.Write("Bar"));
        });
        thread1.Start(); // 使操作系统将当前实例的状态更改为“正在运行”。
        thread2.Start();
        thread1.Join(); //阻止调用线程，直到该实例表示的线程终止，同时继续执行标准的COM和 SendMessage 抽取
        thread2.Join();
    }
}


/// 1116. 打印零与奇偶数
public class Solution_1116
{
    public class ZeroEvenOdd(int Count)
    {
        private readonly AutoResetEvent Event_Zero = new(true);
        private readonly AutoResetEvent Event_Even = new(false);
        private readonly AutoResetEvent Event_Odd  = new(false);


        // 2024-08-07 19:58:22 出现了一个很耽误时间的错误
        // 因为Indexer一开始是从 0开始计数的,所以奇偶判断要反着来
        // 为什么要单独计数,而不是使用统一的Indexer? 我自己卡在这里
        // 后来看答案,如果使用统一的Indexer,极端情况,假设Count = 1 , 那么直接卡死在第三个函数上
        public void Zero(Action<int> printNumber)
        {
            for (var Indexer = 1 ; Indexer <= Count ; Indexer++)
            {
                Event_Zero.WaitOne();
                printNumber(0);

                if (Indexer % 2 == 0)
                {
                    Event_Even.Set();
                }
                else
                {
                    Event_Odd.Set();
                }
            }
        }

        public void Even(Action<int> printNumber)
        {
            for (var Indexer = 2 ; Indexer <= Count ; Indexer += 2)
            {
                Event_Even.WaitOne();


                printNumber(Indexer);

                Event_Zero.Set();
            }
        }

        public void Odd(Action<int> printNumber)
        {
            for (var Indexer = 1 ; Indexer <= Count ; Indexer += 2)
            {
                Event_Odd.WaitOne();

                printNumber(Indexer);

                Event_Zero.Set();
            }
        }
    }


    public class ZeroEvenOdd02(int Count)
    {
        private readonly AutoResetEvent Event_Zero = new(true);
        private readonly AutoResetEvent Event_Even = new(false);
        private readonly AutoResetEvent Event_Odd  = new(false);

        public void Zero(Action<int> printNumber)
        {
            for (var i = 0 ; i < Count ; i++)
            {
                Event_Zero.WaitOne();

                printNumber(0);
                if (i % 2 == 0)
                {
                    Event_Odd.Set();
                }
                else
                {
                    Event_Even.Set();
                }
            }
        }

        public void Even(Action<int> printNumber)
        {
            for (var i = 2 ; i <= Count ; i += 2)
            {
                Event_Even.WaitOne();
                printNumber(i);
                Event_Zero.Set();
            }
        }

        public void Odd(Action<int> printNumber)
        {
            for (var i = 1 ; i <= Count ; i += 2)
            {
                Event_Odd.WaitOne();
                printNumber(i);
                Event_Zero.Set();
            }
        }
    }



    [Test]
    public void METHOD()
    {
        var zeroEvenOdd = new ZeroEvenOdd(3);

        var thread1 = new Thread(() =>
        {
            zeroEvenOdd.Zero(Console.Write);
        });

        var thread2 = new Thread(() =>
        {
            zeroEvenOdd.Even(Console.Write);
        });

        var thread3 = new Thread(() =>
        {
            zeroEvenOdd.Odd(Console.Write);
        });

        thread1.Start();
        thread2.Start();
        thread3.Start();

        thread1.Join();
        thread2.Join();
        thread3.Join();
    }



    // 现有函数 printNumber 可以用一个整数参数调用，并输出该整数到控制台。
    // 例如，调用 printNumber(7) 将会输出 7 到控制台。
    // 给你类 ZeroEvenOdd 的一个实例，该类中有三个函数：zero、even 和 odd 。ZeroEvenOdd 的相同实例将会传递给三个不同线程：

    // 线程 A：调用 zero() ，只输出 0
    // 线程 B：调用 even() ，只输出偶数
    // 线程 C：调用 odd()  ，只输出奇数
    // 修改给出的类，以输出序列 "010203040506..." ，其中序列的长度必须为 2n 。

    // 实现 ZeroEvenOdd 类 :
    // ZeroEvenOdd(int n) 用数字 n 初始化对象，表示需要输出的数
    // void zero(printNumber) 调用 printNumber 以输出一个0
    // void even(printNumber) 调用 printNumber 以输出偶数
    // void odd (printNumber) 调用 printNumber 以输出奇数


    // 示例 1:
    // 
    // 输入: n = 2
    // 输出: "0 1 0 2"
    // 解释：三条线程异步执行，其中一个调用 zero()，另一个线程调用 even()，最后一个线程调用odd()。正确的输出为 "0102"。
    //
    // 示例 2:
    //
    // 输入: n = 5
    // 输出: "0(A) 1(C) 0(A) 2(B) 0(A) 3(C) 0(A) 4(B) 0(A) 5(C)"
    //
    // 提示:
    //
    // 1 <= n <= 1000
}


/// 1117. H2O 生成
public class Solution_1117
{
    public class H2O
    {
        private SemaphoreSlim Sp_H = new(2);
        private SemaphoreSlim Sp_O = new(0);


        public void Hydrogen(Action releaseHydrogen)
        {
            Sp_H.Wait();
            releaseHydrogen();
            if (Sp_H.CurrentCount == 0)
            {
                Sp_O.Release();
            }
        }

        public void Oxygen(Action releaseOxygen)
        {
            Sp_O.Wait();
            releaseOxygen();

            // 如果这么写
            // Sp_O.Wait();
            // Sp_O.Wait();
            // 如果只有两个线程那么,只有专门制作氧气的线程才能执行
            // 如果有更多线程,很可能会被其中两个线程个占用一个Sp_O,导致卡死

            Sp_H.Release(2);
        }
    }


    // 试了蛮久才发现这个问题，写成题解记录一下
    // 题目中提到的氢线程和氧线程是两 种 线程而不是两 个 线程，(应该是一个元素一个线程
    // 所以同类型的氢线程和氧线程可能会有多个。由于C#的SemaphoreSlim并没有Java的acquire(n)类似的一次性减去多个信号量的方法(C#中的Wait(int32)是等待n毫秒后超时)

    [Test]
    public void METHOD()
    {
        var h2O = new H2O();

        var thread1 = new Thread(() =>
        {
            for (var i = 0 ; i < 20 ; i++)
            {
                h2O.Hydrogen(() => Console.Write("H"));
            }
        });

        var thread2 = new Thread(() =>
        {
            for (var i = 0 ; i < 10 ; i++)
            {
                h2O.Oxygen(() => Console.Write("O"));
            }
        });

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();
    }


    // 现在有两种线程，氧 oxygen 和氢 hydrogen，你的目标是组织这两种线程来产生水分子。
    //
    // 存在一个屏障（barrier）使得每个线程必须等候直到一个完整水分子能够被产生出来。
    //
    // 氢和氧线程会被分别给予 releaseHydrogen 和 releaseOxygen 方法来允许它们突破屏障。
    //
    // 这些线程应该三三成组突破屏障并能立即组合产生一个水分子。
    //
    // 你必须保证产生一个水分子所需线程的结合必须发生在下一个水分子产生之前。
    //
    // 换句话说:
    //
    // 如果一个氧线程到达屏障时没有氢线程到达，它必须等候直到两个氢线程到达。
    // 如果一个氢线程到达屏障时没有其它线程到达，它必须等候直到一个氧线程和另一个氢线程到达。
    // 书写满足这些限制条件的氢、氧线程同步代码。
    //
    //
    //
    // 示例 1:
    //
    // 输入: water = "HOH"
    // 输出: "HHO"
    // 解释: "HOH" 和 "OHH" 依然都是有效解。
    // 示例 2:
    //
    // 输入: water = "OOHHHH"
    // 输出: "HHOHHO"
    // 解释: "HOHHHO", "OHHHHO", "HHOHOH", "HOHHOH", "OHHHOH", "HHOOHH", "HOHOHH" 和 "OHHOHH" 依然都是有效解。
    //
    //
    // 提示：
    //
    // 3 * n == water.length
    // 1 <= n <= 20
    // water[i] == 'O' or 'H'
    // 输入字符串 water 中的 'H' 总数将会是 2 * n 。
    // 输入字符串 water 中的 'O' 总数将会是 n 。
}


/// 1195. 交替打印字符串 等价于=> 1116. 打印零与奇偶数
public class Solution_1195
{
    //using System.Threading;


    public class FizzBuzz(int Count)
    {
        private readonly AutoResetEvent Event_Number   = new(false);
        private readonly AutoResetEvent Event_Buzz     = new(false);
        private readonly AutoResetEvent Event_Fizz     = new(false);
        private readonly AutoResetEvent Event_FizzBuzz = new(false);

        public void Fizz(Action printFizz)
        {
            for (var i = 3 ; i <= Count ; i += 3)
            {
                if (i % 5 == 0) continue;

                Event_Fizz.WaitOne();
                printFizz();
                Event_Number.Set();
            }
        }

        public void Buzz(Action printBuzz)
        {
            for (var i = 5 ; i <= Count ; i += 5)
            {
                if (i % 3 == 0) continue;

                Event_Buzz.WaitOne();
                printBuzz();
                Event_Number.Set();
            }
        }

        public void Fizzbuzz(Action printFizzBuzz)
        {
            for (var i = 15 ; i <= Count ; i += 15)
            {
                Event_FizzBuzz.WaitOne();
                printFizzBuzz();
                Event_Number.Set();
            }
        }


        public void Number(Action<int> printNumber)
        {
            for (var i = 1 ; i <= Count ; i++)
            {
                if (i % 3 == 0 && i % 5 == 0)
                {
                    Event_FizzBuzz.Set();
                    Event_Number.WaitOne();
                }
                else if (i % 3 == 0)
                {
                    Event_Fizz.Set();
                    Event_Number.WaitOne();
                }
                else if (i % 5 == 0)
                {
                    Event_Buzz.Set();
                    Event_Number.WaitOne();
                }
                else
                {
                    printNumber(i);
                }
            }
        }
    }


    // 编写一个可以从 1 到 n 输出代表这个数字的字符串的程序，但是：
    //
    // 如果这个数字可以被 3 整除，输出 "fizz"。
    // 如果这个数字可以被 5 整除，输出 "buzz"。
    // 如果这个数字可以同时被 3 和 5 整除，输出 "fizzbuzz"。
    // 例如，当 n = 15，输出： 1, 2, fizz, 4, buzz, fizz, 7, 8, fizz, buzz, 11, fizz, 13, 14, fizzbuzz。
    //
    // 假设有这么一个类：
    //
    // class FizzBuzz
    // {
    //     public FizzBuzz(int n) { ... }              // constructor
    //     public void fizz(printFizz)         { ... } // only output "fizz"
    //     public void buzz(printBuzz)         { ... } // only output "buzz"
    //     public void fizzbuzz(printFizzBuzz) { ... } // only output "fizzbuzz"
    //     public void number(printNumber)     { ... } // only output the numbers
    // }
    //
    // 请你实现一个有四个线程的多线程版  FizzBuzz， 同一个 FizzBuzz 实例会被如下四个线程使用：
    //
    // 线程A将调用 fizz() 来判断是否能被 3 整除，如果可以，则输出           fizz。
    // 线程B将调用 buzz() 来判断是否能被 5 整除，如果可以，则输出           buzz。
    // 线程C将调用 fizzbuzz() 来判断是否同时能被 3 和 5 整除，如果可以，则输出 fizzbuzz。
    // 线程D将调用 number() 来实现输出既不能被 3 整除也不能被 5 整除的数字。
    //
    //
    // 提示：
    //
    // 本题已经提供了打印字符串的相关方法，如 printFizz() 等，具体方法名请参考答题模板中的注释部分。
}


/// 1226. 哲学家进餐
public class Solution_1226
{
    // 前面说过，该题的本质是考察如何避免死锁
    // 而当5个哲学家都左手持有其左边的叉子 或 当5个哲学家都右手持有其右边的叉子时，会发生死锁
    // 故只需设计1个避免发生上述情况发生的策略即可
    // 即可以让一部分哲学家优先去获取其左边的叉子，再去获取其右边的叉子；再让剩余哲学家优先去获取其右边的叉子，再去获取其左边的叉子
    // 或者只让四个哲学家吃,其他的等待即可


    public class DiningPhilosophers
    {
        private readonly AutoResetEvent[] forks =
        [
            new(true)
          , new(true)
          , new(true)
          , new(true)
          , new(true)
        ];

        private readonly SemaphoreSlim semaphore = new(4);



        public void WantsToEat(int philosopher, Action pickLeftFork, Action pickRightFork, Action eat, Action putLeftFork, Action putRightFork)
        {
            var leftEvent  = forks[philosopher];
            var rightEvent = forks[(philosopher + 1) % 5];

            semaphore.Wait();

            leftEvent.WaitOne();
            pickLeftFork();

            rightEvent.WaitOne();
            pickRightFork();

            eat();

            putRightFork();
            rightEvent.Set();

            putLeftFork();
            leftEvent.Set();

            semaphore.Release();
        }



        //private final Semaphore[] chopsList =
        //{
        //    new Semaphore(1),
        //    new Semaphore(1),
        //    new Semaphore(1),
        //    new Semaphore(1),
        //    new Semaphore(1),
        //};
        //
        //private final Semaphore philosopherNums = new Semaphore(4);
        //
        //    
        //public void wantsToEat(int      philosopher,
        //                       Runnable pickLeftFork,
        //                       Runnable pickRightFork,
        //                       Runnable eat,
        //                       Runnable putLeftFork,
        //                       Runnable putRightFork) throws InterruptedException
        // {
        //     int leftIndex  = (philosopher + 1) % 5;
        //     int rightIndex = philosopher;
        //     philosopherNums.acquire();
        //     chopsList[leftIndex].acquire();
        //     chopsList[rightIndex].acquire();
        //     pickLeftFork.run();
        //     pickRightFork.run();
        //     eat.run();
        //     putLeftFork.run();
        //     putRightFork.run();
        //     chopsList[leftIndex].release();
        //     chopsList[rightIndex].release();
        //     philosopherNums.release();
        // }
        //
    }


    // 5 个沉默寡言的哲学家围坐在圆桌前，每人面前一盘意面。叉子放在哲学家之间的桌面上。（5 个哲学家，5 根叉子）
    // 所有的哲学家都只会在思考和进餐两种行为间交替。
    // 哲学家只有同时拿到左边和右边的叉子才能吃到面，而同一根叉子在同一时间只能被一个哲学家使用。
    // 每个哲学家吃完面后都需要把叉子放回桌面以供其他哲学家吃面。
    // 只要条件允许，哲学家可以拿起左边或者右边的叉子，但在没有同时拿到左右叉子时不能进食。
    // 假设面的数量没有限制，哲学家也能随便吃，不需要考虑吃不吃得下。
    // 设计一个进餐规则（并行算法）使得每个哲学家都不会挨饿；也就是说，在没有人知道别人什么时候想吃东西或思考的情况下，每个哲学家都可以在吃饭和思考之间一直交替下去。

    //哲学家从 0 到 4 按 顺时针 编号。请实现函数 void wantsToEat(philosopher, pickLeftFork, pickRightFork, eat, putLeftFork, putRightFork)：
    //
    //philosopher  哲学家的编号。
    //pickLeftFork 和 pickRightFork 表示拿起左边或右边的叉子。
    //eat          表示吃面。
    //putLeftFork  和 putRightFork 表示放下左边或右边的叉子。
    //由于哲学家不是在吃面就是在想着啥时候吃面，所以思考这个方法没有对应的回调。
    //给你 5 个线程，每个都代表一个哲学家，请你使用类的同一个对象来模拟这个过程。在最后一次调用结束之前，可能会为同一个哲学家多次调用该函数。
    //
    //示例：
    //
    //输入：n = 1
    //输出：[[4,2,1],[4,1,1],[0,1,1],[2,2,1],[2,1,1],[2,0,3],[2,1,2],[2,2,2],[4,0,3],[4,1,2],[0,2,1],[4,2,2],[3,2,1],[3,1,1],[0,0,3],[0,1,2],[0,2,2],[1,2,1],[1,1,1],[3,0,3],[3,1,2],[3,2,2],[1,0,3],[1,1,2],[1,2,2]]
    //解释:
    //n 表示每个哲学家需要进餐的次数。
    //输出数组描述了叉子的控制和进餐的调用，它的格式如下：
    //output[i] = [a, b, c] (3个整数)
    //- a 哲学家编号。
    //- b 指定叉子：{1 : 左边, 2 : 右边}.
    // - c 指定行为：{1 : 拿起, 2 : 放下, 3 : 吃面}。
    // 如 [4,2,1] 表示 4 号哲学家拿起了右边的叉子。
    //  
    //
    // 提示：
    //
    // 1 <= n <= 60
}