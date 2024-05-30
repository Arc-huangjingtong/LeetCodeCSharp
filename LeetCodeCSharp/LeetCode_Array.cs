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

        for (var i = 1 ; i < tasks.Length ; i++)
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
        foreach (var task in tasks)
        {
            cnts.TryAdd(task, 0);
            ++cnts[task];
        }

        foreach (var cnt in cnts.Values)
        {
            if (cnt == 1) return -1;
            ans += (cnt + 2) / 3;
        }

        return ans;
    }
}


/// <summary> 1953. 你可以工作的最大周数 </summary>
public class Solution_1953
{
    [TestCase(new[] { 1, 2, 3 }, ExpectedResult = 6)]
    [TestCase(new[] { 5, 2, 1 }, ExpectedResult = 7)]
    public long NumberOfWeeks(int[] milestones)
    {
        var longest = milestones.Max();                                // 耗时最长工作所需周数
        var rest    = milestones.Select(x => (long)x).Sum() - longest; // 其余工作所需周数

        if (longest > rest + 1)
        {
            // 此时无法完成所耗时最长的工作
            return rest * 2 + 1;
        }

        // 此时可以完成所有工作
        return longest + rest;
    }

    // 1953. 你可以工作的最大周数  中等
    // 给你 n 个项目，编号从 0 到 n - 1 。同时给你一个整数数组 milestones ，其中每个 milestones[i] 表示第 i 个项目中的阶段任务数量。
    //
    // 你可以按下面两个规则参与项目中的工作：
    //
    // 每周，你将会完成                                       某一个 项目中的  恰好一个 阶段任务。你每周都 必须 工作。
    // 在                                              连续的 两周中，你 不能 参与并完成同一个项目中的两个阶段任务。
    // 一旦所有项目中的全部阶段任务都完成，或者仅剩余一个阶段任务都会导致你违反上面的规则，那么你将 停止工作 。注意，由于这些条件的限制，你可能无法完成所有阶段任务。
    //
    // 返回在不违反上面规则的情况下你 最多 能工作多少周。
    //
    //  
    //
    // 示例 1：
    //
    // 输入：milestones = [1,2,3]
    // 输出：6
    // 解释：一种可能的情形是：
    // - 第 1 周，你参与并完成项目 0 中的一个阶段任务
    // - 第 2 周，你参与并完成项目 2 中的一个阶段任务
    // - 第 3 周，你参与并完成项目 1 中的一个阶段任务
    // - 第 4 周，你参与并完成项目 2 中的一个阶段任务
    // - 第 5 周，你参与并完成项目 1 中的一个阶段任务
    // - 第 6 周，你参与并完成项目 2 中的一个阶段任务
    // 总周数是 6 。
    // 示例 2：
    //
    // 输入：milestones = [5,2,1]
    // 输出：7
    // 解释：一种可能的情形是：
    // - 第 1 周，你参与并完成项目 0 中的一个阶段任务。
    // - 第 2 周，你参与并完成项目 1 中的一个阶段任务。
    // - 第 3 周，你参与并完成项目 0 中的一个阶段任务。
    // - 第 4 周，你参与并完成项目 1 中的一个阶段任务。
    // - 第 5 周，你参与并完成项目 0 中的一个阶段任务。
    // - 第 6 周，你参与并完成项目 2 中的一个阶段任务。
    // - 第 7 周，你参与并完成项目 0 中的一个阶段任务。
    // 总周数是 7 。
    // 注意，你不能在第 8 周参与完成项目 0 中的最后一个阶段任务，因为这会违反规则。
    // 因此，项目 0 中会有一个阶段任务维持未完成状态。
    //  
    //
    // 提示：
    //
    // n == milestones.length
    // 1 <= n             <= 105
    // 1 <= milestones[i] <= 109
}


/// <summary> 826. 安排工作以达到最大收益 </summary>
public class Solution_826
{
    //从最高利润中，找到满足自己能力的工作了，大概率是二分查找
    [TestCase(new[] { 2, 4, 6, 8, 10 }, new[] { 10, 20, 30, 40, 50 }, new[] { 4, 5, 6, 7 }, ExpectedResult = 100)]
    public int MaxProfitAssignment(int[] difficulty, int[] profit, int[] worker)
    {
        var n      = difficulty.Length;
        var sorted = new SortedList<int, int>();

        var result = 0;

        for (var i = 0 ; i < n ; i++)
        {
            if (!sorted.TryGetValue(difficulty[i], out var value))
            {
                sorted.Add(difficulty[i], profit[i]);
            }
            else
            {
                sorted[difficulty[i]] = Math.Max(value, profit[i]);
            }
        }

        //二分查找
        foreach (var w in worker)
        {
            int left = 0, right = sorted.Count - 1;

            while (left < right)
            {
                var mid = left + (right - left) / 2;

                if (sorted.Keys[mid] <= w)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid;
                }
            }

            if (sorted.Keys[left] <= w)
            {
                result += sorted.Values[left];
            }
        }


        return result;
    }


    // 826. 安排工作以达到最大收益
    // 你有 n 个工作和 m 个工人。给定三个数组： difficulty, profit 和 worker ，其中:
    //
    // difficulty[i] 表示第 i 个工作的难度，
    //     profit[i] 表示第 i 个工作的收益。
    //     worker[i] 表示第 i 个工人的能力，即该工人只能完成难度小于等于 worker[i] 的工作
    // 每个工人 最多 只能安排 一个 工作，但是一个工作可以 完成多次 。
    //
    // 举个例子，如果 3 个工人都尝试完成一份报酬为 $1 的同样工作，那么总收益为 $3 。如果一个工人不能完成任何工作，他的收益为 $0 。
    // 返回 在把工人分配到工作岗位后，我们所能获得的最大利润 。
    //
    //
    //
    // 示例 1：
    //
    // 输入: difficulty = [2,4,6,8,10], profit = [10,20,30,40,50], worker = [4,5,6,7]
    // 输出: 100 
    // 解释: 工人被分配的工作难度是 [4,4,6,6] ，分别获得 [20,20,30,30] 的收益。
    // 示例 2:
    //
    // 输入: difficulty = [85,47,57], profit = [24,66,99], worker = [40,25,25]
    // 输出: 0
    //
    //
    // 提示:
    //
    // n == difficulty.length
    // n == profit.length
    // m == worker.length
    // 1 <= n, m                                <= 10^4
    // 1 <= difficulty[i], profit[i], worker[i] <= 10^5
}


public class Solution_Test
{
    [TestCase(0, 3, 0, 0, 3, 0, 0, 3, ExpectedResult = false)]
    [TestCase(3, 3, 0, 0, 3, 0, 0, 3, ExpectedResult = true)]
    [TestCase(3, 3, 3, 0, 3, 0, 0, 3, ExpectedResult = true)]
    [TestCase(1, 2, 1, 0, 1, 1, 1, 1, ExpectedResult = true)]
    [TestCase(1, 1, 1, 1, 1, 1, 1, 1, ExpectedResult = true)]
    public bool canMatch(int a, int r, int s, int n, int need_a, int need_r, int need_s, int need_n)
    {
        if (a < need_a || r < need_r || s < need_s) return false;

        var need_any = need_a + need_r + need_s + need_n;

        return a + r + s + n >= need_any;
    }
}


/// <summary> 2981. 找出出现至少三次的最长特殊子字符串 I </summary>
public class Solution_2981
{
    public static List<int> List => new List<int>();

    //[TestCase("abcdef", ExpectedResult = -1)]
    [TestCase("aada", ExpectedResult = 1)]
    public int MaximumLength(string s)
    {
        var ans = -1;
        var len = s.Length;

        var chs = new List<int>[26];
        //Array.Fill(chs, []); 这样写是错误的，显然不能用Fill函数初始化引用类型

        for (var i = 0 ; i < 26 ; i++)
        {
            chs[i] = [];
        }

        var cnt = 0;
        for (var i = 0 ; i < len ; i++)
        {
            cnt++;
            if (i + 1 == len || s[i] != s[i + 1])
            {
                var ch = s[i] - 'a';
                chs[ch].Add(cnt);
                cnt = 0;

                for (var j = chs[ch].Count - 1 ; j > 0 ; j--)
                {
                    if (chs[ch][j] > chs[ch][j - 1])
                    {
                        (chs[ch][j], chs[ch][j - 1]) = (chs[ch][j - 1], chs[ch][j]);
                    }
                    else
                    {
                        break;
                    }
                }

                if (chs[ch].Count > 3)
                {
                    chs[ch].RemoveAt(chs[ch].Count - 1);
                }
            }
        }

        for (var i = 0 ; i < 26 ; i++)
        {
            if (chs[i].Count > 0 && chs[i][0] > 2)
            {
                ans = Math.Max(ans, chs[i][0] - 2);
            }

            if (chs[i].Count > 1 && chs[i][0] > 1)
            {
                ans = Math.Max(ans, Math.Min(chs[i][0] - 1, chs[i][1]));
            }

            if (chs[i].Count > 2)
            {
                ans = Math.Max(ans, chs[i][2]);
            }
        }

        return ans;
    }


    [TestCase("ereerrrererrrererre",                                ExpectedResult = 2)]
    [TestCase("dceeddedcedcdcdedcdddeeedddsssdcdcdeeeccdccedeeedd", ExpectedResult = 3)]
    public int MaximumLength2(string s)
    {
        var chs = Enumerable.Range(0, 26).Select(_ => new PriorityQueue<int, int>()).ToArray();

        var cnt = 0;

        foreach (var c in chs)
        {
            cnt++;
        }

        for (var i = 0 ; i < s.Length ; i++)
        {
            cnt++;
            if (i + 1 == s.Length || s[i] != s[i + 1])
            {
                chs[s[i] - 'a'].Enqueue(cnt, -cnt);
                cnt = 0;
            }
        }

        var ans = -1;

        for (var i = 0 ; i < 26 ; i++)
        {
            if (chs[i].Count > 0 && chs[i].Peek() > 2)
            {
                ans = Math.Max(ans, chs[i].Peek() - 2);
            }

            if (chs[i].Count > 1 && chs[i].Peek() > 1)
            {
                var num1 = chs[i].Dequeue();
                var num2 = chs[i].Dequeue();
                ans = Math.Max(ans, Math.Min(num1 - 1, num2));

                chs[i].Enqueue(num2, -num2);
                chs[i].Enqueue(num1, -num1);
            }

            if (chs[i].Count > 2)
            {
                chs[i].Dequeue();
                chs[i].Dequeue();
                ans = Math.Max(ans, chs[i].Dequeue());
            }
        }

        return ans;
    }



    // 2981. 找出出现至少三次的最长特殊子字符串 I 中等
    //
    // 给你一个仅由[小写英文字母]组成的字符串 s , 3 <= s.length <= 50
    //
    // 如果一个字符串仅由[单一字符]组成，那么它被称为 特殊 字符串。例如，字符串 "abc" 不是特殊字符串，而字符串 "ddd"、"zz" 和 "f" 是特殊字符串。
    //
    // 返回在 s 中出现 至少三次 的 最长特殊子字符串 的长度，如果不存在出现至少三次的特殊子字符串，则返回 -1 。
    //
    // 子字符串 是字符串中的一个连续 非空 字符序列。
    //
    // 输入：s = "aaaa"   输出： 2
    // 解释：出现三次的最长特殊子字符串是 "aa" ：子字符串 "[aa]aa"、"a[aa]a" 和 "aa[aa]"。
    // 可以证明最大长度是 2 。
    //
    // 输入：s = "abcdef" 输出：-1
    // 解释：不存在出现至少三次的特殊子字符串。因此返回 -1 。
    //
    // 输入：s = "abcaba" 输出： 1
    // 解释：出现三次的最长特殊子字符串是 "a" ：子字符串 "[a]bcaba"、"abc[a]ba" 和 "abcab[a]"。
    // 可以证明最大长度是 1 。



    // 因字符串仅含有小写字母，所以可以对每种字母单独处理。
    // 1.对于每一种字母，统计出每部分连续子串的长度，并储存在数组 chs 中。因题目要求出现至少三次，因此只要维护前三大的长度即可。每次往 chs 中添加元素时，可采用冒泡的方法使其有序。如果长度超过 3，则将末尾元素 pop 掉
    //
    // 更新答案时，主要有三种：
    //
    // 最长的 chs[0] 可贡献出 3 个长为 chs[0]−2 的子串，并且需要满足 chs[0]>2。
    // a:aaaa,aaaa,aaaa chs[0]>2
    //
    // 当 chs[0] 与 chs[1] 相等时，可贡献出 4 个长为 chs[0]−1 的子串。不等时可由 chs[0] 贡献出 2 个长为 chs[1] 的子串，加上 chs[1] 本身一共 3 个，并且需要满足 chs[0]>1
    //
    // 可由 chs[0] 与 chs[1] 加上 chs[2] 本身贡献 3 个长为 chs[2] 的子串。
    //
    // 没有更新答案时，则输出 −1
}