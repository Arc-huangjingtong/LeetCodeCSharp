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


    public TreeNode BuildTree(int[] inorder, int[] postorder)
    {
        var inorderMap = new Dictionary<int, int>();

        for (var i = 0 ; i < inorder.Length ; i++)
        {
            inorderMap[inorder[i]] = i;
        }

        return BuildTree(0, inorder.Length - 1, 0, postorder.Length - 1);

        TreeNode BuildTree(int inStart, int inEnd, int postStart, int postEnd)
        {
            if (inStart > inEnd || postStart > postEnd)
            {
                return null;
            }

            var root  = new TreeNode(postorder[postEnd]);
            var index = inorderMap[root.val];

            root.left = BuildTree(inStart, index - 1, postStart, postStart + index - inStart - 1);

            root.right = BuildTree(index + 1, inEnd, postStart + index - inStart, postEnd - 1);

            return root;
        }
    }

    [Test]
    public void TestMethod_BuildTree()
    {
        var inorder   = new[] { 9, 3, 15, 20, 7 };
        var postorder = new[] { 9, 15, 7, 20, 3 };

        var tree    = BuildTree(inorder, postorder);
        var treeStr = CreateArrayString(tree);

        Assert.That(treeStr, Is.EqualTo("[3,9,20,null,null,15,7]"));
    }

    // 889. 根据前序和后序遍历构造二叉树
    // preorder = [1,2,4,5,3,6,7]
    // postorder = [4,5,2,6,7,3,1]

    // 返回如下的二叉树：
    //      1
    //    /   \
    //   2     3
    //  / \   / \
    // 4  5  6   7

    public TreeNode ConstructFromPrePost(int[] preorder, int[] postorder)
    {
        var postMap = new Dictionary<int, int>();
        for (var i = 0 ; i < postorder.Length ; i++)
        {
            postMap[postorder[i]] = i;
        }


        return BuildTree(0, preorder.Length - 1, 0, postorder.Length - 1);

        TreeNode BuildTree(int preStart, int preEnd, int postStart, int postEnd)
        {
            if (preStart > preEnd || postStart > postEnd)
            {
                return null;
            }

            var root = new TreeNode(preorder[preStart]);

            if (preStart == preEnd)
            {
                return root;
            }

            var leftPreStart = preStart + 1;
            var leftPostEnd  = postMap[preorder[leftPreStart]];
            var leftPreEnd   = leftPreStart + leftPostEnd - postStart;

            var rightPreStart  = leftPreEnd  + 1;
            var rightPostStart = leftPostEnd + 1;
            var rightPostEnd   = postEnd     - 1;


            root.left  = BuildTree(leftPreStart,  leftPreEnd, postStart,      leftPostEnd);
            root.right = BuildTree(rightPreStart, preEnd,     rightPostStart, rightPostEnd);

            return root;
        }
    }

    public long KthLargestLevelSum(TreeNode root, int k)
    {
        var levelSum = new List<long>();

        Dfs(root, 0);

        if (levelSum.Count < k)
        {
            return -1;
        }

        return levelSum.OrderByDescending(x => x).Skip(k - 1).First();

        void Dfs(TreeNode node, int level)
        {
            if (node == null)
            {
                return;
            }


            if (levelSum.Count == level)
            {
                levelSum.Add(node.val);
            }
            else
            {
                levelSum[level] += node.val;
            }


            Dfs(node.left,  level + 1);
            Dfs(node.right, level + 1);
        }
    }


    public long KthLargestLevelSum2(TreeNode root, int k)
    {
        var levelSumArray = new List<long>();
        var queue         = new Queue<TreeNode>();

        queue.Enqueue(root);
        while (queue.Count > 0)
        {
            var levelNodes = new List<TreeNode>(queue);
            var levelSum   = 0L;
            queue.Clear();
            foreach (var node in levelNodes)
            {
                levelSum += node.val;
                if (node.left != null)
                {
                    queue.Enqueue(node.left);
                }

                if (node.right != null)
                {
                    queue.Enqueue(node.right);
                }
            }

            levelSumArray.Add(levelSum);
        }


        if (levelSumArray.Count < k)
        {
            return -1;
        }

        levelSumArray.Sort();
        return levelSumArray[^k];
    }

    public string Convert(string s, int numRows)
    {
        if (numRows == 1) return s;

        var list = new List<StringBuilder>();

        for (var i = 0 ; i < numRows ; i++)
        {
            list.Add(new());
        }

        var factor = 2 * numRows - 2;

        for (var i = 0 ; i < s.Length ; i++)
        {
            var index = i % factor;

            index = index < numRows ? index : factor - index;

            list[index].Append(s[i]);
        }

        var ans = string.Empty;

        foreach (var sb in list)
        {
            ans += sb.ToString();
        }

        return ans;
    }



    public IList<IList<int>> ClosestNodes(TreeNode root, IList<int> queries)
    {
        var ans  = new List<IList<int>>();
        var list = new List<int>();
        Dfs(root);
        list.Sort();

        foreach (var query in queries)
        {
            var index = list.BinarySearch(query);
            if (index < 0)
            {
                index = ~index;
            }

            if (index >= list.Count)
            {
                ans.Add(new List<int> { list[^1], -1 });
                continue;
            }

            if (query == list[index])
            {
                ans.Add(new List<int> { query, query });
                continue;
            }


            var left  = index - 1;
            var right = index;

            var leftVal  = left  >= 0 ? list[left] : -1;
            var rightVal = right < list.Count ? list[right] : -1;

            ans.Add(new List<int> { leftVal, rightVal });
        }


        return ans;

        void Dfs(TreeNode node)
        {
            if (node == null)
            {
                return;
            }

            Dfs(node.left);
            list.Add(node.val);
            Dfs(node.right);
        }
    }


    [Test]
    public void TestMethod_Convert()
    {
        var tree = CreateTree("[6,2,8,0,4,7,9,null,null,3,5]");
        var L    = new TreeNode(2);
        var R    = new TreeNode(4);

        var ans = LowestCommonAncestor(tree, L, R);
    }


    public TreeNode LowestCommonAncestor(TreeNode root, TreeNode L, TreeNode R)
    {
        var ans  = (TreeNode)null!;
        var valL = L.val;
        var valR = R.val;

        if (valR < valL)
        {
            (valR, valL) = (valL, valR);
        }

        GetCommon(root);

        return ans;

        void GetCommon(TreeNode node)
        {
            if (node == null)
            {
                return;
            }

            var curValue = node.val;
            if (curValue >= valL && curValue <= valR)
            {
                ans = node;
                return;
            }

            if (curValue >= valL && curValue >= valR)
            {
                GetCommon(node.left);
            }

            if (curValue <= valL && curValue <= valR)
            {
                GetCommon(node.right);
            }
        }
    }


    [TestCase("aa",    "a",        ExpectedResult = false)]
    [TestCase("aa",    "a*",       ExpectedResult = true)]
    [TestCase("ab",    ".*",       ExpectedResult = true)]
    [TestCase("aab",   "c*a*b",    ExpectedResult = true)]
    [TestCase("aaa",   "a*a",      ExpectedResult = true)]
    [TestCase("aaa",   "ab*a*c*a", ExpectedResult = true)]
    [TestCase("a",     ".*..a*",   ExpectedResult = false)]
    [TestCase("bbbba", ".*a*a",    ExpectedResult = true)]
    public bool IsMatch(string s, string p)
    {
        var quene_s  = new Queue<char>(s);
        var quene_p  = new Queue<char>(p);
        var lastChar = ' ';
        var lastStar = false;

        while (quene_s.Count > 0 || quene_p.Count > 0)
        {
            var _p     = quene_p.Count > 0 ? quene_p.Dequeue() : ' ';
            var _pstar = quene_p.Count > 0 && quene_p.Peek() == '*';

            if (_pstar)
            {
                quene_p.Dequeue();

                if (quene_s.Count == 0)
                {
                    if (quene_p.Count > 0 && quene_p.Peek() == lastChar)
                    {
                        quene_p.Dequeue();
                    }

                    continue;
                }

                if (quene_p.Count > 0 && quene_p.Peek() == _p)
                {
                    quene_p.Dequeue();
                }


                while (quene_s.Count > 0 && RegexEqual(quene_s.Peek(), _p))
                {
                    quene_s.Dequeue();
                    lastChar = _p;
                }

                // if (quene_p.Count > 0 && quene_p.Peek() == lastChar)
                // {
                //     quene_p.Dequeue();
                // }
            }
            else
            {
                if (quene_s.Count == 0)
                {
                    return false;
                }

                if (!RegexEqual(quene_s.Peek(), _p))
                {
                    return false;
                }

                quene_s.Dequeue();
            }
        }

        return quene_s.Count == 0 && quene_p.Count == 0;

        bool RegexEqual(char c, char cp)
        {
            if (cp == '.')
            {
                return true;
            }

            return c == cp;
        }
    }

    // 给你一个字符串 s 和一个字符规律 p，请你来实现一个支持 '.' 和 '*' 的正则表达式匹配。
    //
    // '.' 匹配任意单个字符
    // '*' 匹配零个或多个前面的那一个元素
    //     所谓匹配，是要涵盖 整个 字符串 s的，而不是部分字符串。
    //
    //
    // 示例 1：
    //
    // 输入：s = "aa", p = "a"
    // 输出：false
    // 解释："a" 无法匹配 "aa" 整个字符串。
    // 示例 2:
    //
    // 输入：s = "aa", p = "a*"
    // 输出：true
    // 解释：因为 '*' 代表可以匹配零个或多个前面的那一个元素, 在这里前面的元素就是 'a'。因此，字符串 "aa" 可被视为 'a' 重复了一次。
    // 示例 3：
    //
    // 输入：s = "ab", p = ".*"
    // 输出：true
    // 解释：".*" 表示可匹配零个或多个（'*'）任意字符（'.'）。



    public static bool[] Primes = null!;

    public static bool[] CountPrimes(int n)
    {
        if (n < 3) return [];

        Span<bool> isPrime = stackalloc bool[n];

        isPrime.Fill(true);
        isPrime[0] = false;
        isPrime[1] = false;

        var sqrt = (int)Math.Sqrt(n) + 1;

        for (var i = 2 ; i < sqrt ; i++)
        {
            if (!isPrime[i])
            {
                continue;
            }

            //为什么从i*i开始,因为i*2,i*3这些数已经被i*2,i*3这些数标记过了
            for (var j = i * i ; j < n ; j += i)
            {
                isPrime[j] = false;
            }
        }

        return isPrime.ToArray(); //直接减去index:0/1 的情况
    }

    public long CountPaths(int n, int[][] edges)
    {
        //更具题目范围生成质数表
        Primes ??= CountPrimes(100001);

        //1 <= ui, vi <= n 隐藏条件：说明节点值对应1-n;

        //生成邻接表
        var graph = new List<int>[n + 1];

        for (var i = 1 ; i <= n ; i++)
        {
            graph[i] = [];
        }

        foreach (var edge in edges)
        {
            int i = edge[0], j = edge[1];
            graph[i].Add(j);
            graph[j].Add(i);
        }


        Span<long> count = stackalloc long[n + 1];
        var        seen  = new List<int>();
        var        ans   = 0L;

        //以质数为节点，遍历邻接表,找所有的非质数节点,记录每个节点的非质数子节点数量
        //然后遍历质数节点，计算每个质数节点的路径数量
        //最后累加所有质数节点的路径数量 n1*n2+n2*n3+n3*n4+...
        for (var i = 2 ; i <= n ; i++)
        {
            if (!Primes[i])
            {
                continue;
            }

            var cur = 0L;
            foreach (var j in graph[i])
            {
                if (Primes[j]) continue;


                if (count[j] == 0)
                {
                    seen.Clear();
                    DFS(j, 0);
                    var cnt = seen.Count;
                    foreach (var k in seen)
                    {
                        count[k] = cnt;
                    }
                }

                ans += count[j] * cur;
                cur += count[j];
            }

            ans += cur;
        }

        return ans;

        void DFS(int i, int pre)
        {
            seen.Add(i);
            foreach (var j in graph[i])
            {
                if (j != pre && !Primes[j])
                {
                    DFS(j, i);
                }
            }
        }
    }


    [TestCase(7, new[] { 1, 5, 2, 2, 3, 3, 1 }, ExpectedResult = 6)]
    public int MinIncrements(int n, int[] cost)
    {
        int result = 0;
        for (int i = n - 1 ; i > 0 ; i -= 2)
        {
            var (max, min)    =  cost[i] > cost[i - 1] ? (cost[i], cost[i - 1]) : (cost[i - 1], cost[i]);
            result            += (max - min);
            cost[(i - 1) / 2] += max;
        }

        return result;
    }
    // 1 + 5 + 2 = 8   
    // 1 + 5 + 3 = 9
    // 1 + 2 + 3 = 6
    // 1 + 2 + 1 = 4

    [Test]
    public void Test()
    {
        CountPaths(5, [[1, 2], [1, 3], [2, 4], [2, 5]]);
    }

    [TestCase(new[] { 993335, 993336, 993337, 993338, 993339, 993340, 993341 }, ExpectedResult = false)]
    [TestCase(new[] { 4, 4, 4, 5, 6 },                                          ExpectedResult = true)]
    public bool validPartition(int[] nums)
    {
        var len = nums.Length;

        if (len % 3 != 0 && len % 2 != 0 && len % 2 != 1 && len % 3 != 2)
        {
            return false;
        }

        Span<bool> dp = stackalloc bool[++len];
        dp[0] = true;
        dp[2] = nums[0] == nums[1];

        for (var i = 3 ; i < len ; i++)
        {
            var three = i - 3;
            var two   = i - 2;
            var v3    = nums[i - 3];
            var v2    = nums[i - 2];
            var v1    = nums[i - 1];

            if (dp[two]   && dp[i] == false) dp[i] = v2 == v1;
            if (dp[three] && dp[i] == false) dp[i] = v3     == v2 && v2     == v1;
            if (dp[three] && dp[i] == false) dp[i] = v3 + 1 == v2 && v3 + 2 == v1;
        }


        return dp[^1];
    }


    public int ReachableNodes2(int n, int[][] edges, int[] restricted)
    {
        var restr  = new bool[n];
        var graph  = new List<int>[n];
        var answer = 0;

        foreach (var rest in restricted)
        {
            restr[rest] = true;
        }

        for (var i = 0 ; i < n ; i++)
        {
            graph[i] = [];
        }

        foreach (var edge in edges)
        {
            var edge0 = edge[0];
            var edge1 = edge[1];

            graph[edge0].Add(edge1);
            graph[edge1].Add(edge0);
        }

        Dfs(0, -1);

        return answer;


        void Dfs(int x, int f)
        {
            answer++;

            foreach (var y in graph[x])
            {
                if (y != f && !restr[y])
                {
                    Dfs(y, x);
                }
            }
        }
    }

    public int ReachableNodes(int n, int[][] edges, int[] restricted)
    {
        var graph = new List<int>[n];
        for (var i = 0 ; i < n ; i++)
        {
            graph[i] = [];
        }


        var set = restricted.ToHashSet();
        foreach (var edge in edges)
        {
            if (set.Contains(edge[0]) || set.Contains(edge[1])) continue;

            graph[edge[0]].Add(edge[1]);
            graph[edge[1]].Add(edge[0]);
        }

        Span<bool> visited = stackalloc bool[n];
        var        queue   = new Queue<int>();
        var        result  = 1;
        queue.Enqueue(0);
        visited[0] = true;
        while (queue.Count > 0)
        {
            var t = queue.Dequeue();
            foreach (var i in graph[t])
            {
                if (visited[i]) continue;

                queue.Enqueue(i);
                visited[i] = true;
                result++;
            }
        }

        return result;
    }


    public class Solution2
    {
        private List<List<int>> graph  = [];
        private int             result = 1;

        public int ReachableNodes(int n, int[][] edges, int[] restricted)
        {
            for (var i = 0 ; i < n ; i++)
            {
                graph.Add([]);
            }

            var set = restricted.ToHashSet();
            foreach (var edge in edges)
            {
                if (set.Contains(edge[0]) || set.Contains(edge[1])) continue;

                graph[edge[0]].Add(edge[1]);
                graph[edge[1]].Add(edge[0]);
            }

            Dfs(0, -1);
            return result;
        }

        private void Dfs(int x, int parent)
        {
            foreach (var i in graph[x])
            {
                if (i == parent) continue;

                result++;
                Dfs(i, x);
            }
        }
    }


    public class Solution
    {
        public int RootCount(int[][] edges, int[][] guesses, int k)
        {
            var n     = edges.Length + 1;
            var graph = new List<int>[n];
            var cnt   = 0;
            var res   = 0;
            for (var i = 0 ; i < n ; i++)
            {
                graph[i] = [];
            }

            HashSet<long> set = [];

            foreach (var v in edges)
            {
                graph[v[0]].Add(v[1]);
                graph[v[1]].Add(v[0]);
            }

            foreach (var v in guesses)
            {
                set.Add(H(v[0], v[1]));
            }


            Dfs(0, -1);
            Redfs(0, -1, cnt);
            return res;

            long H(int x, int y) => ((long)x << 20) | (uint)y;

            void Dfs(int x, int fat)
            {
                foreach (var y in graph[x])
                {
                    if (y == fat)
                    {
                        continue;
                    }

                    cnt += set.Contains(H(x, y)) ? 1 : 0;
                    Dfs(y, x);
                }
            }

            void Redfs(int x, int fat, int cnt)
            {
                if (cnt >= k)
                {
                    res++;
                }

                foreach (var y in graph[x])
                {
                    if (y == fat)
                    {
                        continue;
                    }

                    Redfs(y, x, cnt - (set.Contains(H(x, y)) ? 1 : 0) + (set.Contains(H(y, x)) ? 1 : 0));
                }
            }
        }
    }



    //[TestCase(new[] { 2, 11, 10, 1, 3 },                                                     10,         ExpectedResult = 2)]
    //[TestCase(new[] { 1, 1, 2, 4, 9 },                                                       20,         ExpectedResult = 4)]
    [TestCase(new[] { 1000000000, 999999999, 1000000000, 999999999, 1000000000, 999999999 }, 1000000000, ExpectedResult = 2)]
    public int MinOperations(int[] nums, int k)
    {
        var order = new PriorityQueue<int, int>(nums.Length);
        var ans   = 0;
        var small = 0;

        for (var i = 0 ; i < nums.Length ; i++)
        {
            var num = nums[i];

            if (num < k) small++;

            order.Enqueue(num, num);
        }

        while (order.Count >= 2 && small > 0)
        {
            var min1 = order.Dequeue();
            var min2 = order.Dequeue();

            if (min1 < k) small--;
            if (min2 < k) small--;

            var n = Math.Max(min1, min2) + Math.Min(min1, min2) * 2L;
            if (n < k) small++;
            if (n > int.MaxValue) n = int.MaxValue;
            order.Enqueue((int)n, (int)n);


            ans++;
        }


        return ans;
    }



    public int[] CountPairsOfConnectableServers(int[][] edges, int signalSpeed)
    {
        var n         = edges.Length + 1;
        var ans       = new int[n];
        var graphDict = new Dictionary<int, (int, HashSet<int>)[]>();

        for (var i = 0 ; i < n ; i++)
        {
            graphDict.Add(i, new (int, HashSet<int>)[n]);
        }

        foreach (var edge in edges)
        {
            graphDict[edge[0]][edge[1]] = (edge[2], []);
            graphDict[edge[1]][edge[0]] = (edge[2], []);
        }


        return ans;

        //Dictionary<int, (int, HashSet<int>)[]>();
        //           节点，链接的节点数组： (权重,经过的节点)


        void DFS(int start)
        {
            for (var index = 0 ; index < graphDict[start].Length ; index++)
            {
                if (index == start) continue;
                var link       = graphDict[start][index];
                var linkWeight = link.Item1;
                if (linkWeight == 0) continue;
                var linkIndex = link.Item2;
            }
        }
    }


    [Test]
    public void TEST()
    {
        int[][] edges = [[0, 6, 3], [6, 5, 3], [0, 3, 1], [3, 2, 7], [3, 1, 6], [3, 4, 2]];

        var temp = new ServerNetwork();
        var anss = temp.CountPairsOfConnectableServers(edges, 3);


        var ans = CountPairsOfConnectableServers(edges, 3);
    }


    public class ServerNetwork
    {
        private Dictionary<int, List<(int neighbor, int weight)>> graph;



        public int[] CountPairsOfConnectableServers(int[][] edges, int signalSpeed)
        {
            graph = new Dictionary<int, List<(int, int)>>();
            foreach (var edge in edges)
            {
                if (!graph.ContainsKey(edge[0])) graph[edge[0]] = new List<(int, int)>();
                if (!graph.ContainsKey(edge[1])) graph[edge[1]] = new List<(int, int)>();

                graph[edge[0]].Add((edge[1], edge[2]));
                graph[edge[1]].Add((edge[0], edge[2]));
            }

            int   n     = graph.Count;
            int[] count = new int[n];

            for (int i = 0 ; i < n ; i++)
            {
                var distances = BFS(i, signalSpeed);
                count[i] = CalculatePairs(distances, signalSpeed);
            }

            return count;
        }

        private int CalculatePairs(Dictionary<int, int> distances, int signalSpeed)
        {
            var pairsCount           = 0;
            var connectableDistances = distances.Where(kvp => kvp.Value % signalSpeed == 0).Select(kvp => kvp.Key).ToList();
            connectableDistances.Sort();

            for (int i = 0 ; i < connectableDistances.Count ; i++)
            {
                for (int j = i + 1 ; j < connectableDistances.Count ; j++)
                {
                    int a = connectableDistances[i];
                    int b = connectableDistances[j];
                    if (a < b)
                    {
                        pairsCount++;
                    }
                }
            }

            return pairsCount;
        }

        private Dictionary<int, int> BFS(int root, int signalSpeed)
        {
            var distances = new Dictionary<int, int>();
            var queue     = new Queue<(int node, int weight)>();

            distances[root] = 0;
            queue.Enqueue((root, 0));

            while (queue.Count > 0)
            {
                var (current, weight) = queue.Dequeue();
                foreach (var (neighbor, edgeWeight) in graph[current])
                {
                    int nextWeight = weight + edgeWeight;
                    if (!distances.ContainsKey(neighbor))
                    {
                        distances[neighbor] = nextWeight;
                        queue.Enqueue((neighbor, nextWeight));
                    }
                }
            }

            return distances;
        }
    }


    public class MyStack
    {
        private readonly Queue<int> queue1 = new();
        private readonly Queue<int> queue2 = new();

        public void Push(int x)
        {
            Queue<int> q1, q2;

            if (queue1.Count == 0)
            {
                q1 = queue1;
                q2 = queue2;
            }
            else
            {
                q2 = queue1;
                q1 = queue2;
            }

            q1.Enqueue(x);

            while (q2.Count > 0)
            {
                var o = q2.Dequeue();
                q1.Enqueue(o);
            }
        }

        public int  Pop()   => queue1.Count == 0 ? queue2.Dequeue() : queue1.Dequeue();
        public int  Top()   => queue1.Count == 0 ? queue2.Peek() : queue1.Peek();
        public bool Empty() => queue1.Count == 0 && queue2.Count == 0;
    }


    //["push","push","push","push","pop","push","pop","pop","pop","pop"]
    //[[1]   ,[2]   ,[3]   ,[4]   ,[]   ,[5]   ,[]   ,[]   ,[]   ,[]]
    public class MyQueue
    {
        private readonly Stack<int> Stack1 = new();
        private readonly Stack<int> Stack2 = new();



        public void Push(int x)
        {
            while (Stack2.Count != 0)
            {
                Stack1.Push(Stack2.Pop());
            }

            Stack1.Push(x);

            while (Stack1.Count != 0)
            {
                Stack2.Push(Stack1.Pop());
            }
        }

        public int  Pop()   => Stack2.Pop();
        public int  Peek()  => Stack2.Peek();
        public bool Empty() => Stack2.Count == 0;

        /**
    * Your MyQueue object will be instantiated and called as such:
    * MyQueue obj = new MyQueue();
    * obj.Push(x);
    * int param_2 = obj.Pop();
    * int param_3 = obj.Peek();
    * bool param_4 = obj.Empty();
    */
    }


    public class Solution_78
    {
        public IList<IList<int>> Combine(int n, int k)
        {
            var nums = new int[n];
            for (var i = 0 ; i < n ; i++)
            {
                nums[i] = i + 1;
            }

            var ans = Subsets(nums);

            return ans.Where(x => x.Count == k).ToList();
        }

        public IList<IList<int>> Subsets(int[] nums)
        {
            var res       = new List<IList<int>>();
            var enumCount = (1 << nums.Length) - 1;

            res.Add(new List<int>());

            while (enumCount > 0)
            {
                var sub = new List<int>();
                for (var i = 0 ; i < nums.Length ; i++)
                {
                    if ((enumCount & (1 << i)) != 0)
                    {
                        sub.Add(nums[i]);
                    }
                }

                res.Add(sub);
                enumCount--;
            }

            return res;
        }
    }


    [Test]
    public void Test09()
    {
        var ans = Subsets([1, 2, 3]);

        var ccc = ans;
    }

    public IList<IList<int>> Subsets(int[] nums)
    {
        var res = new List<IList<int>>();
        for (var i = 0 ; i < 1 << nums.Length ; i++)
        {
            var sub = new List<int>();
            for (var j = 0 ; j < nums.Length ; j++)
            {
                if (((i >> j) & 1) == 1)
                {
                    sub.Add(nums[j]);
                }
            }

            res.Add(sub);
        }

        return res;
    }

    public static BitArray Decrement(BitArray bits)
    {
        bool carry   = true; // 我们要减一，所以从最低位开始借位
        int  i       = 0;
        var  newBits = new BitArray(bits);

        while (carry && i < newBits.Length)
        {
            carry      = !newBits[i]; // 如果当前位是0，我们借位，carry保持true
            newBits[i] = !newBits[i]; // 如果当前位是0，变成1；如果是1，变成0（借位）
            i++;
        }

        return newBits;
    }
}