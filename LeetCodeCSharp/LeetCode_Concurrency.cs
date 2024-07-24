namespace LeetCodeCSharp;

using System.Diagnostics;


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
        private AutoResetEvent _second = new(false);
        private AutoResetEvent _three  = new(false);



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