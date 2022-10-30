using Core.exception;
using Core.Model;
using Core.Service;

namespace UnitTests;

public class Tests
{
    private Scanner _scanner;
    
    [SetUp]
    public void Setup()
    {
        _scanner = new Scanner();
    }

    [Test]
    public void InvalidInputParametersTest()
    {
        Assert.Throws<IllegalThreadCountException>(
            () => _scanner.StartScan(".", 0)
        );
        Assert.Throws<InvalidPathException>(
            () => _scanner.StartScan("invalidPath", 10)
        );
    }

    [Test]
    public void FileScanTest()
    {
        Node node = _scanner.StartScan("X:\\Programming\\University\\test\\rez.txt", 10);
        Assert.Multiple(() =>
        {
            Assert.That(node.Name, Is.EqualTo("rez.txt"));
            Assert.That(node.Size, Is.EqualTo(0));
            Assert.That(node.ChildrenNodes.Count, Is.EqualTo(0));
        });
    }

    [Test]
    public void DirectoryScanTest()
    {
        Node node = _scanner.StartScan("X:\\Programming\\University\\test", 10);
        Assert.Multiple(() =>
        {
            Assert.That(node.Name, Is.EqualTo("test"));
            Assert.That(node.Size, Is.EqualTo(96));
            Assert.That(node.ChildrenNodes.Count, Is.EqualTo(4));
        });
        Assert.Multiple(() =>
        {
            Assert.That(node.ChildrenNodes[0].Name, Is.EqualTo("123"));
            Assert.That(node.ChildrenNodes[0].Size, Is.EqualTo(11));
            Assert.That(node.ChildrenNodes[0].ChildrenNodes.Count, Is.EqualTo(2));
        });
        Assert.Multiple(() =>
        {
            Assert.That(node.ChildrenNodes[1].Name, Is.EqualTo("456"));
            Assert.That(node.ChildrenNodes[1].Size, Is.EqualTo(85));
            Assert.That(node.ChildrenNodes[1].ChildrenNodes.Count, Is.EqualTo(2));
        });
        Assert.Multiple(() =>
        {
            Assert.That(node.ChildrenNodes[2].Name, Is.EqualTo("MongoDBCompass.lnk"));
            Assert.That(node.ChildrenNodes[2].Size, Is.EqualTo(0));
            Assert.That(node.ChildrenNodes[2].ChildrenNodes.Count, Is.EqualTo(0));
        });
        Assert.Multiple(() =>
        {
            Assert.That(node.ChildrenNodes[3].Name, Is.EqualTo("rez.txt"));
            Assert.That(node.ChildrenNodes[3].Size, Is.EqualTo(0));
            Assert.That(node.ChildrenNodes[3].ChildrenNodes.Count, Is.EqualTo(0));
        });
    }

    [Test]
    public void ScanLessWhenCanceled()
    {
        Node nodeWithoutCanceling = _scanner.StartScan("X:/", 10);
        Node nodeWithCanceling = null;
        Scanner _scannerWithCanceling = new Scanner();
        var task = Task.Run(() => nodeWithCanceling = _scannerWithCanceling.StartScan("X:/", 10));
        _scannerWithCanceling.CancelScan();
        Task.WaitAll(task);
        Assert.That(nodeWithoutCanceling.Size, Is.GreaterThan(nodeWithCanceling.Size));
        
    }
}