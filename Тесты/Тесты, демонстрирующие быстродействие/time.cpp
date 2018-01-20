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

#define ERROR  { out << "Incorrect test"; goto DA_ETO_METKA ; }


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
    srand(time(0));
    setlocale(LC_ALL, "Rus");
    heap *h = new heap();
    int m = 1000000, move;
    vector<heap*> heaps(m, new heap());

    ifstream in("input.txt");
    ofstream out("output.txt");

    string s, s1, s2;
    int n;

    in >> s;
    if (s == "INSERT")
    {
        move = 0;
    } else
        if (s == "EXTRACT")
        {
            move = 1;
        }
        else ERROR;

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
    if (move == 0)
    {
        int k = rand();
        for (int i = 0; i < m; i++) heaps[i]->insert(k);
    }
    else
    {
        for (int i = 0; i < m; i++) heaps[i]->extract_min();
    }
    out << (clock() - time);
DA_ETO_METKA:
    in.close();
    out.close();
}