
#include <fstream>
#include <climits>
#include <string>
#include <iostream>
#include <algorithm>
using namespace std;

int get_length_of_auction(string fil, int number_of_reads){
  ifstream in_fil{fil};
  for (int a = 0; a < number_of_reads; a++){
    in_fil.ignore(INT_MAX, '}');
  }
  int length = in_fil.tellg();
  in_fil.close();
  return length;
}
int find_whole_data(string file){

  ifstream raw_data{file};
  raw_data.ignore(133, '{');
  int position = 0;
  int times = 0;

  while (true){

      for (int a = 0; a < 18; a++){
        int past_pos = raw_data.tellg();
        raw_data.ignore(INT_MAX, ',');
        int temp = raw_data.tellg();
        int difference = temp - past_pos;

        if (difference >= 133){
          position = past_pos;
          cout << "Här slutade det i data-delen." << endl;
          break;
        }else{
          cout << a << ": Här " << endl;
        }
  }
    times++;
    raw_data.close();
  return position;
  }
}

string get_one_auction(string file, int number_of_reads){

  ifstream raw_data{file};
  string auction;
  string auction2;
  size_t position{};

  for (int a = 0; number_of_reads >= a; a++){
    raw_data.ignore(INT_MAX, '{');
  }

  getline(raw_data, auction, '}');
  position = auction.find("IP 185.218.131.112.5056");

if (position != string::npos){
  auction.erase(position, 134);
  auction.erase(remove(auction.begin(), auction.end(), '\n'), auction.end());
}else{

}
  cout << position << '\t';

  auction = '{' + auction + '}';
  raw_data.close();

  return auction;
}

void print_sorted_data(string auction){
  ofstream sorted_data;
  sorted_data.open("sorted_data.txt",ios_base::app);
    sorted_data << auction << endl;
  sorted_data.close();
}

void proccess_sorting_data(string fil){
  int number_of_reads = 0;
  ifstream  in_fil{fil};

      while (true){
        string auction = get_one_auction(fil, number_of_reads);
        print_sorted_data(auction);

        cout << number_of_reads << endl;
        number_of_reads++;
    }
    in_fil.close();
  }
