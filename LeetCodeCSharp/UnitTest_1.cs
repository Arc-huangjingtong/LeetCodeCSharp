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
}