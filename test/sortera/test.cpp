
#include <iostream>
#include "sorting_rawdata.h"

using namespace std;

int main(){
  string file;
  string sorted_auction;
  int times_read{0};

  cout << "Which file do you want to sort?" << endl;
  cin >> file;

  while(true){
    sorted_auction = get_one_auction(file, times_read);
    print_sorted_data(sorted_auction);
    // cout << times_read << endl;
    times_read++;
}
  return 0;
}
