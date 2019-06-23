#include "auction_type.h"
#include <iostream>


using namespace std;
int main(){

  string fil;
  auction_type auction;

  cout << "Which file do you want to proccess:" << endl;
  cin >> fil;

  proccess_auctions(fil, auction);

  return 0;
}
