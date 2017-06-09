using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App3.Class
{

    public class NTree<T> : IEnumerator, IEnumerable
    {
        public delegate void TreeVisitor(T nodeData);

        private T data;
        private LinkedList<NTree<T>> children;
        private int chPos = 0;

        public T Data
        {
            get
            {
                return data;
            }
        }

        public NTree(T data)
        {
            this.data = data;
            children = new LinkedList<NTree<T>>();
        }

        public NTree<T> AddChild(T data)
        {
            NTree<T> newNode = new NTree<T>(data);
            children.AddLast(newNode);
            return newNode;
        }

        public NTree<T> GetChild(int i)
        {
            foreach (NTree<T> n in children)
                if (--i == 0)
                    return n;
            return null;
        }

        public int IndexOf(T t)
        {
            int i = children.Count();
            foreach (NTree<T> n in children)
            {
                i--;
                if (n.data.Equals(t))
                    break;
            }
            return i;
        }

        public NTree<T> RSearch(T pdata, ref List<T> path)
        {
            if (data.Equals(pdata))
            {
                return this;
            }
            else
            {
                NTree<T> n = null;
                foreach(NTree<T> kid in children)
                {
                    n = kid.RSearch(pdata, ref path);
                    if (n != null)
                    {
                        path.Insert(0, data);
                        break;
                    }
                }
                return n;
            }
        }

        public void Traverse(NTree<T> node, TreeVisitor visitor)
        {
            visitor(node.data);
            foreach (NTree<T> kid in node.children)
                Traverse(kid, visitor);
        }

        public LinkedList<NTree<T>> Children
        {
            get { return this.children; }
        }

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }

        public object Current
        {
            get 
            {
                return children.ElementAt<NTree<T>>(chPos);
            }
        }

        public bool MoveNext()
        {
            chPos++;
            return (chPos < children.Count);
        }

        public void Reset()
        {
            chPos = 0;
        }
    }
}
