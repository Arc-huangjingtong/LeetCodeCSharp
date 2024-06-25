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


public class Solution_2732
{
    public IList<int> GoodSubsetofBinaryMatrix(int[][] grid)
    {
        var m = grid.Length;
        var n = grid[0].Length;

        Span<int> casterArray = stackalloc int[m];

        foreach (var vertical in grid)
        {
            for (var i = 0 ; i < m ; i++)
            {
                casterArray[i] |= vertical[i] << i;
            }
        }

        casterArray.Sort(); // 排序 从小到大

        //Horizontal and vertical

        return default;
    }

    // 2732. 找到矩阵中的好子集  困难
    // 
    // grid   : 一个下标从 0 开始大小为 m x n 的二进制矩阵 
    // return : 一个整数数组，它包含好子集的行下标，请你将其 [升序] 返回
    // key    : 最多只有五列，所以可以用位运算来表示列的状态
    //
    // 从原矩阵中选出若干行构成一个行的 非空 子集，如果子集中任何一列的和至多为子集大小的一半，那么我们称这个子集是 好子集
    //
    // 更正式的，如果选出来的行子集大小（即行的数量）为 k，那么每一列的和 <= floor(k / 2)
    //
    // 如果有多个好子集，你可以返回任意一个。如果没有好子集，请你返回一个空数组
    //
    // 一个矩阵 grid 的行 子集 ，是删除 grid 中某些（也可能不删除）行后，剩余行构成的元素集合
    //
    //
    //
    // 示例 1：
    // Dic: 
    // 输入：grid = [ [0,1,1,0,0],
    //               [0,0,0,1,0],
    //               [1,1,1,1,0] ]
    // 输出：[0,1]
    // 解释：我们可以选择第 0 和第 1 行构成一个好子集。
    // 选出来的子集大小为 2
    // - 第 0 列的和为 0 + 0 = 0 ，小于等于子集大小的一半
    // - 第 1 列的和为 1 + 0 = 1 ，小于等于子集大小的一半
    // - 第 2 列的和为 1 + 0 = 1 ，小于等于子集大小的一半
    // - 第 3 列的和为 0 + 1 = 1 ，小于等于子集大小的一半
    // - 第 4 列的和为 0 + 0 = 0 ，小于等于子集大小的一半
    // 示例 2:
    // 
    // 输入：grid = [[0]]
    // 输出：[0]
    // 解释：我们可以选择第 0 行构成一个好子集。
    // 选出来的子集大小为 1 。
    // - 第 0 列的和为 0 ，小于等于子集大小的一半
    //
    // 示例 3:
    //
    // 输入：grid = [[1,1,1],[1,1,1]]
    // 输出：[]
    // 解释：没有办法得到一个好子集
    //
    // 输入：grid = [ [0,1,1,0,0],
    //               [0,0,0,1,0],
    //               [1,1,1,1,0] ]
    //
    // 提示：
    //      m == grid.length
    //      n == grid[i].length
    //      1 <= m <= 10^4
    //      1 <= n <= 5
    //      grid[i][j] 要么是 0 ，要么是 1
}