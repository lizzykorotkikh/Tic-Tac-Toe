#include <fstream>
#include <vector>
#include <iostream>
#include <ctime>
#include <string>
#include <algorithm>

using namespace std;

void Level_1();
void Level_2();

string Winner (string field[][3], string win){

    string line="";

    //rows
    for (int i=0; i<3; i++) {
        for (int j = 0; j < 3; j++)
            line +=  field[i][j];
        if (line=="OOO") return win="Second";
        else if (line=="XXX") return win="First";
        else line="";
    }


    //columns
    for (int i=0; i<3; i++) {
        for (int j = 0; j < 3; j++)
            line +=  field[j][i];
        if (line=="OOO") return win="Second";
        else if (line=="XXX") return win="First";
        else line="";
    }

    //diagonals
    line=field[0][0]+field[1][1]+field[2][2];
    if (line=="OOO") return win="Second";
    else if (line=="XXX") return win="First";
         else line="";



    line=field[0][2]+field[1][1]+field[2][0];
    if (line=="OOO") return win="Second";
    else if (line=="XXX") return win="First";
         else line="";

    return win="";

}

void Show_Field( string field[][3]){

    cout<<endl;
    cout<<"Field:"<<endl;
    cout<<endl;


    for (int i=0; i<3; i++) {

        for (int j = 0; j < 3; j++) {
            if (j == 2) cout << field[i][j];
            else cout  << field[i][j]<<"|";
        }
        cout<<endl;
        if (i<2) cout<<"—————"<<endl;
    }
}

string IsFull(string field[][3]){

    string check="full";

    for (int i=0; i<3; i++)
        for (int j=0; j<3; j++)

            if (field[i][j]>="1" && field[i][j]<="9") return check="not";

    return check;



}

void Put_In( string field[][3], string num, string element){


    for (int i=0; i<3; i++)
        for (int j=0; j<3; j++)

            if (field[i][j]==num) field[i][j]=element;



}

int find(string var[], string line){

    for ( int j=0;j<3;++j)
        if (var[j]==line) {

            switch(j){

                case 0:
                    return 2;


                case 1:
                    return 0;

                case 2:
                    return 1;

            }


        }
    return -1;
}

int Com_Step(string field[][3]){

    string line="";
    string copy_ar[3][3];
    string varP1[]={"XXN","NXX","XNX"};
    string varP2[]={"OON","NOO","ONO"};
    string varP2_m[]={"NON","NNO","ONN"};


//get the copy of field
    for (int i=0; i<3; ++i)
        for (int j=0;j<3;++j)
            if (field[i][j]>="1" && field[i][j]<="9") copy_ar[i][j]="N";
            else copy_ar[i][j]=field[i][j];


//check if it is one step before win of himself rows
    for (int i=0;i<3;++i) {
        for (int j = 0; j < 3; ++j)
            line += copy_ar[i][j];
        if ((find(varP2,line))!=-1) {Put_In(field,field[i][find(varP2,line)], "O"); return 0;}
        else line="";
    }

//check if it is one step before win of himself columns
    for (int i=0;i<3;++i) {
        for (int j = 0; j < 3; ++j)
            line += copy_ar[j][i];
        if ((find(varP2,line))!=-1) {Put_In(field,field[find(varP2,line)][i], "O"); return 0;}
        else line="";
    }

//check if it is one step before win of himself diagonals
    line=copy_ar[0][0]+copy_ar[1][1]+copy_ar[2][2];
    if ((find(varP2,line))!=-1) {Put_In(field,field[find(varP2,line)][find(varP2,line)], "O"); return 0;}
    else line="";

    line=copy_ar[2][0]+copy_ar[1][1]+copy_ar[0][2];
    if ((find(varP2,line))!=-1) {Put_In(field,field[abs(find(varP2,line)-2)][find(varP2,line)], "O"); return 0;}
    else line="";


//check if it is one step before win of first p rows
    for (int i=0;i<3;++i) {
        for (int j = 0; j < 3; ++j)
            line += copy_ar[i][j];
        if ((find(varP1,line))!=-1) {Put_In(field,field[i][find(varP1,line)], "O"); return 0;}
        else line="";
    }

//check if it is one step before win of first p columns
    for (int i=0;i<3;++i) {
        for (int j = 0; j < 3; ++j)
            line += copy_ar[j][i];
        if ((find(varP1,line))!=-1) {Put_In(field,field[find(varP1,line)][i], "O"); return 0;}
        else line="";
    }

//check if it is one step before win of first p diagonals
    line=copy_ar[0][0]+copy_ar[1][1]+copy_ar[2][2];
    if ((find(varP1,line))!=-1) {Put_In(field,field[find(varP1,line)][find(varP1,line)], "O"); return 0;}
    else line="";

    line=copy_ar[2][0]+copy_ar[1][1]+copy_ar[0][2];
    if ((find(varP1,line))!=-1) {Put_In(field,field[abs(find(varP1,line)-2)][find(varP1,line)], "O"); return 0;}
    else line="";

//check if it is one O on the row
    for (int i=0;i<3;++i) {
        for (int j = 0; j < 3; ++j)
            line += copy_ar[i][j];
        if ((find(varP2_m,line))!=-1) {Put_In(field,field[i][find(varP2_m,line)], "O"); return 0;}
        else line="";
    }

//check if it is one O on the column
    for (int i=0;i<3;++i) {
        for (int j = 0; j < 3; ++j)
            line += copy_ar[i][j];
        if ((find(varP2_m,line))!=-1) {Put_In(field,field[find(varP2_m,line)][i], "O"); return 0;}
        else line="";
    }

//check if it is one O on the diagonals
    line=copy_ar[0][0]+copy_ar[1][1]+copy_ar[2][2];
    if ((find(varP2_m,line))!=-1) {Put_In(field,field[find(varP2_m,line)][find(varP2_m,line)], "O"); return 0;}
    else line="";

    line=copy_ar[2][0]+copy_ar[1][1]+copy_ar[0][2];
    if ((find(varP2_m,line))!=-1) {Put_In(field,field[abs(find(varP2_m,line)-2)][find(varP2_m,line)], "O"); return 0;}
    else line="";

//check if it is NNN in rows
    for (int i=0;i<3;++i) {
        for (int j = 0; j < 3; ++j)
            line += copy_ar[i][j];
        if (line=="NNN") {Put_In(field,field[i][0],"O" ); return 0;}
        else line="";
    }
//check if it is NNN in columns
    for (int i=0;i<3;++i) {
        for (int j = 0; j < 3; ++j)
            line += copy_ar[j][i];
        if (line=="NNN") {Put_In(field,field[0][i],"O" ); return 0;}
        else line="";
    }
//check if it is NNN diagonals
    line=copy_ar[0][0]+copy_ar[1][1]+copy_ar[2][2];
    if (line=="NNN") {Put_In(field,field[0][0],"O" ); return 0;}
    else line="";

    line=copy_ar[0][2]+copy_ar[1][1]+copy_ar[2][0];
    if (line=="NNN") {Put_In(field,field[2][0],"O" ); return 0;}
    else line="";

//put random

    for (int i=0;i<3;++i)
        for (int j = 0; j < 3; ++j)
            if (copy_ar[i][j]=="N") {Put_In(field,field[i][j],"O" );return 0;}



}

int main()
{
	setlocale(LC_ALL, "Rus");
	char answer;
	answer='y';
	int level;



    cout<<"RULES FOR TIC-TAC-TOE"<<endl;
    cout<<"1. The game is played on a grid that's 3 squares by 3 squares."<<endl;
    cout <<"2. You are X, your friend (or the computer in this case) is O. Players take turns putting their marks in empty squares."<<endl;
    cout<<"3. The first player to get 3 of her marks in a row (up, down, across, or diagonally) is the winner."<<endl;
    cout<<"4. When all 9 squares are full, the game is over. If no player has 3 marks in a row, the game ends in a tie."<<endl;
    cout<<endl;



	while (answer=='y')
	{
        cout<<"Choose who you want to play with:"<<endl;
        cout<<"1. With a computer"<<endl;
        cout<<"2. With a friend"<<endl;
        cout<<"Choose the number (1 or 2): ";
        cin>>level;
        cout<<endl;

        if (level==1) Level_1();
        else Level_2();

		cout << "Want to continue? Write (y) if yes, (n) if no: ";
		cin >> answer;
		cout<<endl;
		if (answer == 'n') cout << "Bye!" << endl;
	}

}

void Level_1() 
{

    string field[3][3];
    string p;
    string check="";
    string win="";
    int k=1;



    for (int i=0; i<3; i++)
        for (int j=0; j<3; j++) {
            field[i][j] = std::to_string(k);
            k++;
        }

    cout<<"This level is for playing with a computer."<<endl;
    cout<<"The first player will be X."<<endl;
    cout<<"The second player (computer) will be O."<<endl;
    cout<<endl;


    Show_Field(field);

	while (true)
	{


        cout<<"First player: choose the place on the field: ";
        cin>>p;
        Put_In(field,p,"X");

        //check if the field is full if yes break
        check=IsFull(field);
        if (check=="full") {Show_Field(field); break;}


        //check the winner
        win=Winner(field,win);
        if (win!="") { Show_Field(field); break;}



        int i=Com_Step(field);

        //check the winner
        win=Winner(field,win);
        if (win!="") { Show_Field(field); break;}

        Show_Field(field);
	}

    if (win=="") cout<<"Game over. Draw"<<endl;
    else cout<<win<<", you are winner"<<endl;


}





void Level_2(){

    string field[3][3];
    string p;
    string check="";
    string win="";
    int k=1;

    for (int i=0; i<3; i++)
        for (int j=0; j<3; j++) {
            field[i][j] = std::to_string(k);
            k++;
        }

        cout<<"This level is with two players."<<endl;
        cout<< "The first player will be X."<<endl;
        cout<< "The second player will be O."<<endl;
        cout<<endl;


         Show_Field(field);


    while(true){


        cout<<"First player: choose the place on the field: ";
        cin>>p;
        Put_In(field,p,"X");

        //check if the field is full if yes break

        check=IsFull(field);
        if (check=="full") {Show_Field(field); break;}


        //check the winner
        win=Winner(field,win);
        if (win!="") { Show_Field(field); break;}


        Show_Field(field);
        cout<<"Second player: choose the place on the field: ";
        cin>>p;
        Put_In(field,p,"O");

		//check the winner
        win=Winner(field,win);
        if (win!="") { Show_Field(field); break;}

        Show_Field(field);

    }

    if (win=="") cout<<"Game over. Draw"<<endl;
    else cout<<win<<", you are winner"<<endl;





}
