namespace LeetCodeCSharp;

//      前缀和
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


// 给你一个长度为 n 的整数数组 nums ，n 是 偶数 ，同时给你一个整数 k 。
//
// 你可以对数组进行一些操作。每次操作中，你可以将数组中 任一 元素替换为 0 到 k 之间的 任一 整数。
//
// 执行完所有操作以后，你需要确保最后得到的数组满足以下条件：
//
// 存在一个整数     X ，满足对于所有的 (0 <= i < n) 都有 abs(a[i] - a[n - i - 1]) = X 。
// 请你返回满足以上条件 最少 修改次数。


public class Solution
{
    public int MinChanges(int[] nums, int k)
    {
        var len  = nums.Length;
        var temp = new (int delta, int setMax)[len / 2];
        var dict = new Dictionary<int, int>();

        for (var i = 0 ; i < len / 2 ; i++)
        {
            var left  = nums[i];
            var right = nums[len - i - 1];
            var max1  = Math.Max(left,      right);
            var max2  = Math.Max(k - right, k - left);
            var max   = Math.Max(max1,      max2);
            var dis   = Math.Abs(left - right);
            temp[i] = (dis, max);

            dict.TryAdd(dis, 0);
            dict[dis]++;
        }

        var result = int.MaxValue;

        var minOne = int.MaxValue;

        foreach (var keyValue in dict)
        {
            var key = keyValue.Key;
            if (keyValue.Value == 1)
            {
                minOne = Math.Min(minOne, key);
                continue;
            }

            var count = 0;

            for (var i = 0 ; i < temp.Length ; i++)
            {
                if (temp[i].delta == key)
                {
                    continue;
                }

                if (key > temp[i].setMax)
                {
                    count += 2;
                }
                else
                {
                    count += 1;
                }
            }

            result = Math.Min(result, count);
        }

        if (minOne != int.MaxValue)
        {
            var count = 0;

            for (var i = 0 ; i < temp.Length ; i++)
            {
                if (temp[i].delta == minOne)
                {
                    continue;
                }

                if (minOne > temp[i].setMax)
                {
                    count += 2;
                }
                else
                {
                    count += 1;
                }
            }

            result = Math.Min(result, count);
        }


        return result;
    }

    // 示例 1：
    //
    // 输入：nums = [1,0,1,2,4,3], k = 4
    //             [2-3,4-4,1-3]  
    // 输出：2
    //
    // 解释：
    // 我们可以执行以下操作：
    //
    // 将  nums[1] 变为 2 ，结果数组为 nums = [1,2,1,2,4,3] 。
    // 将  nums[3] 变为 3 ，结果数组为 nums = [1,2,1,3,4,3] 。
    // 整数 X 为 2 。
    //
    // 示例 2：
    //
    // 输入：nums = [0,1,2,3,3,6,5,4], k = 6
    //             [4-4,4-5,4-6,0-3]
    // 输出：2
    //
    // 解释：
    // 我们可以执行以下操作：
    //
    // 将  nums[3] 变为 0 ，结果数组为 nums = [0,1,2,0,3,6,5,4] 。
    // 将  nums[4] 变为 4 ，结果数组为 nums = [0,1,2,0,4,6,5,4] 。
    // 整数 X 为 4 。
}


// 给你一个大小为 n x n 的二维矩阵 grid ，一开始所有格子都是白色的。一次操作中，你可以选择任意下标为 (i, j) 的格子，并将第 j 列中从最上面到第 i 行所有格子改成黑色。
//
// 如果格子 (i, j) 为白色，且左边或者右边的格子至少一个格子为黑色，那么我们将 grid[i][j] 加到最后网格图的总分中去。
//
// 请你返回执行任意次操作以后，最终网格图的 最大 总分数。


public class Solution_
{
    //public long MaximumScore(int[][] grid) { }
}



