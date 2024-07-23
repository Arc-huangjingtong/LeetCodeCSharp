namespace LeetCodeCSharp;

public class Study_Concurrency
{
    public async Task Test01()
    {
        for (int i = 0 ; i < 100 ; i++)
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