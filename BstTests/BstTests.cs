using System;
using System.Diagnostics;
using CodingPractice;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BstTests
{
    [TestClass]
    public class BstTests
    {
        [TestMethod]
        public void TestIsDataPresent()
        {
            var bst = new MyBinarySearchTree<int>(50);
            Assert.IsTrue(bst.IsDataPresent(50), "Could not find root node.");

            bst.AddNode(40);
            Assert.IsTrue(bst.IsDataPresent(40), "Could not find added node 40");

            Assert.IsFalse(bst.IsDataPresent(1), "Found a node that isn't there.");

            var bst2 = new MyBinarySearchTree<String>("hello");
            bst2.AddNode("world");
            Assert.ThrowsException<ArgumentNullException>(() => bst2.IsDataPresent(null));

        }

        [TestMethod]
        public void TestAdd()
        {
            var bst = new MyBinarySearchTree<int>(50);
            Assert.IsNotNull(bst.RootNode);
            Assert.AreEqual(50, (int)bst.RootNode.Data, "Root node is not the expected value 50. Instead, its value is {0}", bst.RootNode.Data);

            //try to add a value already there; should get back the same node
            var expectedRootNode = bst.AddNode(50);
            Assert.ReferenceEquals(expectedRootNode, bst.RootNode);

            var fortyNode = bst.AddNode(40);

            var rootNode = bst.RootNode;
            Assert.IsNotNull(rootNode.LeftNode, "Root's left node is null when it should not be. Node 40 added incorrectly");
            Assert.IsNull(rootNode.RightNode, "Root's right node is not null when it should be. Node 40 added incorrectly");
            Assert.IsTrue(bst.IsDataPresent(40), "Could not find added node 40");
            Assert.AreEqual(2, bst.Count, $"Expected count 2. Actual count is {bst.Count}");

            var sixtyNode = bst.AddNode(60);
            Assert.IsNotNull(rootNode.RightNode, "Root's right node is null when it should not be. Node 60 added incorrectly");
            Assert.IsTrue(bst.IsDataPresent(60), "Could not find added node 60");
            Assert.AreEqual(3, bst.Count, $"Expected count 3. Actual count is {bst.Count}");

            //add another "greater than" node, to the right of 60
            var seventyNode = bst.AddNode(70);
            Assert.IsNotNull(rootNode.RightNode.RightNode, "Right node of root's right node is null when it should not be. Node 70 added incorrectly");

            //add a node on the left of the root's right node
            var fiftyFiveNode = bst.AddNode(55);
            Assert.IsNotNull(rootNode.RightNode.LeftNode, "Left node of root's right is null when it should not be. Node 55 added incorrectly");

            //add a node to the right of the root's left node
            var fortyFiveNode = bst.AddNode(45);
            Assert.IsNotNull(rootNode.LeftNode.RightNode, "Right node of root's left is null when it should not be. Node 45 added incorrectly");

            //add a node to the left of the root's left node
            var thirtyFiveNode = bst.AddNode(35);
            Assert.IsNotNull(rootNode.LeftNode.LeftNode, "Left node of root's left is null when it should not be. Node 35 added incorrectly");

            //try to add a null node
            var bst2 = new MyBinarySearchTree<String>("hello");
            Assert.ThrowsException<ArgumentNullException>(() => bst2.AddNode(null));

        }

        [TestMethod]
        public void TestDeleteNode()
        {
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Debug.AutoFlush = true;

            var bst = new MyBinarySearchTree<int>(50);
            bst.AddNode(60);
            bst.AddNode(40);
            bst.AddNode(70);
            bst.AddNode(80);
            bst.AddNode(75);
            bst.AddNode(90);
            bst.AddNode(73);
            bst.AddNode(74);
            bst.AddNode(85);
            bst.AddNode(82);
            bst.AddNode(87);
            bst.AddNode(100);
            bst.AddNode(110);
            bst.AddNode(55);
            bst.AddNode(53);
            bst.AddNode(45);
            bst.AddNode(30);
            bst.AddNode(35);
            bst.AddNode(25);
            bst.AddNode(31);
            /* we should now have a binary tree that looks like this:
             *                          50
             *                     40        60
             *                  30    45   55   70
             *               25    35    53        80
             *                  31              75     90
             *                               73      85   100
             *                                 74  82  87    110
             *                  
             * We need to try to delete:
             * 1) A leaf node that is its parent's right node (45)
             * 1a) A leaf node that is its parent's left node (82)
             * 2) A node with one right node. Deleted node is the parent's right node (100)
             * 2a) A node with one right node. Deleted node is the parent's left node (75)
             * 3) A node with one left node. Deleted node is the parent's right node (add 86, then delete 87)
             * 3a) A node with one left node. Deleted node is the parent's left node (75)
             * 4) A node with both right and left nodes. Deleted node is the parent's right node (90)
             * 4a) A node with both right and left nodes. Deleted node is the parent's left node (30)
             * 5) A value that's not in the tree
             * 6) null
             * 
             */
            Debug.WriteLine(bst.ToString());

            //1) Leaf node that is the right node of its parent
            int dataToDelete = 45;
            Assert.IsTrue(bst.IsDataPresent(dataToDelete), $"Can't test delete of {dataToDelete}, because it wasn't added properly.");
            var deletedNode = bst.DeleteNode(dataToDelete);
            Assert.IsNotNull(deletedNode, $"DeleteNode({dataToDelete}) returned a null node.");
            Assert.AreEqual(deletedNode.Data, dataToDelete);
            Assert.IsFalse(bst.IsDataPresent(dataToDelete), $"We deleted the node with data of {dataToDelete}, but it's still in the BST.");

            Debug.WriteLine(bst.ToString());

            //1a) Leaf node on the left side
            dataToDelete = 82;
            Assert.IsTrue(bst.IsDataPresent(dataToDelete), $"Can't test delete of {dataToDelete}, because it wasn't added properly.");
            deletedNode = bst.DeleteNode(dataToDelete);
            Assert.IsNotNull(deletedNode, $"DeleteNode({dataToDelete}) returned a null node.");
            Assert.AreEqual(deletedNode.Data, dataToDelete);
            Assert.IsFalse(bst.IsDataPresent(dataToDelete), $"We deleted the node with data of {dataToDelete}, but it's still in the BST.");

            Debug.WriteLine(bst.ToString());

            //2) node with one right node, and the deleted node is the parent's right node
            dataToDelete = 100;
            Assert.IsTrue(bst.IsDataPresent(dataToDelete), $"Can't test delete of {dataToDelete}, because it wasn't added properly.");
            deletedNode = bst.DeleteNode(dataToDelete);
            Assert.IsNotNull(deletedNode, $"DeleteNode({dataToDelete}) returned a null node.");
            Assert.AreEqual(deletedNode.Data, dataToDelete);
            Assert.IsFalse(bst.IsDataPresent(dataToDelete), $"We deleted the node with data of {dataToDelete}, but it's still in the BST.");
            var parentOfDeletedNode = bst.RootNode.RightNode.RightNode.RightNode.RightNode;
            //check to see if the child of the deleted node is now in the deleted node's place
            Assert.AreEqual(110, parentOfDeletedNode.RightNode.Data);

            Debug.WriteLine(bst.ToString());

            //2a) node with one right node, and the deleted node is the parent's left node
            dataToDelete = 73;
            Assert.IsTrue(bst.IsDataPresent(dataToDelete), $"Can't test delete of {dataToDelete}, because it wasn't added properly.");
            deletedNode = bst.DeleteNode(dataToDelete);
            Assert.IsNotNull(deletedNode, $"DeleteNode({dataToDelete}) returned a null node.");
            Assert.AreEqual(deletedNode.Data, dataToDelete);
            Assert.IsFalse(bst.IsDataPresent(dataToDelete), $"We deleted the node with data of {dataToDelete}, but it's still in the BST.");
            parentOfDeletedNode = bst.RootNode.RightNode.RightNode.RightNode.LeftNode;
            //check to see if the child of the deleted node is now in the deleted node's place
            Assert.AreEqual(74, parentOfDeletedNode.LeftNode.Data);

            Debug.WriteLine(bst.ToString());

            //3) node with one left node, and the deleted node is the parent's right node
            dataToDelete = 87;
            bst.AddNode(86);
            Assert.IsTrue(bst.IsDataPresent(dataToDelete), $"Can't test delete of {dataToDelete}, because it wasn't added properly.");
            deletedNode = bst.DeleteNode(dataToDelete);
            Assert.IsNotNull(deletedNode, $"DeleteNode({dataToDelete}) returned a null node.");
            Assert.AreEqual(deletedNode.Data, dataToDelete);
            Assert.IsFalse(bst.IsDataPresent(dataToDelete), $"We deleted the node with data of {dataToDelete}, but it's still in the BST.");
            parentOfDeletedNode = bst.RootNode.RightNode.RightNode.RightNode.RightNode.LeftNode;
            //check to see if the child of the deleted node is now in the deleted node's place
            Assert.AreEqual(parentOfDeletedNode.RightNode.Data, 86);

            Debug.WriteLine(bst.ToString());

            //3a) node with one left node, and the deleted node is the parent's left node
            dataToDelete = 75;
            Assert.IsTrue(bst.IsDataPresent(dataToDelete), $"Can't test delete of {dataToDelete}, because it wasn't added properly.");
            deletedNode = bst.DeleteNode(dataToDelete);
            Assert.IsNotNull(deletedNode, $"DeleteNode({dataToDelete}) returned a null node.");
            Assert.AreEqual(deletedNode.Data, dataToDelete);
            Assert.IsFalse(bst.IsDataPresent(dataToDelete), $"We deleted the node with data of {dataToDelete}, but it's still in the BST.");
            parentOfDeletedNode = bst.RootNode.RightNode.RightNode.RightNode;
            //check to see if the child of the deleted node is now in the deleted node's place
            Assert.AreEqual(74, parentOfDeletedNode.LeftNode.Data);

            Debug.WriteLine(bst.ToString());

            //4) node with both right and left child nodes. Deleted node is parent's right node
            dataToDelete = 90;
            var newValueOfDeletedNode = 110;
            Assert.IsTrue(bst.IsDataPresent(dataToDelete), $"Can't test delete of {dataToDelete}, because it wasn't added properly.");
            deletedNode = bst.DeleteNode(dataToDelete);
            Assert.IsNotNull(deletedNode, $"DeleteNode({dataToDelete}) returned a null node.");
            Assert.AreEqual(newValueOfDeletedNode, deletedNode.Data);
            Assert.IsFalse(bst.IsDataPresent(dataToDelete), $"We deleted the node with data of {dataToDelete}, but it's still in the BST.");
            parentOfDeletedNode = bst.RootNode.RightNode.RightNode.RightNode;
            //check to see if the child of the deleted node is now in the deleted node's place
            Assert.AreEqual(newValueOfDeletedNode, parentOfDeletedNode.RightNode.Data);

            Debug.WriteLine(bst.ToString());

            //4a) node with both right and left child nodes. Deleted node is parent's left node
            dataToDelete = 30;
            newValueOfDeletedNode = 31;
            Assert.IsTrue(bst.IsDataPresent(dataToDelete), $"Can't test delete of {dataToDelete}, because it wasn't added properly.");
            deletedNode = bst.DeleteNode(dataToDelete);
            Assert.IsNotNull(deletedNode, $"DeleteNode({dataToDelete}) returned a null node.");
            Assert.AreEqual(newValueOfDeletedNode, deletedNode.Data);
            Assert.IsFalse(bst.IsDataPresent(dataToDelete), $"We deleted the node with data of {dataToDelete}, but it's still in the BST.");
            parentOfDeletedNode = bst.RootNode.LeftNode;
            //check to see if the child of the deleted node is now in the deleted node's place
            Assert.AreEqual(newValueOfDeletedNode, parentOfDeletedNode.LeftNode.Data);


            //5) a node that's not there
            dataToDelete = 1000;
            Assert.IsFalse(bst.IsDataPresent(dataToDelete), $"The node with value {dataToDelete} was unexpectedly found.");
            deletedNode = bst.DeleteNode(dataToDelete);
            Assert.IsNull(deletedNode, "We tried to delete a non-existent node, but it still returned a node");
            
            //6) try to delete a node with value null
            var bst2 = new MyBinarySearchTree<String>("five");
            bst2.AddNode("six");
            var ex = Assert.ThrowsException<ArgumentNullException>(() => bst2.DeleteNode(null));
        }

        [TestMethod]
        public void TestCount()
        {
            //test that count is one after initialization
            var bst = new MyBinarySearchTree<int>(50);
            Assert.AreEqual(1, bst.Count);

            //test that count is still one when adding a node that's already there
            bst.AddNode(50);
            Assert.AreEqual(1, bst.Count);

            //test that count is 2 after adding a second node
            bst.AddNode(40);
            Assert.AreEqual(2, bst.Count);

            //test that count is back to 1 after deleting a single node
            bst.DeleteNode(40);
            Assert.AreEqual(1, bst.Count);

            //test that count is still the same after trying to delete a non-existent node
            bst.DeleteNode(40);
            Assert.AreEqual(1, bst.Count);
        }
    }
}
