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