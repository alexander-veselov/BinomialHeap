#pragma once
#include <iostream>
#include <vector>
using namespace std;

struct node
{
    node(int k)
    {
        key = k;
        parent = NULL;
        child = NULL;
        next = NULL;
        rank = 0;
    }
    int key;
    int rank;
    node* parent;
    node* child;
    node* next;
};

class heap
{
public:

    node* insert(int k)
    {
        node *n = new node(k);
        head = merge_(head, n);
        return n;
    }

    heap* merge(heap *H)
    {
        heap *nHeap = new heap();
        nHeap->head = merge_(this->head, H->head);
        return nHeap;
    }

    void print()
    {
        print(head, 0);
    }

    int get_min()
    {
        node *heap1 = this->head, *h2 = NULL, *h1 = heap1, *p = h1;
        int min = head->key;
        if (h1 == NULL)
        {
            cout << "Куча пустая\n";
            return 0;
        }
        while (p->next != NULL)
        {
            if ((p->next)->key < min)
            {
                min = (p->next)->key;
                h2 = p;
                h1 = p->next;
            }
            p = p->next;
        }
        return min;
    }

    int extract_min()
    {
        reverseNode = NULL;
        node* prev = NULL, *minNode = head;
        if (minNode == NULL)
        {
            cout << "Куча пустая\n";
            return 0;
        }
        int min = minNode->key;
        node* p = minNode;
        while (p->next != NULL)
        {
            if ((p->next)->key< min)
            {
                min = (p->next)->key;
                prev = p;
                minNode = p->next;
            }
            p = p->next;
        }
        if (prev == NULL && minNode->next == NULL)
        {
            head = NULL;
        }
        else
            if (prev == NULL)
            {
                head = minNode->next;
            }
            else
                if (prev->next == NULL)
                {
                    prev = NULL;
                }
                else
                {
                    prev->next = minNode->next;
                }

        if (minNode->child != NULL)
        {
            reverse(minNode->child);
            (minNode->child)->next = NULL;
        }
        head = merge_(head, reverseNode);
        return min;
    }


    int decrease_key(node *p, int k)
    {
        node *H = this->head, *i, *j;
        if (p == NULL)
        {
            //cout << "Данного элемента не существует\n";
            return -1;
        }
        if (k > p->key)
        {
            //cout << "Попытка увеличить ключ\n";
            return -1;
        }
        p->key = k;
        i = p;
        j = p->parent;
        int swapVar;
        while (j != NULL&&i->key < j->key)
        {
            swapVar = i->key;
            i->key = j->key;
            j->key = swapVar;
            i = j;
            j = j->parent;
        }
        return 0;
    }

    bool empty()
    {
        return !head;
    }

    void delete_key(node *n)
    {
        if (head == NULL)
        {
            cout << "Куча пустая\n";
            return;
        }
        if (n == NULL)
        {
            cout << "Узла не существует\n";
            return;
        }

        decrease_key(n, INT_MIN);
        extract_min();
    }
private:
    node* merge_(node* heap1, node* heap2)
    {
        node* prev, *next, *h3, *nHeap = NULL, *h1 = heap1, *h2 = heap2, *i, *j;

        vector<node*> st;
        st.reserve(1000);
        while (h1 && h2)
        {
            if (h1->rank < h2->rank)
            {
                st.push_back(h1);
                h1 = h1->next;
            }
            else
            {
                st.push_back(h2);
                h2 = h2->next;
            }
        }
        while (h1)
        {
            st.push_back(h1);
            h1 = h1->next;
        }
        while (h2)
        {
            st.push_back(h2);
            h2 = h2->next;
        }
        if (!st.empty())
            for (int i = 0; i < st.size() - 1; i++)
            {
                st[i]->next = st[i + 1];
            }
        if (st.empty()) nHeap = NULL; else
        {
            st[st.size() - 1]->next = NULL;
            nHeap = st[0];
        }
        if (nHeap == NULL)
        {
            return nHeap;
        }
        prev = NULL;
        h3 = nHeap;
        next = h3->next;
        while (next != NULL)
        {
            if ((h3->rank != next->rank) || ((next->next != NULL) && (next->next)->rank == h3->rank))
            {
                prev = h3;
                h3 = next;
            }
            else
            {
                if (h3->key <= next->key)
                {
                    h3->next = next->next;
                    next->parent = h3;
                    next->next = h3->child;
                    h3->child = next;
                    h3->rank = h3->rank + 1;
                }
                else
                {
                    if (prev == NULL)
                    {
                        nHeap = next;
                    }
                    else
                    {
                        prev->next = next;
                    }
                    h3->parent = next;
                    h3->next = next->child;
                    next->child = h3;
                    next->rank = next->rank + 1;
                    h3 = next;
                }
            }
            next = h3->next;
        }
        return nHeap;
    }

    node *reverseNode = NULL;

    void reverse(node* y)
    {
        if (y->next != NULL)
        {
            reverse(y->next);
            (y->next)->next = y;
        }
        else
        {
            reverseNode = y;
        }
    }

    void print(node *heap, int level)
    {
        if (heap == 0) return;
        print(heap->child, level + 1);
        for (int i = 0; i < level; i++) cout << "     ";
        cout << heap->key << endl;
        print(heap->next, level);
    }
    node *head = NULL;
};