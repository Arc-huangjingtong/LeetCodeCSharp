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


///[前缀和]
/// <summary> 2559. 找出区间内的元音单词 </summary>
public class Solution_2559
{
    private static readonly HashSet<char> vowels = ['a', 'e', 'i', 'o', 'u'];

    public int[] VowelStrings(string[] words, int[][] queries)
    {
        var       result = new int[queries.Length];
        Span<int> temp   = stackalloc int[words.Length + 1];

        for (var i = 0 ; i < words.Length ; i++)
        {
            temp[i + 1] = isVowel(words[i]) ? temp[i] + 1 : temp[i];
        }

        for (var i = 0 ; i < queries.Length ; i++)
        {
            result[i] = temp[queries[i][1] + 1] - temp[queries[i][0]];
        }

        return result;


        bool isVowel(string str) => vowels.Contains(str[0]) && vowels.Contains(str[^1]);
    }



    [Test]
    public void Test()
    {
        string[] words   = ["aba", "bcb", "ece", "aa", "e"];
        int[][]  queries = [[0, 2], [1, 4], [1, 1]];


        Console.WriteLine(string.Join("_", VowelStrings(words, queries)));
    }
}


///[排序]+[前缀和]+[二分上界]
/// <summary> 2389.和有限的最长子序列 </summary>
public class Solution_2389
{
    public int[] AnswerQueries(int[] nums, int[] queries)
    {
        Array.Sort(nums);

        var result = new int[queries.Length];
        var temp   = new int[nums.Length + 1];

        for (var i = 0 ; i < nums.Length ; i++)
        {
            temp[i + 1] = temp[i] + nums[i];
        }

        for (var i = 0 ; i < queries.Length ; i++)
        {
            var target = queries[i];
            var index  = Array.BinarySearch(temp, target);

            if (index < 0)
            {
                index = ~index - 1;
            }

            result[i] = index;
        }

        return result;
    }

    //1,2,4,5

    // 给你一个长度为 n 的整数数组 nums ，和一个长度为 m 的整数数组 queries 。
    // 返回一个长度为 m 的数组 answer ，其中 answer[i] 是 nums 中 元素之和小于等于 queries[i] 的 子序列 的 最大 长度  。
    //
    // 子序列 是由一个数组删除某些元素（也可以不删除）但不改变剩余元素顺序得到的一个数组。
    //
    // 示例 1：
    //
    // 输入：nums = [4,5,2,1], queries = [3,10,21] 
    // 输出：[2,3,4]
    // 解释：queries 对应的 answer 如下：
    // - 子序列 [2,1] 的和小于或等于 3 。可以证明满足题目要求的子序列的最大长度是 2 ，所以 answer[0] = 2 。
    //     - 子序列 [4,5,1] 的和小于或等于 10 。可以证明满足题目要求的子序列的最大长度是 3 ，所以 answer[1] = 3 。
    //     - 子序列 [4,5,2,1] 的和小于或等于 21 。可以证明满足题目要求的子序列的最大长度是 4 ，所以 answer[2] = 4 。
    // 示例 2：
    //
    // 输入：nums = [2,3,4,5], queries = [1]
    // 输出：[0]
    // 解释：空子序列是唯一一个满足元素和小于或等于 1 的子序列，所以 answer[0] = 0 。
    //
    //
    // 提示：
    //
    // n == nums.length
    //     m == queries.length
    // 1 <= n, m <= 1000
    // 1 <= nums[i], queries[i] <= 10^6
}


/// <summary> 2391.收集垃圾的最少总时间 </summary>
public class Solution_2391
{
    [TestCase(new[] { "G", "P", "GP", "GG" }, new[] { 2, 4, 3 }, ExpectedResult = 21)]
    [TestCase(new[] { "MMM", "PGM", "GP" }, new[] { 3, 10 }, ExpectedResult = 37)]
    public int GarbageCollection(string[] garbage, int[] travel)
    {
        var result = 0;

        //收集垃圾的时间
        result += garbage.Sum(x => x.Length);

        //赶路的时间
        result += travel.Sum() * 3; 

        //减去不需要赶路的时间
        var set = new HashSet<char>();

        for (int i = garbage.Length - 1 ; i >= 1 ; i--)
        {
            if (set.Count == 3)
            {
                break;
            }

            foreach (var garb in garbage[i])
            {
                set.Add(garb);
            }

            result -= travel[i-1] * (3 - set.Count);
        }


        return result;
    }
}