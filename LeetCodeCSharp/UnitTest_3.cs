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



    public class Solution_1997
    {
        // [TestCase(new[] { 0, 0 },                         ExpectedResult = 2)]
        // [TestCase(new[] { 0, 0, 2 },                      ExpectedResult = 6)]
        // [TestCase(new[] { 0, 1, 2, 0 },                   ExpectedResult = 6)]
        // [TestCase(new[] { 0, 1, 2, 3, 4, 5, 6, 7 },       ExpectedResult = 14)]
        // [TestCase(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 7 },    ExpectedResult = 16)]
        // [TestCase(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, ExpectedResult = 18)]
        [TestCase(new[] { 0, 1, 2, 1, 4, 5, 6, 7, 8, 0 }, ExpectedResult = 18)]
        public int FirstDayBeenInAllRooms(int[] nextVisit)
        {
            const int  mod        = 1000000007;
            var        Len        = nextVisit.Length;
            var        currentLen = 0;
            var        currentDay = -1;
            var        currentPos = 0;
            Span<long> DP         = stackalloc long[Len];

            while (currentLen != Len)
            {
                currentDay++;

                if (DP[currentPos] == 0)
                {
                    currentLen++;
                }

                DP[currentPos]++;

                if (DP[currentPos] % 2 == 1)
                {
                    currentPos = nextVisit[currentPos];
                }
                else
                {
                    currentPos = (currentPos + 1) % Len;
                }
            }

            return currentDay;
        }


        [TestCase(new[] { 0, 0 },                         ExpectedResult = 2)]
        [TestCase(new[] { 0, 0, 2 },                      ExpectedResult = 6)]
        [TestCase(new[] { 0, 1, 2, 0 },                   ExpectedResult = 6)]
        [TestCase(new[] { 0, 1, 2, 3, 4, 5, 6, 7 },       ExpectedResult = 14)]
        [TestCase(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 7 },    ExpectedResult = 16)]
        [TestCase(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, ExpectedResult = 18)]
        [TestCase(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 0 }, ExpectedResult = 18)]
        public int FirstDayBeenInAllRooms2(int[] nextVisit)
        {
            const int mod = 1000000007;
            var       len = nextVisit.Length;
            Span<int> dp  = stackalloc int[len];

            dp[0] = 2; //初始化原地待一天 + 访问下一个房间一天
            for (var i = 1 ; i < len ; i++)
            {
                var to = nextVisit[i];
                dp[i] = 2 + dp[i - 1];
                if (to != 0)
                {
                    dp[i] = (dp[i] - dp[to - 1] + mod) % mod; //避免负数
                }

                dp[i] = (dp[i] + dp[i - 1]) % mod;
            }

            return dp[len - 2]; //题目保证n >= 2
        }


        // n == nextVisit.length
        // 2 <= n <= 10^5
        // 0 <= nextVisit[i] <= i

        // 1997. 访问完所有房间的第一天   中等
        //
        // 你需要访问 n 个房间，房间从 0 到 n - 1 编号。同时，每一天都有一个日期编号，从 0 开始，依天数递增。你每天都会访问一个房间。
        //
        // 最开始的第 0 天，你访问 0 号房间。给你一个长度为 n 且 下标从 0 开始 的数组 nextVisit 。在接下来的几天中，你访问房间的 次序 将根据下面的 规则 决定：
        //
        // 假设某一天，你访问   i 号房间。
        // 如果算上本次访问，访问 i 号房间的次数为 奇数 ，那么 第二天 需要访问             nextVisit[i] 所指定的房间，其中 0 <= nextVisit[i] <= i 。
        // 如果算上本次访问，访问 i 号房间的次数为 偶数 ，那么 第二天 需要访问 (i + 1) mod n 号房间。
        // 请返回你访问完所有房间的第一天的日期编号。题目数据保证总是存在这样的一天。由于答案可能很大，返回对 10^9 + 7 取余后的结果。
        //
        //
        // 示例 1：
        //
        // 输入：nextVisit = [0,0]
        // 输出：2
        // 解释：
        // - 第 0 天，你访问房间 0 。访问 0 号房间的总次数为 1 ，次数为奇数。
        // 下一天你需要访问房间的编号是 nextVisit[0] = 0
        //     - 第 1 天，你访问房间 0 。访问 0 号房间的总次数为 2 ，次数为偶数。
        // 下一天你需要访问房间的编号是 (0 + 1) mod 2 = 1
        // - 第 2 天，你访问房间 1 。这是你第一次完成访问所有房间的那天。
        // 示例 2：
        //
        // 输入：nextVisit = [0,0,2]
        // 输出：6
        // 解释：
        // 你每天访问房间的次序是 [0,0,1,0,0,1,2,...] 。
        // 第 6 天是你访问完所有房间的第一天。
        // 示例 3：
        //
        // 输入：nextVisit = [0,1,2,0]
        // 输出：6
        // 解释：
        // 你每天访问房间的次序是 [0,0,1,1,2,2,3,...] 。
        // 第 6 天是你访问完所有房间的第一天。
    }
<<<<<<< Updated upstream
}

public class Solution_2028 
{
    public int[] MissingRolls(int[] rolls, int mean, int n) 
    {
        var rollsResSum = mean * (rolls.Length + n) - rolls.Sum();
        
        var res = new int[n];
        
        if (rollsResSum < n || rollsResSum > 6 * n)
        {
            return [];
        }
        
        var avg = rollsResSum / n;
        var mod = rollsResSum % n;
        
        Array.Fill(res, avg); 
        
        for (var i = 0; i < mod; i++)
        {
            res[i]++;
        }
        
        return res;
=======

    
    
    public class Solution_2009
    {
        [TestCase(new[] { 4, 2, 5, 3 },    ExpectedResult = 0)]
        [TestCase(new[] { 1, 2, 3, 5, 6 }, ExpectedResult = 1)]
        public int MinOperations(int[] nums)
        {
            Array.Sort(nums);

            var len         = nums.Length;
            var operatorNum = 0;
            var left        = 0;
            var right       = len - 1;
            
            while (nums[right] - nums[left] > len - 1) // need : MAX - MIN = len - 1
            {
                operatorNum++;
                left++;
                right--;
            }


            return operatorNum;
        }


        // 2009. 使数组连续的最少操作数
        //     困难
        // 相关标签
        //     相关企业
        // 提示
        //     给你一个整数数组 nums 。每一次操作中，你可以将 nums 中 任意 一个元素替换成 任意 整数。
        //
        // 如果 nums 满足以下条件，那么它是 连续的 ：
        //
        // nums                      中所有元素都是 互不相同 的。
        // nums                      中 最大         元素与 最小 元素的差等于 nums.length - 1 。
        // 比方说，nums = [4, 2, 5, 3] 是 连续的 ，但是      nums = [1, 2, 3, 5, 6] 不是连续的 。
        //
        // 请你返回使 nums 连续 的 最少 操作次数。
        //
        //
        //
        // 示例 1：
        //
        // 输入：nums = [4,2,5,3]
        // 输出：0
        // 解释：nums 已经是连续的了。
        // 示例 2：
        //
        // 输入：nums = [1,2,3,5,6]
        // 输出：1
        // 解释：一个可能的解是将最后一个元素变为 4 。
        // 结果数组为 [1,2,3,5,4] ，是连续数组。
        // 示例 3：
        //
        // 输入：nums = [1,10,100,1000]
        // 输出：3
        // 解释：一个可能的解是：
        // - 将第二个元素变为 2 。
        // - 将第三个元素变为 3 。
        // - 将第四个元素变为 4 。
        // 结果数组为 [1,2,3,4] ，是连续数组。
        //
        //
        // 提示：
        //
        // 1 <= nums.length <= 105
        // 1 <= nums[i] <= 109
>>>>>>> Stashed changes
    }
}