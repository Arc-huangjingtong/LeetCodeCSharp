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