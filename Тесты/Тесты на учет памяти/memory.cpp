#include <iostream>
#include <time.h>
#include <stdio.h>
#include <conio.h>
#include <string>
#include <ctype.h>
#include <vector>
#include <fstream>
#include "binomial_heap.h"

#pragma warning(disable : 4996)

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

    setlocale(LC_ALL, "Rus");
    heap *h = new heap();

    ifstream in("input.txt");
    ofstream out("output.txt");

    string s;
    int n;

    in >> s;
    if (!check(s)) ERROR;
    n = atoi(s.c_str());
    if (n <= 0) ERROR;
    if (n > 50000000) ERROR;

    for (int i = 0; i < n; i++)
    {
        h->insert(rand());
    }
    system("pause");
DA_ETO_METKA:
    in.close();
    out.close();
}