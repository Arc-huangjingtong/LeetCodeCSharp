namespace LeetCodeCSharp;

//////////////////////////////////////////////////////// 贪婪问题 ////////////////////////////////////////////////////////
// 1. 这类问题很难抽象出一个统一的步骤,但是在思想上,都是能找到一个局部最优解,"这个操作是百利而无一害的"
// 2. 和动态规划的区别是,贪心问题不需要回溯,因为每一步都是最优解,不需要考虑之前的状态
// 3. 算法的本质就是贪心!!!!


/// 生成特殊数字的最少操作
[MemoryDiagnoser]
public class Solution_2844
{
    [TestCase("2245047", ExpectedResult = 2)]
    public static int MinimumOperations_ArcMethod(string num)
    {
        // 1. 自己在写的时候,总体上都是正确的,但是逻辑有点混乱(但是速度更快),在此记录,建议直接看灵神的做法
        int index1 = -1, index2 = -1;

        for (var i = num.Length - 1 ; i >= 0 ; i--)
        {
            if (num[i] == '0')
            {
                index1 = i;

                i--;


                for (; i >= 0 ; i--)
                {
                    if (num[i] is '5' or '0')
                    {
                        index2 = i;
                        break;
                    }
                }

                break;
            }
        }

        int index3 = -1, index4 = -1;

        for (var i = num.Length - 1 ; i >= 0 ; i--)
        {
            if (num[i] == '5')
            {
                index3 = i;
                i--;
                for (; i >= 0 ; i--)
                {
                    if (num[i] is '2' or '7')
                    {
                        index4 = i;
                        break;
                    }
                }

                break;
            }
        }

        var success1 = index1 != -1 && index2 != -1;
        var success2 = index3 != -1 && index4 != -1;

        if (!success1 && !success2)
        {
            if (index1 != -1)
            {
                return num.Length - 1;
            }

            return num.Length;
        }

        var result1 = success1 ? num.Length - index2 - 2 : int.MaxValue;
        var result2 = success2 ? num.Length - index4 - 2 : int.MaxValue;


        return Math.Min(result1, result2);
    }


    [TestCase("2245047", ExpectedResult = 2)]
    public static int MinimumOperations_LingShenMethod(string num)
    {
        var len = num.Length;

        bool found0 = false, found5 = false; //用一个flag标记是否找到了的状态,然后在循环中依次判断


        for (var i = len - 1 ; i >= 0 ; i--)
        {
            var c = num[i];
            if (found0 && (c == '0' || c == '5') ||
                found5 && (c == '2' || c == '7'))
            {
                return len - i - 2;
            }


            found0 |= c == '0';
            found5 |= c == '5';
        }

        return found0 ? len - 1 : len;
    }

    // 1. 不含前导0 
    // 2. 能被75整除的最小数 末尾特征: 00 50 25 75
    // 3. 如果都找不到,则全部删除,或者删到只剩一个 0 
    // 先找 0 - 5,0  再找 5 - 2,7


    // 给你一个下标从 0 开始的字符串 num ，表示一个非负整数。
    //
    // 在一次操作中，您可以选择 num 的任意一位数字并将其删除。请注意，如果你删除 num 中的所有数字，则 num 变为 0。
    //
    // 返回最少需要多少次操作可以使 num 变成特殊数字。
    //
    // 如果整数 x 能被 25 整除，则该整数 x 被认为是特殊数字。


    [Benchmark]
    public void Test_Arc() => MinimumOperations_ArcMethod("2245047");

    [Benchmark]
    public void Test_LingShen() => MinimumOperations_ArcMethod("2245047");
}


/// <summary> 3111. 覆盖所有点的最少矩形数目 </summary>
public class Solution_3111
{
    public int MinRectanglesToCoverPoints(int[][] points, int w)
    {
        // 选中所有横坐标,排序
        var sorted = points.Select(p => p[0]).OrderBy(x => x);


        var max = -1;
        var res = 0;

        foreach (var x in sorted)
        {
            if (x <= max) continue;

            // 贪心点 :　直接选最大宽度
            // 贪心点 :　如果不够，则直接从下一个点开始
            max = x + w;

            res++;
        }


        return res;
    }



    // 题目是说,用等宽(宽度小于w)的条状矩形,覆盖二维空间下 points中的所有点,最少需要多少个矩形
}


/// <summary> LCP 40. 心算挑战 </summary>
public class Solution_LCP40
{
    // 贪心点 : 先计算最大和,如何是偶数,则直接返回;
    // 贪心点 : 和的奇偶性,是会被一个子集的奇偶性所影响的,所以我们需要找到一个最小的奇数和一个最小的偶数,然后替换掉他们即可
    // 思考  : 思考两种方案的大小为什么不需要全部算出来然后比较?
    // cnt = 3 [10000,a(偶数),b(奇数),x(奇数),y(偶数)] (a > b > x > y) 且 a y为偶数,bx为奇数  => x - a > y - b
    // cnt = 3 [10000,8(偶数),7(奇数),5(奇数),4(偶数)] 如果能证明: b + x > a + y 就行了

    [TestCase(new[] { 1, 2, 8, 9 }, 3, ExpectedResult = 18)]
    [TestCase(new[] { 3, 3, 1 },    1, ExpectedResult = 0)]
    public int MaxmiumScore(int[] cards, int cnt)
    {
        // 1. 从大到小排序
        Array.Sort(cards, (a, b) => b - a);

        // 2. 选取最大的cnt个数
        var sum     = 0;
        var minOdd  = int.MaxValue; // 奇数
        var minEven = int.MaxValue; // 偶数

        for (var i = 0 ; i < cnt ; i++)
        {
            var card = cards[i];
            sum += card;

            if (card % 2 == 1)
            {
                minOdd = Math.Min(minOdd, card);
            }
            else
            {
                minEven = Math.Min(minEven, card);
            }
        }

        if (sum % 2 == 0) return sum;


        // 3. 找到最大奇数和最大偶数
        for (var i = cnt ; i < cards.Length ; i++)
        {
            var card = cards[i];

            if (minEven != int.MaxValue && card % 2 == 1)
            {
                sum += card;
                sum -= minEven;

                return sum;
            }

            if (minOdd != int.MaxValue && card % 2 == 0)
            {
                sum += card;
                sum -= minOdd;
                return sum;
            }
        }

        return 0;
    }



    // 求cards中最大的cnt个子集的偶数和 ,不满足则返回0

    // 偶数 + 偶数 = 偶数
    // 奇数 + 奇数 = 偶数
    // 偶数 + 奇数 = 奇数


    // 最大的前三个数的可能性
    // 1. 三个偶数 [√]
    // 2. 两个偶数,一个奇数 [找到最小的数字,然后换成奇偶不一样的数字即可]
    // 3. 一个偶数,两个奇数 [√]
    // 4. 三个奇数 [只需要把最小的奇数换成偶数即可]
}


//3129. 找出所有稳定的二进制数组 I
public class Solution_3129
{
    public int NumberOfStableArrays(int zero, int one, int limit)
    {
        const int MOD = 1000000007;

        var dp = new int[zero + 1][][];

        for (var i = 0 ; i <= zero ; i++)
        {
            dp[i] = new int[one + 1][];
            for (var j = 0 ; j <= one ; j++)
            {
                dp[i][j] = new int[2];
            }
        }

        for (var i = Math.Min(zero, limit) ; i >= 0 ; i--)
        {
            dp[i][0][0] = 1;
        }

        for (var j = Math.Min(one, limit) ; j >= 0 ; j--)
        {
            dp[0][j][1] = 1;
        }

        for (var i = 1 ; i <= zero ; i++)
        {
            for (var j = 1 ; j <= one ; j++)
            {
                dp[i][j][0] = (dp[i - 1][j][0] + dp[i - 1][j][1]) % MOD;
                if (i > limit)
                {
                    dp[i][j][0] = (dp[i][j][0] - dp[i - limit - 1][j][1] + MOD) % MOD;
                }

                dp[i][j][1] = (dp[i][j - 1][0] + dp[i][j - 1][1]) % MOD;
                if (j > limit)
                {
                    dp[i][j][1] = (dp[i][j][1] - dp[i][j - limit - 1][0] + MOD) % MOD;
                }
            }
        }

        return (dp[zero][one][0] + dp[zero][one][1]) % MOD;
    }



    // 1 <= zero, one, limit <= 200
    // 给你 3 个正整数 zero ，one 和 limit 
    //
    // 一个 二进制数组
    // arr 如果满足以下条件，那么我们称它是 稳定的 ：
    //
    // 0 在 arr 中出现次数 恰好 为 zero
    // 1 在 arr 中出现次数 恰好 为 one 
    // arr 中每个长度超过 limit 的 子数都 同时 包含 0 和 1 。
    // 请你返回     稳定 二进制数组的 总 数目。
    //
    // 由于答案可能很大，将它对 109 + 7 取余 后返回。
}


/// <summary> 2708. 一个小组的最大实力值 算术评级: 4 第 105 场双周赛Q3 1502 </summary>
public class Solution_2780
{
    [TestCase(new[] { 3, -1, -5, 2, 5, -9 }, ExpectedResult = 1350)]
    public long MaxStrength(int[] nums)
    {
        Array.Sort(nums);
        var len = nums.Length;
        var sum = 1L;

        var count = nums.Count(x => x < 0);

        if (nums[^1] == 0 && count <= 1)
        {
            return 0;
        }

        if (len == 1 && count == 1)
        {
            return nums[0];
        }

        if (count % 2 == 0)
        {
            for (var i = 0 ; i < len ; i++)
            {
                if (nums[i] == 0) continue;

                sum *= nums[i];
            }
        }
        else
        {
            for (var i = 0 ; i < count - 1 ; i++)
            {
                sum *= nums[i];
            }

            for (var i = count ; i < len ; i++)
            {
                if (nums[i] == 0) continue;

                sum *= nums[i];
            }
        }


        return sum;
    }


    /// <summary> 完美的答案 </summary>
    public long maxStrength(int[] nums)
    {
        long mn = nums[0];
        long mx = mn;
        for (int i = 1, len = nums.Length ; i < len ; i++)
        {
            long x   = nums[i];
            long tmp = mn;
            mn = Math.Min(Math.Min(mn, x), Math.Min(mn  * x, mx * x));
            mx = Math.Max(Math.Max(mx, x), Math.Max(tmp * x, mx * x));
        }
        //         
        //         从左到右遍历 nums，考虑 nums[i] 选或不选：
        //
        // 不选 nums[i]，那么当前的最大实力值就是前面 nums[0] 到 nums[i−1] 中所选元素的乘积。
        // 选 nums[i]，那么有如下情况可以得到最大实力值：
        // nums[i] 单独一个数作为最大实力值。
        // 如果 nums[i] 是正数，把 nums[i] 和前面所选元素值的最大乘积相乘。
        // 如果 nums[i] 是负数，由于负负得正，把 nums[i] 和前面所选元素值的最小乘积相乘。
        // 具体来说，我们需要在遍历 nums 的同时，维护所选元素的最小乘积 mn 和最大乘积 mx。
        //
        // 无论 x=nums[i] 是正是负还是零，mn 是以下四种情况的最小值：
        //
        // 不选，mn 不变。
        // x 单独一个数组成子序列。
        // mn⋅x。如果 x 是正数，这样可以得到最小乘积。
        // mx⋅x。如果 x 是负数，这样可以得到最小乘积。
        // 同理，mx 是以下四种情况的最大值：
        //
        // 不选，mx 不变。
        // x 单独一个数组成子序列。
        // mn⋅x。如果 x 是负数，这样可以得到最大乘积。
        // mx⋅x。如果 x 是正数，这样可以得到最大乘积。
        // 整理得
        //
        // mn
        // mx
        // ​
        //   
        // =min(mn,x,mn⋅x,mx⋅x)
        // =max(mx,x,mn⋅x,mx⋅x)
        // ​
        //  
        // 注意这两个式子要同时计算。
        //
        // 初始值：mn=mx=nums[0]。
        //
        // 答案：mx。


        return mx;
    }



    // 给你一个下标从 0 开始的整数数组 ，它表示一个班级中所有学生在一次考试中的成绩。
    // 老师想选出一部分同学组成一个 非空 小组，且这个小组的 实力值 最大，如果这个小组里的学生下标为 , , , ... , ，那么这个小组的实力值定义为 。numsi0i1i2iknums[i0] * nums[i1] * nums[i2] * ... * nums[ik​]
    //
    // 请你返回老师创建的小组能得到的最大实力值为多少。
    //
    //
    //
    // 示例 1：
    //
    // 输入：nums = [3,-1,-5,2,5,-9]
    // 输出：1350
    // 解释：一种构成最大实力值小组的方案是选择下标为 [0,2,3,4,5] 的学生。实力值为 3 * (-5) * 2 * 5 * (-9) = 1350 ，这是可以得到的最大实力值。
    // 示例 2：
    //
    // 输入：nums = [-4,-5,-4]
    // 输出：20
    // 解释：选择下标为 [0, 1] 的学生。得到的实力值为 20 。我们没法得到更大的实力值。
    //
    //
    // 提示：
    //
    // 1 <= nums.length <= 13
    // -9 <= nums[i] <= 9
}


/// <summary> 2576. 求出最多标记下标 算术评级: 6 第 334 场周赛Q3-1843 </summary>
public class Solution_2576
{
    public int MaxNumOfMarkedIndices(int[] nums)
    {
        Array.Sort(nums);
        int n = nums.Length;
        int l = 0, r = n / 2;
        while (l < r)
        {
            int m = l + r + 1 >> 1;
            if (Check(nums, m))
            {
                l = m;
            }
            else
            {
                r = m - 1;
            }
        }

        return l * 2;
    }

    public bool Check(int[] nums, int m)
    {
        int n = nums.Length;
        for (int i = 0 ; i < m ; i++)
        {
            if (nums[i] * 2 > nums[n - m + i])
            {
                return false;
            }
        }

        return true;
    }



    // 给你一个下标从 0 开始的整数数组 nums 。
    //
    // 一开始，所有下标都没有被标记。你可以执行以下操作任意次：
    //
    // 选择两个           互不相同且未标记 的下标 i 和 j ，满足 2 * nums[i] <= nums[j] ，标记下标 i 和 j 。
    // 请你执行上述操作任意次，返回 nums 中最多可以标记的下标数目。
    //
    //
    //
    // 示例 1：
    //
    // 输入：nums = [3,5,2,4] => [2 3 4 5]
    // 输出：2
    // 解释：第一次操作中，选择 i = 2 和 j = 1 ，操作可以执行的原因是 2 * nums[2] <= nums[1] ，标记下标 2 和 1 。
    // 没有其他更多可执行的操作，所以答案为 2 。
    // 示例 2：
    //
    // 输入：nums = [9,2,5,4] => [2 4 5 9] => [2 1 0 0]
    // 输出：4
    // 解释：第一次操作中，选择 i = 3 和 j = 0 ，操作可以执行的原因是 2 * nums[3] <= nums[0] ，标记下标 3 和 0 。
    // 第二次操作中，选择    i = 1 和 j = 2 ，操作可以执行的原因是 2 * nums[1] <= nums[2] ，标记下标 1 和 2 。
    // 没有其他更多可执行的操作，所以答案为 4 。
    // 示例 3：
    //
    // 输入：nums = [7,6,8]  => [6 7 8]
    // 输出：0
    // 解释：没有任何可以执行的操作，所以答案为 0 。
    //
    //
    // 提示：
    //
    // 1 <= nums.length <= 10^5
    // 1 <= nums[i] <= 10^9
}


/// <summary> 2207. 字符串中最多数目的子序列 算术评级: 4 第 74 场双周赛 Q2-1550 </summary>
public class Solution_2207
{
    [TestCase("aabb",                        "ab", ExpectedResult = 6)]
    [TestCase("abdcdbc",                     "ac", ExpectedResult = 4)]
    [TestCase("y",                           "yb", ExpectedResult = 1)]
    [TestCase("znzbjgvgfgwxccaajqqqnccqukv", "fu", ExpectedResult = 2)]
    public long MaximumSubsequenceCount(string text, string pattern)
    {
        Span<int> counter = stackalloc int[text.Length];

        var c1     = 0;
        var c2     = 0;
        var result = 0L;

        for (var i = text.Length - 1 ; i >= 0 ; i--)
        {
            if (text[i] == pattern[1])
            {
                c2++;
            }

            counter[i] = c2;
        }

        for (var i = 0 ; i < text.Length ; i++)
        {
            if (text[i] == pattern[0])
            {
                c1++;
                result += counter[i];
            }
        }

        if ((c1 == 0 && c2 == 1) || (c1 == 1 && c2 == 0) || (c1 == c2 && c2 == 1 & pattern[0] == pattern[1]))
        {
            return 1;
        }

        if (pattern[0] == pattern[1])
        {
            return result;
        }


        return result + Math.Max(c1, c2);
    }


    //答案标准做法:和自己做的差不多,相当于更加简洁了
    //贪心点在于,就是加最后,还是最前,最后面就是加之前pattern[0]的数量,最前面就是加之后pattern[1]的数量
    public long MaximumSubsequenceCount2(string text, string pattern)
    {
        long res = 0, cnt1 = 0, cnt2 = 0;

        foreach (var c in text)
        {
            if (c == pattern[1])
            {
                res += cnt1;
                cnt2++;
            }

            if (c == pattern[0])
            {
                cnt1++;
            }
        }

        return res + Math.Max(cnt1, cnt2);
    }



    // 给你一个下标从 0 开始的字符串 text 和另一个下标从 0 开始且长度为 2 的字符串 pattern ，两者都只包含小写英文字母。
    //
    // 你可以在 text 中任意位置插入 一个 字符，这个插入的字符必须是 pattern[0] 或者 pattern[1] 。注意，这个字符可以插入在 text 开头或者结尾的位置。
    //
    // 请你返回插入一个字符后，text 中最多包含多少个等于 pattern 的 子序列 。
    //
    // 子序列 指的是将一个字符串删除若干个字符后（也可以不删除），剩余字符保持原本顺序得到的字符串。
    //
    //
    //
    // 示例 1：
    //
    // 输入：text = "abdcdbc", pattern = "ac"
    // 输出：4
    // 解释： ABAB
    // 如果我们在 text[1] 和 text[2] 之间添加 pattern[0] = 'a' ，那么我们得到 "abadcdbc" 。那么 "ac" 作为子序列出现 4 次。
    // 其他得到 4 个 "ac" 子序列的方案还有 "aabdcdbc" 和 "abdacdbc" 。
    // 但是，"abdcadbc" ，"abdccdbc" 和 "abdcdbcc" 这些字符串虽然是可行的插入方案，但是只出现了 3 次 "ac" 子序列，所以不是最优解。
    // 可以证明插入一个字符后，无法得到超过 4 个 "ac" 子序列。
    // 示例 2：
    //
    // 输入：text = "aabb", pattern = "ab"
    // 输出：6
    // 解释：
    // 可以得到 6 个 "ab" 子序列的部分方案为 "aaabb" ，"aaabb" 和 "aabbb" 。
    //
    //
    // 提示：
    //
    // 1 <= text.length <= 105
    // pattern.length == 2
    // text 和 pattern 都只包含小写英文字母
}


/// <summary> 134. 加油站 算术评级: 6 中等 </summary>
public class Solution_134
{
    public int CanCompleteCircuit(int[] gas, int[] cost)
    {
        int n = gas.Length;
        int i = 0;
        while (i < n)
        {
            int sumOfGas = 0, sumOfCost = 0;
            int cnt      = 0;
            while (cnt < n)
            {
                int j = (i + cnt) % n;
                sumOfGas  += gas[j];
                sumOfCost += cost[j];
                if (sumOfCost > sumOfGas)
                {
                    break;
                }

                cnt++;
            }

            if (cnt == n)
            {
                return i;
            }

            i = i + cnt + 1;
        }

        return -1;
    }


    // 
    // 在一条环路上有 n 个加油站，其中第 i 个加油站有汽油 gas[i] 升。
    //
    // 你有一辆油箱容量无限的的汽车，从第 i 个加油站开往第 i+1 个加油站需要消耗汽油 cost[i] 升。你从其中的一个加油站出发，开始时油箱为空。
    //
    // 给定两个整数数组 gas 和 cost ，如果你可以按顺序绕环路行驶一周，则返回出发时加油站的编号，否则返回 -1 。如果存在解，则 保证 它是 唯一 的。
    //
    //
    //
    // 示例 1:
    //
    // 输入: gas = [1,2,3,4,5], cost = [3,4,5,1,2]
    // 输出: 3
    // 解释:
    // 从 3 号加油站(索引为 3 处)出发，可获得 4 升汽油。此时油箱有 = 0 + 4 = 4 升汽油
    //     开往 4 号加油站，此时油箱有 4 - 1 + 5 = 8 升汽油
    //     开往 0 号加油站，此时油箱有 8 - 2 + 1 = 7 升汽油
    //     开往 1 号加油站，此时油箱有 7 - 3 + 2 = 6 升汽油
    //     开往 2 号加油站，此时油箱有 6 - 4 + 3 = 5 升汽油
    //     开往 3 号加油站，你需要消耗 5 升汽油，正好足够你返回到 3 号加油站。
    // 因此，3 可为起始索引。
    // 示例 2:
    //
    // 输入: gas = [2,3,4], cost = [3,4,3]
    // 输出: -1
    // 解释:
    // 你不能从 0 号或 1 号加油站出发，因为没有足够的汽油可以让你行驶到下一个加油站。
    // 我们从 2 号加油站出发，可以获得 4 升汽油。 此时油箱有 = 0 + 4 = 4 升汽油
    //     开往 0 号加油站，此时油箱有 4 - 3 + 2 = 3 升汽油
    //     开往 1 号加油站，此时油箱有 3 - 3 + 3 = 3 升汽油
    //     你无法返回 2 号加油站，因为返程需要消耗 4 升汽油，但是你的油箱只有 3 升汽油。
    // 因此，无论怎样，你都不可能绕环路行驶一周。
    //
    //
    // 提示:
    //
    // gas.length == n
    //     cost.length == n
    // 1 <= n <= 10^5
    // 0 <= gas[i], cost[i] <= 10^4
}


public class Solution
{
    public int TwoEggDrop(int n)
    {
        return (int)Math.Ceiling((-1 + Math.Sqrt(1 + 8 * n)) / 2);
    }

    public int TwoEggDrop2(int n)
    {
        int[] f = new int[n + 1];
        Array.Fill(f, int.MaxValue / 2);
        f[0] = 0;
        for (int i = 1 ; i <= n ; i++)
        {
            for (int k = 1 ; k <= i ; k++)
            {
                f[i] = Math.Min(f[i], Math.Max(k - 1, f[i - k]) + 1);
            }
        }

        return f[n];
    }

    // n  1  2  3
    // r  0  2   


    // 1884. 鸡蛋掉落-两枚鸡蛋
    //     算术评级: 7
    // 同步题目状态
    //
    //     中等
    // 相关标签
    //     相关企业
    // 提示
    //     给你 2 枚相同 的鸡蛋，和一栋从第 1 层到第 n 层共有 n 层楼的建筑。
    //
    // 已知存在楼层 f ，满足 0 <= f <= n ，任何从 高于 f 的楼层落下的鸡蛋都 会碎 ，从 f 楼层或比它低 的楼层落下的鸡蛋都 不会碎 。
    //
    // 每次操作，你可以取一枚 没有碎 的鸡蛋并把它从任一楼层 x 扔下（满足 1 <= x <= n）。如果鸡蛋碎了，你就不能再次使用它。如果某枚鸡蛋扔下后没有摔碎，则可以在之后的操作中 重复使用 这枚鸡蛋。
    //
    // 请你计算并返回要确定 f 确切的值 的 最小操作次数 是多少？
    //
    //
    //
    // 示例 1：
    //
    // 输入：n = 2
    // 输出：2
    // 解释：我们可以将第一枚鸡蛋从 1 楼扔下，然后将第二枚从 2 楼扔下。
    // 如果第一枚鸡蛋碎了，可知        f = 0；
    // 如果第二枚鸡蛋碎了，但第一枚没碎，可知 f = 1；
    // 否则，当两个鸡蛋都没碎时，可知     f = 2。
    // 示例 2：
    //
    // 输入：n = 100
    // 输出：14
    // 解释：
    // 一种最优的策略是：
    // - 将第一枚鸡蛋从 9 楼扔下。如果碎了，那么                f 在 0 和 8 之间。将第二枚从 1 楼扔下，然后每扔一次上一层楼，在 8 次内找到      f 。总操作次数 = 1 + 8 = 9 。
    // - 如果第一枚鸡蛋没有碎，那么再把第一枚鸡蛋从 22 层扔下。如果碎了，那么 f 在 9 和 21 之间。将第二枚鸡蛋从 10 楼扔下，然后每扔一次上一层楼，在 12 次内找到 f 。总操作次数 = 2 + 12 = 14 。
    // - 如果第一枚鸡蛋没有再次碎掉，则按照类似的方法从 34, 45, 55, 64, 72, 79, 85, 90, 94, 97, 99 和 100 楼分别扔下第一枚鸡蛋。
    // 不管结果如何，最多需要扔 14 次来确定 f 。
    //
    //
    // 提示：
    //
    // 1 <= n <= 1000
}