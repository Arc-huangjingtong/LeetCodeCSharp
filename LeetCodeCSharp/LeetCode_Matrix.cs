namespace LeetCodeCSharp;
///////////////////////////////////////////////////////矩阵相关题型///////////////////////////////////////////////////////


///<summary> 1329. 将矩阵按对角线排序 </summary>
public class Solution_1329
{
    //纯粹的对角线排序,模拟
    public int[][] DiagonalSort(int[][] mat)
    {
        var n = mat.Length;
        var m = mat[0].Length;

        var diag = new List<List<int>>(m + n);

        for (var i = 0 ; i < m + n ; i++)
        {
            diag.Add([]);
        }

        for (var i = 0 ; i < n ; i++)
        {
            for (var j = 0 ; j < m ; j++)
            {
                diag[i - j + m].Add(mat[i][j]); //i-j+m是为了保证对角线的元素在同一个list中
            }
        }

        foreach (var d in diag)
        {
            d.Sort((a, b) => b - a);
        }

        for (var i = 0 ; i < n ; i++)
        {
            for (var j = 0 ; j < m ; j++)
            {
                mat[i][j] = diag[i - j + m][diag[i - j + m].Count - 1];

                diag[i - j + m].RemoveAt(diag[i - j + m].Count - 1);
            }
        }

        return mat;
    }


    [Test]
    public void Test()
    {
        int[][] mat =
        [
            [11, 25, 66, 1, 69, 7],  // 
            [23, 55, 17, 45, 15, 52] //
          , [75, 31, 36, 44, 58, 8]  //
          , [22, 27, 33, 25, 68, 4]  //
          , [84, 28, 14, 11, 5, 50]
        ];
        //[[5, 17, 4, 1,52, 7],
        // [11,11,25,45,8 ,69],
        // [14,23,25,44,58,15],
        // [22,27,31,36,50,66],
        // [84,28,75,33,55,68]]

        var result = DiagonalSort(mat);
        foreach (var row in result)
        {
            Console.WriteLine(string.Join(",", row));
        }
    }
}


///<summary> 994. 腐烂的橘子 </summary>
public class Solution_994
{
    public static readonly (int, int)[] Directions = [(0, 1), (0, -1), (1, 0), (-1, 0)];

    public static int OrangesRotting2(int[][] grid)
    {
        var n     = grid.Length;
        var m     = grid[0].Length;
        var queue = new Queue<(int, int)>();
        var fresh = 0;

        for (var i = 0 ; i < n ; i++)
        {
            for (var j = 0 ; j < m ; j++)
            {
                switch (grid[i][j])
                {
                    case 2 :
                        queue.Enqueue((i, j));
                        break;
                    case 1 :
                        fresh++;
                        break;
                }
            }
        }

        if (fresh == 0) return 0;

        var minutes = 0;

        while (queue.Count > 0)
        {
            minutes++;
            var size = queue.Count;
            for (var i = 0 ; i < size ; i++)
            {
                var (x, y) = queue.Dequeue();
                foreach (var (dx, dy) in Directions)
                {
                    var (nx, ny) = (x + dx, y + dy);

                    if (nx < 0 || nx >= n || ny < 0 || ny >= m || grid[nx][ny] != 1)
                    {
                        continue;
                    }

                    grid[nx][ny] = 2;
                    fresh--;
                    queue.Enqueue((nx, ny));
                }
            }
        }

        return fresh == 0 ? minutes - 1 : -1;
    }

    public int OrangesRotting(int[][] grid) => OrangesRotting2(grid);
}


public class Temp_1024
{
    [Test]
    public void Test()
    {
        // We use 5 here because we want layers: 2^2, 2^3, 2^4, ... 2^6 (32 directions)
        var DetectionAngle = Enumerable.Range(0, 5)
                                       .SelectMany(i => Enumerable.Range(0, (int)Math.Pow(2, i + 2))
                                                                  .Select(j => j * (360f / (int)Math.Pow(2, i + 2))))
                                       .OrderBy(angle => angle)
                                       .ToArray();

        foreach (var i in DetectionAngle)
        {
            Console.WriteLine(i);
        }
    }
}


public class Solution_2813
{
    public long FindMaximumElegance(int[][] items, int k)
    {
        // 按照利润降序排序
        Array.Sort(items, (item0, item1) => item1[0] - item0[0]);

        var categorySet = new HashSet<int>();
        var stack       = new Stack<int>();
        var profit      = 0L;
        var result      = 0L;

        for (var i = 0 ; i < items.Length ; i++)
        {
            if (i < k)
            {
                profit += items[i][0];
                if (!categorySet.Add(items[i][1]))
                {
                    stack.Push(items[i][0]);
                }
            }
            else if (stack.Count > 0 && categorySet.Add(items[i][1]))
            {
                profit += items[i][0] - stack.Pop();
            }

            result = Math.Max(result, profit + (long)categorySet.Count * categorySet.Count);
        }

        return result;
    }

    // 2813. 子序列最大优雅度 困难
    // 给你一个长度为 n 的二维整数数组 items 和一个整数 k 。
    //
    // items[i] = [profiti, categoryi]，其中 profiti 和 categoryi 分别表示第 i 个项目的利润和类别。
    //
    // 现定义 items 的 子序列 的 优雅度 可以用 total_profit + distinct_categories2 计算，其中 total_profit 是子序列中所有项目的利润总和，distinct_categories 是所选子序列所含的所有类别中不同类别的数量。
    //
    // 你的任务是从 items 所有长度为 k 的子序列中，找出 最大优雅度 。
    //
    // 用整数形式表示并返回 items 中所有长度恰好为 k 的子序列的最大优雅度。
    //
    // 注意：数组的子序列是经由原数组删除一些元素（可能不删除）而产生的新数组，且删除不改变其余元素相对顺序
}


public class Solution_2713
{
    public int MaxIncreasingCells(int[][] mat)
    {
        int m   = mat.Length, n = mat[0].Length;
        var dic = new Dictionary<int, IList<int[]>>();
        var row = new int[m];
        var col = new int[n];

        for (var i = 0 ; i < m ; i++)
        {
            for (var j = 0 ; j < n ; j++)
            {
                dic.TryAdd(mat[i][j], new List<int[]>());
                dic[mat[i][j]].Add([i, j]);
            }
        }

        var keys = new List<int>(dic.Keys);
        keys.Sort();
        foreach (var key in keys)
        {
            var pos = dic[key];
            var res = new List<int>(); // 存放相同数值的答案，便于后续更新 row 和 col
            foreach (var arr in pos)
            {
                res.Add(Math.Max(row[arr[0]], col[arr[1]]) + 1);
            }

            for (var i = 0 ; i < pos.Count ; i++)
            {
                var arr = pos[i];
                var d   = res[i];
                row[arr[0]] = Math.Max(row[arr[0]], d);
                col[arr[1]] = Math.Max(col[arr[1]], d);
            }
        }

        return row.Max();
    }
}


///<summary> 3128. 最大三角形的周长 </summary>
public class Solution_3128
{
    public long NumberOfRightTriangles(int[][] grid)
    {
        var       n   = grid.Length;
        var       m   = grid[0].Length;
        Span<int> row = stackalloc int[m];
        Span<int> col = stackalloc int[n];

        for (var i = 0 ; i < n ; i++)
        {
            for (var j = 0 ; j < m ; j++)
            {
                if (grid[i][j] == 1)
                {
                    row[j]++;
                    col[i]++;
                }
            }
        }

        var result = 0L;

        for (var i = 0 ; i < n ; i++)
        {
            for (var j = 0 ; j < m ; j++)
            {
                if (grid[i][j] == 1)
                {
                    var rowSide = row[j] - 1;
                    var colSide = col[i] - 1;

                    result += rowSide * colSide;
                }
            }
        }


        return result;
    }

    // 算术评级: 4 第 129 场双周赛 Q2  1541
    // 给你一个二维 boolean 矩阵 grid
    // 请你返回使用 grid 中的 3 个元素可以构建的 直角三角形 数目，且满足 3 个元素值 都 为 1 。
    // 注意：
    // 如果 grid 中 3 个元素满足：一个元素与另一个元素在 同一行，同时与第三个元素在 同一列 ，那么这 3 个元素称为一个 直角三角形 。这 3 个元素互相之间不需要相邻。
    // 示例 1:
    // 0 1 0
    // 0 1 1
    // 0 1 0
    // 输入：grid = [[0,1,0],[0,1,1],[0,1,0]]
    //
    // 输出：2
    //
    // 解释:有 2 个直角三角形
    //
    // 示例 2：
    //
    // 1 0 0 0
    // 0 1 0 1
    // 1 0 0 0
    // 输入：grid = [[1,0,0,0],[0,1,0,1],[1,0,0,0]]
    //
    // 输出：0
    //
    // 解释:没有直角三角形。
    //
    // 示例 3：
    //
    // 1 0 1
    // 1 0 0
    // 1 0 0
    // 输入：grid = [[1,0,1],[1,0,0],[1,0,0]]
    //
    // 输出：2
    //
    // 解释：
    //
    // 有两个直角三角形。
    //
    //
    //
    // 提示：
    //
    // 1 <= grid.length <= 1000
    // 1 <= grid[i].length <= 1000
    // 0 <= grid[i][j] <= 1
}