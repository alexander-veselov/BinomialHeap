#include <iostream>
#include <time.h>
#include <stdio.h>
#include <conio.h>
#include <string>
#include <ctype.h>
#include <vector>
#include <fstream>
#include "binomial_heap.h"

using namespace std;

#define ERROR  { out << "Некорректный тест"; goto DA_ETO_METKA ; }


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


int main()
{

    setlocale(LC_ALL, "Rus");
    heap *h = new heap();
    int m = 1000000;
    vector<heap*> heaps(m, new heap());

    ifstream in("input.txt");
    ofstream out("output.txt");

    string s, s1, s2;
    int n;

        in >> s;

        if (!check(s)) ERROR;
        n = atoi(s.c_str());

        if (n <= 0) ERROR;
        if (n > 100) ERROR;
        for (int i = 0; i < m; i++)
        {
            int k = rand();
            for (int j = 0; j < n; j++)
            {
                heaps[i]->insert(k);
            }
        }

        long long time = clock();
        for (int i = 0; i < m; i++) heaps[i]->insert(rand());
        out << clock() - time;
    DA_ETO_METKA:
        in.close();
        out.close();
}