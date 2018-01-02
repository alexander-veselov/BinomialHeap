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

    setlocale(LC_ALL, "Rus");
    vector<heap*> heaps;
    vector<node*> nodes;

    ifstream in("input.txt");
    ofstream out("output.txt");

    string s, s1, s2;
    int k, j, x, n;
    in >> s;

    if (!check(s)) ERROR;

    n = atoi(s.c_str());

    if (n <= 0)ERROR;
    if (n > 50000000) ERROR;

    for (int i = 0; i < n; i++)
    {
        in >> s;
        if (s == "CREATE_HEAP")
        {
            heaps.push_back(new heap);
            continue;
        }
        if (s != "HEAP") ERROR;
        in >> s;
        if (!check(s)) ERROR;
        j = atoi(s.c_str());

        if (j < heaps.size())
        {
            if (heaps[j] == NULL) ERROR;
        }
        else ERROR;
        in >> s;
        if (s == "INSERT")
        {
            in >> s;
            if (!check(s)) ERROR;
            k = atoi(s.c_str());
            heaps[j]->insert(k);
        }
        else
            if (s == "INSERT_STORABLE")
            {
                in >> s;
                if (!check(s)) ERROR;
                k = atoi(s.c_str());
                nodes.push_back(heaps[j]->insert(k));
            }
            else
                if (s == "EXTRACT_MIN")
                {
                    if (heaps[j]->empty()) ERROR;
                    out << heaps[j]->extract_min();
                    out << endl;
                }
                else
                    if (s == "GET_MIN")
                    {
                        if (heaps[j]->empty()) ERROR;
                        out << heaps[j]->get_min();
                        out << endl;
                    }
                    else
                        if (s == "DELETE")
                        {
                            in >> s;
                            if (!check(s)) ERROR;
                            k = atoi(s.c_str());

                            if (k < nodes.size() || k<0)
                            {
                                if (nodes[k] == NULL) ERROR;
                            }
                            else ERROR;

                            heaps[j]->delete_key(nodes[k]);
                            nodes[k] = NULL;

                        }
                        else
                            if (s == "MERGE")
                            {
                                in >> s;
                                if (!check(s)) ERROR;
                                k = atoi(s.c_str());

                                if (k < heaps.size() || k<0)
                                {
                                    if (heaps[k] == NULL) ERROR;
                                }
                                else ERROR;
                                heaps.push_back(heaps[j]->merge(heaps[k]));
                                heaps[j] = NULL;
                                heaps[k] = NULL;
                            }
                            else
                                if (s == "DECREASE_KEY")
                                {
                                    in >> s;
                                    if (!check(s)) ERROR;
                                    k = atoi(s.c_str());
                                    if (k < nodes.size() || k<0)
                                    {
                                        if (nodes[k] == NULL) ERROR;
                                    }
                                    else ERROR;
                                    in >> s;
                                    if (!check(s)) ERROR;
                                    x = atoi(s.c_str());
                                    if (heaps[j]->decrease_key(nodes[k], x) == -1) ERROR;
                                }
                                else
                                {
                                    if (true) ERROR;
                                }

    }


DA_ETO_METKA:
    in.close();
    out.close();
}