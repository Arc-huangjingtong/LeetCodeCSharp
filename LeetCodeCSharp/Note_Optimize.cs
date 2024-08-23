//////////////////////////////////////////////////记录优化手段////////////////////////////////////////////////////////////


namespace LeetCodeCSharp;

public class Note_Optimize
{
    public class Solution_2549
    {
        //  - 优化方法: 编译期优化, 通过MethodImplOptions.AggressiveInlining特性,告诉编译器,这个方法是一个热点方法,需要进行内联优化
        //  - 优化效果: 通过内联优化,可以减少方法调用的开销,提高方法调用的效率
        //  - 注意事项: LeetCode中没有引用这个特性,但该特性属于System命名空间下,可以通过下述方式,写全名引用
        //  - 注意事项: 该特性只能用于方法,不能用于属性,字段,类等
        //  - 注意事项: 该特性只能用于public或internal方法,不能用于private方法
        //  - 注意事项: 如果只用这一个方法,且方法很简单,则不需要使用这个特性,因为方法很简单,编译器会自动进行内联优化
        public int DistinctIntegers1(int n) => DistinctIntegers3(n);

        public int DistinctIntegers2(int n) => DistinctIntegers4(n);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int DistinctIntegers3(int n) => Math.Max(1, n - 1);

        public int DistinctIntegers4(int n) => Math.Max(1, n - 1);


        [Test] [Repeat(1000)]
        public static void Test2()
        {
            var solution = new Solution_2549();

            for (var i = 0 ; i < 10000 ; i++)
            {
                var n      = 10;
                var result = solution.DistinctIntegers1(n);

                Assert.That(result, Is.EqualTo(9));
            }
        }

        [Test] [Repeat(1000)]
        public static void Test1()
        {
            var solution = new Solution_2549();

            for (var i = 0 ; i < 10000 ; i++)
            {
                var n      = 10;
                var result = solution.DistinctIntegers2(n);

                Assert.That(result, Is.EqualTo(9));
            }
        }
    }
}



// Node：
// 1.当参数范围很小的时候,可以考虑使用暴力枚举

// # Tips:
// - 小范围的连续整数,可以用数组代替哈希表
// - 奇偶表 :
// - 奇偶相同则为偶,奇偶不同则为奇(加减乘都是这个规律)(正负都可)