using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Logic
{
    public class CircularLinkedList
    {
        private Node head;

        public CircularLinkedList(Vector3[] hitPoints)
        {
            if (hitPoints == null || hitPoints.Length == 0) return;

            head = new Node(hitPoints[0]);
            Node current = head;

            // Created the doubly linked list from the array of hitPoints
            for (int i = 1; i < hitPoints.Length; i++)
            {
                Node newNode = new Node(hitPoints[i]);
                current.Next = newNode;
                current = newNode;
            }

            // Making the list circular by linking the last node to the head
            current.Next = head;
        }

        public IEnumerator<Node> GetEnumerator()
        {
            if (head == null) yield break;

            Node current = head;
            do
            {
                yield return current;
                current = current.Next;
            }
            while (current != head);
        }

        public Node FindNodeByValue(Vector3 value)
        {
            if (head == null) return null;

            Node current = head;
            do
            {
                if (current.Value == value)
                {
                    return current;
                }
                current = current.Next;
            }
            while (current != head);

            return null; // Return null if the value was not found
        }

    }

    public class Node
    {
        public Vector3 Value;
        public Node Next;
        public Node(Vector3 value)
        {
            Value = value;
            Next = null;
        }
    }
}
