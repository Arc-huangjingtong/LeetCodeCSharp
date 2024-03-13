namespace LeetCodeCSharp;

public partial class UnitTest
{
    [Test]
    public void TestMethod1()
    {
        var tree = CreateTree("[1,2,3,null,4,null,5]");

        Assert.That(tree.val,             Is.EqualTo(1));
        Assert.That(tree.left.val,        Is.EqualTo(2));
        Assert.That(tree.right.val,       Is.EqualTo(3));
        Assert.That(tree.left.left,       Is.Null);
        Assert.That(tree.left.right.val,  Is.EqualTo(4));
        Assert.That(tree.right.left,      Is.Null);
        Assert.That(tree.right.right.val, Is.EqualTo(5));

        Assert.Pass();
    }

    [TestCase("aa", ".*", ExpectedResult = true)]
    public bool IsMatch(string s, string p)
    {
        var regex = new System.Text.RegularExpressions.Regex(p, System.Text.RegularExpressions.RegexOptions.Compiled);
        return regex.IsMatch(s);
    }


    public class Solution_1976
    {
        //你在一个城市里，城市由 n 个路口组成，路口编号为 0 到 n - 1 ，某些路口之间有 双向 道路。输入保证你可以从任意路口出发到达其他任意路口，且任意两个路口之间最多有一条路。
        //给你一个整数 n 和二维整数数组 roads ，其中 roads[i] = [ui, vi, timei] 表示在路口 ui 和 vi 之间有一条需要花费 timei 时间才能通过的道路。你想知道花费 最少时间 从路口 0 出发到达路口 n - 1 的方案数。
        //请返回花费 最少时间 到达目的地的 路径数目 。由于答案可能很大，将结果对 109 + 7 取余 后返回。

        // 示例 1：
        // 输入：n = 7, roads = [[0,6,7],[0,1,2],[1,2,3],[1,3,3],[6,3,3],[3,5,1],[6,5,1],[2,5,1],[0,4,5],[4,6,2]]
        // 输出：4
        // 解释：从路口 0 出发到路口 6 花费的最少时间是 7 分钟。
        // 四条花费 7 分钟的路径分别为：
        // - 0 ➝ 6
        // - 0 ➝ 4 ➝ 6
        // - 0 ➝ 1 ➝ 2 ➝ 5 ➝ 6
        // - 0 ➝ 1 ➝ 3 ➝ 5 ➝ 6
        // 示例 2：
        //
        // 输入：n = 2, roads = [[1,0,10]]
        // 输出：1
        // 解释：只有一条从路口 0 到路口 1 的路，花费 10 分钟。
        //
        //
        // 提示：
        //
        // 1 <= n <= 200
        // n - 1 <= roads.length <= n * (n - 1) / 2
        // roads[i].length == 3
        // 0 <= ui, vi <= n - 1
        // 1 <= timei <= 109
        // ui != vi
        //     任意两个路口之间至多有一条路。
        // 从任意路口出发，你能够到达其他任意路口。

        public int CountPaths(int n, int[][] roads)
        {
            var end      = n - 1;
            var ansList  = new List<int>();
            var hasEnter = new HashSet<int>();
            var graph    = new Dictionary<int, int>[n];

            for (var i = 0 ; i < n ; i++)
            {
                graph[i] = [];
            }

            foreach (var road in roads)
            {
                var u = road[0];
                var v = road[1];
                var t = road[2];
                graph[u].Add(v, t);
                graph[v].Add(u, t);
            }

            Dfs(0, 0, hasEnter);

            var min = ansList.Min();


            return ansList.Count(x => x == min);

            void Dfs(int start, int weight, HashSet<int> set)
            {
                if (start == end)
                {
                    ansList.Add(weight);
                }

                var dic = graph[start];

                foreach (var pair in dic)
                {
                    set.Add(pair.Key);
                }

                foreach (var pair in dic)
                {
                    // if (set.Contains(pair.Key))
                    // {
                    //     continue;
                    // }

                    Dfs(pair.Key, pair.Value + weight, [..set]);
                }
            }
        }

        private const int Mod = 1000000007;

        public int CountPaths2(int n, int[][] roads)
        {
            var graph = new Dictionary<int, int>[n]; // 邻接表, 用于存储图的边

            for (var i = 0 ; i < n ; i++)
            {
                graph[i] = [];
            }

            foreach (var road in roads)
            {
                int x = road[0], y = road[1], t = road[2];
                graph[x].Add(y, t);
                graph[y].Add(x, t);
            }

            var _queue    = new PriorityQueue<(long, int), long>(); // 优先队列, 用于 Dijkstra 算法
            var distances = new long[n];                            // dis[i] 表示从 0 到 i 的最短距离
            var ways      = new int[n];                             // ways[i] 表示从 0 到 i 的最短距离的路径数

            Array.Fill(distances, long.MaxValue); // 初始化 dis 数组, 全部填充为 long.MaxValue

            _queue.Enqueue((0, 0), 0); // 将起点加入队列

            distances[0] = 0; // 起点到起点的距离为 0
            ways[0]      = 1; // 起点到起点的路径数为 1

            while (_queue.Count > 0)
            {
                var (distance, nextPoint) = _queue.Dequeue();

                if (distance > distances[nextPoint]) // 如果当前距离大于 dis 数组中的距离, 则跳过
                {
                    continue;
                }

                foreach (var (point, dis) in graph[nextPoint]) // 遍历当前节点的所有邻接节点
                {
                    if (distance + dis < distances[point]) // 如果当前节点到邻接节点的距离小于 dis 数组中的距离, 则更新 dis 数组
                    {
                        distances[point] = distance + dis;
                        ways[point]      = ways[nextPoint];
                        _queue.Enqueue((distance + dis, point), distance + dis);
                    }
                    else if (distance + dis == distances[point]) // 如果当前节点到邻接节点的距离等于 dis 数组中的距离, 则更新 ways 数组
                    {
                        ways[point] = (ways[nextPoint] + ways[point]) % Mod;
                    }
                }
            }

            return ways[n - 1];
        }



        [Test]
        public void Test()
        {
            int[][] road = [[0, 6, 7], [0, 1, 2], [1, 2, 3], [1, 3, 3], [6, 3, 3], [3, 5, 1], [6, 5, 1], [2, 5, 1], [0, 4, 5], [4, 6, 2]];

            int[][] road2 = [[0, 1, 3972], [2, 1, 1762], [3, 1, 4374], [0, 3, 8346], [3, 2, 2612], [4, 0, 6786], [5, 4, 1420], [2, 6, 7459], [1, 6, 9221], [6, 3, 4847], [5, 6, 4987], [7, 0, 14609], [7, 1, 10637], [2, 7, 8875], [7, 6, 1416], [7, 5, 6403], [7, 3, 6263], [4, 7, 7823], [5, 8, 10184], [8, 1, 14418], [8, 4, 11604], [7, 8, 3781], [8, 2, 12656], [8, 0, 18390], [5, 9, 15094], [7, 9, 8691], [9, 6, 10107], [9, 1, 19328], [9, 4, 16514], [9, 2, 17566], [9, 0, 23300], [8, 9, 4910], [9, 3, 14954], [4, 10, 26060], [2, 10, 27112], [10, 1, 28874], [8, 10, 14456], [3, 10, 24500], [5, 10, 24640], [10, 6, 19653], [10, 0, 32846], [10, 9, 9546], [10, 7, 18237], [11, 7, 21726], [11, 2, 30601], [4, 11, 29549], [11, 0, 36335], [10, 11, 3489], [6, 11, 23142], [3, 11, 27989], [11, 1, 32363], [11, 8, 17945], [9, 11, 13035], [5, 11, 28129], [2, 12, 33902], [5, 12, 31430], [6, 12, 26443], [4, 12, 32850], [12, 3, 31290], [11, 12, 3301], [12, 1, 35664], [7, 13, 28087], [13, 8, 24306], [6, 13, 29503], [11, 13, 6361], [4, 13, 35910], [13, 12, 3060], [3, 13, 34350], [13, 5, 34490], [13, 2, 36962], [10, 13, 9850], [9, 13, 19396], [12, 14, 8882], [8, 14, 30128], [14, 6, 35325], [14, 5, 40312], [1, 14, 44546], [11, 14, 12183], [15, 12, 13581], [2, 15, 47483], [4, 15, 46431], [15, 10, 20371], [15, 14, 4699], [15, 6, 40024], [15, 7, 38608], [1, 15, 49245], [11, 15, 16882], [8, 15, 34827], [0, 15, 53217], [5, 15, 45011], [15, 3, 44871], [16, 2, 53419], [16, 9, 35853], [1, 16, 55181], [16, 7, 44544], [8, 16, 40763], [0, 16, 59153], [15, 16, 5936], [16, 10, 26307], [16, 6, 45960], [12, 16, 19517], [17, 2, 57606], [17, 3, 54994], [17, 14, 14822], [17, 11, 27005], [0, 17, 63340], [17, 7, 48731], [8, 17, 44950], [17, 16, 4187], [5, 17, 55134], [17, 10, 30494], [17, 9, 40040], [17, 12, 23704], [13, 17, 20644], [17, 1, 59368]];

            var ans1 = CountPaths2(7,  road);
            var ans2 = CountPaths2(18, road2);

            var ans3 = ans1;
        }
    }


    public class Solution_2917
    {
        [TestCase(new[] { 7, 12, 9, 8, 9, 15 },                                                                                                                                                                                                                                                                                                                                                                                             4,  ExpectedResult = 9)]
        [TestCase(new[] { 2, 12, 1, 11, 4, 5 },                                                                                                                                                                                                                                                                                                                                                                                             6,  ExpectedResult = 0)]
        [TestCase(new[] { 10, 8, 5, 9, 11, 6, 8 },                                                                                                                                                                                                                                                                                                                                                                                          1,  ExpectedResult = 15)]
        [TestCase(new[] { 925011496, 103855710, 1584980217, 1804943441, 904176743, 71227402, 658339386, 1949490684, 394057351, 1504638274, 936036729, 516283059, 995417756, 1370320334, 1501991237, 578607899, 981063549, 1950398619, 780236107, 258555692, 2055224506, 521917008, 1643308943, 522924296, 1115988653, 136177651, 2112081121, 1411190147, 1059197244, 1476196073, 1563551833, 477789887, 1901104327, 752532861, 824056222 }, 16, ExpectedResult = 1065156095)]
        public int FindKOr(int[] nums, int k)
        {
            var ans = 0;

            if (nums[0] > 800000000) { }

            for (var i = 0 ; i < 31 ; i++)
            {
                var count = 0;
                foreach (var num in nums)
                {
                    if ((num & (1 << i)) != 0)
                    {
                        count++;

                        if (count >= k) break;
                    }
                }

                if (count >= k)
                {
                    ans += 1 << i;
                }
            }

            return ans;
        }

        [Test]
        public void Test() { }

        // 2917. 找出数组中的 K-or 值  简单
        // 给你一个下标从 0 开始的整数数组 nums 和一个整数 k 。
        //
        // nums 中的 K-or 是一个满足以下条件的非负整数：
        //
        // 只有在 nums 中，至少存在 k 个元素的第 i 位值为 1 ，那么 K-or 中的第 i 位的值才是 1 。
        // 返回  nums 的      K-or    值。
        //
        // 注意 ：对于整数 x ，如果 (2^i AND x) == 2i ，则 x 中的第 i 位值为 1 ，其中 AND 为按位与运算符。
        //
        //
        //
        // 示例 1：
        //
        // 输入：nums = [7,12,9,8,9,15], k = 4
        // 输出：9
        // 解释：nums[0]、nums[2]、nums[4] 和      nums[5] 的第 0 位的值为 1 。
        // nums[0] 和                         nums[5] 的第 1 位的值为 1 。
        // nums[0]、nums[1] 和                 nums[5] 的第 2 位的值为 1 。
        // nums[1]、nums[2]、nums[3]、nums[4] 和 nums[5] 的第 3 位的值为 1 。
        // 只有第 0 位和第 3 位满足数组中至少存在            k 个元素在对应位上的值为 1 。因此，答案为 2^0 + 2^3 = 9 。
        // 示例 2：
        //
        // 输入：nums = [2,12,1,11,4,5], k = 6
        // 输出：0
        // 解释：因为 k == 6 == nums.length ，所以数组的 6-or 等于其中所有元素按位与运算的结果。因此，答案为 2 AND 12 AND 1 AND 11 AND 4 AND 5 = 0 。
        // 示例 3：
        //
        // 输入：nums = [10,8,5,9,11,6,8], k = 1
        // 输出：15
        // 解释：因为 k == 1 ，数组的 1-or 等于其中所有元素按位或运算的结果。因此，答案为 10 OR 8 OR 5 OR 9 OR 11 OR 6 OR 8 = 15 。
        //
        //
        // 提示：
        //
        // 1 <= nums.length <= 50
        // 0 <= nums[i] < 2^31
        // 1 <= k <= nums.length
    }


    public class Solution_2575
    {
        [TestCase("998244353", 3,  ExpectedResult = new[] { 1, 1, 0, 0, 0, 1, 1, 0, 0 })]
        [TestCase("1010",      10, ExpectedResult = new[] { 0, 1, 0, 1 })]
        public int[] DivisibilityArray(string word, int m)
        {
            var       result = new int[word.Length];
            Span<int> dpNums = stackalloc int[word.Length];

            dpNums[0] = (word[0] - '0') % m;

            for (var i = 1 ; i < word.Length ; i++)
            {
                dpNums[i] = (int)((word[i] - '0' + dpNums[i - 1] * 10L) % m);
            }

            for (var i = 0 ; i < word.Length ; i++)
            {
                result[i] = dpNums[i] == 0 ? 1 : 0;
            }

            return result;
        }


        public int[] DivisibilityArray2(string word, int m)
        {
            var  n     = word.Length;
            var  div   = new int[n];
            long value = 0;
            for (var i = 0 ; i < n ; i++)
            {
                var digit = word[i] - '0';
                value = (value * 10 + digit) % m;
                if (value == 0)
                {
                    div[i] = 1;
                }
                else
                {
                    div[i] = 0;
                }
            }

            return div;
        }
        // 2575. 找出字符串的可整除数组  中等
        //
        // 给你一个下标从 0 开始的字符串 word ，长度为 n ，由从 0 到 9 的数字组成。另给你一个正整数 m 。
        //
        // word 的 可整除数组 div  是一个长度为 n 的整数数组，并满足：
        //
        // 如果 word[0,...,i] 所表示的 数值 能被 m 整除，div[i] = 1
        // 否则，div[i] = 0
        // 返回 word 的可整除数组。
        //


        // 示例 1：
        //
        // 输入：word = "998244353", m = 3
        // 输出：[1,1,0,0,0,1,1,0,0]
        // 解释：仅有 4 个前缀可以被 3 整除："9"、"99"、"998244" 和 "9982443"
        // 示例 2：
        //
        // 输入：word = "1010", m = 10
        // 输出：[0,1,0,1]
        // 解释：仅有 2 个前缀可以被 10 整除："10" 和 "1010"
        //
        //
        // 提示：
        //
        // 1 <= n <= 10^5
        // word.length == n
        // word 由数字 0 到 9 组成
        // 1 <= m <= 109
    }


    public class Solution_1261
    {
        public class FindElements(TreeNode root)
        {
            public readonly TreeNode Root = root;

            public bool Find(int target)
            {
                var stack = new Stack<bool>();

                while (target > 0)
                {
                    stack.Push(target % 2 == 0);
                    target = (target - 1) / 2;
                }

                var node = Root;

                while (stack.Count > 0)
                {
                    if (node == null)
                    {
                        return false;
                    }

                    node = stack.Pop() ? node.right : node.left;
                }


                return node != null;
            }
        }
        // 1261. 在受污染的二叉树中查找元素   中等
        // 给出一个满足下述规则的二叉树：
        // 
        // root.val == 0
        // 如果 treeNode.               val == x 且 treeNode.left != null，那么 treeNode.left.  val == 2 * x + 1
        // 如果 treeNode.               val == x 且 treeNode.right != null，那么 treeNode.right.val == 2 * x + 2
        // 现在这个二叉树受到「污染」，所有的 treeNode.val 都变成了 -1。
        //
        // 请你先还原二叉树，然后实现 FindElements 类：
        //
        // FindElements(TreeNode* root) 用受污染的二叉树初始化对象，你需要先把它还原。
        // bool find(int          target) 判断目标值 target 是否存在于还原后的二叉树中并返回结果。


        [Test]
        public void Test()
        {
            var root = CreateTree("[-1,null,-1]");
            var obj  = new FindElements(root);
            var ans1 = obj.Find(1);
            var ans2 = obj.Find(2);
        }

        // * Your FindElements object will be instantiated and called as such:
        // * FindElements obj = new FindElements(root);
        // * bool param_1 = obj.Find(target);
    }
}