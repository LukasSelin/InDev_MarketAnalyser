#include <string>

using namespace std;

struct auction_type{
  int ID;
  int UnitPriceSilver;
  int TotalPriceSilver;
  int Amount;
  int Tier;

  string ItemTypeId;
  string ItemGroupTypeId;

  int EnchantmentLevel;
  int QualityLevel;
};
void proccess_auctions(string fil, auction_type & auction);
void proccess_one_auction(string fil, auction_type & auction, int starting_point);
void print_results(auction_type & auction);
int get_length_of_auction(string fil, int number_of_reads);
