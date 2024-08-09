namespace LeetCodeCSharp;

//////////////////////////////////////////////////////// 贪婪问题 ////////////////////////////////////////////////////////
// 1. 这类问题很难抽象出一个统一的步骤,但是在思想上,都是能找到一个局部最优解,"这个操作是百利而无一害的"
// 2. 和动态规划的区别是,贪心问题不需要回溯,因为每一步都是最优解,不需要考虑之前的状态


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