namespace LeetCodeCSharp;

///<summary> 53. 最大子数组和 </summary>
public class Solution_53
{
    // 最经典的动态规划问题：最大子数组和
    // 1. 状态定义：dp[i] 表示以 nums[i] 结尾的连续子数组的最大和

    public int MaxSubArray(int[] nums)
    {
        int max = nums[0], pre = 0;

        foreach (var item in nums)
        {
            pre = Math.Max(pre + item, item); // 状态转移方程：即要不然累加，要不然重新开始，这是这道题的关键
            max = Math.Max(pre,        max);
        }

        return max;
    }

    // 给你一个整数数组 nums ，请你找出一个具有最大和的连续子数组（子数组最少包含一个元素），返回其最大和。
    //
    // 子数组:是数组中的一个连续部分。


    // [-2, 1,-3, 4,-1, 2, 1,-5, 4]
    // [-2, 1,-2, 4,  
}


/// 1186. 删除一次得到子数组最大和 算术评级: 6 第 153 场周赛 Q3
public class Solution_1186
{
    // 本题是典型的动态规划应用题，
    // 我们可以将问题拆分成多个子问题，即求解以 arr[i] 结尾的最多删除一次的非空子数组的最大和。
    // 我们以 dp[i][k] 表示以 arr[i] 结尾，删除 k 次的非空子数组的最大和（删除前的末尾元素为 arr[i]，就视为以 arr[i] 结尾）。
    // 初始时 dp[0][0]=arr[0]，dp[0][1]＝0（以 arr[0] 结尾，删除一次的非空子数组不存在，因此 dp[0][1] 不会计入结果）。当 i>0 时，转移方程如下：
    //
    //     dp[i][0] = max(dp[i−1][0],0)+arr[i]
    //     dp[i][1] = max(dp[i−1][1]+arr[i],dp[i−1][0])
    //  
    // 第一个转移方程表示在不删除的情况下，以 arr[i] 为结尾的非空子数组的最大和 dp[i][0] 与 dp[i－1][0] 有关，当 dp[i−1][0]>0 时，直接将 arr[i] 与 i−1 时的最大非空子数组连接时，取得最大和，否则只选 arr[i] 时，取得最大和。
    //
    //     第二个转移方程表示在删除一次的情况下，以 arr[i] 为结尾的非空子数组有两种情况：
    //
    //     不删除 arr[i]，那么选择 arr[i] 与 dp[i−1][1] 对应的子数组（已执行一次删除）。
    //
    //     删除 arr[i]，那么选择 dp[i−1][0] 对应的非空子数组（未执行一次删除，但是等同于删除了 arr[i]）。
    //
    //     dp[i][1] 取以上两种情况的最大和的最大值。
    //
    //     注意到 dp[i][∗] 的值只与 dp[i−1][∗] 有关，因此我们可以只使用两个整数来节省空间。


    public int MaximumSum(int[] arr)
    {
        int dp0 = arr[0], dp1 = 0, res = arr[0];
        for (var i = 1 ; i < arr.Length ; i++)
        {
            dp1 = Math.Max(dp0, dp1 + arr[i]);
            dp0 = Math.Max(dp0, 0) + arr[i];
            res = Math.Max(res, Math.Max(dp0, dp1));
        }

        return res;
    }

    // 给你一个整数数组，返回它的某个 非空 子数组（连续元素）在执行一次可选的删除操作后，所能得到的最大元素总和。
    // 换句话说，你可以从原数组中选出一个子数组，并可以决定要不要从中删除一个元素（只能删一次哦）
    // （删除后）子数组中至少应当有一个元素，然后该子数组（剩下）的元素总和是所有子数组之中最大的。
    //
    // 注意，删除一个元素后，子数组 不能为空。
    //
    //
    //
    // 示例 1：
    //
    // 输入：arr = [1,-2,0,3]
    // 输出：4
    // 解释：我们可以选出 [1, -2, 0, 3]，然后删掉 -2，这样得到 [1, 0, 3]，和最大。
    // 示例 2：
    //
    // 输入：arr = [1,-2,-2,3]
    // 输出：3
    // 解释：我们直接选出 [3]，这就是最大和。
    // 示例 3：
    //
    // 输入：arr = [-1,-1,-1,-1]
    // 输出：-1
    // 解释：最后得到的子数组不能为空，所以我们不能选择 [-1] 并从中删去 -1 来得到 0。
    // 我们应该直接选择 [-1]，或者选择 [-1, -1] 再从中删去一个 -1。
    //
    //
    // 提示：
    //
    // 1 <= arr.length <= 10^5
    // -104 <= arr[i] <= 10^4



    //     一、启发思考：寻找子问题
    // 最暴力的做法是枚举子数组的左右端点，以及要删除的元素。这种做法显然会超时。
    //
    // 保留「枚举子数组的右端点」这一想法，看看有没有可以优化的地方。
    //
    // 对于示例 1 中的数组 [1,−2,0,3]，假设元素和最大的连续子数组的右端点是 3。设原问题为：「子数组的右端点是 3，且至多删除一个数，子数组元素和的最大值是多少？」
    //
    // 这个原问题可以拆分成两个问题：
    //
    // 子数组的右端点是 3，且不能删除数字，子数组元素和的最大值是多少？
    // 如果不选 3 左边的数，那么子数组就是 [3]。
    // 如果选 3 左边的数，那么需要解决的问题为：「子数组的右端点是 0，且不能删除数字，子数组元素和的最大值是多少？」
    // 子数组的右端点是 3，且必须删除一个数字，子数组元素和的最大值是多少？
    // 如果不选 3 左边的数，那么必须删除 3，但这违背了题目要求：「（删除后）子数组中至少应当有一个元素」。所以不考虑这种情况。
    // 如果选 3 左边的数：
    // 如果不删除 3，那么需要解决的问题为：「子数组的右端点是 0，且必须删除一个数字，子数组元素和的最大值是多少？」
    // 如果删除 3，那么需要解决的问题为：「子数组的右端点是 0，且不能删除数字，子数组元素和的最大值是多少？」
    // 上面说的三个需要解决的问题，都是和原问题相似的、规模更小的子问题，所以可以用递归解决。
    //
    // 注 1：从右往左思考，主要是为了方便把递归翻译成递推。从左往右思考也是可以的。
    //
    // 注 2：动态规划有「选或不选」和「枚举选哪个」这两种基本思考方式。在做题时，可根据题目要求，选择适合题目的一种来思考。上面用到的是「选或不选」。
    //
    // 二、递归怎么写：状态定义与状态转移方程
    // 根据上面的讨论，递归参数需要一个 i，表示子数组的右端点是 arr[i]。此外，需要知道是否可以删除数字，所以递归参数还需要一个 j。其中 j=0 表示不能删除数字，j=1 表示必须删除一个数。
    //
    // 因此，定义 dfs(i,j) 表示子数组的右端点是 arr[i]，不能/必须删除数字的情况下，子数组元素和的最大值。
    //
    // 根据上面讨论出的子问题，可以得到：
    //
    // 如果 j=0（不能删除）：
    // 如果不选 arr[i] 左边的数，那么 dfs(i,0)=arr[i]。
    // 如果选 arr[i] 左边的数，那么 dfs(i,0)=dfs(i−1,0)+arr[i]。
    // 如果 j=1（必须删除）：
    // 如果不删除 arr[i]，那么 dfs(i,1)=dfs(i−1,1)+arr[i]。
    // 如果删除 arr[i]，那么 dfs(i,1)=dfs(i−1,0)。
    // 取最大值，就得到了 dfs(i,j)。写成式子就是
    //
    // ​
    //   
    // dfs(i,0)=max(dfs(i−1,0),0)+arr[i]
    // dfs(i,1)=max(dfs(i−1,1)+arr[i],dfs(i−1,0))
    // ​
    //  
    // 递归边界：dfs(−1,j)=−∞。这里 −1 表示子数组中「没有数字」，但题目要求子数组不能为空，所以这种情况不合法，用 −∞ 表示，这样取 max 的时候就自然会取到合法的情况。
    //
    // 递归入口：dfs(i,j)。枚举子数组右端点 i，以及是否需要删除数字 j=0,1，取所有结果的最大值，作为答案。
    //
    // 三、递归 + 记录返回值 = 记忆化搜索
    // 以 [1,−2,0,3] 为例。在计算 dfs(3,1) 时，「删除 3，保留 0」和「保留 3，删除 0」，都会递归到 dfs(1,0)。
    //
    // 一叶知秋，整个递归中有大量重复递归调用（递归入参相同）。由于递归函数没有副作用，同样的入参无论计算多少次，算出来的结果都是一样的，因此可以用记忆化搜索来优化：
    //
    // 如果一个状态（递归入参）是第一次遇到，那么可以在返回前，把状态及其结果记到一个 memo 数组（或哈希表）中。
    // 如果一个状态不是第一次遇到，那么直接返回 memo 中保存的结果。
    // Python3
    // Java
    // C++
    // Go
    // class Solution {
    //     private int[] arr;
    //     private int[][] memo;
    //
    //     public int maximumSum(int[] arr) {
    //         this.arr = arr;
    //         int n = arr.length;
    //         int ans = Integer.MIN_VALUE;
    //         memo = new int[n][2];
    //         for (int[] row : memo)
    //             Arrays.fill(row, Integer.MIN_VALUE);
    //         for (int i = 0; i < n; i++)
    //             ans = Math.max(ans, Math.max(dfs(i, 0), dfs(i, 1)));
    //         return ans;
    //     }
    //
    //     private int dfs(int i, int j) {
    //         if (i < 0) return Integer.MIN_VALUE / 2; // 除 2 防止负数相加溢出
    //         if (memo[i][j] != Integer.MIN_VALUE) return memo[i][j]; // 之前计算过
    //         if (j == 0) return memo[i][j] = Math.max(dfs(i - 1, 0), 0) + arr[i];
    //         return memo[i][j] = Math.max(dfs(i - 1, 1) + arr[i], dfs(i - 1, 0));
    //     }
    // }
    // 复杂度分析
    // 时间复杂度：O(n)，其中 n 为 arr 的长度。动态规划的时间复杂度 = 状态个数 × 单个状态的计算时间。本题中状态个数等于 O(n)，单个状态的计算时间为 O(1)，所以动态规划的时间复杂度为 O(n)。
    // 空间复杂度：O(n)。
    // 四、1:1 翻译成递推
    // 我们可以去掉递归中的「递」，只保留「归」的部分，即自底向上计算。
    //
    // 做法：
    //
    // dfs 改成 f 数组。
    // 递归改成循环（每个参数都对应一层循环）。这里 j 只有 0 和 1，可以直接计算，无需循环 j。
    // 递归边界改成 f 数组的初始值。
    // 相当于之前是用递归去计算每个状态，现在是（按照某种顺序）枚举并计算每个状态。
    //
    // 具体来说，f[i][j] 的含义和 dfs(i,j) 的含义是一致的，都表示子数组的右端点是 arr[i]，不能/必须删除数字的情况下，子数组元素和的最大值。
    //
    // 相应的递推式（状态转移方程）也和 dfs 的一致：
    //
    // ​
    //   
    // f[i][0]=max(f[i−1][0],0)+arr[i]
    // f[i][1]=max(f[i−1][1]+arr[i],f[i−1][0])
    // ​
    //  
    // 但是，这种定义方式没有状态能表示递归边界，即 i=−1 的情况。
    //
    // 解决办法：把 f 数组的长度加一，用 f[0][j] 表示 dfs(−1,j)。由于 f[0] 被占用，原来的下标 i 需要全部向右偏移一位，也就是 f[i] 改为 f[i+1]，f[i−1] 改为 f[i]。
    //
    // 修改后 f[i+1] 表示子数组的右端点是 arr[i]，不能/必须删除数字的情况下，子数组元素和的最大值。
    //
    // 修改后的递推式为
    //
    // ​
    //   
    // f[i+1][0]=max(f[i][0],0)+arr[i]
    // f[i+1][1]=max(f[i][1]+arr[i],f[i][0])
    // ​
    //  
    // 问：为什么 arr 的下标不用变？
    //
    // 答：既然是把 f 数组的长度加一，那么就只需要修改和 f 有关的下标，其余任何逻辑都无需修改。
    //
    // 初始值 f[0][j]=−∞，翻译自 dfs(−1,j)=−∞。
    //
    // 答案为所有 f[i][j] 的最大值。
    //
    // Python3
    // Java
    // C++
    // Go
    // class Solution:
    //     def maximumSum(self, arr: List[int]) -> int:
    //         f = [[-inf] * 2] + [[0, 0] for _ in arr]
    //         for i, x in enumerate(arr):
    //             f[i + 1][0] = max(f[i][0], 0) + x
    //             f[i + 1][1] = max(f[i][1] + x, f[i][0])
    //         return max(max(r) for r in f)
    // 复杂度分析
    // 时间复杂度：O(n)，其中 n 为 arr 的长度。动态规划的时间复杂度 = 状态个数 × 单个状态的计算时间。本题中状态个数等于 O(n)，单个状态的计算时间为 O(1)，所以动态规划的时间复杂度为 O(n)。
    // 空间复杂度：O(n)。
    // 五、空间优化
    // 观察上面的状态转移方程，在计算 f[i+1] 时，只会用到 f[i]，不会用到下标 <i 的状态。
    //
    // 因此只需要两个状态表示 j=0,1。
    //
    // 状态转移方程改为
    //
    // ​
    //   
    // f[1]=max(f[1]+arr[i],f[0])
    // f[0]=max(f[0],0)+arr[i]
    // ​
    //  
    // 请注意计算顺序！必须先算 f[1] 再算 f[0]。如果先算 f[0] 再算 f[1]，那么在计算 f[1] 时，相当于用到的不是原来的 f[i][0]，而是新算出来的 f[i+1][0]。
    //
    // 初始值 f[j]=−∞。
    //
    // 一边计算，一边维护 f[j] 的最大值，作为答案。
    //
    // Python3
    // Java
    // C++
    // Go
    // class Solution:
    //     def maximumSum(self, arr: List[int]) -> int:
    //         ans = f0 = f1 = -inf
    //         for x in arr:
    //             f1 = max(f1 + x, f0)  # 注：手动 if 比大小会更快 
    //             f0 = max(f0, 0) + x
    //             ans = max(ans, f0, f1)
    //         return ans
    // 复杂度分析
    // 时间复杂度：O(n)，其中 n 为 arr 的长度。动态规划的时间复杂度 = 状态个数 × 单个状态的计算时间。本题中状态个数等于 O(n)，单个状态的计算时间为 O(1)，所以动态规划的时间复杂度为 O(n)。
    // 空间复杂度：O(1)。只用到常数级的额外空间。
    // 思考题 如果改成至多删除 k 次，要怎么做？恰好删除 k 次呢？至少删除 k 次呢？
}


///[太难了，未解决]3098. 求出所有子序列的能量和 算术评级: 10!!! 第 127 场双周赛 Q4
public class Solution_3098
{
    // 下面的方法超时了，太暴力了
    [TestCase(new[] { 1, 2, 3, 4 }, 3, ExpectedResult = 4)]
    // [TestCase(new[] { -24, -921, 119, -291, -65, -628, 372, 274, 962, -592, -10, 67, -977, 85, -294, 349, -119, -846, -959, -79, -877, 833, 857, 44, 826, -295, -855, 554, -999, 759, -653, -423, -599, -928 }, 19, ExpectedResult = 990202285)]
    public int SumOfPowers(int[] nums, int k)
    {
        const int mod = 1000000007;

        // 1. 排序
        Array.Sort(nums);

        // 2. 枚举子序列
        long indexer    = (1  << k)           - 1;
        var  maxIndexer = (1L << nums.Length) - 1;
        long sum        = 0;

        while (indexer != 0 && indexer <= maxIndexer)
        {
            sum += GetPower(k, nums, indexer);

            indexer = Gosper_Hack(indexer);
        }

        sum %= mod;

        return (int)sum;
    }

    public static int GetPower(int k, int[] nums, long indexer)
    {
        Span<int> sub   = stackalloc int[k];
        var       index = 0;
        var       min   = int.MaxValue;
        for (var i = 0 ; i < nums.Length ; i++)
        {
            if ((indexer & (1L << i)) != 0)
            {
                sub[index++] = nums[i];
            }
        }

        for (var i = 1 ; i < k ; i++)
        {
            min = Math.Min(min, sub[i] - sub[i - 1]);
        }

        return min;
    }

    public static long Gosper_Hack(long num)
    {
        if (num <= 0) return 0;

        var lowBit = num & -num;
        var left   = num + lowBit;
        var p      = left ^ num;
        var right  = (p >> 2) / lowBit;
        var result = left | right;
        return result;
    }



    public int SumOfPowers2(int[] nums, int k)
    {
        const int MOD = 1000000007, INF = 0x3f3f3f3f;

        var length = nums.Length;
        var dictss = new Dictionary<int, int>[length][];
        for (var i = 0 ; i < length ; i++)
        {
            dictss[i] = new Dictionary<int, int>[k + 1];
            for (var j = 0 ; j <= k ; j++)
            {
                dictss[i][j] = [];
            }
        }

        var res = 0;
        Array.Sort(nums);
        for (var i = 0 ; i < length ; i++)
        {
            dictss[i][1].Add(INF, 1);
            for (var j = 0 ; j < i ; j++)
            {
                var diff = Math.Abs(nums[i] - nums[j]);
                for (var p = 2 ; p <= k ; p++)
                {
                    foreach (var pair in dictss[j][p - 1])
                    {
                        int v       = pair.Key, cnt = pair.Value;
                        var currKey = Math.Min(diff, v);
                        dictss[i][p].TryAdd(currKey, 0);
                        dictss[i][p][currKey] = (dictss[i][p][currKey] + cnt) % MOD;
                    }
                }
            }


            foreach (var (v, cnt) in dictss[i][k])
            {
                res = (int)((res + 1L * v * cnt % MOD) % MOD);
            }
        }

        return res;
    }



    // 给你一个长度为 n 的整数数组 nums 和一个 正 整数 k 。
    // 一个 子序列 的  能量 定义为子序列中 任意 两个元素的差值绝对值的 最小值 。
    // 请你返回 nums 中长度 等于 k 的 所有 子序列的 能量和 。
    // 由于答案可能会很大，将答案对 109 + 7 取余 后返回。
    // 2 <= n == nums.length <= 50
    // -10^8 <= nums[i] <= 10^8 
    // 2 <= k <= n

    // help1:Sort nums
    // help2:There are at most n2 distinct differences. 翻译：最多有 n2 个不同的差值
    // help3:For a particular difference d, let dp[len][i][j] be the number of subsequences of length len in the subarray nums[0..i] where the last element picked was at index j.
    // 翻译：对于特定的差值 d，令 dp[len][i][j] 表示子数组 nums[0..i] 中长度为 len 的子序列的数量，其中最后一个选取的元素的下标为 j。
    // help4:For each index, we can check if it can be picked if nums[i] - nums[j] <= d.
    // 翻译：对于每个下标，我们可以检查是否可以选取 nums[i]，如果 nums[i] - nums[j] <= d。
}