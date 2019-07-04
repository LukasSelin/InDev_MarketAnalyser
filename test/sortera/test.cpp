
#include <iostream>
#include "sorting_rawdata.h"

using namespace std;

int main(){
  string file;
  string sorted_auction;

  cout << "Which file do you want to sort?" << endl;
  cin >> file;

    proccess_sorting_data(file);

  return 0;
}
