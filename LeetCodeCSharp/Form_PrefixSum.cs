namespace LeetCodeCSharp;

// 提炼 : 找到一种按顺序枚举子数组的高效方法,即维护左,枚举右,这样可以在O(n)的时间复杂度内解决问题
//       本质上是一种贪心,这种有顺序的枚举,可以在过程中进行一些统计,方便解决问题
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

// 提炼: 前缀和只是双变量问题的变体,核心思想任然是[ 枚举右，维护左 ]
//      在[ 前缀和 + 哈希表 ]中, 前缀和一般用于当前最大子数组,哈希表用于统计各种操作


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


/// <summary> 1524. 和为奇数的子数组数目 算术评级: 5 第 31 场双周赛Q2 </summary>
public class Solution_1524
{
    // 难点:子数组的奇偶性统计,会随着遍历的递进发生逆转
    [TestCase(new[] { 1, 3, 5 },             ExpectedResult = 4)]
    [TestCase(new[] { 2, 4, 6 },             ExpectedResult = 0)]
    [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7 }, ExpectedResult = 16)]
    [TestCase(new[] { 100, 100, 99, 99 },    ExpectedResult = 4)]
    public int NumOfSubarrays(int[] arr)
    {
        const int mod     = 1000000007;
        var       counter = 0L;
        var       odd     = 0; //奇数
        var       even    = 0; //偶数

        for (int i = 0, len = arr.Length ; i < len ; i++)
        {
            var isOdd = arr[i] % 2 == 1;
            counter += (isOdd ? even : odd);
            counter += isOdd ? 1 : 0;

            if (isOdd)
            {
                var temp = odd;
                odd  = even + 1;
                even = temp;
            }
            else
            {
                even++;
            }
        }

        return (int)(counter % mod);
    }

    // 官解 : 这个 sum有点像是一个falg,因为它变化是有规律的(且本题中单项不会超过100,所以不会溢出),只有两个状态,维护一个综合即可
    // 实际sum的意思是目前为止的最大数组,然后减去之前存的子数组的统计,相减即可
    public int NumOfSubarrays2(int[] arr)
    {
        const int MODULO = 1000000007;
        int       odd    = 0, even = 1;
        var       res    = 0;
        var       sum    = 0;
        for (int i = 0, len = arr.Length ; i < len ; i++)
        {
            sum += arr[i];
            res =  (res + (sum % 2 == 0 ? odd : even)) % MODULO;
            if (sum % 2 == 0)
            {
                even++;
            }
            else
            {
                odd++;
            }
        }

        return res;
    }

    // 奇偶表 : 奇偶相同则为偶,奇偶不同则为奇(加减乘都是这个规律)(正负都可)

    // 给你一个整数数组 arr , 请你返回和为 奇数 的子数组数目
    //
    // 由于答案可能会很大，请你将结果对 10^9 + 7 取余后返回
    //
    //
    //
    // 示例 1：
    //
    // 输入：arr = [1,3,5]
    // 输出：4
    // 解释：所有的子数组为 [[1],[1,3],[1,3,5],[3],[3,5],[5]] 。
    // 所有子数组的和为 [1,4,9,3,8,5].
    // 奇数和包括 [1,9,3,5] ，所以答案为 4 。
    // 示例 2 ：
    //
    // 输入：arr = [2,4,6]
    // 输出：0
    // 解释：所有子数组为 [[2],[2,4],[2,4,6],[4],[4,6],[6]] 。
    // 所有子数组和为 [2,6,12,4,10,6] 。
    // 所有子数组和都是偶数，所以答案为 0 。
    // 示例 3：
    //
    // 输入：arr = [1,2,3,4,5,6,7]
    // 输出：16
    // 示例 4：
    //
    // 输入：arr = [100,100,99,99]
    // 输出：4
    // 示例 5：
    //
    // 输入：arr = [7]
    // 输出：1
    //
    //
    // 提示：
    //
    // 1 <= arr.length <= 10^5
    // 1 <= arr[i] <= 100
}


/// <summary> 974. 和可被 K 整除的子数组 算术评级: 5 第 119 场周赛Q3 </summary>
public class Solution_974
{
    // 前缀和+哈希表标准模板题
    [TestCase(new[] { 4, 5, 0, -2, -3, 1 }, 5, ExpectedResult = 7)]
    [TestCase(new[] { 5 },                  9, ExpectedResult = 0)]
    public int SubarraysDivByK(int[] nums, int k)
    {
        // 1. 申明累计和
        var sum = 0;

        // 2. 申明结果
        var res = 0;

        // 3. 申明用于统计之前子数组中的对k的余数的数量
        Span<int> dict = stackalloc int[k];

        // 4. 初始化,表示当前索引本身的情况,处理边界
        dict[0] = 1;

        // 5. 正常遍历数组
        for (int i = 0, len = nums.Length ; i < len ; i++)
        {
            // 6. 计算前缀和
            sum += nums[i];

            // 7. 进行指定操作
            var mod = (sum % k + k) % k;

            // 7.1. 找到指令操作和哈希表之间的联系,进行计数操作,用于迭代结果
            res += dict[mod];

            // 8. 统计指令操作的数量
            dict[mod]++;
        }

        return res;
    }

    // 给定一个整数数组 nums 和一个整数 k ，返回其中元素之和可被 k 整除的非空 子数组 的数目。
    //
    // 子数组 是数组中 连续 的部分。
    //
    // 示例 1：
    //
    // 输入：nums = [4,5,0,-2,-3,1], k = 5
    // 输出：7
    // 解释：
    // 有 7 个子数组满足其元素之和可被 k = 5 整除：
    // [4, 5, 0, -2, -3, 1], [5], [5, 0], [5, 0, -2, -3], [0], [0, -2, -3], [-2, -3]
    // 示例 2:
    //
    // 输入: nums = [5], k = 9
    // 输出: 0
    //
    //
    // 提示:
    //
    // 1 <= nums.length <= 3 * 10^4
    // -10^4 <= nums[i] <= 10^4
    // 2 <= k <= 10^4
}


/// <summary> 523. 连续的子数组和 算术评级: 5 中等 </summary>
public class Solution_523
{
    // 这题虽然是模板提,但是难度在于判断边界情况,根据定义,子数组至少为2,且 0 也是倍数,这个就很恶心
    [TestCase(new[] { 23, 2, 4, 6, 7 },            6,  ExpectedResult = true)]
    [TestCase(new[] { 23, 2, 6, 4, 7 },            6,  ExpectedResult = true)]
    [TestCase(new[] { 23, 2, 6, 4, 7 },            13, ExpectedResult = false)]
    [TestCase(new[] { 0, 0 },                      1,  ExpectedResult = true)]
    [TestCase(new[] { 1, 2, 12 },                  6,  ExpectedResult = false)]
    [TestCase(new[] { 1, 2, 3 },                   5,  ExpectedResult = true)]
    [TestCase(new[] { 0, 1, 0, 3, 0, 4, 0, 4, 0 }, 5,  ExpectedResult = false)] //[0,1,0,3,0,4,0,4,0]
    [TestCase(new[] { 23, 2, 4, 6, 6 },            7,  ExpectedResult = true)]  //[23,2,4,6,6]
    public bool CheckSubarraySum(int[] nums, int k)
    {
        if (nums.Length < 2) return false;

        var dict = new Dictionary<int, bool> { { 0, true } };

        var sum = nums[0];

        for (int i = 1, len = nums.Length ; i < len ; i++)
        {
            sum += nums[i];

            dict.TryAdd((sum + k) % k, false);

            if (sum >= k && dict[(sum + k) % k]) return true;

            if (nums[i] + nums[i - 1] == 0) return true;

            dict.TryAdd((sum - nums[i] - nums[i - 1]) % k, false);

            dict[(sum - nums[i] - nums[i - 1]) % k] = true;
        }

        Console.WriteLine(sum);

        return dict[(sum + k) % k];
    }

    public bool CheckSubarraySum2(int[] nums, int k)
    {
        if (nums.Length < 2) return false;

        var dictionary = new Dictionary<int, int> { { 0, -1 } };

        var remainder = 0;
        for (int i = 0, len = nums.Length ; i < len ; i++)
        {
            remainder = (remainder + nums[i]) % k;
            if (!dictionary.TryAdd(remainder, i))
            {
                var prevIndex = dictionary[remainder];
                if (i - prevIndex >= 2)
                {
                    return true;
                }
            }
        }

        return false;
    }



    // 给你一个整数数组 nums 和一个整数 k ，如果 nums 有一个 好的子数组 返回 true ，否则返回 false：
    //
    // 一个 好的子数组 是：
    // 1. 子数组长度 至少为 2
    // 2. 子数组元素总和为 k 的倍数 (如果存在一个整数 n ，令整数  x 符合 x = n * k ，则称 x 是 k 的一个倍数。0 始终 视为 k 的一个倍数。
    //
    //
    //
    // 示例 1：
    //
    // 输入：nums = [23,2,4,6,7], k = 6
    // 输出：true
    // 解释：[2,4] 是一个大小为 2 的子数组，并且和为 6 。
    // 示例 2：
    //
    // 输入：nums = [23,2,6,4,7], k = 6
    // 输出：true
    // 解释：[23, 2, 6, 4, 7] 是大小为 5 的子数组，并且和为 42 。 
    // 42 是 6 的倍数，因为 42 = 7 * 6 且 7 是一个整数。
    // 示例 3：
    //
    // 输入：nums = [23,2,6,4,7], k = 13
    // 输出：false
    //
    //
    // 提示：
    //
    // 1 <= nums.length <= 105
    // 0 <= nums[i] <= 109
    // 0 <= sum(nums[i]) <= 2^31 - 1
    // 1 <= k <= 2^31 - 1
}


/// <summary> 437. 路径总和 III 算术评级: 5 中等 </summary>
public class Solution_437
{
    public int PathSum(TreeNode root, int targetSum)
    {
        var dict = new Dictionary<long, int> { { 0, 1 } };
        var res  = 0;

        Dfs(root, 0);

        return res;

        void Dfs(TreeNode node, long sum)
        {
            if (node == null) return;

            sum += node.val;
            res += dict.GetValueOrDefault(sum - targetSum);

            dict.TryAdd(sum, 0);

            dict[sum]++;

            Dfs(node.left,  sum);
            Dfs(node.right, sum);

            dict[sum]--;
        }
    }

    [Test]
    public void METHOD()
    {
        int?[] tree = [10, 5, -3, 3, 2, null, 11, 3, -2, null, 1];

        var root = UnitTest.CreateTree(tree);

        var result = PathSum(root, 8);

        Console.WriteLine(result);
    }


    // 给定一个二叉树的根节点 root ，和一个整数 targetSum ，求该二叉树里节点值之和等于 targetSum 的 路径 的数目。
    //
    // 路径 不需要从根节点开始，也不需要在叶子节点结束，但是路径方向必须是向下的（只能从父节点到子节点）。
    //
    //
    //
    // 示例 1：
    //
    //
    //
    // 输入：root = [10,5,-3,3,2,null,11,3,-2,null,1], targetSum = 8
    // 输出：3
    // 解释：和等于 8 的路径有 3 条，如图所示。
    // 示例 2：
    //
    // 输入：root = [5,4,8,11,null,13,4,7,2,null,null,5,1], targetSum = 22
    // 输出：3
    //
    //
    // 提示:
    //
    // 二叉树的节点个数的范围是 [0,1000]
    // -109 <= Node.val <= 10^9 
    // -1000 <= targetSum <= 1000 
}


/// <summary> 2588. 统计美丽子数组数目 算术评级: 5 第 336 场周赛Q3 </summary>
public class Solution_2588
{
    [TestCase(new[] { 4, 3, 1, 2, 4 }, ExpectedResult = 2)]
    [TestCase(new[] { 1, 10, 4 },      ExpectedResult = 0)]
    public long BeautifulSubarrays(int[] nums)
    {
        if (nums.Length < 2) return 0;

        var dict = new Dictionary<int, int> { { 0, 1 } };

        var xor = nums[0];
        var res = 0L;

        for (int i = 1, len = nums.Length ; i < len ; i++)
        {
            var cache_xor = xor;
            xor ^= nums[i];

            dict.TryAdd(xor, 0);

            res += dict[xor];

            dict.TryAdd(cache_xor, 0);
            dict[cache_xor]++;
        }

        return res;
    }

    [TestCase(new[] { 4, 3, 1, 2, 4 }, ExpectedResult = 2)]
    [TestCase(new[] { 1, 10, 4 },      ExpectedResult = 0)]
    public long BeautifulSubarrays2(int[] nums)
    {
        var ans = 0L;
        var sum = 0;
        var cnt = new Dictionary<int, int> { { sum, 1 } };
        foreach (var x in nums)
        {
            sum ^= x;
            ans += cnt.GetValueOrDefault(sum, 0);
            cnt.TryAdd(sum, 0);
            cnt[sum]++;
        }

        return ans;
    }



    // 给你一个下标从0开始的整数数组nums。
    // 每次操作中，你可以:
    // 选择两个满足 0 <= i, j < nums.length的不同下标 i 和 j
    // 选择一个非负整数k ，满足 nums[i] 和 nums[j] 在二进制下的第 k 位（下标编号从 0 开始）是 1
    // 将 nums[i] 和 nums[j] 都减去 2k 。
    // 如果一个子数组内执行上述操作若干次后，该子数组可以变成一个全为 0 的数组，那么我们称它是一个 美丽 的子数组。

    // 0001 0101
    // xor
    // 0001 0001
    // 请你返回数组 nums 中 美丽子数组 的数目。
    //
    // 子数组是一个数组中一段连续 非空 的元素序列。
    //
    //
    //
    // 示例 1：
    //
    // 输入：nums = [4,3,1,2,4]
    // 输出：2
    // 解释：nums 中有 2 个美丽子数组：[4,3,1,2,4] 和 [4,3,1,2,4] 。
    // - 按照下述步骤，我们可以将子数组 [3,1,2] 中所有元素变成 0 ：
    // - 选择 [3, 1, 2] 和 k = 1 。将 2 个数字都减去 21 ，子数组变成 [1, 1, 0] 。
    // - 选择 [1, 1, 0] 和 k = 0 。将 2 个数字都减去 20 ，子数组变成 [0, 0, 0] 。
    // - 按照下述步骤，我们可以将子数组 [4,3,1,2,4] 中所有元素变成 0 ：
    // - 选择 [4, 3, 1, 2, 4] 和 k = 2 。将 2 个数字都减去 22 ，子数组变成 [0, 3, 1, 2, 0] 。
    // - 选择 [0, 3, 1, 2, 0] 和 k = 0 。将 2 个数字都减去 20 ，子数组变成 [0, 2, 0, 2, 0] 。
    // - 选择 [0, 2, 0, 2, 0] 和 k = 1 。将 2 个数字都减去 21 ，子数组变成 [0, 0, 0, 0, 0] 。
    // 示例 2：
    //
    // 输入：nums = [1,10,4]
    // 输出：0
    // 解释：nums 中没有任何美丽子数组。
    //
    //
    // 提示：
    //
    // 1 <= nums.length <= 10^5
    // 0 <= nums[i] <= 10^6
}


/// <summary> 525. 连续数组 算术评级: 5 </summary>
public class Solution_525
{
    [TestCase(new[] { 0, 1, 0 }, ExpectedResult = 2)]
    public int FindMaxLength(int[] nums)
    {
        var dict = new Dictionary<int, int> { { 0, 0 } };

        var balance = 0;
        var res     = 0;

        for (int i = 0, len = nums.Length ; i < len ; i++)
        {
            balance += nums[i] == 0 ? -1 : 1; // -1 , 0 , -1 

            res = Math.Max(res, i + 1 - dict.GetValueOrDefault(balance, int.MaxValue));

            dict.TryAdd(balance, int.MaxValue);
            dict[balance] = Math.Min(dict[balance], i + 1);
        }

        return res;
    }

    // 给定一个二进制数组 nums , 找到含有相同数量的 0 和 1 的最长连续子数组，并返回该子数组的长度。
    //
    // 示例 1:
    //
    // 输入: nums = [0,1]
    // 输出: 2
    // 说明: [0, 1] 是具有相同数量 0 和 1 的最长连续子数组。
    // 示例 2:
    //
    // 输入: nums = [0,1,0]
    // 输出: 2
    // 说明: [0, 1] (或 [1, 0]) 是具有相同数量0和1的最长连续子数组。
    //
    //
    // 提示：
    //
    // 1 <= nums.length <= 10^5
    // nums[i] 不是 0 就是 1
}


/// <summary> 3026. 最大好子数组和 算术评级: 6 第 123 场双周赛Q3 1817 </summary>
public class Solution_3026
{
    [TestCase(new[] { 1, 2, 3, 4, 5, 6 }, 1, ExpectedResult = 11)]
    [TestCase(new[] { -1, 3, 2, 4, 5 },   3, ExpectedResult = 11)]
    [TestCase(new[] { -1, -2, -3, -4 },   2, ExpectedResult = -6)]
    public long MaximumSubarraySum(int[] nums, int k)
    {
        // key:指定的元素值,value:到这个元素值的和
        var dict = new Dictionary<long, long>();
        var ans  = long.MinValue;
        var sum  = 0L;

        for (int i = 0, len = nums.Length ; i < len ; i++)
        {
            // [1, 2, 3, 4, 5, 6], k = 1

            dict.TryAdd(nums[i], long.MaxValue);

            dict[nums[i]] = Math.Min(dict[nums[i]], sum);

            sum += nums[i];

            if (dict.TryGetValue(nums[i] - k, out var value))
            {
                ans = Math.Max(ans, sum - value);
            }

            if (dict.TryGetValue(nums[i] + k, out value))
            {
                ans = Math.Max(ans, sum - value);
            }


            // num - x = k
            //
            // num > x && x == num - k || num < x && x == num + k
        }


        return ans == long.MinValue ? 0 : ans;
    }

    //  ABS(a-b) = k
    //=> a-b = k || b-a = k

    // a > b && b = a - k || a < b && b = a + k


    // 给你一个长度为 n 的数组 nums 和一个 正 整数 k 。
    // 
    // 如果 nums 的一个子数组中，第一个元素和最后一个元素差的绝对值恰好为k ，我们称这个子数组为 好 的
    // 换句话说，如果子数组 nums[i..j] 满足 |nums[i] - nums[j]| == k ，那么它是一个好子数组。
    //
    // 请你返回 nums 中 好 子数组的 最大 和，如果没有好子数组，返回 0 。
    //
    //
    //
    // 示例 1：
    //
    // 输入：nums = [1,2,3,4,5,6], k = 1
    // 输出：11
    // 解释：好子数组中第一个元素和最后一个元素的差的绝对值必须为 1 。好子数组有 [1,2] ，[2,3] ，[3,4] ，[4,5] 和 [5,6] 。最大子数组和为 11 ，对应的子数组为 [5,6] 。
    // 示例 2：
    //
    // 输入：nums = [-1,3,2,4,5], k = 3
    // 输出：11
    // 解释：好子数组中第一个元素和最后一个元素的差的绝对值必须为 3 。好子数组有 [-1,3,2] 和 [2,4,5] 。最大子数组和为 11 ，对应的子数组为 [2,4,5] 。
    // 示例 3：
    //
    // 输入：nums = [-1,-2,-3,-4], k = 2
    // 输出：-6
    // 解释：好子数组中第一个元素和最后一个元素的差的绝对值必须为 2 。好子数组有 [-1,-2,-3] 和 [-2,-3,-4] 。最大子数组和为 -6 ，对应的子数组为 [-1,-2,-3] 。
    //
    //
    // 提示：
    //
    // 2 <= nums.length <= 10^5
    // -10^9 <= nums[i] <= 10^9
    // 1 <= k <= 10^9
}


//[投了] 1850的题确实难
/// <summary> 1477. 找两个和为目标值且不重叠的子数组 算术评级: 6第 28 场双周赛Q3 1851 </summary>
public class Solution_1477
{
    [TestCase(new[] { 3, 2, 2, 4, 3 },                   3, ExpectedResult = 2)]
    [TestCase(new[] { 7, 3, 4, 7 },                      7, ExpectedResult = 2)]
    [TestCase(new[] { 4, 3, 2, 6, 2, 3, 4 },             6, ExpectedResult = -1)] // 只有一个是不行的
    [TestCase(new[] { 5, 5, 4, 4, 5 },                   3, ExpectedResult = -1)] // 一个都没有的情况
    [TestCase(new[] { 1, 2, 2, 3, 2, 6, 7, 2, 1, 4, 8 }, 5, ExpectedResult = 4)]
    public int MinSumOfLengths(int[] arr, int target)
    {
        // 1. key : 前缀和 value : 数组右区间
        var dict = new Dictionary<int, int> { { 0, 0 } };
        var ress = new List<(int, int)>();

        // 2. 申明结果
        var sum = 0;

        // 3. [3,2,2,4,3], target = 3
        for (int i = 0, len = arr.Length ; i < len ; i++)
        {
            sum += arr[i];

            dict.TryAdd(sum, i + 1);
            dict[sum] = i + 1;

            if (dict.TryGetValue(sum - target, out var value))
            {
                ress.Add((value, i));
            }
        }

        if (ress.Count <= 1) return -1;


        var res = int.MaxValue;

        for (int i = 0, len = ress.Count ; i < len ; i++)
        {
            var (left1, right1) = ress[i];

            for (int j = i + 1 ; j < len ; j++)
            {
                var (left2, right2) = ress[j];

                if (left1 > right2 || left2 > right1)
                {
                    res = Math.Min(res, right1 - left1 + right2 - left2 + 2);
                }
            }
        }


        return res == int.MaxValue ? -1 : res;
    }


    // [1,2,2,3,2,6,7,2,1,4,8]  target = 5
    // 

    // 给你一个整数数组 arr 和一个整数值 target
    //
    // 请你在 arr 中找 两个互不重叠的子数组 且它们的和都等于 target 。可能会有多种方案，请你返回满足要求的两个子数组长度和的 最小值
    //
    // 请返回满足要求的最小长度和，如果无法找到这样的两个子数组，请返回 -1
    //
    //
    // 示例 1：
    //
    // 输入：arr = [3,2,2,4,3], target = 3
    // 输出：2
    // 解释：只有两个子数组和为 3 （[3] 和 [3]）。它们的长度和为 2 。
    // 示例 2：
    //
    // 输入：arr = [7,3,4,7], target = 7
    // 输出：2
    // 解释：尽管我们有 3 个互不重叠的子数组和为 7 （[7], [3,4] 和 [7]），但我们会选择第一个和第三个子数组，因为它们的长度和 2 是最小值。
    // 示例 3：
    //
    // 输入：arr = [4,3,2,6,2,3,4], target = 6
    // 输出：-1
    // 解释：我们只有一个和为 6 的子数组。
    // 示例 4：
    //
    // 输入：arr = [5,5,4,4,5], target = 3
    // 输出：-1
    // 解释：我们无法找到和为 3 的子数组。
    // 示例 5：
    //
    // 输入：arr = [3,1,1,1,5,1,2,1], target = 3
    // 输出：3
    // 解释：注意子数组 [1,2] 和 [2,1] 不能成为一个方案因为它们重叠了。
    //
    //
    // 提示：
    //
    // 1 <= arr.length <= 10^5
    // 1 <= arr[i] <= 1000
    // 1 <= target <= 10^8

    //测试样例不够充分，刚刚测过一个能AC的代码，但是结果是错误的：
    // [1 ,2 ,1 ,3, 1]
    // 4 
    //
    // 预期5，但是输出-1，仍然能过。
    //
    // 该样例特殊性：
    // 子数组有： [1,2,1] [1,3][3,1]，排序之后是 [1,3] [3,1] [1,2,1] 
    // 很多判断 [1,3] [3,1] 重合，[1,3] [1,2,1]也重合，结果返回-1，
    // 而实际结果应为5。 [3,1] 与 [1,2,1]


    // 双指针写法
    [TestCase(new[] { 3, 2, 2, 4, 3 },       3, ExpectedResult = 2)]
    [TestCase(new[] { 7, 3, 4, 7 },          7, ExpectedResult = 2)]
    [TestCase(new[] { 4, 3, 2, 6, 2, 3, 4 }, 6, ExpectedResult = -1)] // 只有一个是不行的
    [TestCase(new[] { 5, 5, 4, 4, 5 },       3, ExpectedResult = -1)] // 一个都没有的情况
    public int MinSumOfLengths2(int[] arr, int target)
    {
        int sum = 0, minSumOfLens = int.MaxValue;

        Span<int> dp = stackalloc int[arr.Length + 1];

        // dp[i]表示区间[0,i)之间最短的和为target的子数组，先初始化为一个较大的数表示不存在。因为会做加法运算，不能初始化为INT_MAX
        dp[0] = arr.Length + 1;

        for (int right = 0, left = 0, length = arr.Length ; right < length ; ++right)
        {
            sum += arr[right]; //累计前缀和

            while (sum > target)
            {
                sum -= arr[left++];
            }

            if (sum == target)
            {
                int len = right - left + 1; // 区间[left,right]是一个和为target的子数组，该子数组长度为len

                minSumOfLens = Math.Min(minSumOfLens, len + dp[left]); // 如果有解，我们遍历了所有的第二个子数组，同时加上它前面长度最短的第一个子数组就是答案

                dp[right + 1] = Math.Min(dp[right], len); // 更新dp，取长度更小的一个
            }
            else
            {
                dp[right + 1] = dp[right]; // 不是一个和为target的子数组，dp[i]保持不变
            }
        }

        return minSumOfLens > arr.Length ? -1 : minSumOfLens; // 大于size说明没有找到两个不重叠的子数组
    }
}


/***************************************************  距离和  **********************************************************/
// 距离和一般指的是两个数之间的距离和,这个距离和可以是绝对值,也可以是平方和,也可以是其他的,特征是需要能有条理的统计距离的正负
// 注意：这类题目一般都比较大，尤其是对本身坐标相乘的时候，尽量使用long，防止溢出
// 写法上，一般需要两次遍历，第一次遍历是统计左边的距离和，第二次遍历是统计右边的距禿和，然后再进行计算


///<summary> 1685. 有序数组中差绝对值之和 算术评级: 6 第 41 场双周赛Q2-1496 </summary>
public class Solution_1685
{
    //其实可以再优化,但是感觉没必要了

    [TestCase(new[] { 2, 3, 5 }, ExpectedResult = new[] { 4, 3, 5 })]
    public int[] GetSumAbsoluteDifferences(int[] nums)
    {
        var result = new int[nums.Length];

        for (int i = 0, len = nums.Length, sum = 0 ; i < len ; i++)
        {
            result[i] =  (2 * i - (len - 1)) * nums[i] - sum;
            sum       += nums[i];
        }

        for (int i = nums.Length - 1, sum = 0 ; i >= 0 ; i--)
        {
            result[i] += sum;
            sum       += nums[i];
        }

        return result;
    }

    // [a1,a2,a3,a4,a5,a6]  a1 >= a2 >= a3 >= a4
    // 
    // r3 = a3-a1 + a3-a2 + || + a4-a3 + a5-a3 + a6-a3
    // r3 = 左 + 右 ;
    // 左 : (3-1)a3 - (a1+a2)    => (Index)*an - (a1+...+a(n-1))
    // 右 : (a4+a5+a6)-(6-3)a3   => (a(n+1)+...+a(Len-1)) - (Len-Index-1)*an
    // 左+右 = (Index)*an - (a1+...+a(n-1)) + (a(n+1)+...+a(Len-1)) - (Len-Index-1)*an
    //      = (2Index - (len - 1)) * an + (a(n+1)+...+a(Len-1)) - (a1+...+a(n-1))
    //    0,1,2
    //   [2,3,5]
    //res[4,3,5] 
    //  len = 3
    //   r1 = -2*2+3+5 = 4
    //   r2 = -2*1+5 = 3
    //


    // 给你一个 非递减 有序整数数组 nums
    // 请你建立并返回一个整数数组 result，它跟 nums 长度相同，
    // 且result[i] 等于 nums[i] 与数组中所有其他元素差的绝对值之和
    //
    // 换句话说， result[i] 等于 sum(|nums[i]-nums[j]|) ，其中 0 <= j < nums.length 且 j != i （下标从 0 开始）。
    //
    //
    //
    // 示例 1：
    //
    // 输入：nums = [2,3,5]
    // 输出：[4,3,5] 
    // sum = 10
    // 10-2*3 = 4
    //
    // 解释：假设数组下标从 0 开始，那么
    //     result[0] = |2-2| + |2-3| + |2-5| = 0 + 1 + 3 = 4，
    // result[1] = |3-2| + |3-3| + |3-5| = 1 + 0 + 2 = 3，
    // result[2] = |5-2| + |5-3| + |5-5| = 3 + 2 + 0 = 5。
    // 示例 2：
    //
    // 输入：nums = [1,4,6,8,10]
    // 输出：[24,15,13,15,21]
    //
    //
    // 提示：
    //
    // 2 <= nums.length <= 10^5
    // 1 <= nums[i] <= nums[i + 1] <= 10^4
}


/// <summary> 2615. 等值距离和 算术评级: 6 第 340 场周赛Q2-1793 </summary>
public class Solution_2615
{
    [TestCase(new[] { 1, 3, 1, 1, 2 }, ExpectedResult = new long[] { 5, 0, 3, 4, 0 })]
    public long[] Distance(int[] nums)
    {
        var result = new long[nums.Length];
        var dict   = new Dictionary<int, (long, long)>();

        for (int i = 0, len = nums.Length ; i < len ; i++)
        {
            dict.TryAdd(nums[i], (0, 0));
            result[i]     = dict[nums[i]].Item1 * i - dict[nums[i]].Item2;
            dict[nums[i]] = (dict[nums[i]].Item1 + 1, dict[nums[i]].Item2 + i);
        }

        dict.Clear();

        for (var i = nums.Length - 1 ; i >= 0 ; i--)
        {
            dict.TryAdd(nums[i], (0, 0));
            dict[nums[i]] =  (dict[nums[i]].Item1 + 1, dict[nums[i]].Item2 + i);
            result[i]     += dict[nums[i]].Item2 - dict[nums[i]].Item1 * i;
        }


        return result;
    }



    // 给你一个下标从 0 开始的整数数组 nums 。
    // 现有一个长度等于 nums.length 的数组 arr 。
    // 对于满足 nums[j] == nums[i] 且 j != i 的所有 j ，arr[i] 等于所有 |i - j| 之和。如果不存在这样的 j ，则令 arr[i] 等于 0 。
    //
    // 返回数组 arr 。
    //
    //
    //
    // 示例 1：
    //
    // 输入：nums = [1,3,1,1,2]
    // 输出：[5,0,3,4,0]
    // 解释：
    // i = 0 ，nums[0] == nums[2] 且 nums[0] == nums[3] 。因此，arr[0] = |0 - 2| + |0 - 3| = 5 。 
    // i = 1 ，arr[1] = 0 因为不存在值等于 3 的其他下标。
    // i = 2 ，nums[2] == nums[0] 且 nums[2] == nums[3] 。因此，arr[2] = |2 - 0| + |2 - 3| = 3 。
    // i = 3 ，nums[3] == nums[0] 且 nums[3] == nums[2] 。因此，arr[3] = |3 - 0| + |3 - 2| = 4 。 
    // i = 4 ，arr[4] = 0 因为不存在值等于 2 的其他下标。
    // 示例 2：
    //
    // 输入：nums = [0,5,3]
    // 输出：[0,0,0]
    // 解释：因为 nums 中的元素互不相同，对于所有 i ，都有 arr[i] = 0 。
    //
    //
    // 提示：
    //
    // 1 <= nums.length <= 105
    // 0 <= nums[i] <= 109
}


/***************************************************  异或和  **********************************************************/
// 异或和核心公式：xor[a..b] = xor[0..a-1] ^ xor[0..b] 
// 推导原理 : a^b^b = a 且 连续异或具有交换律,他是无序的


///<summary> 1310. 子数组异或查询 算术评级: 4 第 170 场周赛Q2-1460 </summary>
public class Solution_1310
{
    public int[] XorQueries(int[] arr, int[][] queries)
    {
        var result = new int[queries.Length];
        var xor    = new int[arr.Length];

        xor[0] = arr[0];

        for (int i = 1, len = arr.Length ; i < len ; i++)
        {
            xor[i] = xor[i - 1] ^ arr[i];
        }

        for (int i = 0, len = queries.Length ; i < len ; i++)
        {
            result[i] = queries[i][0] == 0 ? xor[queries[i][1]] : xor[queries[i][1]] ^ xor[queries[i][0] - 1];
        }

        return result;
    }


    public class PrefixXor
    {
        public static int[] ComputePrefixXor(int[] arr)
        {
            var n         = arr.Length;
            var prefixXor = new int[n];
            prefixXor[0] = arr[0];
            for (var i = 1 ; i < n ; i++)
            {
                prefixXor[i] = prefixXor[i - 1] ^ arr[i];
            }

            return prefixXor;
        }

        public static int QueryXor(int[] prefixXor, int L, int R)
        {
            if (L == 0)
            {
                return prefixXor[R];
            }

            return prefixXor[R] ^ prefixXor[L - 1];
        }
    }


    // 有一个正整数数组 arr，现给你一个对应的查询数组 queries，其中 queries[i] = [Li, Ri]
    //
    // 对于每个查询 i，请你计算从 Li 到 Ri 的 XOR 值（即 arr[Li] xor arr[Li+1] xor ... xor arr[Ri]）作为本次查询的结果。
    //
    // 并返回一个包含给定查询 queries 所有结果的数组
    // Xor : 异或
    // 异或运算是一个二进制运算，用符号XOR或者^表示，其运算法则是对运算符两侧数的每一个二进制位，同值取0，异值取1。
    // 10011 -- 10011
    // 10101 -- 00110
    // 10110 -- 00010
    // 示例 1：
    //
    // 输入：arr = [1,3,4,8], queries = [[0,1],[1,2],[0,3],[3,3]]
    // 输出：[2,7,14,8] 
    // 解释：
    // 数组中元素的二进制表示形式是：
    // 1 = 0001 
    // 3 = 0011 
    // 4 = 0100 
    // 8 = 1000 
    // 查询的 XOR 值为：
    // [0,1] = 1 xor 3 = 2 
    // [1,2] = 3 xor 4 = 7 
    // [0,3] = 1 xor 3 xor 4 xor 8 = 14 
    // [3,3] = 8
    // 示例 2：
    //
    // 输入：arr = [4,8,2,10], queries = [[2,3],[1,3],[0,0],[0,3]]
    // 输出：[8,0,4,4]
    //
    //
    // 提示：
    //
    // 1 <= arr.length <= 3 * 10^4
    // 1 <= arr[i] <= 10^9
    // 1 <= queries.length <= 3 * 10^4
    // queries[i].length == 2
    // 0 <= queries[i][0] <= queries[i][1] < arr.length
}


/// <summary> 1177. 构建回文串检测 算术评级: 4 第 152 场周赛Q3-1848 </summary>
public class Solution_1177
{
    public IList<bool> CanMakePaliQueries(string str, int[][] queries)
    {
        var res = new bool[queries.Length];
        var bit = 0;
        var xor = new int[str.Length];

        xor[0] = 1 << (str[0] - 'a');

        for (int i = 1, len = str.Length ; i < len ; i++)
        {
            xor[i] = xor[i - 1] ^ (1 << (str[i] - 'a'));
        }

        foreach (var entry in queries)
        {
            var left  = entry[0];
            var right = entry[1];
            var k     = entry[2];

            var xorLR = left == 0 ? xor[right] : xor[right] ^ xor[left - 1];

            var oneCount = 0;

            for (int i = 0 ; i < 26 ; i++)
            {
                if ((xorLR & (1 << i)) != 0)
                {
                    oneCount++;
                }
            }

            res[bit++] = oneCount / 2 <= k;
        }


        return res;
    }

    // 
    // 给你一个字符串 s，请你对 s 的子串进行检测。
    //
    // 每次检测，待检子串都可以表示为 queries[i] = [left, right, k]。
    // 我们可以 重新排列 子串 s[left], ..., s[right]，并从中选择 最多 k 项替换成任何小写英文字母
    // 如果在上述检测过程中，子串可以变成回文形式的字符串，那么检测结果为 true，否则结果为 false。
    //
    // 返回答案数组 answer[]，其中 answer[i] 是第 i 个待检子串 queries[i] 的检测结果。
    //
    // 注意：在替换时，子串中的每个字母都必须作为 独立的 项进行计数，也就是说，如果 s[left..right] = "aaa" 且 k = 2，我们只能替换其中的两个字母。（另外，任何检测都不会修改原始字符串 s，可以认为每次检测都是独立的）
    //
    //
    //
    // 示例：
    //
    // 输入：s = "abcda", queries = [[3,3,0],[1,2,0],[0,3,1],[0,3,2],[0,4,1]]
    // 输出：[true,false,false,true,true]
    // 解释：
    // queries[0] : 子串 = "d"，回文。
    // queries[1] : 子串 = "bc"，不是回文。
    // queries[2] : 子串 = "abcd"，只替换 1 个字符是变不成回文串的。
    // queries[3] : 子串 = "abcd"，可以变成回文的 "abba"。 也可以变成 "baab"，先重新排序变成 "bacd"，然后把 "cd" 替换为 "ab"。
    // queries[4] : 子串 = "abcda"，可以变成回文的 "abcba"。
    //
    //
    // 提示：
    //
    // 1 <= s.length, queries.length <= 10^5
    // 0 <= queries[i][0] <= queries[i][1] < s.length
    // 0 <= queries[i][2] <= s.length
    //     s 中只有小写英文字母
}


/*************************************************  二维前缀和  *********************************************************/
// 核心公式 :
// 前缀和生成 : prefixSum[i][j] = prefixSum[i-1][j] + prefixSum[i][j-1] - prefixSum[i-1][j-1] + matrix[i][j]
// 区域和计算 : sumRegion(row1, col1, row2, col2) = prefixSum[row2][col2] - prefixSum[row1-1][col2] - prefixSum[row2][col1-1] + prefixSum[row1-1][col1-1]
// 评价 : 有意思的几何知识


/// <summary>[标准模板] 304. 二维区域和检索 - 矩阵不可变 算术评级: 5 中等 </summary>
public class Solution_304
{
    public class NumMatrix
    {
        int[,] prefixSum;

        public NumMatrix(int[][] matrix)
        {
            var row = matrix.Length;
            var col = matrix[0].Length;

            prefixSum = new int[row + 1, col + 1]; //扩大是为了防止计算[0,0]的时候越界

            for (int i = 1 ; i <= row ; i++)
            {
                for (int j = 1 ; j <= col ; j++)
                {
                    prefixSum[i, j] = prefixSum[i - 1, j] + prefixSum[i, j - 1] - prefixSum[i - 1, j - 1] + matrix[i - 1][j - 1];
                }
            }
        }

        public int SumRegion(int row1, int col1, int row2, int col2)
        {
            //此时如果row1=0或者col1=0，会越界，所以需要特殊处理
            return prefixSum[row2 + 1, col2 + 1] - prefixSum[row1, col2 + 1] - prefixSum[row2 + 1, col1] + prefixSum[row1, col1];
        }
    }


    // 给定一个二维矩阵 matrix，以下类型的多个请求：
    //
    // 计算其子矩形范围内元素的总和，该子矩阵的 左上角 为 (row1, col1) ，右下角 为 (row2, col2) 。
    // 实现                   NumMatrix 类：
    //
    // NumMatrix(int[][] matrix) 给定整数矩阵                                                                       matrix 进行初始化
    // int sumRegion(int row1, int col1, int row2, int col2) 返回 左上角 (row1, col1) 、右下角 (row2, col2) 所描述的子矩阵的元素 总和 。
    //  
    //
    // 示例 1：
    //
    //
    //
    // 输入: 
    // ["NumMatrix","sumRegion","sumRegion","sumRegion"]
    // [[[[3,0,1,4,2],[5,6,3,2,1],[1,2,0,1,5],[4,1,0,1,7],[1,0,3,0,5]]],[2,1,4,3],[1,1,2,2],[1,2,2,4]]
    // 输出: 
    // [null, 8, 11, 12]
    //
    // 解释:
    // NumMatrix numMatrix = new NumMatrix([[3,0,1,4,2],[5,6,3,2,1],[1,2,0,1,5],[4,1,0,1,7],[1,0,3,0,5]]);
    // numMatrix.sumRegion(2, 1, 4, 3); // return 8 (红色矩形框的元素总和)
    // numMatrix.sumRegion(1, 1, 2, 2); // return 11 (绿色矩形框的元素总和)
    // numMatrix.sumRegion(1, 2, 2, 4); // return 12 (蓝色矩形框的元素总和)


    /**
     * Your NumMatrix object will be instantiated and called as such:
     * NumMatrix obj = new NumMatrix(matrix);
     * int param_1 = obj.SumRegion(row1,col1,row2,col2);
     */
}


/// <summary>[模板] 1314. 矩阵区域和 算术评级: 5 第 17 场双周赛Q2-1484 </summary>
public class Solution_1314
{
    public int[][] MatrixBlockSum(int[][] mat, int k)
    {
        var prefixSum = new int[mat.Length + 1, mat[0].Length + 1];

        for (int i = 1 ; i <= mat.Length ; i++)
        {
            for (int j = 1 ; j <= mat[0].Length ; j++)
            {
                prefixSum[i, j] = prefixSum[i - 1, j] + prefixSum[i, j - 1] - prefixSum[i - 1, j - 1] + mat[i - 1][j - 1];
            }
        }

        var result = new int[mat.Length][];

        for (int i = 0 ; i < mat.Length ; i++)
        {
            result[i] = new int[mat[0].Length];

            for (int j = 0 ; j < mat[0].Length ; j++)
            {
                int row1 = Math.Max(0, i          - k);
                int col1 = Math.Max(0, j          - k);
                int row2 = Math.Min(mat.Length    - 1, i + k);
                int col2 = Math.Min(mat[0].Length - 1, j + k);

                // 之前+1了,所以这里也要+1
                result[i][j] = prefixSum[row2 + 1, col2 + 1] - prefixSum[row1, col2 + 1] - prefixSum[row2 + 1, col1] + prefixSum[row1, col1];
            }
        }

        return result;
    }



    // 给你一个 m x n 的矩阵 mat 和一个整数 k ，请你返回一个矩阵 answer ，其中每个 answer[i][j] 是所有满足下述条件的元素 mat[r][c] 的和： 
    //
    // i - k <= r <= i + k, 
    // j - k <= c <= j + k
    // 且 (r, c) 在矩阵内。
    //
    //
    // 示例 1：
    //
    // 输入：mat = [
    //             [1,2,3],
    //             [4,5,6],
    //             [7,8,9]
    //            ], k = 1
    // 输出：[
    //       [12,21,16]
    //      ,[27,45,33]
    //      ,[24,39,28]
    //      ]
    // 示例 2：
    //
    // 输入：mat = [[1,2,3],[4,5,6],[7,8,9]], k = 2
    // 输出：[[45,45,45],[45,45,45],[45,45,45]]
    //
    //
    // 提示：
    //
    // m == mat.length
    //     n == mat[i].length
    // 1 <= m, n, k <= 100
    // 1 <= mat[i][j] <= 100
}


/**************************************************  其他前缀和思想  *****************************************************/


///<summary> 3153. 所有数对中数位不同之和 算术评级: 5 第 398 场周赛Q3-1645 </summary>
public class Solution_3153
{
    [TestCase(new[] { 13, 23, 12 },     ExpectedResult = 4)]
    [TestCase(new[] { 10, 10, 10, 10 }, ExpectedResult = 0)]
    public long SumDigitDifferences(int[] nums)
    {
        var  digNum = nums[0].ToString().Length;
        var  dict   = new int[digNum, 10];
        long result = digNum * nums.Length * (nums.Length - 1) / 2;

        foreach (var num in nums)
        {
            var str = num.ToString();
            for (var i = 0 ; i < digNum ; i++)
            {
                var key = str[i] - '0';
                dict[i, key]++;

                if (dict[i, key] >= 2)
                {
                    result -= dict[i, key] - 1;
                }
            }
        }


        return result;
    }

    // 
    //
    // 你有一个数组 nums ，它只包含 正 整数，所有正整数的数位长度都 相同 。
    //
    // 两个整数的 数位不同 指的是两个整数 相同 位置上不同数字的数目。
    //
    // 请你返回 nums 中 所有 整数对里，数位不同之和。
    //
    //
    //
    // 示例 1：
    //
    // 输入：nums = [13,23,12]
    // 1 2 1 = 2
    // 3 3 2 = 2
    //
    // 输出：4
    //
    // 解释：
    // 计算过程如下：
    // - 13 和 23 的数位不同为 1 。
    // - 13 和 12 的数位不同为 1 。
    // - 23 和 12 的数位不同为 2 。
    // 所以所有整数数对的数位不同之和为 1 + 1 + 2 = 4 。
    //
    // 示例 2：
    //
    // 输入：nums = [10,10,10,10]
    //
    // 输出：0
    //
    // 解释：
    // 数组中所有整数都相同，所以所有整数数对的数位不同之和为 0 。
    //
    //
    //
    // 提示：
    //
    // 2 <= nums.length <= 10^5
    // 1 <= nums[i] < 10^9
    // nums 中的整数都有相同的数位长度
}


///<summary> 1895. 最大的幻方 算术评级: 5 第54场双周赛 Q3-1781 </summary>
public class Solution_1895
{
    public int LargestMagicSquare(int[][] grid)
    {
        int m = grid.Length, n = grid[0].Length;

        var rowsum = new int[m, n]; // 每一行的前缀和
        var colsum = new int[m, n]; // 每一列的前缀和

        for (int i = 0 ; i < m ; ++i)
        {
            rowsum[i, 0] = grid[i][0];

            for (int j = 1 ; j < n ; ++j)
            {
                rowsum[i, j] = rowsum[i, j - 1] + grid[i][j];
            }
        }

        for (int j = 0 ; j < n ; ++j)
        {
            colsum[0, j] = grid[0][j];

            for (int i = 1 ; i < m ; ++i)
            {
                colsum[i, j] = colsum[i - 1, j] + grid[i][j];
            }
        }

        // 从大到小枚举边长 edge
        for (int edge = Math.Min(m, n) ; edge >= 2 ; --edge)
        {
            // 枚举正方形的左上角位置 (i,j)
            for (int i = 0 ; i + edge <= m ; ++i)
            {
                for (int j = 0 ; j + edge <= n ; ++j)
                {
                    // 计算每一行、列、对角线的值应该是多少（以第一行为样本）
                    int  stdsum = rowsum[i, j + edge - 1] - (j != 0 ? rowsum[i, j - 1] : 0);
                    bool check  = true;
                    // 枚举每一行并用前缀和直接求和
                    // 由于我们已经拿第一行作为样本了，这里可以跳过第一行
                    for (int ii = i + 1 ; ii < i + edge ; ++ii)
                    {
                        if (rowsum[ii, j + edge - 1] - (j != 0 ? rowsum[ii, j - 1] : 0) != stdsum)
                        {
                            check = false;
                            break;
                        }
                    }

                    if (!check)
                    {
                        continue;
                    }

                    // 枚举每一列并用前缀和直接求和
                    for (int jj = j ; jj < j + edge ; ++jj)
                    {
                        if (colsum[i + edge - 1, jj] - (i != 0 ? colsum[i - 1, jj] : 0) != stdsum)
                        {
                            check = false;
                            break;
                        }
                    }

                    if (!check)
                    {
                        continue;
                    }

                    // d1 和 d2 分别表示两条对角线的和
                    // 这里 d 表示 diagonal
                    int d1 = 0, d2 = 0;
                    // 不使用前缀和，直接遍历求和
                    for (int k = 0 ; k < edge ; ++k)
                    {
                        d1 += grid[i + k][j        + k];
                        d2 += grid[i + k][j + edge - 1 - k];
                    }

                    if (d1 == stdsum && d2 == stdsum)
                    {
                        return edge;
                    }
                }
            }
        }

        return 1;
    }

    // 一个 k x k 的 幻方 指的是一个 k x k 填满整数的方格阵，且每一行、每一列以及两条对角线的和 全部相等 。
    // 幻方中的整数 不需要互不相同 。显然，每个 1 x 1 的方格都是一个幻方。
    //
    // 给你一个 m x n 的整数矩阵 grid ，请你返回矩阵中 最大幻方 的 尺寸 （即边长 k）。
    //
    //
    //
    // 示例 1：
    //
    //
    // 输入：grid =  [7,1,4,5,6]             
    //              [2,5,1,6,4]     5  1  6
    //              [1,5,4,3,2]     5  4  3
    //              [1,2,7,3,4]     2  7  3        
    // 输出：3
    // 解释：最大幻方尺寸为 3 。
    // 每一行，每一列以及两条对角线的和都等于 12 。
    // - 每一行的和：5+1+6 = 5+4+3 = 2+7+3 = 12
    // - 每一列的和：5+5+2 = 1+4+7 = 6+3+3 = 12
    // - 对角线的和：5+4+3 = 6+4+2 = 12
    // 示例 2：
    //
    //
    // 输入：grid = [[5,1,3,1],[9,3,3,1],[1,3,3,8]]
    // 输出：2
    //
    //
    // 提示：
    //
    // m,n == grid.length
    // 1 <= m, n <= 50
    // 1 <= grid[i][j] <= 10^6
}


///<summary> 2860. 让所有学生保持开心的分组方法数 算术评级: 4 第 363 场周赛Q2-1626 </summary>
public class Solution_2860
{
    public int CountWays(IList<int> nums)
    {
        nums = nums.OrderBy(x => x).ToList();

        var ans = nums[0] > 0 ? 1 : 0; // 一个学生都不选

        for (var i = 1 ; i < nums.Count ; i++)
        {
            if (nums[i - 1] < i && i < nums[i])
            {
                ans++;
            }
        }

        return ans + 1; // 一定可以都选
    }



    // 给你一个下标从 0 开始、长度为 n 的整数数组 nums ，其中 n 是班级中学生的总数。班主任希望能够在让所有学生保持开心的情况下选出一组学生：
    //
    // 如果能够满足下述两个条件之一，则认为第 i 位学生将会保持开心：
    //
    // 这位学生被选中，并且被选中的学生人数   严格大于 nums[i] 。
    // 这位学生没有被选中，并且被选中的学生人数 严格小于 nums[i] 。
    // 返回能够满足让所有学生保持开心的分组方法的数目。
    //
    //
    //
    // 示例 1：
    //
    // 输入：nums = [1,1]
    // 输出：2
    // 解释：
    // 有两种可行的方法：
    // 班主任没有选中学生。
    // 班主任选中所有学生形成一组。 
    // 如果班主任仅选中一个学生来完成分组，那么两个学生都无法保持开心。因此，仅存在两种可行的方法。
    // 示例 2：
    //
    // 输入：nums = [6,0,3,3,6,7,2,7] => Sort: [0,2,3,3,6,6,7,7]
    // 输出：3
    // 解释：
    // 存在三种可行的方法：
    // 班主任选中下标为 1 的学生形成一组。          [0]
    // 班主任选中下标为 1、2、3、6 的学生形成一组。  [0,2,3,3]
    // 班主任选中所有学生形成一组。                [0,2,3,3,6,6,7,7]
    //
    //
    // 提示：
    //
    // 1 <= nums.length <= 10^5
    // 0 <= nums[i] < nums.length
}


/*****************************************************  一维差分  *******************************************************/
// 差分思想主要是构造一个差分数组,负责记录每个位置的变化量,然后通过差分数组来计算原数组的变化
// 目前看来,主要是用来解决一些区间覆盖问题


/// <summary> 2848. 与车相交的点 算术评级: 2 第 362 场周赛Q1-1230 </summary>
public class Solution_2848
{
    // 这一题比较抽象,藏得很深
    // [[3,6],[1,5],[4,7]] => 初始化为 [0,0,0,0,0,0,0,0] 8个0
    // 3,6 =>转化为差分数组 [0,0,0,1,0,0,0,-1,0]  => 原数组为 [0,0,0,1,1,1,1,0,0]
    // 1,5 =>转化为差分数组 [0,1,0,1,0,-1,0,-1,0] => 原数组为 [0,1,1,2,2,1,1,0,0]
    // 4,7 =>转化为差分数组 [0,1,0,1,1,0,0,-1,0]  => 原数组为 [0,1,1,2,3,2,2,1,0]
    // 所以遍历并统计所有大于 0 的数量 即 7个
    public int NumberOfPoints(IList<IList<int>> nums)
    {
        Span<int> diff = stackalloc int[102];

        foreach (var p in nums)
        {
            diff[p[0]]++;
            diff[p[1] + 1]--;
        }

        int ans = 0, s = 0;

        foreach (var d in diff)
        {
            s += d;
            if (s > 0)
            {
                ans++;
            }
        }

        return ans;
    }



    // 给你一个下标从 0 开始的二维整数数组 nums 表示汽车停放在数轴上的坐标。
    // 对于任意下标 i，nums[i] = [starti, endi] ，其中 starti 是第 i 辆车的起点，endi 是第 i 辆车的终点。
    // 返回数轴上被车 任意部分 覆盖的整数点的数目
    // 
    // 示例 1：
    //
    // 输入：nums = [[3,6],[1,5],[4,7]]
    // 输出：7
    // 解释：从 1 到 7 的所有点都至少与一辆车相交，因此答案为 7 。
    // 示例 2：
    //
    // 输入：nums = [[1,3],[5,8]]
    // 输出：7
    // 解释：1、2、3、5、6、7、8 共计 7 个点满足至少与一辆车相交，因此答案为 7 。
    //
    //
    // 提示：
    //
    // 1 <= nums.length <= 100
    // nums[i].length == 2
    // 1 <= starti <= endi <= 100
}


/// <summary> 1854. 人口最多的年份 算术评级: 3 第 240 场周赛Q1-1370 </summary>
public class Solution_1854
{
    public int MaximumPopulation(int[][] logs)
    {
        Span<int> diff = stackalloc int[102];

        foreach (var log in logs)
        {
            diff[log[0] - 1950]++;
            diff[log[1] - 1950]--;
        }

        var max = 0;

        for (int i = 1 ; i < 102 ; i++)
        {
            diff[i] += diff[i - 1];
            if (diff[i] > diff[max])
            {
                max = i;
            }
        }

        return max + 1950;
    }

    // [[1950,1961]
    // ,[1960,1971]
    // ,[1970,1981]]   // 1 2 1 1 1 0

    // [2008,2026],
    // [2004,2008],
    // [2034,2035],
    // [1999,2050],
    // [2049,2050],
    // [2011,2035],
    // [1966,2033],
    // [2044,2049]
    // 
    //
    // 给你一个二维整数数组 logs ，其中每个 logs[i] = [birthi, deathi] 表示第 i 个人的出生和死亡年份。
    //
    // 年份 x 的 人口 定义为这一年期间活着的人的数目。
    // 第 i 个人被计入年份 x 的人口需要满足：x 在闭区间 [birthi, deathi - 1] 内。注意，人不应当计入他们死亡当年的人口中。
    //
    // 返回 人口最多 且 最早 的年份。
    //
    //
    //
    // 示例 1：
    //
    // 输入：logs = [[1993,1999],[2000,2010]]
    // 输出：1993
    // 解释：人口最多为 1 ，而 1993 是人口为 1 的最早年份。
    // 示例 2：
    //
    // 输入：logs = [[1950,1961],[1960,1971],[1970,1981]]
    // 输出：1960
    // 解释： 
    // 人口最多为 2 ，分别出现在 1960 和 1970 。
    // 其中最早年份是 1960 。
    //
    //
    // 提示：
    //
    // 1 <= logs.length <= 100
    // 1950 <= birthi < deathi <= 2050
}


/// <summary> 57. 插入区间 算术评级: 5-中等 </summary>
public class Solution_57
{
    public int[][] Insert(int[][] intervals, int[] newInterval)
    {
        var dict = new SortedDictionary<int, int>();

        foreach (var interval in intervals)
        {
            dict.TryAdd(interval[0], 0);
            dict.TryAdd(interval[1], 0);

            dict[interval[0]]++;
            dict[interval[1]]--;
        }

        dict.TryAdd(newInterval[0], 0);
        dict.TryAdd(newInterval[1], 0);

        dict[newInterval[0]]++;
        dict[newInterval[1]]--;

        var res = new List<int[]>();

        var start = 0;

        foreach (var entry in dict)
        {
            start += entry.Value;

            if (start >= 1 && (res.Count == 0 || res[^1].Length != 1))
            {
                res.Add([entry.Key]);
            }
            else if (start == 0 && (res.Count == 0 || res[^1].Length != 1))
            {
                res.Add([entry.Key, entry.Key]);
            }
            else if (start == 0)
            {
                res[^1] = [res[^1][0], entry.Key];
            }
        }


        return res.ToArray();
    }

    [Test]
    public void METHOD()
    {
        int[][] t = [[1, 5]];

        Insert(t, [0, 0]);
    }


    // 给你一个 无重叠的 ，按照区间起始端点排序的区间列表 intervals，
    // 其中 intervals[i] = [starti, endi] 表示第 i 个区间的开始和结束，
    // 并且 intervals 按照 starti 升序排列。同样给定一个区间 newInterval = [start, end] 表示另一个区间的开始和结束。
    //
    // 在 intervals 中插入区间 newInterval，使得 intervals 依然按照 starti 升序排列，且区间之间不重叠（如果有必要的话，可以合并区间）。
    //
    // 返回插入之后的 intervals。
    //
    // 注意 你不需要原地修改 intervals。你可以创建一个新数组然后返回它。
    //
    //
    //
    // 示例 1：
    //
    // 输入：intervals = [[1,3],[6,9]], newInterval = [2,5]
    // 输出：[[1,5],[6,9]]
    // 示例 2：
    //
    // 输入：intervals = [[1,2],[3,5],[6,7],[8,10],[12,16]], newInterval = [4,8]
    // 输出：[[1,2],[3,10],[12,16]]
    // 解释：这是因为新的区间 [4,8] 与 [3,5],[6,7],[8,10] 重叠。
    //
    //
    // 提示：
    //
    // 0 <= intervals.length <= 10^4
    // intervals[i].length == 2
    // 0 <= starti <= endi <= 10^5
    // intervals 根据 starti 按 升序 排列
    // newInterval.length == 2
    // 0 <= start <= end <= 10^5
}


/// <summary> 1094. 拼车 算术评级: 4 第 142 场周赛Q2-1441 </summary>
public class Solution_1094
{
    public bool CarPooling(int[][] trips, int capacity)
    {
        Span<int> diff = stackalloc int[1001];

        for (int i = 0, len = trips.Length ; i < len ; i++)
        {
            diff[trips[i][1]] += trips[i][0];
            diff[trips[i][2]] -= trips[i][0];
        }

        var sum = 0;

        //[2,1,5],[3,3,7]  4

        foreach (var value in diff)
        {
            sum += value;
            if (sum > capacity)
            {
                return false;
            }
        }


        return true;
    }



    // 车上最初有 capacity 个空座位。
    // 车 只能 向一个方向行驶（也就是说，不允许掉头或改变方向）
    //
    // 给定整数 capacity 和一个数组 trips ,
    // trip[i] = [numPassengersi, fromi, toi] 表示第 i 次旅行有 numPassengersi 乘客，
    // 接他们和放他们的位置分别是 fromi 和 toi 。这些位置是从汽车的初始位置向东的公里数。
    //
    // 当且仅当你可以在所有给定的行程中接送所有乘客时，返回 true，否则请返回 false。
    //
    //
    //
    // 示例 1：
    //
    // 输入：trips = [[2,1,5],[3,3,7]], capacity = 4
    // 输出：false
    // 示例 2：
    //
    // 输入：trips = [[2,1,5],[3,3,7]], capacity = 5
    // 输出：true
    //
    //
    // 提示：
    //
    // 1 <= trips.length <= 1000
    // trips[i].length == 3
    // 1 <= numPassengersi <= 100
    // 0 <= fromi < toi <= 1000
    // 1 <= capacity <= 10^5
}


/// <summary> 732. 我的日程安排表 III 算术评级: 6 困难 </summary>
public class Solution_732
{
    public class MyCalendarThree
    {
        private readonly SortedDictionary<int, int> _diff = new();

        public int Book(int startTime, int endTime)
        {
            _diff.TryAdd(startTime, 0);
            _diff.TryAdd(endTime,   0);

            _diff[startTime]++;
            _diff[endTime]--;

            var ans = 0;
            var sum = 0;

            foreach (var value in _diff.Values)
            {
                sum += value;
                ans =  Math.Max(ans, sum);
            }

            return ans;
        }
        
    }


    /*
     * Your MyCalendarThree object will be instantiated and called as such:
     * MyCalendarThree obj = new MyCalendarThree();
     * int param_1 = obj.Book(startTime,endTime);
     */


    // 当 k 个日程存在一些非空交集时（即, k 个日程包含了一些相同时间），就会产生 k 次预订
    //
    // 给你一些日程安排 [startTime, endTime) , 请你在每个日程安排添加后，返回一个整数 k , 表示所有先前日程安排会产生的最大 k 次预订
    //
    // 实现一个 MyCalendarThree 类来存放你的日程安排，你可以一直添加新的日程安排。
    //
    // MyCalendarThree() 初始化对象。
    // int book(int startTime, int endTime) 返回一个整数 k ，表示日历中存在的 k 次预订的最大值
    //
    //
    // 示例:
    //
    // 输入:
    // ["MyCalendarThree", "book", "book", "book", "book", "book", "book"]
    // [[      ], [10, 20], [50, 60], [10, 40], [5, 15], [5, 10], [25, 55]]
    // 输出：
    // [null, 1, 1, 2, 3, 3, 3]
    //
    // 解释：
    // MyCalendarThree myCalendarThree = new MyCalendarThree();
    // myCalendarThree.book(10, 20); // 返回 1 , 第一个日程安排可以预订并且不存在相交，所以最大 k 次预订是 1 次预订
    // myCalendarThree.book(50, 60); // 返回 1 , 第二个日程安排可以预订并且不存在相交，所以最大 k 次预订是 1 次预订
    // myCalendarThree.book(10, 40); // 返回 2 , 第三个日程安排 [10, 40) 与第一个日程安排相交，所以最大 k 次预订是 2 次预订
    // myCalendarThree.book( 5, 15); // 返回 3 , 剩下的日程安排的最大 k 次预订是 3 次预订
    // myCalendarThree.book( 5, 10); // 返回 3
    // myCalendarThree.book(25, 55); // 返回 3
    //
    //
    // 提示:
    // 0 <= startTime < endTime <= 10^9
    // 每个测试用例，调用 book 函数最多不超过 400次
}