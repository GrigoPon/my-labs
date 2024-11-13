#include <iostream>
#include <vector>
#include <fstream>
#include <string>
#include <cstdlib>
#include <algorithm>
#include <queue>

using namespace std;

void split(const string& original_file, const string& result_file_pref, int file_size)
{
	string folder = "Assets\\";
	string file_type = ".txt";
	ifstream reading(original_file);
	if (!reading.is_open())
	{
		cerr << "Ошибка чтения файла: " << original_file << endl;
		exit(EXIT_FAILURE);
	}

	int file_index = 0;
	while (true)
	{
		ofstream writing(folder + result_file_pref + to_string(file_index++) + file_type);
		if (!writing.is_open())
		{
			cerr << "Ошибка чтения файла: " << result_file_pref + to_string(file_index - 1) + file_type << endl;
			exit(EXIT_FAILURE);
		}

		for (int i = 0; i < file_size && !reading.eof(); i++)
		{
			double note;
			reading >> note; //читаю данные с исходного файла
			writing << note << " "; //записываю считанные данные по текущим
		}

		if (reading.eof())
			break;

		writing.close();

	}

	reading.close();

}

void SortData(const string& current_file)
{
	string folder = "Assets\\";
	vector<double> Data;
	double value;
	ifstream reading(folder + current_file);
	if (!reading.is_open())
	{
		cerr << "Ошибка чтения файла: " << folder + current_file << endl;
		exit(EXIT_FAILURE);
	}

	while (reading >> value)
	{
		Data.push_back(value);
	}

	reading.close();

	sort(Data.begin(), Data.end());

	ofstream writing(folder + current_file);
	if (!writing.is_open())
	{
		cerr << "Ошибка открытия файла: " << folder + current_file << endl;
		exit(EXIT_FAILURE);
	}

	for (auto v : Data)
	{
		writing << v << " ";
	}

	writing.close();
}

void MergingFiles(const string& A, const string& B, const string& result_file_pref)
{
	ifstream readA(A), readB(B);
	if (!readA.is_open() || !readB.is_open())
	{
		cerr << "Ошибка чтения файла: " << A << " или " << B << endl;
		exit(EXIT_FAILURE);
	}

	ofstream writing(result_file_pref);
	if (!writing.is_open())
	{
		cerr << "Ошибка открытия файла: '" << result_file_pref << "'" << endl;
		exit(EXIT_FAILURE);
	}

	double values_A, values_B;
	readA >> values_A;
	readB >> values_B;

	while (!readA.eof() && !readB.eof())
	{
		if (values_A <= values_B)
		{
			writing << values_A << " ";
			if (!(readA >> values_A))
				break;
		}
		else
		{
			writing << values_B << " ";
			if (!(readB >> values_B))
				break;
		}
	}

	while (!readA.eof())
	{
		writing << values_A << " ";
		readA >> values_A;
	}

	while (!readB.eof())
	{
		writing << values_B << " ";
		readB >> values_B;
	}
	readA.close();
	readB.close();
	writing.close();
}

void MergingSplits(const string& result_file_pref, int file_index, const string& out)
{
	vector<string> files;
	string folder = "Assets\\";


	for (int i = 0; i < file_index; i++)
	{
		string current_file = folder + result_file_pref + to_string(i) + ".txt";
		files.push_back(current_file);
	}
	
	vector<ifstream> streams(files.size());
	vector<double> currentNumbers(files.size());
	priority_queue<pair<double, double>, vector<pair<double, double>>, greater<>> minHeap;

	for (size_t i = 0; i < files.size(); ++i) {
		streams[i].open(files[i]);
		if (streams[i].is_open()) {
			streams[i] >> currentNumbers[i];
			minHeap.push({ currentNumbers[i], static_cast<int>(i) });
		}
	}

	ofstream outputStream(out);
	if (!outputStream.is_open()) {
		cerr << "Ошибка открытия выходного файла!" << endl;
		return;
	}

	// Слияние файлов
	while (!minHeap.empty()) {
		pair<double, double> top = minHeap.top();
		minHeap.pop();
		outputStream << top.first << " ";

		if (streams[top.second] >> currentNumbers[top.second]) {
			minHeap.push({ currentNumbers[top.second], top.second });
		}
	}

	// Закрываем файлы
	for (size_t i = 0; i < files.size(); ++i) {
		streams[i].close();
	}

	outputStream.close();

		
	/*if (file_index % 2 == 1)
	{
		rename((folder + result_file_pref + to_string(file_index - 1) + ".txt").c_str(),
			(folder + result_file_pref + std::to_string(file_index / 2) + "_merged.txt").c_str());
	}*/
	
}



int main() {

	setlocale(LC_ALL, "RU");
	string inputFile("Assets\\input.txt");
	string outputPrefix("output");
	cout << "В каждом файле будет по ? частей (введите число): ";
	int file_size;
	cin >> file_size;

	// Разделение файла на части
	split(inputFile, outputPrefix, file_size);

	// Подсчет количества созданных частей
	int file_index = 0;
	string folder = "Assets\\";
	while (ifstream(folder + outputPrefix + to_string(file_index) + ".txt")) {
		++file_index;
	}

	// Сортировка каждой части
	for (int i = 0; i < file_index; ++i) {
		SortData(outputPrefix + std::to_string(i) + ".txt");
	}

	string out = "Assets\\mergeout.txt";
	// Слияние всех частей
	MergingSplits(outputPrefix, file_index, out);

	return EXIT_SUCCESS;
}