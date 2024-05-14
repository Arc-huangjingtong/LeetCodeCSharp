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


/// <summary> 2244. 完成所有任务需要的最少轮数 </summary>
public class Solution_2244
{
    [TestCase(new int[] { 2, 2, 3, 3, 2, 4, 4, 4, 4, 4 }, ExpectedResult = 4)]
    public int MinimumRounds(int[] tasks)
    {
        Array.Sort(tasks);

        var temp   = tasks[0];
        var index  = 0;
        var result = 0;

        for (int i = 1 ; i < tasks.Length ; i++)
        {
            if (tasks[i] == temp) continue;

            var length = i - index;

            if (length == 1)
            {
                return -1;
            }

            if (length % 3 == 2 || length % 3 == 1)
            {
                result += length / 3 + 1;
            }
            else
            {
                result += length / 3;
            }

            index = i;
            temp  = tasks[i];
        }

        var lastLength = tasks.Length - index;

        if (lastLength == 1)
        {
            return -1;
        }

        if (lastLength % 3 == 2 || lastLength % 3 == 1)
        {
            result += lastLength / 3 + 1;
        }
        else
        {
            result += lastLength / 3;
        }

        return result;
    }

    //字典存数量
    public int MinimumRounds2(int[] tasks)
    {
        var ans  = 0;
        var cnts = new Dictionary<int, int>();
        foreach (int task in tasks)
        {
            cnts.TryAdd(task, 0);
            ++cnts[task];
        }

        foreach (int cnt in cnts.Values)
        {
            if (cnt == 1) return -1;
            ans += (cnt + 2) / 3;
        }

        return ans;
    }
}