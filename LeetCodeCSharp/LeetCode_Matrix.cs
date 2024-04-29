namespace LeetCodeCSharp;
///////////////////////////////////////////////////////矩阵相关题型///////////////////////////////////////////////////////


///<summary> 1329. 将矩阵按对角线排序 </summary>
public class Solution_1329
{
    
    public int[][] DiagonalSort(int[][] mat)
    {
        var width  = mat.Length;
        var length = mat[0].Length;


        for (int i = 0 ; i < width ; i++)
        {
            for (var j = 0 ; j < length-1 ; j++)
            {
                
            }
        }


        return mat;
    }

    [Test]
    public void Test()
    {
        int[][] mat =
        [
            [11, 25, 66,  1, 69, 7],  // 
            [23, 55, 17, 45, 15, 52] //
          , [75, 31, 36, 44, 58, 8]  //
          , [22, 27, 33, 25, 68, 4]  //
          , [84, 28, 14, 11,  5, 50]
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