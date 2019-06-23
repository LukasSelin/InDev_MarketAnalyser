
#include "auction_type.h"
#include <fstream>
#include <climits>
#include <iostream>

using namespace std;

void proccess_auctions(string fil, auction_type & auction){
  int number_of_reads = 0;
   int starting_point = get_length_of_auction(fil, number_of_reads);

  while (true){

    ifstream  in_fil{fil};
    char c = in_fil.peek();
    in_fil.close();
    if ( c == 'I'){
      starting_point = get_length_of_auction(fil, number_of_reads);
      proccess_one_auction(fil, auction, starting_point);
      print_results(auction);
    //  starting_point = starting_point + 640 ;
      number_of_reads++;

    }else{
      break;
    }
  }
}



void proccess_one_auction(string fil, auction_type & auction, int starting_point){

  string temp;
  ifstream in_fil{fil};
  // cout << "4. This is the number_of_reads value: " << number_of_reads << endl;
  in_fil.seekg (starting_point);
  in_fil.ignore(INT_MAX, '{');
  in_fil.ignore(INT_MAX, ':');
  getline(in_fil, temp, ',');
  auction.ID = stoi(temp);

  in_fil.ignore(INT_MAX, ':');
  getline(in_fil, temp, ',');
  auction.UnitPriceSilver = stoi(temp);

  in_fil.ignore(INT_MAX, ':');
  getline(in_fil, temp, ',');
  auction.TotalPriceSilver = stoi(temp);

  in_fil.ignore(INT_MAX, ':');
  getline(in_fil, temp, ',');
  auction.Amount = stoi(temp);

  in_fil.ignore(INT_MAX, ':');
  getline(in_fil, temp, ',');
  auction.Tier = stoi(temp);

  for (unsigned int a = 0; a < 9; a++){
    in_fil.ignore(INT_MAX, ':');
  }

  getline(in_fil, auction.ItemTypeId, ',');
  cout << auction.ItemTypeId << endl;

  in_fil.ignore(INT_MAX, ':');
  getline(in_fil, auction.ItemGroupTypeId, ',');
  cout << auction.ItemGroupTypeId << endl;

  in_fil.ignore(INT_MAX, ':');
  getline(in_fil, temp, ',');
  auction.EnchantmentLevel = stoi(temp);

  in_fil.ignore(INT_MAX, ':');
  getline(in_fil, temp, ',');
  auction.QualityLevel = stoi(temp);



  in_fil.close();
}

void print_results(auction_type & auction){
  ofstream ut_fil;
  ut_fil.open("resultat.txt",ios_base::app);
    ut_fil << auction.ID << '\t' << auction.UnitPriceSilver << '\t'<< auction.TotalPriceSilver << '\t' << auction.Amount << '\t';
    ut_fil << auction.Tier << '\t' << auction.ItemTypeId << '\t' << auction.ItemGroupTypeId << '\t' << auction.EnchantmentLevel << '\t' << auction.QualityLevel << '\n';
  ut_fil.close();

}
