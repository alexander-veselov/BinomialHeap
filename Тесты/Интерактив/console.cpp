#include <iostream>
#include <time.h>
#include <stdio.h>
#include <conio.h>
#include <string>
#include <ctype.h>

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

    heap* merge(heap *heap1, heap *H2)
    {
        heap *nHeap = new heap();
        nHeap->head = merge_(heap1->head, H2->head);
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
            cout << "Данного элемента не существует\n";
            return 0;
        }
        if (k > p->key)
        {
            cout << "Попытка увеличить ключ\n";
            return 0;
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

        if (h1 != NULL)
        {
            if (h2 != NULL && h1->rank <= h2->rank)
            {
                nHeap = h1;
            }
            else
                if (h2 != NULL && h1->rank > h2->rank)
                {
                    nHeap = h2;
                }
                else
                {
                    nHeap = h1;
                }
        }
        else
        {
            nHeap = h2;
        }
        while (h1 != NULL && h2 != NULL)
        {
            if (h1->rank < h2->rank)
            {
                h1 = h1->next;
            }
            else
                if (h1->rank == h2->rank)
                {
                    i = h1->next;
                    h1->next = h2;
                    h1 = i;
                }
                else
                {
                    j = h2->next;
                    h2->next = h1;
                    h2 = j;
                }
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

    int rank = 1;
    node *head = NULL;
};


heap *h = new heap();

bool check(string s)
{

    for (int i = 0; i < s.length(); i++)
    {
        if (!isdigit(s[i]))
        {
            if (i == 0 && s[i] == '-') continue;
            return false;
        }
    }
    return true;
}

void menu()
{
    system("cls");
    cout << "0 - Выход из режима ввода\n";
    cout << "1 - Вставка элемента в кучу\n";
    cout << "2 - Извлечение минимума\n";
    cout << "3 - Поиск минимума\n";
    cout << "4 - Добавить в кучу N случайных элементов\n";
    cout << "5 - Вывод кучи на экран\n";
    string s;
    cin >> s;
    if (!check(s))
    {
        cout << "Некорректный ввод\n";
        cout << "\nНажмите любую клавишу.\n";
        _getch();
        menu();
        return;
    }
    int a = atoi(s.c_str());
    if (!(a >= 0 && a <= 5))
    {
        cout << "Некорректный ввод\n";
        cout << "\nНажмите любую клавишу.\n";
        _getch();
        menu();
        return;
    }
    switch (a)
    {
    case 0:
    {
        return;
        break;
    }
    case 1:
    {
        cout << "Введите число для вставки: \n";
        string s;
        cin >> s;
        if (!check(s))
        {
            cout << "Некорректный ввод\n";
            cout << "\nНажмите любую клавишу.\n";
            _getch();
            menu();
            return;
        }
        int a = atoi(s.c_str());
        h->insert(a);
        break;
    }
    case 2:
    {
        if (h->empty())
        {
            cout << "Куча пустая.\n";
            cout << "\nНажмите любую клавишу.\n";
            _getch();
            menu();
            return;
        }
        cout << "Извлеченный минимум: ";
        cout << h->extract_min() << endl;
        cout << endl;
        break;
    }
    case 3:
    {
        if (h->empty())
        {
            cout << "Куча пустая.\n";
            cout << "\nНажмите любую клавишу.\n";
            _getch();
            menu();
            return;
        }
        cout << "Минимум: ";
        cout << h->get_min() << endl;
        cout << endl;
        break;
    }
    case 4:
    {
        cout << "Введите N: ";
        string s;
        cin >> s;
        if (!check(s))
        {
            cout << "Некорректный ввод\n";
            cout << "\nНажмите любую клавишу.\n";
            _getch();
            menu();
            return;
        }
        int a = atoi(s.c_str());
        if (a <= 0)
        {
            cout << "Введено не положительное число\n";
            cout << "\nНажмите любую клавишу.\n";
            _getch();
            menu();
            return;
        }
        for (int i = 0; i < a; i++)
        {
            h->insert(rand() % 5000 - 1000);
        }
        cout << endl;
        break;
    }
    case 5:
    {
        if (h->empty())
        {
            cout << "Куча пустая.\n";
            cout << "\nНажмите любую клавишу.\n";
            _getch();
            menu();
            return;
        }
        cout << "Куча: \n";
        h->print();
        cout << endl;
        break;
    }
    }
    cout << "\nНажмите любую клавишу.\n";
    _getch();
    menu();
}
int main()
{
    setlocale(LC_ALL, "Rus");
    menu();
}