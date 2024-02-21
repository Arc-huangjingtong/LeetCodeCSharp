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
}