namespace LeetCodeCSharp;

public partial class UnitTest
{
    public TreeNode BuildTree(int[] preorder, int[] inorder)
    {
        var inorderIndices = new Dictionary<int, int>();
        var length         = preorder.Length;

        for (var i = 0 ; i < length ; i++)
        {
            inorderIndices.Add(inorder[i], i);
        }

        return BuildTree(0, 0, length);


        ///递归创建树
        /// preorderStart:前序遍历序列的起始位置
        /// inorderStart:中序遍历序列的起始位置
        /// nodesCount:节点数量
        TreeNode BuildTree(int preorderStart, int inorderStart, int nodesCount)
        {
            if (nodesCount == 0)
            {
                return null!;
            }

            //前序遍历序列的第一个节点就是根节点
            //题干条件中保证元素不重复,也就是说可以轻松的定位到根节点在中序遍历序列中的位置
            //以中序遍历序列中的根节点为分界点,左边的节点都是左子树的节点,右边的节点都是右子树的节点
            //根据这个特性,我们可以递归的创建左子树和右子树
            var rootVal          = preorder[preorderStart];
            var root             = new TreeNode(rootVal);
            var inorderRootIndex = inorderIndices[rootVal];
            var leftNodesCount   = inorderRootIndex - inorderStart;
            var rightNodesCount  = nodesCount       - 1 - leftNodesCount;

            root.left = BuildTree(preorderStart + 1, inorderStart, leftNodesCount);

            root.right = BuildTree(preorderStart + 1 + leftNodesCount, inorderRootIndex + 1, rightNodesCount);
            return root;
        }
    }


    //[1,2,3,null,4,null,5]



    [Test]
    public void TestMethod1()
    {
        var treeArray = new int?[] { 1, 2, 3, null, 4, null, 5 };
        var tree      = CreateTree(treeArray);

        var result = BuildTree(new[] { 3, 9, 20, 15, 7 }, new[] { 9, 3, 15, 20, 7 });
    }
}