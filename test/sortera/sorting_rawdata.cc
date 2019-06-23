
#include <fstream>
#include <climits>
#include <string>
#include <iostream>
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

  char * buffer = new char [1156];
  string auction;
  raw_data.read(buffer, 1156);

  for (int a = 0; number_of_reads >= a; a++){
    raw_data.ignore(INT_MAX, '{');
  }

  getline(raw_data, auction, '}');
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
   int starting_point = get_length_of_auction(fil, number_of_reads);
  while (true){

    ifstream  in_fil{fil};
    char c = in_fil.peek();
    in_fil.close();
    if ( c == 'I'){
      starting_point = get_length_of_auction(fil, number_of_reads);
    string auction = get_one_auction(fil, starting_point);
      print_sorted_data(auction);
      number_of_reads++;

    }else{
      break;
    }
    }
  }
