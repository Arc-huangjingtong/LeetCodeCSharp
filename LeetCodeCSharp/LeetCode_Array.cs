namespace LeetCodeCSharp;
///////////////////////////////////////////////////////数组相关题型///////////////////////////////////////////////////////


/// <summary> 2007. 从双倍数组中还原原数组 </summary>
public class Solution_2007
{
    /// 超时了,因为使用了Array.FindIndex 然后改成了先使用Array.BinarySearch,但是还是差点超时!
    [TestCase(new[] { 4, 6, 14, 8, 4, 6, 12, 3, 7, 8 }, ExpectedResult = new[] { 3, 4, 4, 6, 7 })]
    public int[] FindOriginalArray(int[] changed)
    {
        var length = changed.Length;


        if (length % 2 == 1)
        {
            return [];
        }

        Array.Sort(changed);

        var startIndex = changed.TakeWhile(t => t == 0).Count();

        var result = new List<int>();

        //给result增加startIndex/2个0
        for (var i = 0 ; i < startIndex / 2 ; i++)
        {
            result.Add(0);
        }


        //处理前缀的0


        for (var i = startIndex ; i < changed.Length ; i++)
        {
            if (changed[i] == -1)
            {
                continue;
            }

            var index = Array.BinarySearch(changed, changed[i] * 2);

            if (index < 0)
            {
                index = Array.FindIndex(changed, i, t => t == changed[i] * 2);
            }


            if (index < 0)
            {
                return [];
            }

            changed[index] = -1;
            result.Add(changed[i]);
        }

        return result.ToArray();
    }

    /// 也不是很理想,把 SortedDictionary 换成了 Dictionary 或许会好一点
    [TestCase(new[] { 4, 6, 14, 8, 4, 6, 12, 3, 7, 8 }, ExpectedResult = new[] { 3, 4, 4, 6, 7 })]
    public int[] FindOriginalArray2(int[] changed)
    {
        if (changed.Length % 2 == 1)
        {
            return [];
        }

        var sortDic = new SortedDictionary<int, int>();
        var result  = new List<int>();

        foreach (var num in changed)
        {
            if (!sortDic.TryAdd(num, 1))
            {
                sortDic[num]++;
            }
        }

        var zeroCount = sortDic.GetValueOrDefault(0, 0);
        if (zeroCount % 2 == 1)
        {
            return [];
        }

        for (var i = 0 ; i < zeroCount / 2 ; i++)
        {
            result.Add(0);
        }


        var DicKeys = sortDic.Keys.ToArray();

        foreach (var key in DicKeys)
        {
            if (sortDic[key] == 0)
            {
                continue;
            }

            if (sortDic.ContainsKey(key * 2))
            {
                sortDic[key * 2] -= sortDic[key];
            }
            else
            {
                return [];
            }


            if (sortDic[key * 2] < 0)
            {
                return [];
            }

            for (var i = 0 ; i < sortDic[key] ; i++)
            {
                result.Add(key);
            }
        }

        return result.ToArray();
    }


    //[1,3,4,2,6,8] => [1,3,4]
    // 排序后: [1,2,3,4,6,8]

    /// 一个整数数组 original 可以转变成一个 双倍 数组 changed ，转变方式为将 original 中每个元素 值乘以 2 加入数组中，然后将所有元素 随机打乱。
    /// 给你一个数组 changed ，如果 change 是 双倍 数组，那么请你返回 original数组，否则请返回空数组。original 的元素可以以 任意 顺序返回。
    /// 1 <= changed.length <= 10^5
    /// 0 <= changed[i] <= 10^5
}


/// <summary> 39. 组合总和 </summary>
public class Solution_39
{
    public IList<IList<int>> CombinationSum(int[] candidates, int target)
    {
        var result = new List<IList<int>>();


        return result;
    }

    //[TestCase(new[] {2, 3, 6, 7}, 7, ExpectedResult = new[] {new[] {2, 2, 3}, new[] {7}})]

    //7, 6+1
}


public class Solution_377
{
    // 377. 组合总和 Ⅳ
    public int CombinationSum4(int[] nums, int target)
    {
        Span<int> f = stackalloc int[target + 1];
        f[0] = 1;
        for (var i = 1 ; i <= target ; ++i)
        {
            foreach (var x in nums)
            {
                if (i >= x)
                {
                    f[i] += f[i - x];
                }
            }
        }

        return f[target];
    }
}


/// <summary> 2639.查询网格图中每一列的宽度 </summary>
public class Solution_2639
{
    public int[] FindColumnWidth(int[][] grid)
    {
        var n      = grid.Length;
        var m      = grid[0].Length;
        var result = new int[m];
        for (var i = 0 ; i < m ; i++)
        {
            for (var j = 0 ; j < n ; j++)
            {
                var num   = grid[j][i];
                var width = 0;
                if (num < 0)
                {
                    width++;
                }

                if (num == 0)
                {
                    width = 1;
                }
                else
                {
                    while (num != 0)
                    {
                        num /= 10;
                        width++;
                    }
                }


                result[i] = Math.Max(result[i], width);
            }
        }

        return result;
    }

    // [[-15,1,  3],
    //  [15 ,7, 12],
    //  [5  ,6, -2]]
    // 输出：[3,1,2]
}


/// [小根堆] 难点：主要需要考虑多种情况，很麻烦
/// <summary> 2462. 雇佣 K 位工人的总代价 </summary>
public class Solution_2462
{
    [TestCase(new[] { 17, 12, 10, 2, 7, 2, 11, 20, 8 }, 3, 4, ExpectedResult = 11)]
    [TestCase(new[] { 1, 2, 4, 1 },                     3, 3, ExpectedResult = 4)]
    public long TotalCost(int[] costs, int k, int candidates)
    {
        var head = new PriorityQueue<int, int>();
        var last = new PriorityQueue<int, int>();
        int headIndex, lastIndex;

        for (headIndex = 0 ; headIndex < candidates ; headIndex++)
        {
            head.Enqueue(costs[headIndex], costs[headIndex]);
        }

        for (lastIndex = costs.Length - 1 ; lastIndex >= costs.Length - candidates ; lastIndex--)
        {
            last.Enqueue(costs[lastIndex], costs[lastIndex]);
        }

        var result = 0;


        for (var i = 0 ; i < k ; i++)
        {
            var headMin = head.Peek();
            var lastMin = last.Peek();

            result += Math.Min(headMin, lastMin);


            if (headMin <= lastMin)
            {
                head.Dequeue();
                head.Enqueue(costs[headIndex], costs[headIndex]);
                headIndex++;
            }
            else
            {
                last.Dequeue();
                last.Enqueue(costs[lastIndex], costs[lastIndex]);
                lastIndex--;
            }
        }

        return result;
    }


    // 根据题目描述，我们需要雇佣代价最小，并且在代价相等时下标最小的工人，
    // 因此我们可以使用小根堆维护所有当前可以雇佣的工人，小根堆的每个元素是一个二元组 (cost,id)，分别表示工人的代价和下标。
    // 初始时，我们需要将数组 costs 中的前 candidates 和后 candidates 个工人放入小根堆中。
    // 需要注意的是，如果 candidates×2≥n（其中 n 是数组 costs 的长度），前 candidates 和后 candidates 个工人存在重复，
    // 【等价于将所有工人都放入小根堆中】这个是最重要的思想，只需要一个小根堆就可以了，无论题干是优先选择前面的工人还是后面的工人，都是一样的。

    // 随后我们用 left 和 right 分别记录前面和后面可以选择的工人编号的边界，它们的初始值分别为 candidates−1 和 n−candidatesn 这样一来，
    // 我们就可以进行 k 次操作，每次操作从小根堆中取出当前最优的工人。
    // 如果它的下标 id≤left，那么它属于前面的工人，我们需要将 left 增加 1，并将新的 (costs[left],left) 放入小根堆中。
    // 同理，如果 id≥right 那么需要将 right 减少 1，并将新的 (costs[right],right) 放入小根堆中。
    //
    // 如果 left+1≥right，说明我们已经可以选择任意的工人，此时再向小根堆中添加工人会导致重复，因此只有在 left+1<right 时，才会移动 left 或 right 并添加工人。


    public long TotalCost2(int[] costs, int k, int candidates)
    {
        var n  = costs.Length;
        var pq = new PriorityQueue<(int, int), long>();

        int left = candidates - 1, right = n - candidates;
        if (left + 1 < right)
        {
            for (var i = 0 ; i <= left ; ++i)
            {
                pq.Enqueue((costs[i], i), (long)costs[i] * n + i);
            }

            for (var i = right ; i < n ; ++i)
            {
                pq.Enqueue((costs[i], i), (long)costs[i] * n + i);
            }
        }
        else
        {
            for (var i = 0 ; i < n ; ++i)
            {
                pq.Enqueue((costs[i], i), (long)costs[i] * n + i);
            }
        }

        var ans = 0L;
        for (var i = 0 ; i < k ; ++i)
        {
            var (cost, id) = pq.Dequeue();

            ans += cost;

            if (left + 1 < right)
            {
                if (id <= left)
                {
                    ++left;
                    pq.Enqueue((costs[left], left), (long)costs[left] * n + left);
                }
                else
                {
                    --right;
                    pq.Enqueue((costs[right], right), (long)costs[right] * n + right);
                }
            }
        }

        return ans;
    }
}


/// [双指针] 难点：维护赢得的次数和赢得位置两个变量即可
/// <summary> 1535. 找出数组游戏的赢家 </summary>
public class Solution_1535
{
    [TestCase(new[] { 2, 1, 3, 5, 4, 6, 7 }, 2, ExpectedResult = 5)]
    public int GetWinner(int[] arr, int k)
    {
        var winner   = arr[0];
        var winCount = 0;

        for (var i = 1 ; i < arr.Length ; i++)
        {
            if (arr[i] > winner)
            {
                winner   = arr[i];
                winCount = 1;
            }
            else
            {
                winCount++;
            }

            if (winCount == k)
            {
                return winner;
            }
        }

        return winner;
    }
}


/// <summary> 22. 括号生成 </summary>
public class Solution_22
{
    [Test]
    public void Test()
    {
        var result = GenerateParenthesis(3);
        foreach (var s in result)
        {
            Console.WriteLine(s);
        }

        var a = -2147483648;
    }

    public void Backtrack(List<string> ans, StringBuilder cur, int balance, int max)
    {
        if (cur.Length == max * 2)
        {
            if (balance == 0)
            {
                ans.Add(cur.ToString());
            }

            return;
        }

        if (balance < max)
        {
            cur.Append('(');
            Backtrack(ans, cur, balance + 1, max);

            cur.Remove(cur.Length - 1, 1);
        }

        if (balance > 0)
        {
            cur.Append(')');
            Backtrack(ans, cur, balance - 1, max);

            cur.Remove(cur.Length - 1, 1);
        }
    }


    public List<string> GenerateParenthesis(int n)
    {
        List<string> ans = [];
        Backtrack(ans, new(), 0, n);
        return ans;
    }

    public void Backtrack(List<string> ans, StringBuilder cur, int open, int close, int max)
    {
        if (cur.Length == max * 2)
        {
            ans.Add(cur.ToString());
            return;
        }

        if (open < max)
        {
            cur.Append('(');
            Backtrack(ans, cur, open + 1, close, max);

            cur.Remove(cur.Length - 1, 1);
        }

        if (close < open)
        {
            cur.Append(')');
            Backtrack(ans, cur, open, close + 1, max);

            cur.Remove(cur.Length - 1, 1);
        }
    }
}


public class Solution_29
{
    public int Divide(int dividend, int divisor)
    {
        var ans = (long)dividend / (long)divisor;

        return (int)Math.Clamp(ans, int.MinValue, int.MaxValue);
    }
}


public class Solution_2965
{
    public int[] FindMissingAndRepeatedValues(int[][] grid)
    {
        var sum    = 0;
        var set    = new HashSet<int>();
        var repeat = 0;

        foreach (var row in grid)
        {
            foreach (var num in row)
            {
                sum += num;
                if (!set.Add(num))
                {
                    repeat = num;
                }
            }
        }

        var lose = (1 + grid.Length * grid.Length) * (grid.Length * grid.Length) / 2 - (sum - repeat);

        return [repeat, lose];
    }
}


/// <summary> 2938.区分黑球和白球 </summary>
public class Solution_2938
{
    [Repeat(10000)]
    [TestCase("0111",               ExpectedResult = 0L)]
    [TestCase("010",                ExpectedResult = 1L)]
    [TestCase("100",                ExpectedResult = 2L)]
    [TestCase("010000110010101010", ExpectedResult = 32L)]
    public long MinimumSteps(string s)
    {
        Span<bool> bits = stackalloc bool[s.Length];

        for (var i = 0 ; i < s.Length ; i++)
        {
            bits[i] = s[i] == '1';
        }


        var rightIndex    = s.Length - 1;
        var operatorCount = 0L;
        var currentIndex  = 0;
        var leftCount     = 0;

        while (rightIndex >= 0)
        {
            if (!bits[rightIndex])
            {
                rightIndex--;
                currentIndex++;
                continue;
            }

            bits[rightIndex] =  false;
            operatorCount    += currentIndex - leftCount;
            leftCount++;
        }


        return operatorCount;
    }

    [Repeat(10000)]
    [TestCase("0111",               ExpectedResult = 0L)]
    [TestCase("010",                ExpectedResult = 1L)]
    [TestCase("100",                ExpectedResult = 2L)]
    [TestCase("010000110010101010", ExpectedResult = 32L)]
    public long MinimumSteps2(string str)
    {
        var ans = 0L;
        var sum = 0;
        foreach (var t in str)
        {
            if (t == '1')
            {
                sum++;
            }
            else
            {
                ans += sum;
            }
        }

        return ans;
    }

    // 1→     0←
    //010000110010101010
    //010000110010101001 =>  1  => 2 - 1 = 1
    //010000110010100011 =>  2  => 4 - 2 = 2
    //010000110010000111 =>  3  => 6 - 3 = 3 
    //010000110000001111 =>  4
    //010000100000011111 =>  6
    //010000000000111111 =>  6
    //000000000001111111 => 10
}


public class Solution_2786
{
    [TestCase(new[] { 9, 58, 17, 54, 91, 90, 32, 6, 13, 67, 24, 80, 8, 56, 29, 66, 85, 38, 45, 13, 20, 73, 16, 98, 28, 56, 23, 2, 47, 85, 11, 97, 72, 2, 28, 52, 33 }, 90, ExpectedResult = 886)]
    public long MaxScore(int[] nums, int x)
    {
        var        len = nums.Length;
        Span<long> DP  = stackalloc long[len];

        DP.Fill(long.MinValue);

        DP[0] = nums[0];

        for (var i = 1 ; i < len ; i++)
        {
            for (var j = 0 ; j < i ; j++)
            {
                if (nums[i] % 2 != nums[j] % 2) //奇偶性不同
                {
                    DP[i] = Math.Max(DP[i], DP[j] + nums[i] - x);
                }
                else
                {
                    DP[i] = Math.Max(DP[i], DP[j] + nums[i]);
                }
            }
        }

        var max = 0L;

        foreach (var d in DP)
        {
            max = Math.Max(max, d);
        }

        return max;
    }

    [TestCase(new[] { 9, 58, 17, 54, 91, 90, 32, 6, 13, 67, 24, 80, 8, 56, 29, 66, 85, 38, 45, 13, 20, 73, 16, 98, 28, 56, 23, 2, 47, 85, 11, 97, 72, 2, 28, 52, 33 }, 90, ExpectedResult = 886)]
    public long MaxScore2(int[] nums, int x)
    {
        long[] dp = [int.MinValue, int.MinValue]; //妙手:最终的结果只与奇数和偶数有关,所以只需要两个变量即可
        dp[nums[0] % 2] = nums[0];
        for (var i = 1 ; i < nums.Length ; i++)
        {
            var parity = nums[i] % 2;
            var cur    = Math.Max(dp[parity] + nums[i], dp[1 - parity] - x + nums[i]);
            dp[parity] = Math.Max(dp[parity], cur);
        }

        return Math.Max(dp[0], dp[1]);
    }
}


// 2786. 访问数组中的位置使分数最大 中等
// 给你一个下标从 0 开始的整数数组 nums 和一个正整数 x 。
//
// 你 一开始 在数组的位置 0 处，你可以按照下述规则访问数组中的其他位置：
//
// 如果你当前在位置  i ，那么你可以移动到满足 i < j 的 任意 位置 j 。
// 对于你访问的位置  i ，你可以获得分数 nums[i] 。
// 如果你从位置    i 移动到位置 j 且 nums[i] 和 nums[j] 的 奇偶性 不同，那么你将失去分数 x 。
// 请你返回你能得到的 最大 得分之和。
//
// 注意 ，你一开始的分数为 nums[0] 。
//
//  
//
// 示例 1：
//
// 输入：nums = [2,3,6,1,9,2], x = 5
// 输出：13
// 解释：我们可以按顺序访问数组中的位置：0 -> 2 -> 3 -> 4 。
// 对应位置的值为 2 ，6 ，1 和 9 。因为 6 和 1 的奇偶性不同，所以下标从 2 -> 3 让你失去 x = 5 分。
// 总得分为：2 + 6 + 1 + 9 - 5 = 13 。
// 示例 2：
//
// 输入：nums = [2,4,6,8], x = 3
// 输出：20
// 解释：数组中的所有元素奇偶性都一样，所以我们可以将每个元素都访问一次，而且不会失去任何分数。
// 总得分为：2 + 4 + 6 + 8 = 20 。
//  
//
// 提示：
//
// 2 <= nums.length <= 105
// 1 <= nums[i], x  <= 106


public class Solution
{
    //[TestCase(new[] { 2, 5, 1, 4 }, ExpectedResult = 5)]
    //
    [TestCase(new[] { 756, 1324, 2419, 495, 106, 111, 1649, 1474, 2001, 1633, 273, 1804, 2102, 1782, 705, 1529, 1761, 1613, 111, 186, 412 }, ExpectedResult = 183)]
    public int CountBeautifulPairs(int[] nums)
    {
        var result = 0;

        for (var i = 0 ; i < nums.Length ; i++)
        {
            var num1 = nums[i];

            while (num1 > 10)
            {
                num1 /= 10;
            }

            for (var j = i + 1 ; j < nums.Length ; j++)
            {
                var num2 = nums[j] % 10;

                if (GCD(num1, num2) == 1)
                {
                    result++;
                }
            }
        }

        return result;

        int GCD(int a, int b) => a == 0 ? b : GCD(b % a, a);
    }
}