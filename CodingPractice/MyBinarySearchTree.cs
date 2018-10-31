using System;
using System.Collections.Generic;
using System.Text;

namespace CodingPractice
{
    public class MyBinarySearchTree<T> where T: IComparable
    {
        private StringBuilder stringRepresentation;
        private Boolean visitBit = false;   //this bit tracks whether you mark the Visited property of a BSTNode<T> to false or true
                                            //it starts as false (as does the Visited property), but as soon as ToString() is called,
                                            //this bit is flipped to True. Then, when you traverse the tree, you flip Visited to True;
                                            //The next time ToString() is called, you flip the bit again, and this time Visited will
                                            //be set to false during traversal

        public int Count { get; private set; }

        public BSTNode<T> RootNode { get; set; }

        public MyBinarySearchTree(T RootNodeData)
        {
            RootNode = new BSTNode<T>(RootNodeData, null)
            {
                LeftNode = null,
                RightNode = null
            };
            Count = 1;
        }

        public bool IsDataPresent(T data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data), $"Argument {nameof(data)} may not be null.");
            return FindMatchingNode(data) != null;
        }

        private BSTNode<T> FindMatchingNode(T data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data), $"Argument {nameof(data)} may not be null.");
            BSTNode<T> curNode = RootNode;
            while (curNode != null)
            {
                if (EqualityComparer<T>.Default.Equals(curNode.Data, data)) return curNode;
                if (data.CompareTo(curNode.Data) > 0)
                {
                    curNode = curNode.RightNode;
                    continue;
                }
                if (data.CompareTo(curNode.Data) < 0)
                {
                    curNode = curNode.LeftNode;
                    continue;
                }
            }
            return null;
        }

        public BSTNode<T> AddNode(T data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data), $"Argument {nameof(data)} may not be null.");
            var curNode = this.RootNode;
            while (curNode != null)
            {
                //there's already a node with this value. return the existing node
                if (EqualityComparer<T>.Default.Equals(curNode.Data, data)) return curNode;

                if (data.CompareTo(curNode.Data) > 0)
                {
                    if (curNode.RightNode != null)
                    {
                        curNode = curNode.RightNode;
                        continue; //keep traversing
                    }
                    else
                    {
                        //we should traverse right, because our value is greater than the current node's data, but there's 
                        // no right node to traverse to, so create the new node here
                        var newNode = new BSTNode<T>(data, curNode);
                        curNode.RightNode = newNode;
                        Count++;
                        return newNode;
                    }
                }
                

                if (data.CompareTo(curNode.Data) < 0) 
                {
                    if (curNode.LeftNode != null)
                    {
                        curNode = curNode.LeftNode;
                        continue; //keep traversing
                    }
                    else
                    {
                        //we should traverse left, because our value is less than the current node's data, but there's 
                        // no left node to traverse to, so create the new node here
                        var newNode = new BSTNode<T>(data, curNode);
                        curNode.LeftNode = newNode;
                        Count++;
                        return newNode;
                    }
                }
            }

            //TODO: we shouldn't get here
            return curNode;
        }

        public BSTNode<T> DeleteNode(T data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data), $"Argument {nameof(data)} may not be null.");
            var nodeToDelete = FindMatchingNode(data);
            if (nodeToDelete == null) return null;

            //leaf node. Just break the link between the parent and child and return
            if (nodeToDelete.RightNode == null && nodeToDelete.LeftNode == null)
            {
                //if the node we're deleting is the parent's right node, 
                //then break the link and set the right node to null
                if (nodeToDelete.ParentNode.RightNode != null && nodeToDelete.ParentNode.RightNode.Equals(nodeToDelete))
                {
                    nodeToDelete.ParentNode.RightNode = null;
                }
                else
                {
                    //if the node we're deleting is the parent's left node, 
                    //then break the link and set the left node to null
                    nodeToDelete.ParentNode.LeftNode = null;
                }
                Count--;
                return nodeToDelete;
            }

            //the greater node is null, the lesser node is not null. Re-parent the lesser node
            if (nodeToDelete.RightNode == null && nodeToDelete.LeftNode != null)
            {
                if (nodeToDelete.ParentNode.LeftNode != null && nodeToDelete.ParentNode.LeftNode == nodeToDelete)
                {
                    //if nodeToDelete is the left node to its parent (lesser), and the left node of nodeToDelete is not null
                    //then it becomes the left node of nodeToDelete's parent
                    nodeToDelete.ParentNode.LeftNode = nodeToDelete.LeftNode;
                }
                else
                {
                    //nodeToDelete is the right node of its parent and the right node of nodeToDelete is null
                    //make nodeToDelete's left node the right node of nodeToDelete's parent
                    nodeToDelete.ParentNode.RightNode = nodeToDelete.LeftNode;
                }
                Count--;
                return nodeToDelete;
            }

            //the lesser (left) node is null, the greater (right) node is not null. Re-parent the greater node
            if (nodeToDelete.RightNode != null && nodeToDelete.LeftNode == null)
            {
                if (nodeToDelete.ParentNode.LeftNode == nodeToDelete)
                {
                    //if nodeToDelete is the left node to its parent (greater), then re-home the nodeToDelete's right
                    //node as the parent's left node
                    nodeToDelete.ParentNode.LeftNode = nodeToDelete.RightNode;
                }
                else
                {
                    //nodeToDelete is the right node of its parent. Make nodeToDelete's right node
                    // the right node of it's parent
                    nodeToDelete.ParentNode.RightNode = nodeToDelete.RightNode;
                }
                Count--;
                return nodeToDelete;
            }

            //the nodeToDelete has two child nodes
            //we have to find the least node in the right-hand tree (the smallest value greater than the one we're replacing)
            if (nodeToDelete.LeftNode != null && nodeToDelete.RightNode != null)
            {
                //start with the node to the right (greater than) the nodeToDelete
                var curNode = nodeToDelete.RightNode;

                //loop through the nodes, only going left. exit when there is no node on the left
                //now you know you have the lowest node on the right hand side of the node to delete
                while (curNode.LeftNode != null)
                {
                    curNode = curNode.LeftNode;
                }

                //we should now be down to the node with the least value in the nodeToDelete's left tree
                //assign the value of this least node to the node that we are meant to delete
                //and then remove the link between this least node and its parent
                nodeToDelete.Data = curNode.Data;
                curNode.ParentNode.LeftNode = null;
                Count--;
                return nodeToDelete;
            }

            //TODO: this shouldn't happen
            return null;
        }

        public override string ToString()
        {
            stringRepresentation = new StringBuilder();
            visitBit = !visitBit;
            BuildPreorderTreeString(this.RootNode);
            return stringRepresentation.ToString();
        }

        private void BuildPreorderTreeString(BSTNode<T> curNode)
        {
            if (curNode == null) throw new ArgumentNullException(nameof(curNode), $"Argument {nameof(curNode)} may not be null.");
            if (curNode.Visited != visitBit)
            {
                stringRepresentation.Append(curNode.Data);
                curNode.Visited = visitBit;
                stringRepresentation.Append("|");
            }

            if (curNode.LeftNode != null)
            {
                BuildPreorderTreeString(curNode.LeftNode);
            }

            if (curNode.RightNode != null)
            {
                BuildPreorderTreeString(curNode.RightNode);
            }
        }
    }

    public class BSTNode<T>
    {
        public BSTNode()
        {

        }

        public BSTNode(T data, BSTNode<T> parentNode)
        {
            this.ParentNode = parentNode;
            this.Data = data;
        }

        public BSTNode(T data, BSTNode<T> parentNode, BSTNode<T> rightNode, BSTNode<T> leftNode)
        {
            this.Data = data;
            this.ParentNode = parentNode;
            this.RightNode = rightNode;
            this.LeftNode = leftNode;
        }

        public BSTNode<T> RightNode { get; set; } = null;

        public BSTNode<T> LeftNode { get; set; } = null;

        public BSTNode<T> ParentNode { get; set; } = null;

        public Boolean Visited { get; set; } = false;

        public T Data { get; set; }
    }
}
