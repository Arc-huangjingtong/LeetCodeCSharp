namespace LeetCodeCSharp;

/// <summary> 572. 另一棵树的子树 </summary>
public class Solution_572
{
    public bool IsSubtree(TreeNode root, TreeNode subRoot)
    {
        return DFS(root);

        bool DFS(TreeNode head)
        {
            if (head == null) return false;

            if (head.val == subRoot.val && IsSubtree(head, subRoot))
            {
                return true;
            }

            return DFS(head.left) || DFS(head.right);
        }


        bool IsSubtree(TreeNode head, TreeNode sub)
        {
            if (head == null && sub == null) return true;

            if (head?.val != sub?.val) return false;

            return IsSubtree(head?.left, sub?.left) || IsSubtree(head?.right, sub?.right);
        }
    }
}


/// <summary> 690. 员工的重要性 </summary>
public class Solution_690
{
    // Definition for Employee.
    public class Employee
    {
        public int        id;
        public int        importance;
        public IList<int> subordinates;
    }


    // DFS 标准模板
    public int GetImportance(IList<Employee> employees, int id)
    {
        var dict = new Dictionary<int, Employee>();

        foreach (var employee in employees)
        {
            dict[employee.id] = employee;
        }

        return DFS(id);

        int DFS(int id)
        {
            var employee = dict[id];
            var sum      = employee.importance;

            foreach (var subordinate in employee.subordinates)
            {
                sum += DFS(subordinate);
            }

            return sum;
        }
    }
}


/// <summary>[未解决] 815. 公交路线 算术评级: 7 第 79 场周赛Q4-1964 </summary>
public class Solution_815
{
    public int NumBusesToDestination(int[][] routes, int source, int target)
    {
        var graph = new Dictionary<int, List<(int, int)>>();

        for (var i = 0 ; i < routes.Length ; i++)
        {
            for (var index = 0 ; index < routes[i].Length - 1 ; index++)
            {
                var start = routes[i][index];
                var end   = routes[i][index + 1];

                graph.TryAdd(start, []);
                graph[start].Add((end, i));
            }

            graph.TryAdd(routes[i][^1], []);
            graph[routes[i][^1]].Add((routes[i][0], i));
        }

        var minStep = int.MaxValue;
        var visited = new HashSet<int>();
        var dict    = new int[routes.Length];


        DFS(source);

        return minStep == int.MaxValue ? -1 : minStep;


        void DFS(int start)
        {
            var num = dict.Count(x => x > 0);
            if (start == target)
            {
                minStep = Math.Min(minStep, num);
                return;
            }

            if (num >= minStep) return;

            foreach (var next in graph[start])
            {
                if (!visited.Add(next.Item1))
                {
                    continue;
                }

                dict[next.Item2]++;

                DFS(next.Item1);

                visited.Remove(next.Item1);
                dict[next.Item2]--;
            }
        }
    }


    public class Solution
    {
        public int NumBusesToDestination(int[][] routes, int source, int target)
        {
            if (source == target)
            {
                return 0;
            }

            int                         n    = routes.Length;
            bool[,]                     edge = new bool[n, n];
            Dictionary<int, IList<int>> rec  = new Dictionary<int, IList<int>>();
            for (int i = 0 ; i < n ; i++)
            {
                foreach (int site in routes[i])
                {
                    IList<int> list = new List<int>();
                    if (rec.ContainsKey(site))
                    {
                        list = rec[site];
                        foreach (int j in list)
                        {
                            edge[i, j] = edge[j, i] = true;
                        }

                        rec[site].Add(i);
                    }
                    else
                    {
                        list.Add(i);
                        rec.Add(site, list);
                    }
                }
            }

            int[] dis = new int[n];
            Array.Fill(dis, -1);
            Queue<int> que = new Queue<int>();
            if (rec.ContainsKey(source))
            {
                foreach (int bus in rec[source])
                {
                    dis[bus] = 1;
                    que.Enqueue(bus);
                }
            }

            while (que.Count > 0)
            {
                int x = que.Dequeue();
                for (int y = 0 ; y < n ; y++)
                {
                    if (edge[x, y] && dis[y] == -1)
                    {
                        dis[y] = dis[x] + 1;
                        que.Enqueue(y);
                    }
                }
            }

            int ret = int.MaxValue;
            if (rec.ContainsKey(target))
            {
                foreach (int bus in rec[target])
                {
                    if (dis[bus] != -1)
                    {
                        ret = Math.Min(ret, dis[bus]);
                    }
                }
            }

            return ret == int.MaxValue ? -1 : ret;
        }
    }


    // 给你一个数组 routes ，表示一系列公交线路，其中每个 routes[i] 表示一条公交线路，第 i 辆公交车将会在上面循环行驶。
    //
    // 例如，路线 routes[0] = [1, 5, 7] 表示第 0 辆公交车会一直按序列 1 -> 5 -> 7 -> 1 -> 5 -> 7 -> 1 -> ... 这样的车站路线行驶。
    // 现在从   source 车站出发（初始时不在公交车上），要前往 target 车站。 期间仅可乘坐公交车。
    //
    // 求出 最少乘坐的公交车数量 。如果不可能到达终点车站，返回 -1 。
    //
    //
    //
    // 示例 1：
    //
    // 输入：routes = [[1,2,7],[3,6,7]], source = 1, target = 6
    // 输出：2
    // 解释：最优策略是先乘坐第一辆公交车到达车站 7 , 然后换乘第二辆公交车到车站 6 。 
    // 示例 2：
    //
    // 输入：routes = [[7,12],[4,5,15],[6],[15,19],[9,12,13]], source = 15, target = 12
    // 输出：-1
    //
    //
    // 提示：
    //
    // 1 <= routes.length <= 500
    // 1 <= routes[i].length <= 10^5
    // routes[i] 中的所有值 互不相同
    // sum(routes[i].length) <= 10^5
    //     0 <= routes[i][j] < 10^6
    // 0 <= source, target < 10^6
}