namespace LeetCodeCSharp;

public partial class UnitTest
{
    [Test]
    public void Test3() => Assert.Pass();


    public class Solution_1969
    {
        public int MinNonZeroProduct(int p)
        {
            const long mod = 1000000007;

            if (p == 1)
            {
                return 1;
            }

            var x = FastPow(2, p, mod) - 1;
            var y = (long)1 << (p - 1);
            return (int)(FastPow(x - 1, y - 1, mod) * x % mod);
        }

        public static long FastPow(long x, long n, long mod)
        {
            long res = 1;
            for (; n != 0 ; n >>= 1)
            {
                if ((n & 1) != 0)
                {
                    res = res * x % mod;
                }

                x = x * x % mod;
            }

            return res;
        }



        // 1969. 数组元素的最小非零乘积  中等
        // 给你一个正整数 p 。你有一个下标从 1 开始的数组 nums ，这个数组包含范围 [1, 2^p - 1] 内所有整数的二进制形式（两端都 包含）。你可以进行以下操作 任意 次：
        //
        // 从      nums 中选择两个元素 x 和  y。
        // 选择     x 中的一位与      y 对应位置的位交换。对应位置指的是两个整数 相同位置 的二进制位。
        // 比方说，如果 x = 1101 且  y = 0011 ，交换右边数起第 2 位后，我们得到 x = 1111 和 y = 0001 。
        //
        // 请你算出进行以上操作 任意次 以后，nums 能得到的 最小非零 乘积。将乘积对 109 + 7 取余 后返回。
        //
        // 注意：答案应为取余 之前 的最小值。
        //
        //
        //
        // 示例 1：
        //
        // 输入：p = 1
        // 输出：1
        // 解释：nums = [1] 。
        // 只有一个元素，所以乘积为该元素。
        // 示例 2：
        //
        // 输入：p = 2
        // 输出：6
        // 解释：nums = [01, 10, 11] 。
        // 所有交换要么使乘积变为 0 ，要么乘积与初始乘积相同。
        // 所以，数组乘积 1 * 2 * 3 = 6 已经是最小值。
        // 示例 3：
        //
        // 输入：p = 3
        // 输出：1512
        // 解释：nums = [   1, 010, 011, 100, 101, 110, 111]
        // - 第一次操作中，我们交换第二个和第五个元素最左边的数位
        // - 结果数组为  [   1, 110, 011, 100,   1, 110, 111] 
        // - 第二次操作中，我们交换第三个和第四个元素中间的数位。
        // - 结果数组为  [   1, 110,   1, 110,   1, 110, 111]
        // 数组乘积 1 * 6 * 1 * 6 * 1 * 6 * 7 = 1512 是最小乘积。
        //
        //
        // 提示：
        //
        // 1 <= p <= 60
    }



    public class Solution_2671
    {
        public class FrequencyTracker
        {
            private Dictionary<int, int> _dictCounter   = new();
            private Dictionary<int, int> _dictFrequency = new() { { 1, 0 } };

            public void Add(int number)
            {
                _dictCounter.TryAdd(number, 0);

                var preCount = _dictCounter[number];

                _dictFrequency.TryAdd(preCount,     0);
                _dictFrequency.TryAdd(preCount + 1, 0);

                _dictCounter[number]++; //计数器加1

                if (_dictFrequency[preCount] > 0)
                {
                    _dictFrequency[preCount]--; //频率计数器减1
                }

                _dictFrequency[preCount + 1]++; //频率计数器加1
            }

            public void DeleteOne(int number)
            {
                _dictCounter.TryAdd(number, 0);

                var preCount = _dictCounter[number];
                _dictFrequency.TryAdd(preCount, 0);

                if (preCount <= 0) return;

                _dictCounter[number]--;     //计数器减1
                _dictFrequency[preCount]--; //频率计数器减1

                _dictFrequency.TryAdd(preCount - 1, 0);
                _dictFrequency[preCount - 1]++; //频率计数器加1
            }

            public bool HasFrequency(int frequency) => _dictFrequency.ContainsKey(frequency) && _dictFrequency[frequency] > 0;
        }


        [Test]
        public void Test()
        {
            var obj = new FrequencyTracker();
            obj.Add(3);
            obj.Add(3);
            var param_3 = obj.HasFrequency(2);

            Assert.That(param_3, Is.True);
        }

        //Your FrequencyTracker object will be instantiated and called as such:
        //FrequencyTracker obj = new FrequencyTracker();
        //obj.Add(number);
        //obj.DeleteOne(number);
        //bool param_3 = obj.HasFrequency(frequency);
        //

        // 2671. 频率跟踪器  中等
        //
        // 请你设计并实现一个能够对其中的值进行跟踪的数据结构，并支持对频率相关查询进行应答。
        //
        // 实现 FrequencyTracker 类：
        //
        // FrequencyTracker()：使用一个空数组初始化 FrequencyTracker 对象。
        // void                          add(int          number)：添加一个            number 到数据结构中。
        // void                          deleteOne(int    number)：从数据结构中删除一个      number 。数据结构 可能不包含 number ，在这种情况下不删除任何内容。
        // bool                          hasFrequency(int frequency): 如果数据结构中存在出现 frequency 次的数字，则返回 true，否则返回 false。
        //
        //
        // 示例 1：
        //
        // 输入
        // ["FrequencyTracker", "add", "add", "hasFrequency"]
        // [[], [3], [3], [2]]
        // 输出
        // [null, null, null, true]
        //
        // 解释
        //     FrequencyTracker frequencyTracker = new FrequencyTracker();
        // frequencyTracker.add(3);          // 数据结构现在包含 [3]
        // frequencyTracker.add(3);          // 数据结构现在包含 [3, 3]
        // frequencyTracker.hasFrequency(2); // 返回 true ，因为 3 出现 2 次
        // 示例 2：
        //
        // 输入
        // ["FrequencyTracker", "add", "deleteOne", "hasFrequency"]
        // [[], [1], [1], [1]]
        // 输出
        // [null, null, null, false]
        //
        // 解释
        //     FrequencyTracker frequencyTracker = new FrequencyTracker();
        // frequencyTracker.add(1);          // 数据结构现在包含 [1]
        // frequencyTracker.deleteOne(1);    // 数据结构现在为空 []
        // frequencyTracker.hasFrequency(1); // 返回 false ，因为数据结构为空
        // 示例 3：
        //
        // 输入
        // ["FrequencyTracker", "hasFrequency", "add", "hasFrequency"]
        // [[], [2], [3], [1]]
        // 输出
        // [null, false, null, true]
        //
        // 解释
        // FrequencyTracker frequencyTracker = new FrequencyTracker();
        // frequencyTracker.hasFrequency(2); // 返回 false ，因为数据结构为空
        // frequencyTracker.add(3);          // 数据结构现在包含 [3]
        // frequencyTracker.hasFrequency(1); // 返回 true ，因为 3 出现 1 次
        //
        //
        // 提示：
        //
        // 1 <= number <= 10^5
        // 1 <= frequency <= 10^5
        // 最多调用 add、deleteOne 和 hasFrequency 共计 2 * 10^5 次
    }
}