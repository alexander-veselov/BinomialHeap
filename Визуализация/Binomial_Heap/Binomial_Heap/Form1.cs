using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Binomial_Heap
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        public class Node
        {
            public Node(ref PictureBox PB, int X, int Y, int val)
            {
                pb = PB;
                x = X;
                y = Y;
                value = val;
                //b = new Bitmap(1265, 675);
                draw();
            }
            public void draw()
            {

                // Bitmap b = new Bitmap();
                Graphics g = Graphics.FromImage(pb.Image);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                // g.Clear(Color.LightGray);
                g.FillEllipse(Brushes.White, x, y, r, r);
                Pen p;
                if (isSelected)
                {
                    p = new Pen(Color.Red);
                }
                else
                {
                    p = new Pen(Color.Black);
                }
                p.Width = 3;
                g.DrawEllipse(p, x, y, r, r);
                int d = value.ToString().Length;
                if (d == 1)
                {
                    d = 12;
                }
                else
                if (d == 2)
                {
                    d = 7;
                }
                else
                if (d == 3)
                {
                    d = 2;
                }
                if (value < 0) d -= 2;
                if (value == -99)
                {
                    g.DrawString(" -∞", Utils.font, Brushes.Black, x + 4 + d, y + 14);
                }
                else
                    g.DrawString(value.ToString(), Utils.font, Brushes.Black, x + 4 + d, y + 14);
                // pb.Image = b;
                p.Dispose();
                //if (b != null) b.Dispose();
            }
            public int value = -1;
            public bool isSelected = false;
            PictureBox pb;
            public Node parent;
            public int x, y, r = 50;
        };

        public class Edge
        {
            public Edge(ref PictureBox PB, int X1, int X2, int Y1, int Y2)
            {
                pb = PB;
                x1 = X1;
                x2 = X2;
                y1 = Y1;
                y2 = Y2;
                draw();
            }
            public void draw()
            {
                //b = new Bitmap();
                Graphics g = Graphics.FromImage(pb.Image);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Pen p = new Pen(Color.Black);
                p.Width = 3;
                g.DrawLine(p, x1, x2, y1, y2);
                p.Dispose();
                //pb.Image = b;

            }
            Bitmap b;
            PictureBox pb;

            public int x1, x2, y1, y2;
        };



        public class Tree
        {
            public Tree()
            {

            }
            public Tree(int posx, int posy, ref PictureBox PB, int value)
            {
                n = 0;
                pb = PB;
                posX = posx;
                posY = posy;
                path = new Edge[0];
                tree = new Node[n + 1][];
                tree[0] = new Node[1];
                tree[0][0] = new Node(ref pb, posX, posY, value);
                tree[0][0].value = value;
                tree[0][0].draw();

            }
            public Tree(int N, int posx, int posy, ref PictureBox PB)
            {
                n = N;
                pb = PB;
                posX = posx;
                posY = posy;
                tree = new Node[n + 1][];

                int z = 0;
                for (int i = 0; i < n + 1; i++)
                {
                    int m = Utils.bci(n, i);
                    tree[i] = new Node[m];
                    int k = 0;
                    for (int j = 0; j < m; j++)
                    {
                        z++;
                        int x = Utils.find(k, i);
                        k = x + 1;
                        tree[i][j] = new Node(ref pb, posX - Utils.a[x].Key * 70, posY + i * 70, -1);
                    }
                }
                path = new Edge[z];
                z = 0;
                for (int i = 1; i < n + 1; i++)
                {
                    int m = Utils.bci(n, i);
                    for (int j = 0; j < m; j++)
                    {
                        int q = findEdge(j, i);
                        tree[i][j].parent = tree[i - 1][q];
                        int I = tree[i - 1][q].x;
                        int J = tree[i - 1][q].y;
                        path[z++] = new Edge(ref pb, tree[i][j].x + 25, tree[i][j].y + 25, I + 25, J + 25);
                    }
                }
                draw();
            }
            public void changePos(int X)
            {
                X = (tree[0][0].x - X);
                for (int i = 0; i < n + 1; i++)
                {
                    for (int j = 0; j < tree[i].Length; j++)
                    {
                        tree[i][j].x -= X;
                    }
                }
                if (path.Length != 0)
                    for (int i = 0; i < path.Length - 1; i++)
                    {
                        path[i].x1 -= X;
                        path[i].y1 -= X;
                    }

            }
            public int findEdge(int x, int y)
            {
                int k = 0;
                int X = tree[y][x].x;
                for (int i = 0; i < tree[y - 1].Length; i++)
                {
                    if (tree[y][x].x <= tree[y - 1][i].x)
                    {
                        if (Math.Abs(tree[y - 1][k].x - X) > Math.Abs(tree[y - 1][i].x - X))
                        {
                            k = i;
                        }
                    }
                }
                return k;
            }
            public Tree merge(Tree b)
            {

                int N = n + 1;
                Tree newTree = new Tree(N, posX, posY, ref pb);
                if (b.tree[0][0].value > tree[0][0].value)
                {
                    for (int i = 0; i < N; i++)
                    {

                        for (int j = 0; j < tree[i].Length; j++)
                        {
                            newTree.tree[i][j].value = tree[i][j].value;
                        }
                    }
                    for (int i = 1; i < N + 1; i++)
                    {
                        for (int j = 0; j < tree[i - 1].Length; j++)
                        {
                            int k = newTree.tree[i].Length - tree[i - 1].Length;

                            newTree.tree[i][j + k].value = b.tree[i - 1][j].value;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < N; i++)
                    {

                        for (int j = 0; j < tree[i].Length; j++)
                        {
                            newTree.tree[i][j].value = b.tree[i][j].value;
                        }
                    }
                    for (int i = 1; i < N + 1; i++)
                    {
                        for (int j = 0; j < tree[i - 1].Length; j++)
                        {
                            int k = newTree.tree[i].Length - tree[i - 1].Length;

                            newTree.tree[i][j + k].value = tree[i - 1][j].value;
                        }
                    }
                }
                newTree.draw();
                return newTree;
            }
            public void draw()
            {

                if (path.Length != 0)
                    for (int i = 0; i < path.Length - 1; i++)
                    {
                        path[i].draw();
                    }
                for (int i = 0; i < n + 1; i++)
                {
                    int m = Utils.bci(n, i);
                    for (int j = 0; j < m; j++)
                    {
                        tree[i][j].draw();
                    }
                }
            }
            public Node getFirst()
            {
                return tree[0][0];
            }

            ~Tree()
            {
                tree = null;
                path = null;
            }
            public int n;
            int posX, posY;
            PictureBox pb;
            public Node[][] tree;
            public Edge[] path;
        }

        public class Heap
        {
            public Heap(ref PictureBox PB, ref Label L)
            {
                l = L;
                pb = PB;
                heap = new Tree[1024];
                path = new Edge[1024];
                n = 0;
            }

            public Heap copy()
            {
                normalize();
                Heap heap1 = new Heap(ref pb, ref l);
                heap1.n = n;
                heap1.mess = mess;
                for (int i = 0; i < n; i++)
                {
                    heap1.heap[i] = new Tree(heap[i].n, heap[i].getFirst().x, heap[i].getFirst().y, ref pb);
                    for (int j = 0; j < heap[i].tree.Length; j++)
                    {
                        for (int k = 0; k < heap[i].tree[j].Length; k++)
                        {
                            // = new Node(ref pb, heap[i].tree[j][k].x, heap[i].tree[j][k].y, heap[i].tree[j][k].value);
                            heap1.heap[i].tree[j][k].y = heap[i].tree[j][k].y;
                            heap1.heap[i].tree[j][k].x = heap[i].tree[j][k].x;
                            heap1.heap[i].tree[j][k].value = heap[i].tree[j][k].value;

                        }
                    }
                }
                if (selected != null && heap1.n != 0)
                {
                    Node zz = heap1.findNode(selected.x, selected.y);
                    heap1.selected = new Node(ref pb, zz.x, zz.y, zz.value);
                }
                for (int i = 0; i < path.Length; i++)
                {
                    if (path[i] != null)
                        heap1.path[i] = new Edge(ref pb, path[i].x1, path[i].x2, path[i].y1, path[i].y2);
                }
                return heap1;
            }

            public void insert(Tree t)
            {
                normalize();
                n++;
                heap[findFree()] = t;

                //normalize();
                //draw();
            }

            public bool merge()
            {
                bool isChanged = false;
                for (int i = 0; i < n + 20; i++)
                {
                    for (int j = 0; j < n + 20; j++)
                    {

                        if (i != j)
                            if (heap[i] != null && heap[j] != null)
                            {
                                if (heap[i].n == heap[j].n)
                                {
                                    heap[i] = heap[i].merge(heap[j]);
                                    n--;
                                    //heap[i] = null;
                                    isChanged = true;
                                    heap[j] = null;
                                }
                            }
                    }
                }
                return isChanged;
            }

            public void draw()
            {
                //pb.Image.Dispose();
                l.Text = mess;
                pb.Image = null;
                pb.Image = new Bitmap(1200, 570);
                setLines();
                for (int i = 0; i < 1024; i++)
                {
                    if (heap[i] != null) heap[i].draw();
                }
                if (selected != null) selected.isSelected = false;
            }

            public void setLines()
            {
                for (int i = 0; i < n + 20; i++)
                {
                    path[i] = null;
                }
                int k = findFree();
                normalize();
                for (int i = 0; i < n - 1; i++)
                {
                    path[i] = new Edge(ref pb, heap[i].getFirst().x, 75, heap[i + 1].getFirst().x, 75);
                }
            }

            public int extract_min()
            {
                if (selected != null) selected.isSelected = false;
                int min = 0;
                for (int i = 0; i < n + 20; i++)
                {
                    if (heap[i] != null)
                    {
                        if (heap[i].getFirst().value < heap[min].getFirst().value) min = i;
                    }
                }
                int minElement = heap[min].getFirst().value;
                int k = findFree();
                int m = heap[min].n;
                int[] c = new int[m];
                for (int i = 0; i < m; i++) c[i] = 0;
                for (int i = k; i < k + m; i++)
                {
                    int d = m - (i - k) - 1;
                    heap[i] = new Tree(d, 1100 - freePos(), 50, ref pb);
                    for (int j = 0; j < d + 1; j++)
                    {
                        for (int z = 0; z < heap[i].tree[j].Length; z++)
                        {
                            heap[i].tree[j][z].value = heap[min].tree[j + 1][heap[min].tree[j + 1].Length - c[j] - z - 1].value;
                        }
                        for (int z = 0; z < heap[i].tree[j].Length / 2; z++)
                        {
                            int temp = heap[i].tree[j][z].value;
                            heap[i].tree[j][z].value = heap[i].tree[j][heap[i].tree[j].Length - 1 - z].value;
                            heap[i].tree[j][heap[i].tree[j].Length - 1 - z].value = temp;
                        }
                        c[j] += heap[i].tree[j].Length;
                    }
                    normalize();
                }
                n += m - 1;
                heap[min] = null;
                normalize();
                draw();
                normalize();
                return minElement;
            }

            public void normalize()
            {
                for (int i = 0; i < n + 20; i++)
                {
                    if (heap[i] == null && heap[i + 1] != null)
                    {
                        heap[i] = heap[i + 1];
                        heap[i + 1] = null;
                    }
                }
                for (int i = 0; i < n + 20; i++)
                {
                    for (int j = i; j < n + 20; j++)
                    {
                        if (heap[i] != null && heap[j] != null)
                        {
                            if (heap[i].n < heap[j].n)
                            {
                                Tree temp = heap[i];
                                heap[i] = heap[j];
                                heap[j] = temp;
                            }
                        }
                    }
                }
                for (int i = 0; i < n + 20; i++)
                {
                    if (heap[i] == null && heap[i + 1] != null)
                    {
                        heap[i] = heap[i + 1];
                        heap[i + 1] = null;
                    }
                }
                int k = 0;
                for (int i = 0; i < n + 20; i++)
                {
                    if (heap[i] != null)
                    {
                        int q = heap[i].n;
                        heap[i].changePos(1100 - k * 70);
                        k += (int)Math.Pow(2, q - 1);
                    }
                }

            }

            public int freePos()
            {
                normalize();
                int j = findFree();
                int k = 0;
                for (int i = 0; i < j; i++)
                {
                    int q = heap[i].n;
                    k += (int)Math.Pow(2, q - 1);
                }
                return k * 70;
            }
            int findFree()
            {
                for (int i = 0; i < 1024; i++)
                {
                    if (heap[i] == null) return i;
                }
                return 0;
            }
            public int getMin()
            {
                int min = 10000;
                for (int i = 0; i < n; i++)
                {
                    if (heap[i] != null)
                    {
                        if (heap[i].getFirst().value < min) min = heap[i].getFirst().value;
                    }
                }
                return min;
            }
            public Node findNode(int X, int Y)
            {
                if (selected != null) selected.isSelected = false;
                double minLen = 1000000;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < heap[i].tree.Length; j++)
                    {
                        for (int k = 0; k < heap[i].tree[j].Length; k++)
                        {
                            int x1 = heap[i].tree[j][k].x;
                            int y1 = heap[i].tree[j][k].y;
                            double len = Math.Sqrt(Math.Pow(X - x1, 2) + Math.Pow(Y - y1, 2));
                            if (len < minLen)
                            {
                                selected = heap[i].tree[j][k];
                                minLen = len;
                            }
                        }
                    }

                }
                if (selected != null)
                    selected.isSelected = true;
                draw();
                return selected;
            }
            public void setMessage(String s)
            {
                mess = s;
            }
            public void decrease_key(int val)
            {
                selected.isSelected = false;

                if (selected == null || selected.value < val)
                {
                    MessageBox.Show("Trying to increase");
                    draw();
                    return;
                }
                selected.value = val;


                draw();
            }

            public bool decup()
            {
                Node parent = selected.parent;
                while (selected.parent != null && selected.value < parent.value)
                {
                    int temp = selected.value;
                    selected.value = selected.parent.value;
                    selected.parent.value = temp;
                    selected = parent;
                    parent = selected.parent;
                    return true;
                }
                return false;
            }
            public void delete()
            {
                decrease_key(-99);
                extract_min();
            }
            public int getMaxRank()
            {
                int max = 0;
                for (int i = 0; i < n + 20; i++)
                {
                    if (heap[i] != null)
                    {
                        if (heap[i].n > max) max = heap[i].n;
                    }
                }
                return max;
            }
            public void kek()
            {

            }
            public Node selected = null;
            public int n;
            PictureBox pb;
            Label l;
            String mess = "";
            public Tree[] heap;
            Edge[] path;
        };



        public static class Utils
        {
            public static void init()
            {
                a = new KeyValuePair<int, int>[1024];
                a[0] = new KeyValuePair<int, int>(0, 0);
                a[1] = new KeyValuePair<int, int>(0, 1);
                r = new Random();
                bin = new int[1024][];
                fac = new int[10];
                for (int i = 0; i < 1024; i++)
                {
                    bin[i] = new int[1024];
                    for (int j = 0; j < 1024; j++)
                    {
                        bin[i][j] = -1;
                    }
                }
                for (int i = 0; i < 9; i++)
                {
                    fac[i] = fact(i);
                    merge();
                }
            }

            static int fact(int n)
            {
                int r;
                for (r = 1; n > 0; --n)
                    r *= n;
                return r;
            }

            public static int factorial(int n)
            {
                return fac[n];
            }

            public static int bci(int n, int k)
            {
                if (bin[n][k] == -1)
                {
                    bin[n][k] = factorial(n) / (factorial(k) * factorial(n - k));
                }
                return bin[n][k];
            }

            public static int rand()
            {
                return r.Next();
            }

            public static void merge()
            {
                for (int i = 0; i < c; i++)
                {
                    int f = a[i].Key + c / 2;
                    int s = a[i].Value + 1;
                    a[i + c] = new KeyValuePair<int, int>(f, s);
                }
                c *= 2;
            }

            public static int find(int j, int v)
            {
                for (int i = j; i < 1024; i++)
                {
                    if (a[i].Value == v) return i;
                }
                return 0;
            }

            public static int[] fac;
            public static int[][] bin;
            public static KeyValuePair<int, int>[] a;
            public static int c = 2;
            public static Random r;
            public static int delay = 0;
            public static Font font = new Font("TimesNewRoman", 12);
        };


        class version
        {
            public version(ref PictureBox p, ref Label l)
            {
                versions = new Heap[1000];
                versions[vers++] = new Heap(ref p, ref l);

            }
            public static Heap add()
            {
                versions[vers] = versions[vers - 1].copy();
                return versions[vers++];
            }
            public Heap left()
            {
                Heap h = versions[--vers - 1];
                if (h.selected != null) h.selected.isSelected = false;
                h.draw();
                return h;
            }
            public Heap right()
            {
                Heap h = versions[++vers - 1];
                if (h.selected != null) h.selected.isSelected = false;
                h.draw();
                return h;
            }
            public void delLast()
            {
                versions[--vers] = null;
            }
            public Heap getCurrent()
            {
                return versions[vers - 1];
            }
            public bool isLast()
            {
                return versions[vers] == null;
            }
            public bool isEmpty()
            {
                return getCurrent().n == 0;
            }
            public bool isFirst()
            {
                return vers == 1;
            }
            public Heap swap()
            {
                Heap temp = versions[vers - 1];
                versions[vers - 1] = versions[vers - 2];
                versions[vers - 2] = temp;

                return versions[vers - 1];
            }
            public bool WTF()
            {
                return vers == 1024;
            }
            public Heap setLast()
            {
                int i;
                for (i = 0; i < 1000; i++)
                {
                    if (versions[i] == null) break;
                }
                vers = i - 1;
                return versions[vers];
            }
            public static int vers = 0;
            public static Heap[] versions;
        };
        Heap h;
        version v;

        bool check()
        {
            int n = textBox1.Text.Length;
            String s = textBox1.Text;
            if (s.Length >= 10)
            {
                error();
                return false;
            }
            for (int i=0; i<n; i++)
            {
                if (s[i]=='-' && i!=0)
                {
                    error();
                    return false;
                }
                if (s[i] != '-') 
                if (!(s[i] >= '0' && s[i] <= '9'))
                {
                    error();
                    return false;
                }
            }
            int b = int.Parse(s);
            if (b <= -99 || b > 999)
            {
                error();
                return false;
            }
            return true;
        }

        void error()
        {
            MessageBox.Show("Incorrect data");
            textBox1.Text = (Utils.rand() % 1000).ToString();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            Utils.init();
            pictureBox1.Image = new Bitmap(1200, 570);
            v = new version(ref pictureBox1, ref label3);
            h = v.getCurrent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (v.WTF())
            {
                MessageBox.Show("Слишком много версий, братан");
                return;
            }
            h = version.add();
            h.setMessage("Insert");
            label6.Text = "Операция вставки: в конец кучи добавляется новый элемент, после чего деревья одинакого ранга сливаются.";
            h.draw();
            label2.Text = "";
            button3.Enabled = true;
            button2.Enabled = true;
            if (!check()) return;
            h.insert(new Tree(1100 - h.freePos(), 50, ref pictureBox1, int.Parse(textBox1.Text)));
            bool q = false;
            while (h.merge())
            {

                h = version.add();
                h.draw();
                this.Enabled = false;
                System.Threading.Thread.Sleep(Utils.delay);
                this.Enabled = true;
                q = true;
            };
            if (q) v.delLast();
            textBox1.Text = (Utils.rand() % 1000).ToString();
            if (h.getMaxRank() == 5) button1.Enabled = false;
            h.draw();

            button6.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //label2.Text = "Minimum: " + h.getMin();
            h.setMessage("Get minimum");
            label6.Text = "Операция поиска минимума: поиск наименьшего числа среди корней деревьев кучи.";
            int min = 10000;
            for (int i = 0; i < h.n; i++)
            {
                h.heap[i].getFirst().isSelected = true;
                h.draw();
                
                if (h.heap[i] != null)
                {
                    if (h.heap[i].getFirst().value < min) min = h.heap[i].getFirst().value;

                }
                string q = min.ToString();
                if (min == 10000) q = "∞";
                label2.Text = "Minimum: " + q;
                this.Enabled = false;
                System.Threading.Thread.Sleep(Utils.delay);
                this.Enabled = true;
                h.heap[i].getFirst().isSelected = false;
            }
            h.draw();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);

            h = version.add();
            h.setMessage("Extract minimum");
            label6.Text = "Операция извлечения: поиск наименьшего числа среди корней деревьев кучи. После чего найденная вершина удаляется, а его 'дети' сливаются с оставшимися деревьями.";
            button1.Enabled = true;
            button4.Enabled = false;

            string q = h.extract_min().ToString();
            if (q == "-99") q = " -∞";
            label2.Text = "Minimum: " + q;
            while (h.merge())
            {
                //h = version.add();
                h.normalize();

                h.draw();


                this.Enabled = false;
                System.Threading.Thread.Sleep(Utils.delay);
                this.Enabled = true;
            }
            h.draw();
            if (h.n == 0)
            {
                button3.Enabled = false;
                button2.Enabled = false;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            int x = h.selected.x;
            int y = h.selected.y;
            label6.Text = "Операция понижения значения вершины: установить вершине новое значение, меняеть его с родителем до тех пор, пока значение родителя не будет больше";
            h = version.add();
            h.setMessage("Decrease key");
            h.selected = h.findNode(x, y);
            button4.Enabled = false;
            h.decrease_key(int.Parse(textBox1.Text));

            while (h.decup())
            {


                h.draw();


                this.Enabled = false;
                System.Threading.Thread.Sleep(Utils.delay);
                this.Enabled = true;
            }
            h.draw();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (v.isEmpty()) return;
            if (v.isLast())
            {
                button4.Enabled = true;
                button5.Enabled = true;
                int mouseX = Cursor.Position.X - this.Location.X - 50;
                int mouseY = Cursor.Position.Y - this.Location.Y - 65;
                h.findNode(mouseX, mouseY);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //h = version.add();
            //h.delete();
           
            textBox1.Text = "-99";
            button4_Click(sender, e);
            button3_Click(sender, e);
            label6.Text = "Операция удаления: понизить значение выбранной вершины до минимально возможной, выполнить операцию извлечения минимума.";
            h.setMessage("Delete");
            h.draw();
            label2.Text = "";
            button5.Enabled = false;
            button4.Enabled = false;
            button1.Enabled = true;
            if (h.n == 0)
            {
                button3.Enabled = false;
                button2.Enabled = false;
            }
            textBox1.Text = (Utils.rand() % 100).ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {

            h.kek();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            label6.Text = "Hints: просматривайте предыдущие шаги.";
            h = v.left();
            if (v.isFirst())
            {
                button6.Enabled = false;
            }
            button7.Enabled = true;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            label6.Text = "Hints: просматривайте предыдущие шаги.";
            h = v.right();
            if (v.isLast())
            {
                button1.Enabled = true;
                button7.Enabled = false;
                if (h.n != 0)
                {
                    button2.Enabled = true;
                    button3.Enabled = true;
                }

            }

            button6.Enabled = true;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Utils.delay = trackBar1.Value * 100;
            label6.Text = "Hints: вы можете менять скорость анимации.";
            //Graphics g = Graphics.FromImage(pictureBox1.Image);
            //g.DrawImage(pictureBox1.Image, new Point(140, 200));
        }
        Heap t1;
        private void button8_Click(object sender, EventArgs e)
        {
            t1 = new Heap(ref pictureBox1, ref label3);
            version.versions[version.vers++] = t1;
            Utils.delay = 0;
            textBox1.Text = (Utils.rand() % 1000).ToString();
            for(int i=0; i<31; i++)
            button1_Click(sender, e);
            Utils.delay = 800;
            button1_Click(sender, e);
            Utils.delay = trackBar1.Value * 100;
            button5.Enabled = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            t1 = new Heap(ref pictureBox1, ref label3);
            version.versions[version.vers++] = t1;
            Utils.delay = 0;

            textBox1.Text = (0).ToString();
            for (int i = 0; i < 17; i++)
            {
                
                button1_Click(sender, e);
                textBox1.Text = (i+1).ToString();
            }
            button4.Enabled = true;
            button5.Enabled = true;
            int mouseX = 1200;
            int mouseY = -100;
            Node z = h.findNode(mouseX, mouseY);
            z.value = -1;
            z.isSelected = false;
            h.draw();
            
            Utils.delay = 800;
            button3_Click(sender, e);
            Utils.delay = trackBar1.Value * 100;
            button5.Enabled = false;
        }
    }
}
