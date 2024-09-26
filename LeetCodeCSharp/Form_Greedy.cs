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


