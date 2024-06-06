namespace LeetCodeCSharp;
///////////////////////////////////////////////////////图论相关题型///////////////////////////////////////////////////////


public class Solution_924
{
    public int MinMalwareSpread(int[][] graph, int[] initial)
    {
        //1.分框操作:一共有N个节点,每个节点都有一个集合,集合中存放的是与该节点相连的节点
        List<HashSet<int>> sets = [];

        for (var i = 0 ; i < graph.Length ; i++)
        {
            sets.Add([i]);
        }

        //2.遍历所有的节点,将相连的节点的集合合并到一起
        for (var i = 0 ; i < graph.Length ; i++)
        {
            for (var j = 0 ; j < graph.Length ; j++)
            {
                if (graph[i][j] == 1)
                {
                    sets[i].UnionWith(sets[j]);
                    sets[j] = sets[i];
                }
            }
        }

        //3.删除重复的集合
        sets = sets.Distinct().ToList();
        //4.统计initial数组中的节点所在多少个框中
        var counts = new int[sets.Count];

        foreach (var node in initial)
        {
            foreach (var set in sets)
            {
                if (set.Contains(node))
                {
                    counts[sets.IndexOf(set)]++;
                    break;
                }
            }
        }

        //5.找到initial数组中能拯救最多的节点
        var saves = new int[sets.Count];


        for (var i = 0 ; i < saves.Length ; i++)
        {
            saves[i] = counts[i] == 1 ? sets[i].Count : 0;
        }


        if (saves.All(x => x == 0) || saves.Count(x => x == saves.Max()) == saves.Length)
        {
            return initial.Min();
        }

        var saveList = new List<int>(saves);
        saveList.RemoveAll(x => x == 0);

        if (saveList.Count != 1 && saveList.Count == saveList.Count(x => x == saveList.Max()))
        {
            return initial.Min();
        }


        var maxSaveSet = sets[saves.ToList().IndexOf(saves.Max())];

        //6.找到initial数组中能拯救最多的节点中的最小节点

        maxSaveSet.IntersectWith(initial);

        return maxSaveSet.Min();
    }



    //推荐:直接枚举所有点,找到能拯救最多的点
    public int MinMalwareSpread2(int[][] graph, int[] initial)
    {
        var len      = graph.Length;
        var ids      = new int[len];
        var idToSize = new Dictionary<int, int>();
        var id       = 0;

        for (var i = 0 ; i < len ; ++i)
        {
            if (ids[i] != 0) continue;

            id++;
            var size  = 1;
            var queue = new Queue<int>();
            queue.Enqueue(i);
            ids[i] = id;

            while (queue.Count > 0)
            {
                var u = queue.Dequeue();
                for (var v = 0 ; v < len ; ++v)
                {
                    if (ids[v] == 0 && graph[u][v] == 1)
                    {
                        ++size;
                        queue.Enqueue(v);
                        ids[v] = id;
                    }
                }
            }

            idToSize.Add(id, size);
        }

        var idToInitials = new Dictionary<int, int>();

        foreach (var u in initial)
        {
            idToInitials.TryAdd(ids[u], 0);
            idToInitials[ids[u]]++;
        }

        int ans = len + 1, ansRemoved = 0;

        foreach (var u in initial)
        {
            var removed = (idToInitials[ids[u]] == 1 ? idToSize[ids[u]] : 0);
            if (removed > ansRemoved || (removed == ansRemoved && u < ans))
            {
                ans        = u;
                ansRemoved = removed;
            }
        }

        return ans;
    }



    [Test]
    public void Test()
    {
        int[][] graph = [[1, 1, 0], [1, 1, 0], [0, 0, 1]];
        // [
        //   // 0  1  2  3  4  5  6  7  8  9  10
        //     [1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1], // 0
        //     [0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0]  // 1
        //   , [0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0]  // 2
        //   , [0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0]  // 3
        //   , [1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0]  // 4
        //   , [0, 0, 0, 1, 0, 1, 0, 0, 1, 1, 0]  // 5
        //   , [0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0]  // 6
        //   , [0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0]  // 7
        //   , [0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0]  // 8
        //   , [0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0]  // 9
        //   , [1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1]  //10
        // ];


        //[   0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17]
        //0  [1,0,0,0,0,0,0,0,0,0, 0, 0, 1, 0, 0, 0, 0, 1],
        //1  [0,1,0,0,0,0,0,1,0,0, 0, 0, 0, 0, 1, 0, 0, 0],
        //2  [0,0,1,0,0,1,0,0,0,0, 0, 0, 0, 0, 0, 0, 0, 0],
        //3  [0,0,0,1,0,0,0,1,0,0, 0, 1, 0, 1, 1, 0, 0, 0],
        //4  [0,0,0,0,1,0,0,0,0,0, 0, 0, 0, 0, 0, 1, 0, 1],
        //5  [0,0,1,0,0,1,0,1,0,0, 0, 0, 0, 1, 0, 0, 0, 0],
        //6  [0,0,0,0,0,0,1,0,0,0, 0, 0, 1, 0, 0, 1, 1, 0],
        //7  [0,1,0,1,0,1,0,1,0,0, 0, 0, 0, 0, 0, 0, 0, 0],
        //8  [0,0,0,0,0,0,0,0,1,0, 0, 0, 1, 0, 0, 0, 0, 0],
        //9  [0,0,0,0,0,0,0,0,0,1, 0, 0, 0, 0, 1, 0, 0, 0],
        //10 [0,0,0,0,0,0,0,0,0,0, 1, 0, 1, 0, 0, 0, 0, 0],
        //11 [0,0,0,1,0,0,0,0,0,0, 0, 1, 0, 0, 0, 0, 0, 0],
        //12 [1,0,0,0,0,0,1,0,1,0, 1, 0, 1, 0, 0, 0, 0, 1],
        //13 [0,0,0,1,0,1,0,0,0,0, 0, 0, 0, 1, 0, 0, 0, 0],
        //14 [0,1,0,1,0,0,0,0,0,1, 0, 0, 0, 0, 1, 0, 0, 0],
        //15 [0,0,0,0,1,0,1,0,0,0, 0, 0, 0, 0, 0, 1, 0, 0],
        //16 [0,0,0,0,0,0,1,0,0,0, 0, 0, 0, 0, 0, 0, 1, 0],
        //17 [1,0,0,0,1,0,0,0,0,0, 0, 0, 1, 0, 0, 0, 0, 1]]
        int[] initial = [0, 1, 2];

        Console.WriteLine(MinMalwareSpread(graph, initial));
    }
}


public class Solution_928
{
    //我们可以从 initial 中删除一个节点，并完全移除该节点以及从该节点到任何其他节点的任何连接
    //请返回移除后能够使 M(initial) 最小化的节点。如果有多个节点满足条件，返回索引 最小的节点

    public int MinMalwareSpread(int[][] graph, int[] initial)
    {
        var length   = graph.Length;
        var graphSet = new HashSet<int>[length];

        for (var i = 0 ; i < length ; i++)
        {
            graphSet[i] = [];
            for (var j = 0 ; j < length ; j++)
            {
                if (graph[i][j] == 1)
                {
                    graphSet[i].Add(j);
                }
            }
        }

        return 0;
    }

    [Test]
    public void Test()
    {
        int[][] graph   = [[1, 1, 0], [1, 1, 0], [0, 0, 1]];
        int[]   initial = [0, 1];

        Console.WriteLine(MinMalwareSpread(graph, initial));
    }
}


/// 难点：计算组合数： ans = 0 x c1 + c1 x c2 + (c1+c2) x c3 + (c1+c2+c3) x c4 
public class Solution_3067
{
    public int[] CountPairsOfConnectableServers(int[][] edges, int signalSpeed)
    {
        var nodeCount = edges.Length + 1;                              // 节点数量
        var graph     = new List<(int node, int distance)>[nodeCount]; // 声明邻接表
        var ans       = new int[nodeCount];                            // 存储结果

        InitGraph(nodeCount, graph, edges); //初始化邻接表

        for (var i = 0 ; i < nodeCount ; i++)
        {
            if (graph[i].Count == 1) continue; //如果只有一个节点与之相连,则不可能有通过该节点的两个节点

            var sum = 0;

            foreach (var edge in graph[i]) //遍历所有的节点 : 思维点:第一层子节点，当成子树，计算每一个子树的能整除的节点数
            {
                var cnt = DFS(edge.node, i, edge.distance);
                ans[i] += cnt * sum;
                sum    += cnt;
            }
        }

        return ans;

        int DFS(int x, int fa, int sum)
        {
            var cnt = sum % signalSpeed == 0 ? 1 : 0;

            foreach (var (_node, _distance) in graph[x])
            {
                if (_node != fa)
                {
                    cnt += DFS(_node, x, sum + _distance);
                }
            }

            return cnt;
        }


        static void InitGraph(int nodeCount, List<(int node, int distance)>[] graph, int[][] edges)
        {
            for (var i = 0 ; i < nodeCount ; i++)
            {
                graph[i] = [];
            }

            foreach (var edge in edges)
            {
                var node1    = edge[0];
                var node2    = edge[1];
                var distance = edge[2];
                graph[node1].Add((node2, distance));
                graph[node2].Add((node1, distance));
            }
        }
    }



    [Test]
    public void Test()
    {
        // int[][] edges       = [[0, 1, 1], [1, 2, 5], [2, 3, 13], [3, 4, 9], [4, 5, 2]];
        // var     signalSpeed = 1;
        int[][] edges       = [[0, 6, 3], [6, 5, 3], [0, 3, 1], [3, 2, 7], [3, 1, 6], [3, 4, 2]];
        var     signalSpeed = 3;

        var result = CountPairsOfConnectableServers(edges, signalSpeed);

        foreach (var i in result)
        {
            Console.WriteLine(i);
        }
    }
}