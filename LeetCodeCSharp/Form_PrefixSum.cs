﻿namespace LeetCodeCSharp;

/****************************************************  基本前缀和  ******************************************************/
// 前缀和
// 特征点: 
// 1.需要提取子数组
// 2.子数组的信息,由从0开始的right区间 - 从0开始的left区间决定
// 3.这个信息不一定是前缀和,可能是计数操作(即某一个区间内符合某个条件的数量)
// 注意点：
// 1.前缀和往往特别大,需要使用long类型

// 对于数组 nums ，定义它的前缀和
//          s[0] = 0
//          s[1] = nums[0]
//          s[2] = nums[0] + nums[1]
//          s[i] = nums[0] + nums[1] + ... + nums[i - 1]
//        s[i+1] = nums[0] + nums[1] + ... + nums[i - 1] + nums[i]
//          s[n] = nums[0] + nums[1] + ... + nums[n - 1]
//
// 为什么要定义 s[0]=0 见下文答疑
//
// 根据这个定义，有
// s[i+1]=s[i]+a[i]
//
// 例如 [−2,0,3,−5,2,−1]，对应的前缀和数组 s = [0,−2,−2,1,−4,−2,−3]
// 通过前缀和，我们可以把连续子数组的元素和转换成两个前缀和的差，
//  a[left] 到 a[right] 的元素和等于 s[right+1]−s[left]
//
// 有了这个式子，示例中子数组 [3,−5,2,−1] 的和，就可以 O(1) 地用 s[6]−s[2] = −3−(−2)=−1 算出来。
//
// 答疑
//     问：为什么要定义 s[0]=0，这样做有什么好处？
//
// 答：如果 left=0，要计算的子数组是一个前缀（从 a[0] 到 a[right]），
//    我们要用 s[right+1] 减去 s[0]。如果不定义 s[0]=0，就必须特判 left=0 的情况了（读者可以试试）。
//    通过定义 s[0]=0，任意子数组（包括前缀）都可以表示为两个前缀和的差。
//    此外，如果 a 是空数组，定义 s[0]=0 的写法是可以兼容这种情况的。
//


public class Solution_3152
{
    // 关键点:如何构造前缀和数组,这个信息隐藏的太深了
    // 及前缀表示到此未知奇偶相同的数量
    public static bool[] IsArraySpecial(int[] nums, int[][] queries)
    {
        Span<int> prefix = stackalloc int[nums.Length];

        prefix[0] = 0;
        for (var i = 1 ; i < nums.Length ; i++)
        {
            prefix[i] = prefix[i - 1] + ((nums[i] ^ nums[i - 1] ^ 1) & 1);
        }

        var result = new bool[queries.Length];

        for (var i = 0 ; i < queries.Length ; i++)
        {
            result[i] = prefix[queries[i][0]] == prefix[queries[i][1]];
        }


        return result;
    }


    // 如果数组的每一对相邻元素都是两个奇偶性不同的数字，则该数组被认为是一个 特殊数组 
    //
    // 周洋哥有一个整数数组 nums 和一个二维整数矩阵 queries，对于 queries[i] = [fromi, toi]，请你帮助周洋哥检查子数组 nums[fromi..toi] 是不是一个 特殊数组 。
    //
    // 返回布尔数组 answer，如果 nums[fromi..toi] 是特殊数组，则 answer[i] 为 true ，否则，answer[i] 为 false 。
    //
    //
    //
    // 示例 1：
    //
    // 输入：nums = [3,4,1,2,6], queries = [[0,4]]
    //             [0,0,0,0,1]
    // 输出：[false]
    //
    // 解释：
    //
    // 子数组是 [3,4,1,2,6]。2 和 6 都是偶数。
    //
    // 示例 2：
    //
    // 输入：nums = [4,3,1,6], queries = [[0,2],[2,3]]
    //
    // 输出：[false,true]
    //
    // 解释：
    //
    // 子数组是 [4,3,1]。3 和 1 都是奇数。因此这个查询的答案是 false。
    // 子数组是 [1,6]。只有一对：(1,6)，且包含了奇偶性不同的数字。因此这个查询的答案是 true。
    //
    //
    // 提示：
    //
    // 1 <= nums.length <= 10^5
    // 1 <= nums[i] <= 10^5
    // 1 <= queries.length <= 10^5
    // queries[i].length == 2
    // 0 <= queries[i][0] <= queries[i][1] <= nums.length - 1
}


/// <summary> 1744. 你能在你最喜欢的那天吃到你最喜欢的糖果吗？ </summary>
public class Solution_1744
{
    // 标准前缀和模板
    // 注
    public bool[] CanEat(int[] candiesCount, int[][] queries)
    {
        Span<long> prefix = stackalloc long[candiesCount.Length + 1];

        for (var i = 1 ; i <= candiesCount.Length ; i++)
        {
            prefix[i] = prefix[i - 1] + candiesCount[i - 1];
        }

        var result = new bool[queries.Length];

        for (var i = 0 ; i < queries.Length ; i++)
        {
            var type = queries[i][0];
            var day  = queries[i][1];
            var cap  = queries[i][2];

            // key : 前缀和小于 天数 * 每天吃的数量 且 前缀和大于天数（保证每天至少吃一颗）
            result[i] = prefix[type] < (day + 1) * 1L * cap && prefix[type + 1] > day;
        }

        return result;
    }

    // 给你一个下标从 0 开始的正整数数组 candiesCount ，其中 candiesCount[i] 表示你拥有的第 i 类糖果的数目。同时给你一个二维数组 queries ，其中 queries[i] = [favoriteTypei, favoriteDayi, dailyCapi] 。
    //
    // 你按照如下规则进行一场游戏：
    //
    // 你从第 0 天开始吃糖果。
    // 你在吃完            所有 第         i - 1 类糖果之前，不能 吃任何一颗第 i 类糖果。
    // 在吃完所有糖果之前，你必须每天 至少 吃         一颗 糖果。
    // 请你构建一个布尔型数组     answer ，用以给出 queries 中每一项的对应答案。此数组满足：
    //
    // answer.length == queries.length 。answer[i] 是                queries[i] 的答案。
    // answer[i] 为 true 的条件是：
    // 在每天吃不超过 dailyCapi 颗糖果的前提下，你可以在第 favoriteDayi 天吃到第 favoriteTypei 类糖果；否则 answer[i] 为 false 。
    // 注意，只要满足上面 3 条规则中的第二条规则，你就可以在同一天吃不同类型的糖果。
    //
    // 请你返回得到的数组 answer 。
    //
    //
    //
    // 示例 1：
    //
    // 输入：candiesCount = [7,4,5,3,8], queries = [[0,2,2],[4,2,4],[2,13,1000000000]]
    // 输出：[true,false,true]
    // 提示：
    // 1- 在第 0 天吃 2 颗糖果(类型 0），第 1 天吃 2 颗糖果（类型 0），第 2 天你可以吃到类型 0 的糖果。
    // 2- 每天你最多吃 4 颗糖果。即使第 0 天吃 4 颗糖果（类型 0），第 1 天吃 4 颗糖果（类型 0 和类型 1），你也没办法在第 2 天吃到类型 4 的糖果。换言之，你没法在每天吃 4 颗糖果的限制下在第 2 天吃到第 4 类糖果。
    // 3- 如果你每天吃 1 颗糖果，你可以在第 13 天吃到类型 2 的糖果。
    // 示例 2：
    //
    // 输入：candiesCount = [5,2,6,4,1], queries = [[3,1,2],[4,10,3],[3,10,100],[4,100,30],[1,3,1]]
    // 输出：[false,true,true,false,false]
    //
    //
    // 提示：
    //
    // 1 <= candiesCount.length <= 10^5
    // 1 <= candiesCount[i] <= 10^5
    // 1 <= queries.length <= 10^5
    // queries[i].length == 3
    // 0 <= favoriteTypei < candiesCount.length
    // 0 <= favoriteDayi <= 10^9
    // 1 <= dailyCapi <= 10^9
}


/// <summary> 2055. 蜡烛之间的盘子 </summary>
public class Solution_2055
{
    //符合特查找征子字符串,并且是查找符合某一个特征的数量,符合前缀和的特征

    public int[] PlatesBetweenCandles(string s, int[][] queries)
    {
        var n = s.Length;

        //1.前缀和:迄今为止的盘子数量
        Span<int> preSum = stackalloc int[n];

        for (int i = 0, sum = 0 ; i < n ; i++) //值得一提的是,前缀和中计数操作可以使用下面的方式,保存一个sum变量即可避免多次地址访问
        {
            if (s[i] == '*') sum++;
            preSum[i] = sum;
        }

        //2.前缀和:迄今为止最近的蜡烛位置(左边)
        Span<int> left = stackalloc int[n];
        //3.前缀和:迄今为止最近的蜡烛位置(右边)
        Span<int> right = stackalloc int[n];

        for (int i = 0, l = -1 ; i < n ; i++)
        {
            if (s[i] == '|') l = i;
            left[i] = l;
        }

        for (int i = n - 1, r = -1 ; i >= 0 ; i--)
        {
            if (s[i] == '|') r = i;
            right[i] = r;
        }

        var ans = new int[queries.Length];
        for (var i = 0 ; i < queries.Length ; i++)
        {
            var query = queries[i];
            int x     = right[query[0]], y = left[query[1]];
            // x:起点区间右边最近的蜡烛位置坐标
            // y:终点区间左边最近的蜡烛位置坐标
            //key:如果满足任意其一,则为0
            //1.起点区间右边没有蜡烛
            //2.终点区间左边没有蜡烛
            //3.起点区间右边大于等于终点区间左边:即区间内没有蜡烛
            //(第三点这部分的推理是一个难点,实际上是说,他们的区间内部)   "|**[*****]***|";
            ans[i] = x == -1 || y == -1 || x >= y ? 0 : preSum[y] - preSum[x];
        }

        return ans;
    }


    [TestCase]
    public void Test()
    {
        var     s       = "|**********|";
        int[][] queries = [[2, 5], [5, 9]];
        var     result  = PlatesBetweenCandles(s, queries);
    }

    // 字符串 s (3 <= s.length <= 10^5) 仅有 盘子‘*’ 和 蜡烛‘|’ 组成
    // 二维数组 queries (1 <= queries.length <= 10^5) 仅有两个元素,表示查询的区间 queries[i] = [lefti...righti](闭区间
    // 两蜡烛之间的盘子: 盘子在 子字符串中 左边和右边 都 至少有一支蜡烛

    // 比方说，s = "||**||**|*" ，查询 [3, 8] ，表示的是子字符串 "*||**|" 。子字符串中在两支蜡烛之间的盘子数目为 2 ，子字符串中右边两个盘子在它们左边和右边 都 至少有一支蜡烛。
    // 请你返回一个整数数组answer ，其中 answer[i] 是第 i 个查询的答案。
    //
    //
    //
    // 示例 1:
    //
    // 输入：s = "**|**|***|", queries = [[2,5],[5,9]]
    // 输出：[2,3]
    // 解释：
    // - queries[0] 有两个盘子在蜡烛之间。
    // - queries[1] 有三个盘子在蜡烛之间。
    //
    // 示例 2:
    //
    // 输入：s = "***|**|*****|**||**|*", queries = [[1,17],[4,5],[14,17],[5,11],[15,16]]
    // 输出：[9,0,0,0,0]
    // 解释：
    // - queries[0] 有 9 个盘子在蜡烛之间。
    // - 另一个查询没有盘子在蜡烛之间。
    //
    //
    // 提示：
    //
    // 3 <= s.length <= 105
    // s 只包含字符 '*' 和 '|' 。
    // 1 <= queries.length <= 105
    // queries[i].length == 2
    // 0 <= lefti <= righti < s.length
}


/// <summary> 2438. 二的幂数组中查询范围内的乘积 </summary>
public class Solution_2438
{
    /// 也是非前缀和的模板题,但是这里的取余操作,是重点需要注意的地方
    /// 之前错误的做法,就是始终用int装乘积,但是始终会有溢出的风险,其实只要在最开始用long存储即可
    /// 为什么说是非模板题呢?恰恰是因为取余操作,因为取余的触发是需要特殊处理的,前缀和中的取余会失真
    public int[] ProductQueries(int n, int[][] queries)
    {
        const int mod = 1000000007;

        var powers = new List<int>();

        while (n > 0)
        {
            powers.Add(n & -n);
            n &= n - 1;
        }

        var result = new int[queries.Length];

        for (var i = 0 ; i < queries.Length ; i++)
        {
            var query = queries[i];
            var left  = query[0];
            var right = query[1];
            var temp  = 1L;

            for (var j = left ; j <= right ; j++)
            {
                temp = temp * powers[j] % mod;
            }

            result[i] = (int)temp;
        }


        return result;
    }

    [Test]
    public void METHOD()
    {
        var n       = 919;
        var queries = new int[3][];
        queries[0] = [0, 6];
        queries[1] = [2, 2];
        queries[2] = [0, 3];
        var result = ProductQueries(n, queries);
    }



    // 给你一个正整数 n ，你需要找到一个下标从 0 开始的数组 powers ，
    // 它包含 最少 数目的 2 的幂，且它们的和为 n 。powers 数组是 非递减 顺序的。
    // 根据前面描述，构造 powers 数组的方法是唯一的
    // 同时给你一个下标从 0 开始的二维整数数组 queries ，
    // 其中 queries[i] = [left_i, right_i] ，
    // 其中 queries[i] 表示请你求出满足 left_i <= j <= right_i 的所有 powers[j] 的乘积
    // 请你返回一个数组 answers ，长度与 queries 的长度相同，其中 answers[i]是第 i 个查询的答案。由于查询的结果可能非常大，请你将每个 answers[i] 都对 10^9 + 7 取余


    // 示例 1：
    //
    // 输入：n = 15, queries = [[0,1],[2,2],[0,3]]
    // 输出：[2,4,64]
    // 解释：     [0b1111]       [1,1,2,8,64]
    // 对于 n = 15 ，得到 powers = [1,2,4,8] 。没法得到元素数目更少的数组。
    // 第 1 个查询的答案：powers[0] * powers[1] = 1 * 2 = 2 。
    // 第 2 个查询的答案：powers[2] = 4 。
    // 第 3 个查询的答案：powers[0] * powers[1] * powers[2] * powers[3] = 1 * 2 * 4 * 8 = 64 。
    // 每个答案对 10^9 + 7 得到的结果都相同，所以返回 [2,4,64] 。
    // 示例 2：
    //
    // 输入：n = 2, queries = [[0,0]]
    // 输出：[2]
    // 解释：
    // 对于 n = 2, powers = [2] 。
    // 唯一一个查询的答案是 powers[0] = 2 。答案对 10^9 + 7 取余后结果相同，所以返回 [2] 。
    //
    //
    // 提示：
    //
    // 1 <= n <= 10^9
    // 1 <= queries.length <= 10^5
    // 0 <= starti <= endi < powers.length
}


/**********************************************  前缀和 + 哈希表  ******************************************************/
// 相对于基本体型,多了一层统计子数组的操作
// 子数组 是数组的一段连续部分
// 因此子数组是有序的,可以在计算前缀和的时候,同时统计此时前缀和的数量
// 且子数组可以等于本身,因此在迭代的时候,需要考虑当前索引本身的情况
// ↑ 因此在计算子数组和的时候,初始化的时候,需要加入一个和为0的情况,表示当前索引本身的情况

// 之前的双变量问题,有一个思想,就是 枚举右，维护左 ,这个思想在前缀和中有异曲同工之妙
// 1. 枚举右: 子数组的右边界,即当前索引
// 2. 维护左: 通过前缀和,计算差值,得到左边界,维护的是一个哈希表容器 

// 总结反思 1
// 如果我没有任何算法基础,我能做的最好的就是,暴力枚举所有的情况
// 在暴力枚举的时候,我可能是按照子数组的长度去枚举,先枚举所有长度为1的,然后是长度为2的....
// 通过算法的学习,我们就能发现,枚举子数组,就是枚举子数组的左右边界
// 通过 前缀和+哈希表,我们就能发现,枚举子数组的右边界,维护子数组的左边界,是一个非常好的思路
// 算法世界,就是一个不断优化的过程,不断优化的过程,就是不断的学习的过程


/// <summary> 930. 和相同的二元子数组 </summary>
public class Solution_930
{
    [TestCase(new[] { 0, 0, 1, 0, 0 },                0, ExpectedResult = 6)]
    [TestCase(new[] { 1, 0, 1, 0, 1 },                2, ExpectedResult = 4)]
    [TestCase(new[] { 0, 1, 1, 1, 1 },                3, ExpectedResult = 3)]
    [TestCase(new[] { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 }, 0, ExpectedResult = 27)]
    public int NumSubarraysWithSum(int[] nums, int goal)
    {
        // 1. 计算前缀和
        for (var i = 1 ; i < nums.Length ; i++)
        {
            nums[i] += nums[i - 1];
        }

        // 2. 生成map,表示当前前缀和的数量
        // 需要注意的是,子数组是可以等于数组本身的,迭代计算的时候,需要考虑当前索引本身的情况
        var map = new Dictionary<int, int> { { 0, 1 } };

        var ans = 0;

        for (int i = 0, len = nums.Length ; i < len ; i++)
        {
            // 3. 计算当前索引的前缀和
            var right = nums[i];
            // 4. 计算差值
            var left = right - goal;

            // 5. 累加数量:map统计了目前为止,从0开始的前缀和的数量,相当于子数组(这个子数组可以理解成,必须用当前索引做末尾的子数组)的数量
            ans += map.GetValueOrDefault(left);

            map.TryAdd(right, 0);
            map[right]++;
        }

        return ans;
    }


    // 给你一个二元数组 nums ，和一个整数 goal ，请你统计并返回有多少个和为 goal 的 [非空] 子数组。
    //
    // 子数组 是数组的一段连续部分
    //
    // 示例 1：
    // 输入：nums = [1,0,1,0,1], goal = 2
    // 输出：4
    // 解释：
    // 有 4 个满足题目要求的子数组：[1,0,1]、[1,0,1,0]、[0,1,0,1]、[1,0,1]
    // 示例 2：
    //
    // 输入：nums = [0,0,0,0,0], goal = 0
    // 输出：15
    //
    // 提示：
    //
    // 1 <= nums.length <= 3 * 10^4
    // nums[i] 不是 0 就是 1
    // 0 <= goal <= nums.length
}